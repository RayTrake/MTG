using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameBoard gb;

    void Start()
    {
        gb = new GameBoard();
        gb.InitTest();
        gb.DrawBoard();
    }

    void Update()
    {
        bool moveResult = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveResult = gb.Move(BaseBoard.EMoveType.Left, true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveResult = gb.Move(BaseBoard.EMoveType.Right, true);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveResult = gb.Move(BaseBoard.EMoveType.Down, true);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveResult = gb.Move(BaseBoard.EMoveType.Up, true);
        }

        if (moveResult)
        {
            if (gb.IsSolved())
            {
                Debug.LogError("Solved");
            }
        }

        gb.Update();
    }
}
