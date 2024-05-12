using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DrummerController : MonoBehaviour
{
    DragonBoatMovement movement;

    public Canvas DummerParent;
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

    private void PressDurmEvent(bool Middle)
    {
        UIManager.Instance.ShowBuffList(Middle);
    }
}
