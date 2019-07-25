using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//OKAY COCKSUCKER, DO A WHOLE BOARD MATCH CHECK FUNCTION BY RUNNING CHECKLOOP ON EVERY SECOND CELL ON EVERY SECOND ROW SO X+2 and Y+2
//ALSO YOU HAVE TO DO THE DESTROY AND DROP THING SIDEWAYS, GOODLUCK YOU BASTARD

public class CellManager : MonoBehaviour
{
    private static CellManager previousSelected = null;     //Stores details of first selected cell

    public SpriteRenderer render;       //SpriteRenderer of current cell
    private bool isSelected = false;    //Stores if cell has been clicked

    //List of directions in which an adjacent cell can be found to match with
    private Vector2[] adjacentDirections = new Vector2[] { new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, -1) };
    private IEnumerator check;

    private Sprite sprite1;   //Renderer of first neighbour quark sprite
    private Sprite sprite2;   //Renderer of second neighbour quark sprite

    List<char> flavours = new List<char>(); //A list of quark flavours in the detected quarks
    private int charge = 0; //Total charge of combined quarks in the detected quarks

    public int protons;     //Number of protons matched
    public int neutrons;    //Number of neutrons matched

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
                    ClearMatch();
                    previousSelected.ClearMatch();
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

    //Sends a raycast and returns hit cell
    private GameObject getNeighbour(Vector3 castDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + castDir, new Vector2(0.1f, 0.1f));
        if (hit.collider != null && hit.collider != this.gameObject.GetComponent<Collider2D>())
        {
            return hit.collider.gameObject;
        }
        return null;

    }

    //Sends a raycast to all adjacent cells next to each other to find group of three matches
    private List<GameObject> getNeighbours()
    {
        List<GameObject> neighbours = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            neighbours.Add(getNeighbour(adjacentDirections[i]));
        }
        return neighbours;
    }

    public void ClearMatch()
    {
        try
        {
            if (BoardManager.instance.IsShifting == true)
            {
                return;
            }   //Don't match if animations are occuring

            for (int neighbour = 0; neighbour < 5; neighbour++)
            {
                sprite1 = getNeighbours()[neighbour].GetComponent<SpriteRenderer>().sprite;  //Gets the name of a neighbour
                sprite2 = getNeighbours()[neighbour + 1].GetComponent<SpriteRenderer>().sprite;  //Gets the name of a second common neighbour
                //Ensures that neigbours are there
                if (sprite1 != null && sprite2 != null)
                {

                    if (MatCheck(sprite1, sprite2))     //Checks for matches, and deletes when a match is found, otherwise returns false
                    {
                        StopCoroutine(BoardManager.instance.ClearAllMatches());
                        StartCoroutine(BoardManager.instance.ClearAllMatches());
                        BoardManager.instance.IsShifting = false;
                        return;
                    }
                }
            }
            //Checks last and first neighbour
            sprite1 = getNeighbours()[0].GetComponent<SpriteRenderer>().sprite;
            sprite2 = getNeighbours()[5].GetComponent<SpriteRenderer>().sprite;
            if (sprite1 != null && sprite2 != null)
            {
                if (MatCheck(sprite1, sprite2))
                {
                    StopCoroutine(BoardManager.instance.ClearAllMatches());
                    StartCoroutine(BoardManager.instance.ClearAllMatches());
                    BoardManager.instance.IsShifting = false;
                    return;
                }
            }
            BoardManager.instance.IsShifting = false;
        }
        catch (ExecutionEngineException e)
        {
            print(e);
        }
    }

    public bool MatCheck(Sprite sprite1, Sprite sprite2)
    {     
        //Makes an array of the neighbour's flavours
        char[] flavourarray = { render.sprite.name[render.sprite.name.Length - 1], sprite1.name[sprite1.name.Length - 1], sprite2.name[sprite2.name.Length - 1] };
        flavours.AddRange(flavourarray);
        //Makes sure all the colors charges are different (so red green and blue)
        if (sprite1.name[0] != render.sprite.name[0] && sprite2.name[0] != render.sprite.name[0] && sprite2.name[0] != sprite1.name[0])
        {
            //Checks if there is at least one of each flavour (i.e. there is at least one up or down quark in a triplet)
            if (flavours.Contains('p') && flavours.Contains('n'))
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
                    neutrons++;
                    sprite1 = null;
                    sprite2 = null;
                    render.sprite = null;
                    BoardManager.instance.IsShifting = true;
                    charge = 0;
                    flavours = new List<char>();
                    return true;
                }
                if (charge == 1)
                {
                    protons++;
                    sprite1 = null;
                    sprite2 = null;
                    render.sprite = null;
                    BoardManager.instance.IsShifting = true;
                    charge = 0;
                    flavours = new List<char>();
                    return true;
                }
                charge = 0;
                flavours = new List<char>();
                return false;
            }
            charge = 0;
            flavours = new List<char>();
            return false;
        }
        charge = 0;
        flavours = new List<char>();
        return false;
    }

    public void SwapSprite(SpriteRenderer render2)
    {
        if (render.sprite == render2.sprite)
        {
            return;
        }

        Sprite tempSprite = render2.sprite;
        render2.sprite = render.sprite;
        render.sprite = tempSprite;
    }

    private void Select()
    {
        isSelected = true;
        previousSelected = gameObject.GetComponent<CellManager>();
        previousSelected.render.color = Color.grey;
    }

    private void Deselect()
    {
        isSelected = false;
        previousSelected.render.color = Color.white;
        previousSelected = null;
    }
}
