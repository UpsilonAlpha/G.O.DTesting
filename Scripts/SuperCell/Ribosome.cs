using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ribosome : MonoBehaviour
{
    public MapManager MM;
    public float delay;

    void Start()
    {
        MM = MapManager.instance;
        InvokeRepeating("GenerateProtein", delay, delay);
    }

    public void GenerateProtein()
    {
        MM.Proteins++;
        Debug.Log(MM.Proteins.ToString());
    }
}
