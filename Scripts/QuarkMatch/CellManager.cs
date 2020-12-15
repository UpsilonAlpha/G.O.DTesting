using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
//OKAY COCKSUCKER, DO A WHOLE BOARD MATCH CHECK FUNCTION BY RUNNING CHECKLOOP ON EVERY SECOND CELL ON EVERY SECOND ROW SO X+2 and Y+2

//MAKE STUFF FALL IN THE DIRECTION THE PLAYER CLICKS!!!

<<<<<<< Updated upstream
=======
//Make the check function return something if it doesn't find any matches. Recursiony.

=======
>>>>>>> Stashed changes
>>>>>>> Stashed changes
public class CellManager : MonoBehaviour
{
    public SpriteRenderer render;       //SpriteRenderer of current cell

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

    BoardManager bm = BoardManager.instance;
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
    public bool SwapSprite(SpriteRenderer render2)
    {
        if (render.sprite == render2.sprite)
        {
            return false;
        }

        Sprite tempSprite = render2.sprite;
        render2.sprite = render.sprite;
        render.sprite = tempSprite;
        return AnhilationAndDecay(render, render2) || AnhilationAndDecay(render2, render);
    }

    bool AnhilationAndDecay(SpriteRenderer render, SpriteRenderer render2)
    {
        string quark = render.sprite.name;

        if (quark[0] == 'R' && render2.sprite.name[0] == 'C')
        {
            if (quark[quark.Length - 1] == render2.sprite.name[render2.sprite.name.Length - 1])
            {
                render2.sprite = null;
                render.sprite = null;
                return true;
            }
        }
        if (quark[0] == 'G' && render2.sprite.name[0] == 'M')
        {
            if (quark[quark.Length - 1] == render2.sprite.name[render2.sprite.name.Length - 1])
            {
                render2.sprite = null;
                render.sprite = null;
                return true;
            }
        }
        if (quark[0] == 'B' && render2.sprite.name[0] == 'Y')
        {
            if (quark[quark.Length - 1] == render2.sprite.name[render2.sprite.name.Length - 1])
            {
                render2.sprite = null;
                render.sprite = null;
                return true;
            }
        }

        if (quark[quark.Length - 1] == 'm')
        {
            switch (quark)
            {
                case "RedCharm":
                    render.sprite = bm.genTwo[0];
                    break;
                case "GreenCharm":
                    render.sprite = bm.genTwo[1];
                    break;
                case "BlueCharm":
                    render.sprite = bm.genTwo[2];
                    break;
                case "CyanCharm":
                    render.sprite = bm.genTwo[3];
                    break;
                case "MagentaCharm":
                    render.sprite = bm.genTwo[4];
                    break;
                case "YellowCharm":
                    render.sprite = bm.genTwo[5];
                    break;
            }
        }
        if (quark[quark.Length - 1] == 'e')
        {
            switch (quark)
            {
                case "RedStrange":
                    render.sprite = bm.genOne[0];
                    break;
                case "GreenStrange":
                    render.sprite = bm.genOne[1];
                    break;
                case "BlueStrange":
                    render.sprite = bm.genOne[2];
                    break;
                case "CyanStrange":
                    render.sprite = bm.genOneAnti[0];
                    break;
                case "MagentaStrange":
                    render.sprite = bm.genOneAnti[1];
                    break;
                case "YellowStrange":
                    render.sprite = bm.genOneAnti[2];
                    break;
            }
        }

        return false;
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
        if (render.sprite == null || bm.IsShifting)
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
                    
                    //Checks both cells that were swapped
                    if (!SwapSprite(previousSelected.render) && !previousSelected.ClearMatch())
                        ClearMatch();

                    StartCoroutine(bm.ShiftAlLeft());

                    previousSelected.Deselect();

                    bm.Turn--;
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
        if (bm.IsShifting == true)
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
                        bm.IsShifting = false;
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
                    bm.IsShifting = false;
                    return true;
                }
            }
        }
        bm.IsShifting = false;
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

        if (baryon || antibaryon)
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
<<<<<<< Updated upstream
                        bm.neutrons++;
=======
<<<<<<< Updated upstream
                        neutrons++;
>>>>>>> Stashed changes
                    else
                        bm.neutrons--;
                    sprite1.sprite = null;
                    sprite2.sprite = null;
                    render.sprite = null;
<<<<<<< Updated upstream
                    bm.IsShifting = true;
=======
                    BoardManager.instance.IsShifting = true;
=======
                        bm.Neutrons++;
                    else
                        bm.Neutrons--;

                    sprite2.transform.parent.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("WhiteFlash"));
                    render.transform.parent.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("WhiteFlash"));
                    sprite1.transform.parent.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("WhiteFlash"));

                    sprite1.sprite = null;
                    sprite2.sprite = null;
                    render.sprite = null;

                    bm.IsShifting = true;
>>>>>>> Stashed changes
>>>>>>> Stashed changes
                    Reset();
                    return true;
                }
                if (charge == 1)
                {
                    if (baryon)
<<<<<<< Updated upstream
                        bm.protons++;
                    else
                        bm.protons--;
=======
<<<<<<< Updated upstream
                        protons++;
                    else
                        protons--;
=======
                        bm.Protons++;
                    else
                        bm.Protons--;

                    sprite2.transform.parent.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("WhiteFlash"));
                    render.transform.parent.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("WhiteFlash"));
                    sprite1.transform.parent.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("WhiteFlash"));

>>>>>>> Stashed changes
>>>>>>> Stashed changes
                    sprite1.sprite = null;
                    sprite2.sprite = null;
                    render.sprite = null;
                    bm.IsShifting = true;
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