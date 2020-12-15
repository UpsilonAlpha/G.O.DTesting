using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;


    void Update()
    {
        float xPos = transform.position.x + Input.GetAxisRaw("Horizontal") * speed;
        float yPos = transform.position.y + Input.GetAxisRaw("Vertical") * speed;

        transform.position = new Vector2(xPos, yPos);
    }
}
