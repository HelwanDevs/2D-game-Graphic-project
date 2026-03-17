using UnityEngine;
using System.Collections;

public class WindSystem : MonoBehaviour
{
    public static float windForce; // all can read (UI too) 

    private float windStrength;

    private float minChangeTime = 5f;
    private float maxChangeTime = 15f;

    private void Start()
    {
        StartCoroutine(WindLoop()); // Rescheduling
    }

    IEnumerator WindLoop()
    {
        while (true)
        {
            WindDirGen(); // wind direction generator 

            float waitTime = Random.Range(minChangeTime, maxChangeTime);

            yield return new WaitForSeconds(waitTime);
        }
    }

    void WindDirGen()
    {
        int windDir = Random.Range(0, 3); // randomize the dir
        windStrength =  Random.Range(1f, 2f); // randomize the sterngth per update 

        switch (windDir)
        {

            case 0:
                windForce = -windStrength; break; // left
            case 1:
                windForce = 0; break; //neutral 
            case 2:
                windForce = windStrength; break; //right 

        }

        Debug.Log("wind force :" + windForce);
    }

}