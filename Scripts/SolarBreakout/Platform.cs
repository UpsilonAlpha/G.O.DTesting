using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;

    private Vector2 platypos = new Vector2(0, -4);

    void Update()
    {
        float xPos = transform.position.x + Input.GetAxisRaw("Horizontal") * speed;
        platypos = new Vector2(Mathf.Clamp(xPos, -5.3f, 5.3f), -4);
        transform.position = platypos;
    }
}
