using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class GachaUI : MonoBehaviour
{
    public string xmlFile = "XML/name_list";
    public XmlNodeList allNodeList;

    //�̱� ������ ����Ʈ
    public List<GachaItem> gachaItems = new List<GachaItem>();

    private GachaState gachaState;

    public Text realName;
    public Text fakeName;

    public GameObject startButton;
    public GameObject stopButton;
    public GameObject nextButton;

    public Text infoText;

    public Animation nameParent;

    public float scrollTimer = 0.1f;
    private float countdown = 0.0f;

    private int winnerIndex = 0;

    private int scrollCount = 0;
    public int dummyNumber = 4;
    public int fakeNumber = 1;

    private int nowIndex = 0;
    //private int startIndex = 0;

    public string readyText;

    public GachaManger gachaManger;


    private void Start()
    {
        LoadXmlFile(xmlFile);
    }

    private void Update()
    {
        switch (gachaState)
        {
            case GachaState.Scroll_1:
                PlayAnimation();
                break;

            case GachaState.Scroll_2:
                PlayAnimation();
                break;

            case GachaState.Scroll_3:
                PlayAnimation();
                break;

            case GachaState.Result:
                break;
        }
    }

    //���Ͽ� �ִ� �����͸� �о gachaItems�� ����
    private void LoadXmlFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        allNodeList = xmlDoc.SelectNodes("root/item");

        foreach (XmlNode node in allNodeList)
        {
            GachaItem item = new GachaItem();
            item.number = int.Parse(node["number"].InnerText);
            item.name = node["name"].InnerText;
            item.rate = int.Parse(node["rate"].InnerText);

            gachaItems.Add(item);
        }
    }

    public void ResetGachaItem()
    {
        gachaItems.Clear();

        foreach (XmlNode node in allNodeList)
        {
            GachaItem item = new GachaItem();
            item.number = int.Parse(node["number"].InnerText);
            item.name = node["name"].InnerText;
            item.rate = int.Parse(node["rate"].InnerText);

            gachaItems.Add(item);
        }
    }

    public void ReadyAnimation(bool isStart)
    {
        gachaState = GachaState.Ready;

        realName.text = readyText;
        fakeName.text = readyText;

        startButton.SetActive(isStart);

        infoText.text = "���� ��ư�� ��������";
    }

    public void StartAnimation()
    {
        gachaState = GachaState.Scroll_1;

        scrollTimer = 0.1f;
        countdown = 0f;

        startButton.SetActive(false);
        stopButton.SetActive(true);
        gachaManger.resetButton.SetActive(false);

        infoText.text = "���� ��ư�� ��������";
    }

    public void StopAnimation()
    {
        //
        if (gachaManger.gachaType == GachaType.name)
            gachaManger.CreatWinnerSlot();

        //�̱� ����
        winnerIndex = GetGachaItem();
        fakeNumber = Random.Range(1, 4);
        nowIndex = winnerIndex - (dummyNumber + fakeNumber);
        if (nowIndex < 0)
            nowIndex += gachaItems.Count;

        //��÷ �������� ����ġ�� ����
        gachaItems[winnerIndex].rate = 0;

        gachaState = GachaState.Scroll_2;

        scrollTimer = 0.5f;
        countdown = 0f;

        scrollCount = 0;

        stopButton.SetActive(false);

        infoText.text = "";
    }

    private void GotoScroll_3()
    {
        gachaState = GachaState.Scroll_3;

        scrollTimer = 2f;
        countdown = 0f;

        scrollCount = 0;
    }

    private void GotoResult()
    {
        gachaState = GachaState.Result;

        nextButton.SetActive(true);

        infoText.text = gachaItems[winnerIndex].name + " ��÷ �Ǿ����ϴ�";
    }

    public void Next()
    {
        nextButton.SetActive(false);

        gachaManger.NextGacha(gachaItems[winnerIndex].name);
    }

    private void PlayAnimation()
    {
        if (countdown <= 0)
        {
            //���๮
            switch (gachaState)
            {
                case GachaState.Scroll_1:
                    nameParent.Stop("NameScroll01");
                    nameParent.Play("NameScroll01");
                    break;

                case GachaState.Scroll_2:
                    nameParent.Stop("NameScroll02");
                    if (scrollCount == dummyNumber)
                    {
                        GotoScroll_3();
                        return;
                    }
                    nameParent.Play("NameScroll02");
                    break;

                case GachaState.Scroll_3:
                    nameParent.Stop("NameScroll03");
                    if (scrollCount == fakeNumber)
                    {
                        GotoResult();
                        return;
                    }
                    nameParent.Play("NameScroll03");
                    break;
            }
            countdown = scrollTimer;

            scrollCount++;

            nowIndex++;
            if (nowIndex >= gachaItems.Count)
                nowIndex = 0;

            if (nowIndex == 0)
            {
                realName.text = gachaItems[0].name;
                fakeName.text = gachaItems[gachaItems.Count - 1].name;
            }
            else
            {
                realName.text = gachaItems[nowIndex].name;
                fakeName.text = gachaItems[nowIndex - 1].name;
            }

        }
        countdown -= Time.deltaTime;
    }

    private int GetGachaItem()
    {
        int result = 0;

        //gachaItems
        int total = 0;
        for (int i = 0; i < gachaItems.Count; i++)
        {
            total += gachaItems[i].rate;
        }

        int rand = Random.Range(0, total);

        total = 0;
        for (int i = 0; i < gachaItems.Count; i++)
        {
            total += gachaItems[i].rate;

            if (rand < total)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}

public enum GachaState
{
    Ready, //��ŸƮ ��ư �����⸦ ��ٸ��� ����
    Scroll_1, //��ŸƮ ��ư ������ ������ �������� �����ִ� ����, ������ ��ٸ��� ����
    Scroll_2, //���� ������ ���� ����, ���� ��ư�� ���� ����, ��ũ���� �ӵ� ����
    Scroll_3, //���信 ��������鼭 �ִϰ� ���ߴ� �����ϴ� ����
    Result, //������ �����ش�
}