using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject BorderHex;
    public GameObject Map;
    public static MapManager instance;
    public Genome genome;
    public GameObject temp;
    public Text ATPText;
    private Organelle toBuild;

    public int mapWidth;
    public int mapHeight;

    public float OffsetX;
    public float OffsetY;

    public bool CanBuild { get { return toBuild != null; } }
    private float atp;
    private float proteins;

    public float ATP
    {
        get {return atp;}
        set
        {
            atp = value;
            ATPText.text = "ATP: " + atp.ToString();
        }
    }
    public float Proteins;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        
        CreateMap();
    }

    public void Update()
    {
        if (CanBuild)
        {
            FollowCursor(temp);
        }
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
                    tile.transform.position = new Vector3((x - 10) * OffsetX, (y-7) * OffsetY, 1);
                }
                else
                {
                    tile.transform.position = new Vector3((x - 10) * OffsetX + OffsetX / 2, (y-7) * OffsetY, 1);
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
        Destroy(temp);
        temp = Instantiate(toBuild.prefab);
        temp.transform.localScale = temp.transform.localScale * new Vector2(0.3f, 0.3f);
    }

    public void BuildOn(TileManager tile)
    {
        GameObject organelle = Instantiate(toBuild.prefab, tile.transform);
        for (int i = 0; i < tile.getNeighbours(toBuild.size, new Color32(0, 200, 100, 255)).Count; i++)
        {
            tile.getNeighbours(toBuild.size, new Color32(0, 200, 100, 255))[i].GetComponent<TileManager>().organelle = organelle;
        }
    }

    public void FollowCursor(GameObject temp)
    {
        temp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.transform.position += new Vector3(0, 0, 10);
    }
}