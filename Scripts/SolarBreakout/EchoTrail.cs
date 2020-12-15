using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoTrail : MonoBehaviour
{
    public float EchoTime;
    private float echoTime;

    public Object echo;
    
    void Update()
    {
        if(echoTime <= 0)
        {
            Instantiate(echo, transform.position, Quaternion.identity);
            echoTime = EchoTime;
        }
        else
        {
            echoTime -= Time.deltaTime;
        }
        
    }
}
