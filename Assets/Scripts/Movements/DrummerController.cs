using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrummerController : MonoBehaviour
{
    DragonBoatMovement movement;

    public Animator anim;

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
        DummerCenter_Btn.interactable = !movement.getBuff;
        DummerEdge_Btn.interactable = !movement.getBuff;
    }

    /// <summary>
    /// ÇÃ»÷¹ÄÊÂ¼þ
    /// </summary>
    /// <param name="isMiddle"></param>
    private void PressDurmEvent(bool isMiddle)
    {
        string str = isMiddle ? "DoRight" : "DoLeft";

        movement.DrummerTest(isMiddle);

        UIManager.Instance.ShowBuffList(isMiddle);
        anim.SetTrigger(str);
    }

}
