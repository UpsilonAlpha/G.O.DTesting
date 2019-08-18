using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour
{
    MapManager MM;
    public Organelle ribosome;
    public Organelle mitochondria;
    public GameObject ui;

    private void Start()
    {
        MM = MapManager.instance;
    }

    public void Ribosome()
    {
        MM.SettoBuild(ribosome);
    }

    public void Mitochondria()
    {
        MM.SettoBuild(mitochondria);
    }

    public void Slide()
    {
        ui.SetActive(!ui.activeSelf);
    }
}
