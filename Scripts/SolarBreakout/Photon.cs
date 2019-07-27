using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photon : MonoBehaviour
{
    public float c;

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
            rb.velocity = Vector2.up * c;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Platform")
        {
            float x = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.x);
            Vector2 dir = new Vector2(x, 1).normalized;
            rb.velocity = dir * c;
        }
    }

    float hitFactor(Vector2 photonPos, Vector2 platformPos, float platformWidth)
    {
        return (photonPos.x - platformPos.x) / platformWidth;
    }
}
