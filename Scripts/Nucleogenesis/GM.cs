using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtomicNumber
{
    H = 1,
    H2= 2,
    H3 = 3,
    H4 = 4,
    C = 6,
    Ne = 10,
    O = 8,
    Si = 14,
    Fe = 26
}

public enum GameState
{
    Playing,
    Moving,
    GameOver
}

public class GM : MonoBehaviour
{
    public GameState state;
    public float delay;
    private bool moveMade;
    private bool[] lineMoveFin = new bool[5] { true, true, true, true, true };



    private Tile[,] AllTiles = new Tile[5, 5];
    private List<Tile[]> cols = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();
    private List<Tile> EmptyTiles = new List<Tile>();

    public int combo;

    private void Start()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (Tile t in tiles)
        {
            t.Number = 0;
            AllTiles[t.indRow, t.indCol] = t;
            EmptyTiles.Add(t);


        }

        #region Rows and Cols Definition
        cols.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 0], AllTiles[2, 0], AllTiles[3, 0], AllTiles[4, 0], });
        cols.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 1], AllTiles[2, 1], AllTiles[3, 1], AllTiles[4, 1], });
        cols.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 2], AllTiles[2, 2], AllTiles[3, 2], AllTiles[4, 2], });
        cols.Add(new Tile[] { AllTiles[0, 3], AllTiles[1, 3], AllTiles[2, 3], AllTiles[3, 3], AllTiles[4, 3], });
        cols.Add(new Tile[] { AllTiles[0, 4], AllTiles[1, 4], AllTiles[2, 4], AllTiles[3, 4], AllTiles[4, 4], });

        rows.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3], AllTiles[0, 4], });
        rows.Add(new Tile[] { AllTiles[1, 0], AllTiles[1, 1], AllTiles[1, 2], AllTiles[1, 3], AllTiles[1, 4], });
        rows.Add(new Tile[] { AllTiles[2, 0], AllTiles[2, 1], AllTiles[2, 2], AllTiles[2, 3], AllTiles[2, 4], });
        rows.Add(new Tile[] { AllTiles[3, 0], AllTiles[3, 1], AllTiles[3, 2], AllTiles[3, 3], AllTiles[3, 4], });
        rows.Add(new Tile[] { AllTiles[4, 0], AllTiles[4, 1], AllTiles[4, 2], AllTiles[4, 3], AllTiles[4, 4], });
        #endregion

        Generate();
        Generate();
    }

    bool MoveDownIndex(Tile[] LineOfTiles)
    {
        for (int i = 0; i < LineOfTiles.Length - 1; i++)
        {
            //Moving
            if(LineOfTiles[i].Number == 0 && LineOfTiles[i+1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;
                return true;
            }

            //Fusing
            if(LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i + 1].Number)
            {
                LineOfTiles[i].Number++;
                LineOfTiles[i + 1].Number = 0;
                LineOfTiles[i].Merge();
                combo++;
                return true;
            }
        }
        return false;
    }

    bool MoveUpIndex(Tile[] LineOfTiles)
    {
        for (int i = LineOfTiles.Length - 1; i > 0; i--)
        {
            //Moving
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i - 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
                LineOfTiles[i - 1].Number = 0;
                return true;
            }

            //Fusing
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i - 1].Number)
            {
                LineOfTiles[i].Number++;
                LineOfTiles[i - 1].Number = 0;
                LineOfTiles[i].Merge();
                combo++;
                return true;
            }
        }
        return false;
    }

    public void UpdateEmptyTiles()
    {
        EmptyTiles.Clear();
        foreach  (Tile t in AllTiles)
        {
            if(t.Number == 0)
            {
                EmptyTiles.Add(t);
            }
        }
    }

    public void Shift(Dir d)
    {
        if (delay > 0)
        {
            StartCoroutine(ShiftCo(d));
        }
        else
        {
            for (int i = 0; i < rows.Count; i++)
            {
                switch (d)
                {
                    case Dir.Right:
                        while (MoveUpIndex(rows[i])) { moveMade = true; }
                        break;
                    case Dir.Left:
                        while (MoveDownIndex(rows[i])) { moveMade = true; }
                        break;
                    case Dir.Up:
                        while (MoveDownIndex(cols[i])) { moveMade = true; }
                        break;
                    case Dir.Down:
                        while (MoveUpIndex(cols[i])) { moveMade = true; }
                        break;
                }
            }
            if (moveMade)
            {
                UpdateEmptyTiles();
                Generate();
            }
        }
    }

    IEnumerator ShiftCo(Dir d)
    {
        state = GameState.Moving;
        for (int i = 0; i < rows.Count; i++)
        {
            switch (d)
            {
                case Dir.Right:
                    for (i = 0; i < rows.Count; i++)
                        StartCoroutine(MoveUpIndexCo(rows[i], i));
                    break;
                case Dir.Left:
                    for (i = 0; i < rows.Count; i++)
                        StartCoroutine(MoveDownIndexCo(rows[i], i));
                    break;
                case Dir.Up:
                    for (i = 0; i < cols.Count; i++)
                        StartCoroutine(MoveDownIndexCo(cols[i], i));
                    break;
                case Dir.Down:
                    for (i = 0; i < cols.Count; i++)
                        StartCoroutine(MoveUpIndexCo(cols[i], i));
                    break;
            }
        }
        while (!(lineMoveFin[0] && lineMoveFin[1] && lineMoveFin[2] && lineMoveFin[3] && lineMoveFin[4]))
            yield return null;

        if (moveMade)
        {
            UpdateEmptyTiles();
            Generate();
        }
        state = GameState.Playing;
        StopAllCoroutines();
    }

    IEnumerator MoveUpIndexCo(Tile[] line, int index)
    {
        lineMoveFin[index] = false;
        while (MoveUpIndex(line))
        {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveFin[index] = true;
    }

    IEnumerator MoveDownIndexCo(Tile[] line, int index)
    {
        lineMoveFin[index] = false;
        while (MoveDownIndex(line))
        {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveFin[index] = true;
    }

    public void Generate()
    {
        if(EmptyTiles.Count > 0)
        {
            int indRand = Random.Range(0, EmptyTiles.Count);
            int heliNum = Random.Range(0, 10);
            if (heliNum == 4)
                EmptyTiles[indRand].Number = 4;
            else if (heliNum == 3)
                EmptyTiles[indRand].Number = 3;
            else
                EmptyTiles[indRand].Number = 1;
            EmptyTiles[indRand].Appear();
            EmptyTiles.RemoveAt(indRand);
        }
    }
}
