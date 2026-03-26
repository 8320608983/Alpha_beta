using Unity.VisualScripting;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    public void HideView()
    {
        this.gameObject.SetActive(false);
    }
    public void ShowView()
    {
        this.gameObject.SetActive(true);
    }

}
