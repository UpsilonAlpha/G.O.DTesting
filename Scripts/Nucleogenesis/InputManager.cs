using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    Right, Left, Up, Down
}

public class InputManager : MonoBehaviour
{
    public GM gm;

    private void Update()
    {
        if (gm.state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                gm.Shift(Dir.Right);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gm.Shift(Dir.Left);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                gm.Shift(Dir.Up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                gm.Shift(Dir.Down);
            }
        }
    }
}
