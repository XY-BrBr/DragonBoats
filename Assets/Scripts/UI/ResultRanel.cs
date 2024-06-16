using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultRanel : MonoBehaviour
{
    public void ReturnToMain()
    {
        MenuUI.Instance.SetGameObjectActive();
        SceneManager.LoadScene("Menu");
    } 
}
