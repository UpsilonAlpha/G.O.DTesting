using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proton : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D hit)
    {
        Destroy(gameObject);
    }
}
