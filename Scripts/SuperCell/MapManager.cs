using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MapManager : MonoBehaviour
{
    public GameObject BorderHex;
    public GameObject Map;
    public static MapManager instance;
    public Genome genome;

    private Organelle toBuild;

    public int mapWidth;
    public int mapHeight;

    public float OffsetX = 1.8f;
    public float OffsetY = 1.565f;

    public bool CanBuild { get { return toBuild != null; } }

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        
        CreateMap();
    }


    void CreateMap()
    {
        for (int x = 0; x <= mapWidth; x++)
        {
            for (int y = 0; y <= mapHeight; y++)
            {
                GameObject tile = Instantiate(BorderHex);

                if(y%2 == 0)
                {
                    tile.transform.position = new Vector2((x - 10) * OffsetX, (y-7) * OffsetY);
                }
                else
                {
                    tile.transform.position = new Vector2((x - 10) * OffsetX + OffsetX / 2, (y-7) * OffsetY);
                }

                tile.transform.parent = Map.transform;
                tile.name = x.ToString() + ", " + y.ToString();
                tile.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }


    public Organelle GettoBuild()
    {
        return toBuild;
    }

    public void SettoBuild(Organelle organelle)
    {
        toBuild = organelle;
        genome.Slide();
    }

    public void BuildOn(TileManager tile)
    {
        GameObject organelle = Instantiate(toBuild.prefab, tile.transform);
        tile.organelle = organelle;
    }

    public void FollowCursor(Organelle organelle)
    {
        organelle.prefab.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        organelle.prefab.transform.position += new Vector3(0, 0, 10);
    }
}