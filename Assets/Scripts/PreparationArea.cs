using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationArea : MonoBehaviour
{
    public Button Head;
    public Button Body;
    public Button Tail;
    public Button GoodBoom;
    public Button Wood;
    public Button Shiper;
    public Button Helmet;
    public Button Drummer;

    public Button Return;

    public Transform Images;

    public Image Finish;

    // Start is called before the first frame update
    void Start()
    {
        Head.onClick.AddListener(() => { Images.GetChild(0).gameObject.SetActive(true); });
        Body.onClick.AddListener(() => { Images.GetChild(1).gameObject.SetActive(true); });
        Tail.onClick.AddListener(() => { Images.GetChild(2).gameObject.SetActive(true); });
        GoodBoom.onClick.AddListener(() => { Images.GetChild(3).gameObject.SetActive(true); });
        Wood.onClick.AddListener(() => { Images.GetChild(4).gameObject.SetActive(true); });
        Shiper.onClick.AddListener(() => { Images.GetChild(5).gameObject.SetActive(true); });
        Helmet.onClick.AddListener(() => { Images.GetChild(6).gameObject.SetActive(true); });
        Drummer.onClick.AddListener(() => { Images.GetChild(7).gameObject.SetActive(true); });

        for (int i = 0; i < Images.childCount; i++)
        {
            int index = i;

            Images.GetChild(index).GetComponent<Button>().onClick.AddListener(() => 
            {
                HideSelf(index);
            });
        }

        Return.onClick.AddListener(() => { MenuUI.Instance.SwitchScene("PreparationArea", "MainScreen"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideSelf(int i)
    {
        StartCoroutine(Show());

        Images.GetChild(i).gameObject.SetActive(false);
    }

    public IEnumerator Show()
    {
        Finish.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Finish.gameObject.SetActive(false);
    }
}
