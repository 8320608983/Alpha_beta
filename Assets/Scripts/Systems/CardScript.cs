using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardScript : MonoBehaviour
{

    [SerializeField] GameObject frontSide;
    [SerializeField] GameObject backSide;
    [SerializeField] Button btnClick;
    private float flipTime = 0.15f;
    internal bool isfliped;
    private void OnEnable()
    {
        btnClick.onClick.RemoveAllListeners();
        btnClick.onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        if (isfliped)
            return;
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
        transform.localScale = Vector3.one; 
    }
    public void FlipBack()
    {
        StartCoroutine(Flip(false));
    }
}
