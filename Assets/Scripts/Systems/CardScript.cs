using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardScript : MonoBehaviour
{

    [SerializeField] GameObject frontSide;
    [SerializeField] GameObject backSide;

    [SerializeField] Image img_Original;
    [SerializeField] Button btnClick;

    [SerializeField] CanvasGroup canvasGroup; 
    internal int ID => data.id;

    private bool isfliped; 
    private bool isMatched;
    private bool isActive = true;
    private float flipTime = 0.15f;
    
    private CardData data;

    private void OnEnable()
    {
        btnClick.onClick.RemoveAllListeners();
        btnClick.onClick.AddListener(OnClick);
    }
    public void Setup(CardData cardData)
    {
        data = cardData;
        img_Original.sprite = data.sprite;
        frontSide.SetActive(false);
        backSide.SetActive(true); 
        isfliped = false; isActive = true;
    }

    public void OnClick()
    {
        if (!isActive || isfliped || isMatched)
            return; 
        StartCoroutine(Flip(true));
        MatchManager.Instance.RegisterCard(this);
    }
    IEnumerator Flip(bool showFront)
    {
        float time = 0; 
        while (time < flipTime)
        {
            transform.localScale = new Vector3(Mathf.Lerp(1, 0, time / flipTime), 1, 1);
            time += Time.deltaTime;
            yield return null;
        } 
        frontSide.SetActive(showFront);
        backSide.SetActive(!showFront); 
        
        time = 0; 
        while (time < flipTime)
        {
            transform.localScale = new Vector3(Mathf.Lerp(0, 1, time / flipTime), 1, 1); 
            time += Time.deltaTime;
            yield return null;
        }
        isfliped = showFront;

        transform.localScale = Vector3.one; 
    }
    public void FlipBack()
    {
        StartCoroutine(Flip(false));
    }
    public void MatchSuccess()
    {
        isMatched = true;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        btnClick.interactable = false;
    }
}
