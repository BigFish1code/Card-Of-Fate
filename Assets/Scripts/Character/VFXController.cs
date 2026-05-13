using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject Getbuff, GetDebuff;
    private float timeCounter;

    private void Update()
    {
        if (Getbuff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if(timeCounter >= 1.2f)
            {
                timeCounter = 0f;
                Getbuff.SetActive(false);
            }
        }
        if (GetDebuff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f)
            {
                timeCounter = 0f;
                GetDebuff.SetActive(false);
            }
        }
    }
}
