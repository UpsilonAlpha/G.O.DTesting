using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Quark
{
    public string Name;
    public string Flavor;
    public Color Color;

    public Quark(string name, Color color, string flavor)
    {
        Name = name;
        Flavor = flavor;
        Color = color;
    }
}


public class BoardManager : MonoBehaviour
{
    #region Public Variables
    public static BoardManager instance = null;

    public GameObject Hex;  //These are the white hexagons in the background. Good for possible textures and anims in the future
    public GameObject Cell; //This is the actual quark sprite and collider
    public GameObject Board;    //Parent of everything

    public bool IsShifting { get; set; }     //Variable that stops input during the 'fall' animation
    public int Turn = 0; //Turn counter
    public int gridWidth;   //Size of the board
    public int gridHeight;
    public float delay;

    public List<Quark> genOne = new List<Quark>()
    {   new Quark("RedUp", Color.red, "u"),
        new Quark("GreenUp", Color.green, "u"),
        new Quark("BlueUp", Color.blue, "u"),
        new Quark("RedDown", Color.red, "d"),
        new Quark("GreenDown", Color.green, "d"),
        new Quark("BlueDown", Color.blue, "d")
    };
    public List<Quark> genTwo = new List<Quark>()
    {   new Quark("RedStrange", Color.red, "s"),
        new Quark("GreenStrange", Color.green, "s"),
        new Quark("BlueStrange", Color.blue, "s"),
        new Quark("RedCharm", Color.red, "c"),
        new Quark("GreenCharm", Color.green, "c"),
        new Quark("BlueCharm", Color.blue, "c"),
        new Quark("CyanStrange", Color.cyan, "s̄"),
        new Quark("MagentaStrange", Color.magenta, "s̄"),
        new Quark("YellowStrange", Color.yellow, "s̄"),
        new Quark("CyanCharm", Color.cyan, "c̄"),
        new Quark("MagentaCharm", Color.magenta, "c̄"),
        new Quark("YellowCharm", Color.yellow, "c̄")
    };
    public List<Quark> genOneAnti = new List<Quark>()
    {   new Quark("CyanUp", Color.cyan, "ū"),
        new Quark("MagentaUp", Color.magenta, "ū"),
        new Quark("YellowUp", Color.yellow, "ū"),
        new Quark("CyanDown", Color.cyan, "d̄"),
        new Quark("MagentaDown", Color.magenta, "d̄"),
        new Quark("YellowDown", Color.yellow, "d̄")
    };

    public SpriteRenderer[,] cells;    //2D array of cell objects
    #endregion

    #region Utility Functions
    void setHexSizes()
    {
        hexWidth = Hex.GetComponent<Renderer>().bounds.size.x;
        hexHeight = Hex.GetComponent<Renderer>().bounds.size.y;
    }

    //Finds the initial position of sprites
    Vector2 calcInitPos()
    {
        Vector2 initPos;
        initPos = new Vector2(-hexWidth * gridWidth / 2f + hexWidth / 2, gridHeight / 2f * hexHeight / 2);
        return initPos;
    }

    //Positions board objects
    public Vector2 calcWorldCoord(Vector2 gridPos)
    {
        Vector2 initPos = calcInitPos();
        float xoffset = 0;
        float yoffset = 0;
        if (gridPos.y % 2 != 0)
            xoffset = hexWidth / 2;

        float x = initPos.x + xoffset + gridPos.x * hexWidth;
        yoffset = 0.75f;
        float y = initPos.y - gridPos.y * hexHeight * yoffset;
        return new Vector2(x, y);
    }
    #endregion

    #region Private Variables
    private float hexWidth;     //Dimensions of background hexes
    private float hexHeight;
    private bool[] moveFin = new bool[5] { true, true, true, true, true };
    private List<SpriteRenderer[]> rows = new List<SpriteRenderer[]>();
    #endregion

    //Calls game creation functions
    private void Awake()
    {
        //Ensures singelton pattern
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        setHexSizes();
        createBoard();

        rows.Add(new SpriteRenderer[] { cells[0, 0], cells[1, 0], cells[2, 0], cells[3, 0], cells[4, 0], });
        rows.Add(new SpriteRenderer[] { cells[0, 1], cells[1, 1], cells[2, 1], cells[3, 1], cells[4, 1], });
        rows.Add(new SpriteRenderer[] { cells[0, 2], cells[1, 2], cells[2, 2], cells[3, 2], cells[4, 2], });
        rows.Add(new SpriteRenderer[] { cells[0, 3], cells[1, 3], cells[2, 3], cells[3, 3], cells[4, 3], });
        rows.Add(new SpriteRenderer[] { cells[0, 4], cells[1, 4], cells[2, 4], cells[3, 4], cells[4, 4], });

    }

