using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//OKAY COCKSUCKER, DO A WHOLE BOARD MATCH CHECK FUNCTION BY RUNNING CHECKLOOP ON EVERY SECOND CELL ON EVERY SECOND ROW SO X+2 and Y+2

//MAKE STUFF FALL IN THE DIRECTION THE PLAYER CLICKS!!!

public class CellManager : MonoBehaviour
{
    #region Private Variables
    private static CellManager previousSelected = null;     //Stores details of first selected cell

    private bool isSelected = false;    //Stores if cell has been clicked

    //List of directions in which an adjacent cell can be found to match with
    private Vector2[] adjacentDirections = new Vector2[] { new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, -1) };
    private IEnumerator check;

    private SpriteRenderer sprite1;   //Renderer of first neighbour quark sprite
    private SpriteRenderer sprite2;   //Renderer of second neighbour quark sprite

    List<char> flavours = new List<char>(); //A list of quark flavours in the detected quarks
    List<char> colors = new List<char>();   //A list of the colours of detected quarks
    private int charge = 0; //Total charge of combined quarks in the detected quarks

    bool baryon = false;    //Helps check if the match is matter or antimatter
    bool antibaryon = false;
    #endregion

    #region Public Variables
    public int protons;     //Number of protons matched
    public int neutrons;    //Number of neutrons matched
    public SpriteRenderer render;       //SpriteRenderer of current cell
    #endregion

    #region Utility Functions
    //Stores the clicked quark
    private void Select()
    {
        isSelected = true;
        previousSelected = gameObject.GetComponent<CellManager>();
        previousSelected.render.color = Color.grey;
    }

    //Removes the clicked quark
    private void Deselect()
    {
        isSelected = false;
        previousSelected.render.color = Color.white;
        previousSelected = null;
    }

    //Sends a raycast and returns hit cell
    private GameObject getNeighbour(Vector3 castDir)
    {
        //Fires a ray in the given direction and returns the hit or lack thereof
        RaycastHit2D hit = Physics2D.Raycast(transform.position + castDir, new Vector2(0.1f, 0.1f));
        if (hit.collider != null && hit.collider != this.gameObject.GetComponent<Collider2D>() && hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite != null)
        {
            return hit.collider.gameObject;
        }
        return null;

    }

    //Sends a raycast to all adjacent cells next to each other to find group of three matches
    private List<GameObject> getNeighbours()
    {
        //Loops through all directions and adds quarks to the list
        List<GameObject> neighbours = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            neighbours.Add(getNeighbour(adjacentDirections[i]));
        }
        return neighbours;
    }

    //Switches sprites using temporary variables
    public void SwapSprite(SpriteRenderer render2)
    {
        if (render.sprite == render2.sprite)
        {
            return;
        }

        Sprite tempSprite = render2.sprite;
        render2.sprite = render.sprite;
        render.sprite = tempSprite;
        if(render.sprite.name[render.sprite.name.Length -1] == 'm')
        {
            render.sprite = BoardManager.instance.genTwo[4];
        }
        if (render.sprite.name[render.sprite.name.Length - 1] == 'e')
        {
            render.sprite = BoardManager.instance.genTwo[4];
        }
    }

    public bool Reset()
    {
        charge = 0;
        flavours = new List<char>();
        colors = new List<char>();
        baryon = false;
        antibaryon = false;
        return false;
    }
    #endregion

    public void OnMouseDown()
    {

        //If you missclick or click when animations are happening, ignore it
        if (render.sprite == null || BoardManager.instance.IsShifting)
        {
            return;
        }

        //If clicked cell is already selected, then deselect it
        if (isSelected)
        {
            Deselect();
        }
        else
        {
            //If a cell isn't selected, select the clicked cell
            if (previousSelected == null)
            {
                Select();
            }
            else
            {
                //Checks if cells are next to each other
                if (getNeighbours().Contains(previousSelected.gameObject))
                {
                    //Swaps sprites with the previously selected cell
                    SwapSprite(previousSelected.render);
                    //Checks both cells that were swapped
                    if(!previousSelected.ClearMatch())
                        ClearMatch();
                    StartCoroutine(BoardManager.instance.ShiftAlLeft());
                    

                    previousSelected.Deselect();
                }
                else
                {
                    //If cells aren't adjacent select the new cell
                    previousSelected.GetComponent<CellManager>().Deselect();
                    Select();
                }
            }
        }
    }

    public bool ClearMatch()
    {
        if (BoardManager.instance.IsShifting == true)
        {
            return false;
        }   //Don't match if animations are occuring

        for (int neighbour = 0; neighbour < 5; neighbour++)
        {
            if (getNeighbours()[neighbour] != null && getNeighbours()[neighbour + 1] != null)
            {
                sprite1 = getNeighbours()[neighbour].GetComponent<SpriteRenderer>();  //Gets the name of a neighbour
                sprite2 = getNeighbours()[neighbour + 1].GetComponent<SpriteRenderer>();  //Gets the name of a second common neighbour
                //Ensures that neigbours are there
                if (sprite1.sprite != null && sprite2.sprite != null)
                {

                    if (MatCheck(sprite1, sprite2))     //Checks for matches, and deletes when a match is found, otherwise returns false
                    {
                        BoardManager.instance.IsShifting = false;
                        return true;
                    }
                }
            }
        }
        //Checks last and first neighbour
        if (getNeighbours()[0] != null && getNeighbours()[5] != null)
        {
            sprite1 = getNeighbours()[0].GetComponent<SpriteRenderer>();
            sprite2 = getNeighbours()[5].GetComponent<SpriteRenderer>();
            if (sprite1.sprite != null && sprite2.sprite != null)
            {
                if (MatCheck(sprite1, sprite2))
                {
                    BoardManager.instance.IsShifting = false;
                    return true;
                }
            }
        }
        BoardManager.instance.IsShifting = false;
        return false;
    }

    public bool MatCheck(SpriteRenderer sprite1, SpriteRenderer sprite2)
    {     
        //Makes an array of the neighbour's flavours
        char[] flavourarray = { render.sprite.name[render.sprite.name.Length - 1], sprite1.sprite.name[sprite1.sprite.name.Length - 1], sprite2.sprite.name[sprite2.sprite.name.Length - 1] };
        flavours.AddRange(flavourarray);
        char[] colorarray = { sprite1.sprite.name[0], render.sprite.name[0], sprite2.sprite.name[0] };
        colors.AddRange(colorarray);

        //Makes sure all the colors charges are balanced (e.g. red green and blue)
        if (colors.Contains('R') && colors.Contains('G') && colors.Contains('B'))
            baryon = true;
        if (colors.Contains('C') && colors.Contains('M') && colors.Contains('Y'))
            antibaryon = true;

        if(baryon || antibaryon)
        {
            //Checks if there is at least one of each flavour (i.e. there is at least one up or down quark in a triplet)
            if (flavours.Contains('p') && flavours.Contains('n') && (flavours.Contains('p') || flavours.Contains('n')))
            {
                foreach (char flavour in flavours)
                {
                    //If the triplet has two 'p's it's a proton, if it has two 'n's then it's a neutron
                    if (flavour == 'p')

                        charge++;
                    if (flavour == 'n')
                        charge--;
                }
                if (charge == -1)
                {
                    //Deletes sprites and resets values
                    if (baryon)
                        neutrons++;
                    else
                        neutrons--;
                    sprite1.sprite = null;
                    sprite2.sprite = null;
                    render.sprite = null;
                    BoardManager.instance.IsShifting = true;
                    Reset();
                    return true;
                }
                if (charge == 1)
                {
                    if (baryon)
                        protons++;
                    else
                        protons--;
                    sprite1.sprite = null;
                    sprite2.sprite = null;
                    render.sprite = null;
                    BoardManager.instance.IsShifting = true;
                    Reset();
                    return true;
                }
                return Reset();
            }
            return Reset();
        }
        return Reset();
    }



}
