using UnityEngine;

public class Admob : MonoBehaviour
{
    public static Admob instance;
    public bool isReward;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    

}
