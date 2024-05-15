using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BoatManController : MonoBehaviour
{
    private DragonBoatMovement movement;

    public List<Animator> animators;

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
        for (int i = 0; i < animators.Count; i++) animators[i].SetTrigger("DoAnim");

        movement.GetAcceleration();
    }
}
