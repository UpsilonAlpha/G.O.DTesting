using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour
{
    Vector2 offset;

    void OnMouseDown()
    {
        offset = (Vector2)gameObject.transform.position - GetMouseWorldPos();
        GetComponent<Collider2D>().isTrigger = true;
    }

    Vector2 GetMouseWorldPos()
    {
        Vector2 mousePoint = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        GetComponent<Collider2D>().isTrigger = false;
    }
}
