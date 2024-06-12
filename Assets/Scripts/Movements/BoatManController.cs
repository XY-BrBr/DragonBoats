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

    public Button TurnAround_Btn;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<DragonBoatMovement>();

        ShiperTowards_Btn.onClick.AddListener(() => 
        {
            MoveTowards();
        });

        ShiperOrder_Btn.onClick.AddListener(() =>
        {
            SlowDown();
        });

        TurnAround_Btn.onClick.AddListener(() => 
        {
            movement.TurnAround();
        });
    }

    // Update is called once per frame
    void Update()
    {
        TurnAround_Btn.gameObject.SetActive(movement.canTurn);
    }

    private void MoveTowards()
    {
        animators.AnimaSetTrigger("DoAnim");

        movement.GetAcceleration();
    }

    private void SlowDown()
    {
        //animators.AnimaSetTrigger("DoAnim");

        movement.SlowDown();
    }
}
