using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BoatManController : MonoBehaviour
{
    private DragonBoatMovement movement;

    //ÒÆ¶¯¶Ë°´Å¥
    public Canvas ShiperParent;
    public Button ShiperTowards_Btn;
    public Button ShiperOrder_Btn;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<DragonBoatMovement>();

        ShiperTowards_Btn.onClick.AddListener(() => 
        {
            MoveTowards();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoveTowards()
    {
        movement.GetAcceleration();
    }
}
