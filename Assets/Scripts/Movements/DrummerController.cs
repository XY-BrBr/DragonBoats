using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DrummerController : MonoBehaviour
{
    DragonBoatMovement movement;

    public Button DummerCenter_Btn;
    public Button DummerEdge_Btn;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<DragonBoatMovement>();

        DummerCenter_Btn.onClick.AddListener(() => { PressDurmEvent(true); });
        DummerEdge_Btn.onClick.AddListener(() => { PressDurmEvent(false); });
    }

    // Update is called once per frame
    void Update()
    {
        DummerCenter_Btn.interactable = !GameManager.Instance.getBuff;
        DummerEdge_Btn.interactable = !GameManager.Instance.getBuff;
    }

    private void PressDurmEvent(bool isMiddle)
    {
        if (GameManager.Instance.canBuff && isMiddle)
        {
            StartCoroutine(GetBuffLastTime());
            GameManager.Instance.canBuff = false;
        }

        if(!GameManager.Instance.canBuff)
        {
            UIManager.Instance.ShowBuffList(isMiddle);
        }
    }

    IEnumerator GetBuffLastTime()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.getBuff = true;
        UIManager.Instance.ShowBuff();
        yield return new WaitForSeconds(2);
        GameManager.Instance.buffList = new int[15];
        GameManager.Instance.nowBuffPoint = 0;
        GameManager.Instance.getBuff = false;
        GameManager.Instance.canBuff = true;
        UIManager.Instance.HideBuff();
        yield break;
    }
}
