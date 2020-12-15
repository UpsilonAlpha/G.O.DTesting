using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonGenerator : MonoBehaviour
{
    List<GameObject> Objects = new List<GameObject>();
    [SerializeField]
    public GameObject proton;
    public GameObject electron;
    public BoxCollider2D col;
    public int offset;
    public int density;
    float MaxX;
    float MinX;
    float MaxY;
    float MinY;
    Vector2 spawnPos1;
    Vector2 spawnPos2;
    Vector2 spawnPos3;
    Vector2 spawnPos4;

    void Start()
    {
        float size = Camera.main.GetComponent<Camera>().orthographicSize;
        MaxY = size;
        MinY = -size;
        MaxX = size*Screen.width/Screen.height;
        MinX = -size*Screen.width/Screen.height;
        
        Debug.Log(MaxX);
        Debug.Log(MinX);
        Debug.Log(MaxY);
        Debug.Log(MinY);

        for(int i=0; i < density/4; i++)
        {
            spawnPos1 = new Vector2(Random.Range(MinX, MaxX),Random.Range(MinY, MaxY));
            Objects.Add((GameObject)Instantiate(proton, spawnPos1, Quaternion.Euler(0,0,90)));
            spawnPos1 = new Vector2(Random.Range(MinX, MaxX),Random.Range(MinY, MaxY));
            Objects.Add((GameObject)Instantiate(electron, spawnPos1, Quaternion.identity));
        }
    }
    void Update()
    {
        if (Objects.Count < density)
        {
            float xPos = transform.position.x;
            float yPos = transform.position.y;
            spawnPos1 = new Vector2(Random.Range(MaxX+xPos, MaxX+offset+xPos), Random.Range(MinY+yPos, MaxY+yPos));
            spawnPos2 = new Vector2(Random.Range(MinX+xPos, MinX-offset+xPos), Random.Range(MinY+yPos, MaxY+yPos));
            spawnPos3 = new Vector2(Random.Range(MinX+xPos, MaxX+xPos), Random.Range(MaxY+yPos, MaxY+offset+yPos));
            spawnPos4 = new Vector2(Random.Range(MinX+xPos, MaxX+xPos), Random.Range(MinY+yPos, MinY-offset+yPos));
            Objects.Add((GameObject)Instantiate(proton, spawnPos1, Quaternion.Euler(0,0,90)));
            Objects.Add((GameObject)Instantiate(proton, spawnPos2, Quaternion.Euler(0,0,90)));
            Objects.Add((GameObject)Instantiate(proton, spawnPos3, Quaternion.Euler(0,0,90)));
            Objects.Add((GameObject)Instantiate(proton, spawnPos4, Quaternion.Euler(0,0,90)));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "RedAlphaHex(Clone)")
        {
            Objects.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
        else
        {
            float colY = col.size.y;
            float colX = col.size.x;
            Vector2 pos = other.gameObject.transform.position;
            Vector2 cpos = transform.position;
            if(pos.y >= cpos.y+colY/2)
            {
                pos.y -= colY;
            }
            else if(pos.y < cpos.y-colY/2)
            {
                pos.y += colY;
            }
            else if(pos.x > cpos.x+colX/2)
            {
                pos.x -= colX;
            }
            else if(pos.x < cpos.x-colX/2)
            {
                pos.x += colX;
            }
            other.gameObject.transform.position = pos;
        }
    }
}
