using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpFollow : MonoBehaviour
{
    public GameObject photon;
    void Update()
    {
        LeanTween.moveY(gameObject, photon.transform.position.y-3, 0f);
    }
}
