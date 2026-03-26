using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardScript : MonoBehaviour
{

    [SerializeField] GameObject frontSide;
    [SerializeField] GameObject backSide;

    [SerializeField] Image img_Original;
    [SerializeField] Button btnClick;

    internal bool isfliped;
    private float flipTime = 0.15f;

    private void OnEnable()
    {
        btnClick.onClick.RemoveAllListeners();
        btnClick.onClick.AddListener(OnClick);
    }
    public void Setup()
    { 
        frontSide.SetActive(false);
        backSide.SetActive(true); 
        isfliped = false; 
    }

    public void OnClick()
    {
        if (isfliped)
        {
            FlipBack();
            return;
        }
        StartCoroutine(Flip(true));
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
}
