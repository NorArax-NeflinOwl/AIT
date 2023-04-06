using UnityEngine;

public class GameInstance : MonoBehaviour
{
    private static GameInstance instance;

    private float timer;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public static GameInstance Instance
    {
        get
        {
            return instance;
        }
    }
}
