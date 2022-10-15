using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Color timetextColor;
    CapsuleCollider2D colliders;
    [HideInInspector]
    public Rigidbody2D rigid;
    [Header("----------Move")]
    public float maxSpeed;
    public bool[] moveVec;
    bool movebuttonDown;
    bool jumpable = true;
    public int jumpPower = 0;
    bool doubleJump = false;
    bool isAttach;
    public bool isWallTouch;
    public bool isTouchBorder;
    bool isWind;
    [Header("----------Level")]
    public int Level;
    public int levelCycle;
    public bool isLevelUp;
    public float levelWaitRate;
    public bool levelTargetnum;
    int levelFactor;
    [Header("----------SetAciveObj")]
    public GameObject bgManager;
    public GameObject fade;
    public GameObject[] initGrounds;
    public GameObject finish;
    public GameObject border;
    [Header("----------CameraControl")]
    float cameraSize;
    bool camerable = false;
    float maxCamsize = 130f;
    static float mincamSize;
    [Header("----------Para")]
    public GameObject redPara;
    public float parable;
    bool usePara;
    bool para = false;
    [Header("----------TimeRelated")]
    public Text timeOverText;
    public Text timeText;
    public Text timingText;
    public GameObject praise;
    float timeStart;
    int timeFlow = 0;
    string timeFlowStr;
    public Animation anim;
    public AnimationClip text1;
    public AnimationClip text2;
    [Header("----------OverHit")]
    bool isOverHit;
    int jumpHit;
    float overCatchTime;
    [Header("----------Particle")]
    public TrailRenderer trail;
    public ParticleSystem[] jumpEffect;

    [Header("----------Etc")]
    public bool usingTimingBelt;
    public float reflect;
    public GameObject dayWater;
    public GameObject nightWater;

    public float h = 0;
    void levelSet(int level)
    {
        switch (level)
        {
            case 0:
                levelWaitRate = 0.1f;
                levelFactor = 100;
                anim.clip = text1;
                dayWater.SetActive(true);
                nightWater.SetActive(false);
                break;
            case 1:
                levelWaitRate = 0.05f;
                levelFactor = 100;
                anim.clip = text1;
                dayWater.SetActive(true);
                nightWater.SetActive(false);
                break;
            case 2:
                levelWaitRate = 0.04f;
                levelFactor = 120;
                anim.clip = text1;
                dayWater.SetActive(true);
                nightWater.SetActive(false);
                break;
            case 3:
                levelWaitRate = 0.08f;
                levelFactor = 120;
                anim.clip = text2;
                dayWater.SetActive(false);
                nightWater.SetActive(true);
                break;
            case 4:
                levelWaitRate = 0.02f;
                levelFactor = 240;
                anim.clip = text2;
                dayWater.SetActive(false);
                nightWater.SetActive(true);
                break;
            case 5:
                levelWaitRate = 0.01f;
                levelFactor = 240;
                anim.clip = text2;
                dayWater.SetActive(false);
                nightWater.SetActive(true);
                break;
            default:
                levelWaitRate = 0.01f;
                levelFactor = 600;
                anim.clip = text2;
                dayWater.SetActive(false);
                nightWater.SetActive(true);
                break;
        }
        if (levelCycle >= 3)
        {
            gameManager.levelTarget[5] = "98~02";
            gameManager.levelTarget[6] = "99~01";
        }
        rigid.gravityScale = 4 + level;
    }
    void Awake()
    {
        InitGround();
        mincamSize = Camera.main.fieldOfView;
        timetextColor = timeText.color;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        colliders = GetComponent<CapsuleCollider2D>();
        levelSet(Level % 7);
    }
    void Start()
    {
        StartCoroutine(TimeTxt());
    }
    void CameraControl()
    {
        if (camerable)
        {
            Camera.main.fieldOfView += Time.deltaTime * cameraSize;
            if (Camera.main.fieldOfView >= maxCamsize)
            {
                Camera.main.fieldOfView = maxCamsize;
            }
        }
        if (!camerable)
        {
            Camera.main.fieldOfView -= Time.deltaTime * cameraSize * 0.8f;
            if (Camera.main.fieldOfView <= mincamSize)
            {
                Camera.main.fieldOfView = mincamSize;
            }
        }
    }
    IEnumerator TimeTxt()
    {
        if (timeFlow == 0)
        {
            timeFlowStr = "00";
            timeText.text = timeFlowStr;
            yield return new WaitForSeconds(levelWaitRate);
        }
        if (timeFlow == 99)
        {
            timeFlow = 0;
            timeFlowStr = "0" + timeFlow;
            timeText.text = timeFlowStr;
            yield return new WaitForSeconds(levelWaitRate);
        }

        while (timeFlow < 9)
        {
            timeFlow++;
            timeFlowStr = "0" + timeFlow;
            timeText.text = timeFlowStr;
            yield return new WaitForSeconds(levelWaitRate);
        }
        while (timeFlow < 99)
        {
            timeFlow++;
            timeFlowStr = timeFlow.ToString();
            timeText.text = timeFlowStr;

            yield return new WaitForSeconds(levelWaitRate);
        }

        StartCoroutine(TimeTxt());
    }
    void JumpTxtUpdate()
    {
        timeStart += Time.deltaTime;
        if (jumpable && !isOverHit)
        {
            timeText.color = timetextColor;
        }
        else
        {
            timeText.color = new Color(0, 0, 0, 0.3f);
        }
        timeOverText.text = timeStart.ToString("F1");
    }
    public void moveVecTrue(int index)
    {
        moveVec[index] = true;
    }
    public void moveVecFalse(int index)
    {
        moveVec[index] = false;
    }
    void Move()
    {
        if (isLevelUp)
        {
            animator.SetBool("isWalk", false);
            return;
        }
        if (!gameManager.isMobile)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }
            if (Input.GetButton("Horizontal"))
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }

        if (gameManager.isMobile)
        {

            if (moveVec[0])
                h = -1;
            else if (moveVec[1])
                h = 1;
            else
            {
                h = 0;
            }
            if (movebuttonDown)
            {
                rigid.AddForce(Vector2.right * h * 1.3f, ForceMode2D.Impulse);
                spriteRenderer.flipX = h == 1;
            }

        }
        if (Mathf.Abs(rigid.velocity.x) < 0.2)
        {
            animator.SetBool("isWalk", false);
        }
        else
        {
            animator.SetBool("isWalk", true);
        }
    }
    public void MoveDown()
    {
        movebuttonDown = true;
    }
    public void MoveUp()
    {
        movebuttonDown = false;
    }
    void Update()
    {
        Move();
        JumpTxtUpdate();
        CameraControl();
        Jump();
        CatchOverHit();
        ParaChute();
    }

    void CatchOverHit()
    {
        GoBroken();
        overCatchTime += Time.deltaTime;
        if (overCatchTime > 1)
        {
            overCatchTime = 0;
            jumpHit = 0;
        }
    }
    void GoBroken()
    {
        if (jumpHit >= 5)
        {
            jumpHit = 0;
            gameManager.bgmPlayer[1].Play();
            isOverHit = true;
            praise.GetComponent<Text>().text = "Broken..";
            praise.GetComponent<Text>().color = Color.red;
            praise.GetComponent<Text>().fontSize = 40;
            praise.SetActive(true);
            Invoke("OverHitBack", 3.5f);
        }
    }
    void OverHitBack()
    {
        gameManager.bgmPlayer[1].Stop();
        praise.SetActive(false);
        isOverHit = false;
    }
    void Jumping(float _jumpPower, float _cameraSize, bool _doublejump, int _index)
    {
        rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        cameraSize = _cameraSize;
        doubleJump = _doublejump;
    }
    public void Jump()
    {
        if (gameManager.isMobile)
            return;
        if (!Input.GetButtonDown("Jump"))
            return;
        jumpHit++;
        if (jumpable && !isOverHit)
        {
            StartCoroutine(TimingCatch());
            camerable = true;
            ParaHide();
            para = false;
            if (!doubleJump)
            {
                animator.SetBool("isJump", true);
            }
            else
            {
                animator.SetTrigger("reJump");

            }
            switch (Level % 7)
            {
                case 0:
                    levelTargetnum = timeFlow == 0 || timeFlow == 90 || timeFlow == 80 || timeFlow == 70
                        || timeFlow == 60 || timeFlow == 50 || timeFlow == 40 || timeFlow == 30 || timeFlow == 20 || timeFlow == 10;
                    break;
                case 1:
                    levelTargetnum = timeFlow <= 1 || timeFlow >= 99 || (timeFlow >= 19 && timeFlow <= 21)
                        || (timeFlow >= 39 && timeFlow <= 41) || (timeFlow >= 59 && timeFlow <= 61)
                        || (timeFlow >= 79 && timeFlow <= 81);
                    break;
                case 2:
                    levelTargetnum = (timeFlow >= 24 && timeFlow <= 26) || (timeFlow >= 49 && timeFlow <= 51)
                       || (timeFlow >= 74 && timeFlow <= 76) || timeFlow <= 1 || timeFlow >= 99;
                    break;
                case 3:
                    levelTargetnum = (timeFlow >= 24 && timeFlow <= 26) || (timeFlow >= 49 && timeFlow <= 51)
                       || (timeFlow >= 74 && timeFlow <= 76) || timeFlow <= 1 || timeFlow >= 99;
                    break;
                case 4:
                    levelTargetnum = ((timeFlow >= 49 && timeFlow <= 51) || timeFlow >= 99 || timeFlow <= 1);
                    break;
                case 5:
                    if (levelCycle <= 3)
                        levelTargetnum = timeFlow >= 97 || timeFlow <= 3;
                    else
                        levelTargetnum = timeFlow >= 98 || timeFlow <= 2;
                    break;
                case 6:
                    if (levelCycle <= 3)
                        levelTargetnum = timeFlow >= 98 || timeFlow <= 2;
                    else
                        levelTargetnum = timeFlow >= 99 || timeFlow <= 1;
                    break;
            }
            if (levelTargetnum)
            {
                jumpEffect[jumpPower <= 6 ? jumpPower : jumpEffect.Length - 1].Play();
                gameManager.PlaySound(GameManager.Sfx.SuperJump);
                trail.enabled = true;
                jumpPower++;
                if (Level % 7 == 0 || Level % 7 == 1 || Level % 7 == 2 || Level % 7 == 3)
                {
                    Jumping((levelFactor + 10 * (Level + 1)) * jumpPower, 80, true, 1);
                }
                else
                {
                    Jumping((levelFactor + Level) * Mathf.Pow(2, jumpPower - 1), 80, true, 1);
                }
                StartCoroutine(Praise(new Color(0.2f, 1, 1, 1), jumpPower + " Combo!", 40, true));
                if (!isTouchBorder)
                    rigid.gravityScale = 10 - (Level % 7);
                gameManager.jumpCnt++;
                gameManager.paraScrollbar.size += 0.2f;
            }
            else
            {
                jumpPower = 0;
                Jumping(20f + Level % 7, 20f, true, 0);
                StartCoroutine(Praise(new Color(0, 0, 0.5f, 1), "Oops!", 35, true));
                gameManager.PlaySound(GameManager.Sfx.OopsJump);
            }
        }

    }
    public void MobileJump()
    {
        jumpHit++;
        if (jumpable && !isOverHit)
        {
            StartCoroutine(TimingCatch());
            camerable = true;
            ParaHide();
            para = false;
            if (!doubleJump)
            {
                animator.SetBool("isJump", true);
            }
            else
            {
                animator.SetTrigger("reJump");

            }
            switch (Level % 7)
            {
                case 0:
                    levelTargetnum = timeFlow == 0 || timeFlow == 90 || timeFlow == 80 || timeFlow == 70
                        || timeFlow == 60 || timeFlow == 50 || timeFlow == 40 || timeFlow == 30 || timeFlow == 20 || timeFlow == 10;
                    break;
                case 1:
                    levelTargetnum = timeFlow <= 1 || timeFlow >= 99 || (timeFlow >= 19 && timeFlow <= 21)
                        || (timeFlow >= 39 && timeFlow <= 41) || (timeFlow >= 59 && timeFlow <= 61)
                        || (timeFlow >= 79 && timeFlow <= 81);
                    break;
                case 2:
                    levelTargetnum = (timeFlow >= 24 && timeFlow <= 26) || (timeFlow >= 49 && timeFlow <= 51)
                       || (timeFlow >= 74 && timeFlow <= 76) || timeFlow <= 1 || timeFlow >= 99;
                    break;
                case 3:
                    levelTargetnum = (timeFlow >= 24 && timeFlow <= 26) || (timeFlow >= 49 && timeFlow <= 51)
                       || (timeFlow >= 74 && timeFlow <= 76) || timeFlow <= 1 || timeFlow >= 99;
                    break;
                case 4:
                    levelTargetnum = ((timeFlow >= 49 && timeFlow <= 51) || timeFlow >= 99 || timeFlow <= 1);
                    break;
                case 5:
                    if (levelCycle <= 3)
                        levelTargetnum = timeFlow >= 97 || timeFlow <= 3;
                    else
                        levelTargetnum = timeFlow >= 98 || timeFlow <= 2;
                    break;
                case 6:
                    if (levelCycle <= 3)
                        levelTargetnum = timeFlow >= 98 || timeFlow <= 2;
                    else
                        levelTargetnum = timeFlow >= 99 || timeFlow <= 1;
                    break;
            }
            if (levelTargetnum)
            {
                jumpEffect[jumpPower <= 6 ? jumpPower : jumpEffect.Length - 1].Play();
                gameManager.PlaySound(GameManager.Sfx.SuperJump);
                trail.enabled = true;
                jumpPower++;
                if (Level % 7 == 0 || Level % 7 == 1 || Level % 7 == 2 || Level % 7 == 3)
                {
                    Jumping((levelFactor + 10 * (Level + 1)) * jumpPower, 80, true, 1);
                }
                else
                {
                    Jumping((levelFactor + Level) * Mathf.Pow(2, jumpPower - 1), 80, true, 1);
                }
                StartCoroutine(Praise(new Color(0.2f, 1, 1, 1), jumpPower + " Combo!", 40, true));
                if (!isTouchBorder)
                    rigid.gravityScale = 10 - (Level % 7);
                gameManager.jumpCnt++;
                gameManager.paraScrollbar.size += 0.2f;
            }
            else
            {
                jumpPower = 0;
                Jumping(20f + Level % 7, 20f, true, 0);
                StartCoroutine(Praise(new Color(0, 0, 0.5f, 1), "Oops!", 35, true));
                gameManager.PlaySound(GameManager.Sfx.OopsJump);
            }
        }
    }

    IEnumerator TimingCatch()
    {
        timingText.gameObject.SetActive(true);
        timingText.text = timeFlow.ToString();
        yield return new WaitForSeconds(0.6f);
        timingText.gameObject.SetActive(false);
    }
    IEnumerator Praise(Color color, string text, int size, bool _jumpable)
    {
        jumpable = false;
        praise.SetActive(true);
        praise.GetComponent<Text>().text = text;
        praise.GetComponent<Text>().color = color;
        praise.GetComponent<Text>().fontSize = size;
        yield return new WaitForSeconds(0.6f);
        jumpable = _jumpable;
        if (!isOverHit)
            praise.SetActive(false);
    }

    void DrawThreeRay()
    {
        Vector2 downvec = rigid.position;
        Vector2 rightvec = new Vector2(rigid.position.x + 0.5f, rigid.position.y);
        Vector2 leftvec = new Vector2(rigid.position.x - 0.5f, rigid.position.y);
        Debug.DrawRay(downvec, Vector2.down, new Color(0, 1, 0));
        Debug.DrawRay(rightvec, Vector2.down, new Color(0, 1, 0));
        Debug.DrawRay(leftvec, Vector2.down, new Color(0, 1, 0));
    }
    void WindBgm()
    {
        if (gameManager.bgmPlayer[0].volume <= 0.3 && isWind)
            gameManager.bgmPlayer[0].volume += Time.deltaTime * 0.02f;
        if (!isWind)
        {
            gameManager.bgmPlayer[0].volume -= Time.deltaTime * 0.04f;
        }
    }
    private void FixedUpdate()
    {
        VelocityRestrict();
        WallRestrict();
        DrawThreeRay();

        if (rigid.position.y > 10 && !para && !isTouchBorder)
        {

            rigid.gravityScale += Time.deltaTime * (1f + (levelCycle + 1));
        }
        if (isTouchBorder)
            rigid.gravityScale = 3;
        if (rigid.velocity.y < 0)
        {
            isWind = true;
            //if (rigid.velocity.y < -0.5f && !para)
            //{
            //rigid.gravityScale += Time.deltaTime * 2;
            //}
            usePara = true;
            camerable = false;
        }
        if (para)
        {
            isWind = false;
            float maxDrag = 10f + (6 - Level % 7);
            float minGravity = 4;
            if (parable <= 0)
            {
                para = false;
                return;
            }
            parable -= Time.deltaTime;
            if (rigid.drag < maxDrag)
                rigid.drag += Time.deltaTime * (1f + (levelCycle + 1));
            else
                rigid.drag = maxDrag;

            if (rigid.gravityScale > minGravity)
                rigid.gravityScale -= Time.deltaTime;
            else
                rigid.gravityScale = minGravity;
        }
        else if (!para)
        {
            ParaHide();
        }

        if (rigid.velocity.y == 0)
        {
            Vector2 downvec = rigid.position;
            Vector2 rightvec = new Vector2(rigid.position.x + 0.5f, rigid.position.y);
            Vector2 leftvec = new Vector2(rigid.position.x - 0.5f, rigid.position.y);
            RaycastHit2D[] rayHits = new RaycastHit2D[3];
            rayHits[0] = Physics2D.Raycast(downvec, Vector2.down, 1, LayerMask.GetMask("Platform"));
            rayHits[1] = Physics2D.Raycast(rightvec, Vector2.down, 1, LayerMask.GetMask("Platform"));
            rayHits[2] = Physics2D.Raycast(leftvec, Vector2.down, 1, LayerMask.GetMask("Platform"));

            if (rayHits[0].collider != null || rayHits[1].collider != null || rayHits[2].collider != null)
            {
                rigid.gravityScale = 10 - (Level % 7);
                redPara.SetActive(false);
                usePara = false;
                para = false;
                timeText.color = timetextColor;
                doubleJump = false;
                trail.enabled = false;
                isWind = false;
                animator.SetBool("isJump", false);
            }
        }
        if (rigid.velocity.y > 0)
        {
            usePara = false;
            isWind = false;
        }
        WindBgm();
        if (transform.localPosition.y < -0.15f)
        {
            animator.SetBool("isJump", true);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

    }
    void VelocityRestrict()
    {
        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
    }
    void WallRestrict()
    {
        if (rigid.velocity.x > 0)
        {
            Vector2 rightvec = new Vector2(rigid.position.x + 1, rigid.position.y);
            RaycastHit2D rayHit = Physics2D.Raycast(rightvec, Vector3.down, 1, LayerMask.GetMask("PlayerWall"));
            if (rayHit.collider != null)
            {
                isWallTouch = true;
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
            else
                isWallTouch = false;
        }
        if (rigid.velocity.x < 0)
        {
            Vector2 leftvec = new Vector2(rigid.position.x - 1, rigid.position.y);
            RaycastHit2D rayHit = Physics2D.Raycast(leftvec, Vector3.down, 1, LayerMask.GetMask("PlayerWall"));
            if (rayHit.collider != null)
            {
                isWallTouch = true;
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
            else
                isWallTouch = false;
        }

    }
    void ParaHide()
    {
        redPara.SetActive(false);
        rigid.drag = 2f;
    }
    public void ParaChute()
    {
        if (Input.GetButtonDown("Fire1") && usePara && !gameManager.isMobile)
        {
            if (parable > 0)
            {
                redPara.SetActive(!redPara.activeSelf);
                para = redPara.activeSelf;
                gameManager.PlaySound(redPara.activeSelf ? GameManager.Sfx.Parachute : GameManager.Sfx.Attach);
            }
        }
    }
    public void ParaChuteMobile()
    {
        if (usePara)
        {
            if (parable > 0)
            {
                redPara.SetActive(!redPara.activeSelf);
                para = redPara.activeSelf;
                gameManager.PlaySound(redPara.activeSelf ? GameManager.Sfx.Parachute : GameManager.Sfx.Attach);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Border")
        {
            isTouchBorder = true;
            rigid.velocity = Vector2.zero;
            finish.GetComponent<BoxCollider2D>().enabled = true;
            finish.GetComponent<SpriteRenderer>().enabled = true;
            Invoke("BorderTouchBack", 2f);
        }
        if (colliders.gameObject.tag != "Background")
        {

            StartCoroutine(AttachSound());
        }
    }
    void BorderTouchBack()
    {
        isTouchBorder = false;
        rigid.gravityScale = 10 - (Level % 7);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            finish.GetComponent<BoxCollider2D>().enabled = false;
            finish.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.SetActive(false);
            gameManager.PlaySound(GameManager.Sfx.Finish);
            jumpPower = 0;
            LevelUp();
        }
    }
    IEnumerator AttachSound()
    {
        if (isAttach)
            yield break;
        isAttach = true;
        gameManager.PlaySound(GameManager.Sfx.Attach);
        yield return new WaitForSeconds(0.3f);
        isAttach = false;
    }
    void LevelUp()
    {
        isLevelUp = true;
        Level++;
        if (Level == 7 * (levelCycle + 1))
            levelCycle++;
        levelSet(Level % 7);
        StartCoroutine(Fade());
        InitGround();
        Invoke("Reposition", 1f);
    }
    IEnumerator Fade()
    {
        fade.SetActive(true);
        bgManager.SetActive(false);
        gameManager.BackgroundHide();
        gameManager.gameSet.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        border.gameObject.SetActive(false);
        bgManager.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        gameManager.gameSet.SetActive(true);
        gameManager.Background();
        timeFlow = 99;
        finish.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        fade.SetActive(false);
        border.SetActive(true);
        isLevelUp = false;
    }
    void InitGround()
    {
        for (int i = 0; i < initGrounds.Length; i++)
        {
            initGrounds[i].SetActive(false);
        }
        if (Level <= 6)
        {
            initGrounds[Level].SetActive(true);
            initGrounds[Level].transform.position = new Vector2(0, -0.5f);
        }
        else
        {
            initGrounds[6].SetActive(true);
            initGrounds[6].transform.position = new Vector2(0, -0.5f);
        }
    }
    void Reposition()
    {
        VelocityZero();
        if (Level <= 6)
            transform.position = initGrounds[Level].transform.position;
        else
            transform.position = initGrounds[6].transform.position;
    }
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

}
