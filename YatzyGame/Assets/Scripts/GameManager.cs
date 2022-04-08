using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(singleton);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        
    }

    public void OnGameSceneLoad()
    {

    }


}
