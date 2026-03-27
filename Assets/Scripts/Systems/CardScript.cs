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
        GameEvents.OnFlip?.Invoke();
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
        StartCoroutine(MatchAnimation());
    }
    IEnumerator MatchAnimation()
    {
        float duration = 0.2f;
        float t = 0;

        Vector3 start = Vector3.one;
        Vector3 target = Vector3.one * 1.1f;

        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(start, target, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = target;
        t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(target, start, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one;

        float fadeTime = 0.2f;
        t = 0;

        while (t < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        btnClick.interactable = false;
    }
}