    //Creates board and populates with cells
    void createBoard()
    {
        cells = new SpriteRenderer[gridWidth, gridHeight];      //gives 'cells' height and width
        GameObject boardObject = Board;             //Uses unity to get the board gameobject

        //Loops through all the cells in the grid
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                //Initializing position on board
                Vector2 gridPos = new Vector2(x, y);

                //Positions background hexes in grid form
                GameObject thisHex = (GameObject)Instantiate(Hex);
                thisHex.transform.position = calcWorldCoord(gridPos);
                thisHex.transform.parent = boardObject.transform;

                //Spawns in sprites for the quarks
                GameObject cell = (GameObject)Instantiate(Cell);
                CellManager manager = cell.GetComponent<CellManager>();
                manager.AssignQuark();
                cell.transform.position = calcWorldCoord(gridPos);      //Positions quark sprites
                cell.transform.parent = boardObject.transform;

                cell.transform.Translate(0, 0, -1);
                cells[x, y] = manager.render;     //Adds gameobject to the array
            }
        }

        //Positions board
        boardObject.transform.Translate(0, 1, 0);
        StartCoroutine(ClearAllMatches());
    }

    //Clears all matches on the board
    public IEnumerator ClearAllMatches()
    {
        yield return new WaitForEndOfFrame();
        for (int y = 0; y < gridHeight; y += 2)
        {
            for (int x = 0; x < gridWidth; x += 2)
            {
                cells[x, y].GetComponent<CellManager>().ClearMatch();
            }
        }
        StartCoroutine(ShiftAlLeft());
    }


    //Makes cells fall
    #region Shift Functions
    //Runs RowShiftCo on every cell and waits for it to finish
    public IEnumerator ShiftAlLeft()
    {
        //Waits to show user a match was found
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < rows.Count; i++)        //Loops through all the cells calling the RowShift function
        {
            for (i = 0; i < rows.Count; i++)
            {
                StartCoroutine(RowShiftCo(rows[i], i));
            }
        }
        while (!(moveFin[0] && moveFin[1] && moveFin[2] && moveFin[3] && moveFin[4]))   //Repeats until all the rows have fallen
            yield return null;
        StopAllCoroutines();
    }

    //Adds a delay to the Rowhsift function and runs it until all sprites have fallen
    public IEnumerator RowShiftCo(SpriteRenderer[] line, int index)
    {
        moveFin[index] = false;
        while (RowShift(line))
        {
            yield return new WaitForSeconds(delay);
        }
        moveFin[index] = true;
    }

    //Loops through the row and swaps null sprites to the top
    public bool RowShift(SpriteRenderer[] QuarkRow)
    {
        for (int i = 0; i < QuarkRow.Length - 1; i++)
        {
            //Creates new sprites at the right to fall down
            if (QuarkRow[QuarkRow.Length - 1].sprite == null)
            {
                //QuarkRow[QuarkRow.Length - 1].sprite = genOne[Random.Range(0, genOne.Count)];
            }

            //Checks for a space below the sprite
            if (QuarkRow[i].sprite == null && QuarkRow[i + 1].sprite != null)
            {
                //Swaps the falling sprite with the empty space
                QuarkRow[i].sprite = QuarkRow[i + 1].sprite;
                QuarkRow[i + 1].sprite = null;
                return true;
            }
        }
        return false;
    }
    #endregion

    /*
    
    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleQuarks = new List<Sprite>();
        possibleQuarks.AddRange(quarks);
        if (x > 0)
        {
            possibleQuarks.Remove(cells[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < gridHeight - 1)
        {
            possibleQuarks.Remove(cells[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleQuarks.Remove(cells[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }
        return possibleQuarks[Random.Range(0, possibleQuarks.Count)];
    }
    public IEnumerator FindNullCells()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridWidth; y++)
            {
                if (cells[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftCellsDown(x, y));
                    break;
                }
            }
        }
    }
    public IEnumerator ShiftCellsDown(int y, int xStart, float shiftDelay = 1f)
    {
        IsShifting = true;
        //Creates a list to store the falling cells in the row
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0;
        //Loops through row finding number of spaces
        for (int x = xStart; x < gridWidth; x++)
        {
            SpriteRenderer render = cells[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null)
            {
                nullCount++;
            }
            renders.Add(render);
        }
        //Loops through each space
        for (int i = 0; i < nullCount; i++)
        {
            yield return new WaitForSeconds(shiftDelay);
            //Shifts each sprite dwon
            for (int k = 0; k < renders.Count - 1; k++)
            {
                renders[k].sprite = renders[k + 1].sprite;
                renders[k + 1].sprite = null;
            }
        }
        IsShifting = false;
    }
    */
}