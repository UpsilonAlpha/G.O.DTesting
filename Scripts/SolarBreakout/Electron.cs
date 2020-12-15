using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron : MonoBehaviour
{
    public int e;
    Rigidbody2D rb;
    Vector2[] dirs = {new Vector2(1,1), new Vector2(1,-1), new Vector2(-1,0), new Vector2(-1,-1), new Vector2(-1,1), new Vector2(1,0),};
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = dirs[Random.Range(0,6)].normalized*e;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        rb.velocity = dirs[Random.Range(0,6)].normalized*e;
        if(other.gameObject.name == "Photon")
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = rb.velocity*-2;
        }
    }

    void Update()
    {
        rb.velocity = rb.velocity.normalized*e;
    }
}
