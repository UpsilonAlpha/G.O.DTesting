using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitochondria : MonoBehaviour
{
    public MapManager MM;
    public float delay;

    void Start()
    {
         MM = MapManager.instance;
         InvokeRepeating("GenerateATP", delay, delay);
    }

    public void GenerateATP()
    {
        MM.ATP++;
        Debug.Log(MM.ATP.ToString());
    }
}
