using UnityEngine;
using UnityEngine.UI;

public class ForceIndicator : MonoBehaviour
{
    [Header("References")]
    public Image arcFill;


    void Start()
    {
        arcFill.fillAmount = 0f;
    }

    public void SetForce(float current, float min, float max)
    {
        float normalized = Mathf.InverseLerp(min, max, current);
        arcFill.fillAmount = normalized * 0.4f;
    }

    public void Hide()
    {
        arcFill.fillAmount = 0f;
    }
}