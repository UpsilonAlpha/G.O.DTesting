using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photon : MonoBehaviour
{
    public float photonInitVel;

    private Rigidbody2D rb;
    private bool photonReleased;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && photonReleased == false)
        {
            transform.parent = null;
            photonReleased = true;
            rb.isKinematic = false;
            rb.AddForce(new Vector2(photonInitVel, photonInitVel));
        }
    }
}
