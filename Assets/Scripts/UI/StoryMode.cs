using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryMode : MonoBehaviour
{
    [SerializeField]int index;
    bool isTyping;

    Coroutine typeCoroutine;

    public GameObject backGround1;
    public GameObject backGround2;

    public List<Image> Speaker_Image;
    public List<Image> Subtitle_Image;
    public Button NextSubtitle_Btn;
    public Button Accpet_Btn;

    private void Awake()
    {
        
    }

    private void Start()
    {
        index = Managers.Instance.isEnd ? 12 : 0;

        Managers.Instance.isStory = true;

        Accpet_Btn.gameObject.SetActive(false);

        isTyping = false;
        typeCoroutine = null;

        NextSubtitle_Btn.onClick.AddListener(() => 
        {
            OnNextButtonClicked();
        });

        Accpet_Btn.onClick.AddListener(() =>
        {
            MenuUI.Instance.SetGameObjectActiveF();
            SceneManager.LoadScene("ChaJi");
        });
    }

    private void Update()
    {

    }

    /// <summary>
    /// 显示下一段剧情
    /// </summary>
    private void ShowNextSubtitle()
    {
        if(index < Subtitle_Image.Count)
        {
            backGround2.SetActive(index == 8);
            typeCoroutine = StartCoroutine(TypeSubtitle(index));
            index++;
        }
        else
        {
            Managers.Instance.isStory = false;
            MenuUI.Instance.SetGameObjectActive();
            Destroy(gameObject);
            SceneManager.LoadScene("Menu");
        }
    }

    /// <summary>
    /// 字幕滑动效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypeSubtitle(int index)
    {
        isTyping = true;
        HideAll();
        ShowSpeaker();

        Subtitle_Image[index].gameObject.SetActive(true);

        float time = 1.3f;
        float second = 0;

        //字母图片逐渐显示
        while (second < time)
        {
            second += Time.deltaTime;
            Subtitle_Image[index].fillAmount = Mathf.Clamp01(second / time);
            yield return null;
        }

        yield return null;
        isTyping = false;
    }

    /// <summary>
    /// 点击下一段
    /// </summary>
    private void OnNextButtonClicked()
    {
        if (isTyping)
        {
            Debug.Log("打印完了");
            if(typeCoroutine != null)
                StopCoroutine(typeCoroutine);
            Subtitle_Image[index - 1].fillAmount = 1;
            //直接把剧情全部显示
            isTyping = false;
        }
        else
        {
            if (index == 12 && !Managers.Instance.isEnd)
            {
                Managers.Instance.isEnd = true;
                Accpet_Btn.gameObject.SetActive(true);
                NextSubtitle_Btn.interactable = false;
                return;
            }
            ShowNextSubtitle();
        }
    }

    private void ShowSpeaker()
    {
        switch (index) 
        {
            case 0:
            case 1:
                Speaker_Image[0].gameObject.SetActive(true);
                break;
            case 2:
            case 3:
            case 4:
                Speaker_Image[1].gameObject.SetActive(true);
                break;
            case 5:
            case 6:
                Speaker_Image[0].gameObject.SetActive(true);
                break;
            case 7:
                Speaker_Image[2].gameObject.SetActive(true);
                break;
            case 8:
                Speaker_Image[3].gameObject.SetActive(true);
                break;
            case 9:
                Speaker_Image[4].gameObject.SetActive(true);
                break;
            case 10:
                Speaker_Image[5].gameObject.SetActive(true);
                break;
            case 11:
                Speaker_Image[6].gameObject.SetActive(true);
                break;
            case 12:
            case 13:
                Speaker_Image[7].gameObject.SetActive(true);
                break;
        }
    }

    private void HideAll()
    {
        for(int i = 0;i< Speaker_Image.Count; i++)
        {
            Speaker_Image[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < Subtitle_Image.Count; i++)
        {
            Subtitle_Image[i].gameObject.SetActive(false);
        }
    }
}
