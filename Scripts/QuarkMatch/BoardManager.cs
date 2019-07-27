﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance = null;

    public GameObject Hex;  //These are the white hexagons in the background. Good for possible textures and anims in the future
    public GameObject Cell; //This is the actual quark sprite and collider
    public GameObject Board;    //Parent of everything
    public List<Sprite> quarks = new List<Sprite>();    //Lists of quark sprites

    public bool IsShifting { get; set; }     //Variable that stops input during the 'fall' animation
    public int Turn = 0; //Turn counter
    public int gridWidth;   //Size of the board
    public int gridHeight;

    public SpriteRenderer[,] cells;    //2D array of cell objects
    private float hexWidth;     //Dimensions of background hexes
    private float hexHeight;
    private List<SpriteRenderer[]> rows = new List<SpriteRenderer[]>();

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
        rows.Add(new SpriteRenderer[] { cells[0, 0], cells[0, 1], cells[0, 2], cells[0, 3], cells[0, 4], });
        rows.Add(new SpriteRenderer[] { cells[1, 0], cells[1, 1], cells[1, 2], cells[1, 3], cells[1, 4], });
        rows.Add(new SpriteRenderer[] { cells[2, 0], cells[2, 1], cells[2, 2], cells[2, 3], cells[2, 4], });
        rows.Add(new SpriteRenderer[] { cells[3, 0], cells[3, 1], cells[3, 2], cells[3, 3], cells[3, 4], });
        rows.Add(new SpriteRenderer[] { cells[4, 0], cells[4, 1], cells[4, 2], cells[4, 3], cells[4, 4], });

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
                Sprite newSprite = quarks[Random.Range(0, quarks.Count)];       //Gets a random quark sprite
                cell.transform.position = calcWorldCoord(gridPos);      //Positions quark sprites
                cell.transform.parent = boardObject.transform;
                cell.GetComponent<SpriteRenderer>().sprite = newSprite;     //Renders sprites
                cell.transform.Translate(0, 0, -1);
                cell.GetComponent<CellManager>().render = cell.GetComponent<SpriteRenderer>();
                cells[x, y] = cell.GetComponent<SpriteRenderer>();     //Adds gameobject to the array
            }
        }

        //Positions board
        boardObject.transform.Translate(0, 1, 0);
    }

    //Finds empty cells and triggers ShiftCellsDown
    /*public IEnumerator FindNullCells()
    {
        for (int x = 0; x < gridHeight; x++)
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

    public IEnumerator ShiftCellsDown(int x, int yStart, float shiftDelay = 1f)
    {
        IsShifting = true;
        //Creates a list to store the falling cells in the row
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0;

        //Loops through row finding number of spaces
        for (int y = yStart; y < gridWidth; y++)
        {
            SpriteRenderer render = cells[y, x].GetComponent<SpriteRenderer>();
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
    }*/

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

    //Checks all cells for a match
    public void ClearAllMatches()
    {
        for (int y = 0; y < rows.Count; y++)
        {
            while (RowShift(rows[y])) { }
        }
    }

    public bool RowShift(SpriteRenderer[] QuarkRow)
    {
        for (int i = 0; i < QuarkRow.Length-1; i++)
        {
            if(QuarkRow[i].sprite == null && QuarkRow[i+1].sprite != null)
            {
                QuarkRow[i].sprite = QuarkRow[i + 1].sprite;
                QuarkRow[i + 1].sprite = null;
                return true;
            }
        }
        return false;
    }
}