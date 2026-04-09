using UnityEngine;
using TMPro;

public class WindIndicator : MonoBehaviour
{
    public GameObject arrowLeft;
    public GameObject arrowRight;
    public TMP_Text label;

    void Update()
    {
        float wind = WindSystem.windForce;

        arrowLeft.SetActive(wind < 0f);
        arrowRight.SetActive(wind > 0f);
    }
}