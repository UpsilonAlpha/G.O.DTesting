using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementDetails
{
    public string Name;
    public int Number;
    public Color32 ElementColor;
}

public class Element : MonoBehaviour
{
    public static Element instance;
    public ElementDetails[] elementDetails;

    private void Awake()
    {
        instance = this;
    }
}
