using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileManager : MonoBehaviour
{
    public Color hoverColor;
    public SpriteRenderer render;
    MapManager MM;
    public GameObject organelle;
    private Vector2[] adjacentDirections = new Vector2[] { (Vector3)MathHelpers.DegreeToVector2(-120), Vector2.left , (Vector3)MathHelpers.DegreeToVector2(120) , (Vector3)MathHelpers.DegreeToVector2(60), Vector2.right, (Vector3)MathHelpers.DegreeToVector2(-60), new Vector2(0, 0)};
    public float size;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        MM = MapManager.instance;
    }

    private void OnMouseEnter()
    {
        if (organelle == null)
        {
            if (MM.CanBuild)
            {
                size = MM.GettoBuild().size;
                getNeighbours(size, hoverColor);
                ZoneCheck();
            }
            else
                getNeighbours(size, Color.gray);
        }
        else
            ZoneCheck();
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            if (getNeighbour(adjacentDirections[i] * new Vector2(size, size)).GetComponent<TileManager>().organelle != null)
            {
                getNeighbour(adjacentDirections[i] * new Vector2(size, size)).GetComponent<TileManager>().render.color = new Color32(0, 200, 100, 255);
            }
            else
                getNeighbour(adjacentDirections[i] * new Vector2(size, size)).GetComponent<TileManager>().render.color = Color.black;

        }
    }

    public bool ZoneCheck()
    {
        for (int i = 0; i < getNeighbours(size, Color.red).Count; i++)
        {
            if (getNeighbours(size, Color.red)[i].GetComponent<TileManager>().organelle != null)
            {
                return false;
            }
        }
        getNeighbours(size, hoverColor);
        return true;
    }

    public List<GameObject> getNeighbours(float size, Color color)
    {
        List<GameObject> zone = new List<GameObject>();

        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            if (getNeighbour(adjacentDirections[i] * new Vector2(size, size)) != null)
            {
                getNeighbour(adjacentDirections[i] * new Vector2(size, size)).GetComponent<TileManager>().render.color = color;
                zone.Add(getNeighbour(adjacentDirections[i] * new Vector2(size, size)));
            }

        }

        return zone;
    }

    private GameObject getNeighbour(Vector3 castDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + castDir, new Vector2(0.1f, 0.1f));

        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!MM.CanBuild)
            return;
        if (!ZoneCheck())
            return;
        MM.BuildOn(this);
    }

}
