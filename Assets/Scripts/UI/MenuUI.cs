using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public static MenuUI Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameObjectActive()
    {
        gameObject.SetActive(true);
    }

    public void SetGameObjectActiveF()
    {
        gameObject.SetActive(false);
    }

    public void SwitchScene(string sceneFrom, string sceneTo)
    {
        transform.Find(sceneFrom).gameObject.SetActive(false);
        transform.Find(sceneTo).gameObject.SetActive(true);
    }
}
