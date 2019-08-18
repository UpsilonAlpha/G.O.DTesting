using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float panSpeed;
    public float zoomSpeed;

    void Update()
    {
        if (Input.GetKey("w"))
            transform.Translate(Vector2.up * panSpeed);
        if (Input.GetKey("a"))
            transform.Translate(Vector2.left * panSpeed);
        if (Input.GetKey("s"))
            transform.Translate(Vector2.down * panSpeed);
        if (Input.GetKey("d"))
            transform.Translate(Vector2.right * panSpeed);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        GetComponent<Camera>().orthographicSize -= scroll * 30 * zoomSpeed;

    }
}
