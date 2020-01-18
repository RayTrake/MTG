using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelsHolder Holder;
    public GameObject LevelPassedPanel;
    public Button TryAgain;
    public Button NextLevel;
    public Button UseHint;
    public Button Reload;

    public GameObject hintArrow;

    public TMPro.TMP_Text MovesLabel;
    public TMPro.TMP_Text PerfectMovesLabel;
    public TMPro.TMP_Text LevelLabel;

    private int currentLevel = 27;

    GameBoard gameBoard;
    BoardSolver solver;

    private int currentMoves;
    private bool blockMovement = false;

    void Start()
    {
        var level = Holder.Levels[currentLevel];
        gameBoard = new GameBoard();
        gameBoard.Init(level.LevelData);
        gameBoard.DrawBoard();

        LevelLabel.text = "Level " + (currentLevel + 1) + "/" + Holder.Levels.Count;

        PerfectMovesLabel.text = "Perfect: " + level.Moves;
        solver = new BoardSolver();

        Reload.onClick.AddListener(() =>
        {
            var l = Holder.Levels[currentLevel];
            Destroy(gameBoard.board);
            gameBoard = new GameBoard();
            gameBoard.Init(l.LevelData);
            gameBoard.DrawBoard();
            blockMovement = false;

            PerfectMovesLabel.text = "Perfect: " + l.Moves;
            LevelLabel.text = "Level " + (currentLevel + 1) + "/" + Holder.Levels.Count;

            currentMoves = 0;
            MovesLabel.text = "Moves: " + currentMoves;
        });

        TryAgain.onClick.AddListener(() =>
        {
            LevelPassedPanel.SetActive(false);

            var l = Holder.Levels[currentLevel];
            Destroy(gameBoard.board);
            gameBoard = new GameBoard();
            gameBoard.Init(l.LevelData);
            gameBoard.DrawBoard();
            blockMovement = false;

            PerfectMovesLabel.text = "Perfect: " + l.Moves;
            LevelLabel.text = "Level " + (currentLevel + 1) + "/" + Holder.Levels.Count;

            currentMoves = 0;
            MovesLabel.text = "Moves: " + currentMoves;
        });

        NextLevel.onClick.AddListener(() =>
        {
            LevelPassedPanel.SetActive(false);
            currentLevel++;
            if (currentLevel < Holder.Levels.Count)
            {
                var l = Holder.Levels[currentLevel];
                Destroy(gameBoard.board);
                gameBoard = new GameBoard();
                gameBoard.Init(l.LevelData);
                gameBoard.DrawBoard();
                blockMovement = false;

                currentMoves = 0;

                PerfectMovesLabel.text = "Perfect: " + l.Moves;
                LevelLabel.text = "Level " + (currentLevel + 1) + "/" + Holder.Levels.Count;
                MovesLabel.text = "Moves: " + currentMoves;
            }
        });

        UseHint.onClick.AddListener(() =>
        {
            blockMovement = true;

            BaseBoard.EMoveType nextMove = solver.FindNextMove(gameBoard.Clone());
            if (nextMove != BaseBoard.EMoveType.Count)
            {
                hintArrow.gameObject.SetActive(true);

                float angle = 0;
                switch (nextMove)
                {
                    case BaseBoard.EMoveType.Right:
                        {
                            angle = 0f;
                        }
                        break;

                    case BaseBoard.EMoveType.Up:
                        {
                            angle = 90f;
                        }
                        break;

                    case BaseBoard.EMoveType.Down:
                        {
                            angle = -90f;
                        }
                        break;

                    case BaseBoard.EMoveType.Left:
                        {
                            angle = 180f;
                        }
                        break;
                }

                hintArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            blockMovement = false;
        });
    }

    void Update()
    {
        bool moveResult = false;

        if (!blockMovement)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveResult = gameBoard.Move(BaseBoard.EMoveType.Left, true);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveResult = gameBoard.Move(BaseBoard.EMoveType.Right, true);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveResult = gameBoard.Move(BaseBoard.EMoveType.Down, true);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveResult = gameBoard.Move(BaseBoard.EMoveType.Up, true);
            }
        }

        gameBoard.Update();

        if (moveResult ||
            blockMovement)
        {
            blockMovement = gameBoard.IsSolved();
            if (blockMovement)
            {
                if (gameBoard.MovementsComplete())
                {
                    if (!LevelPassedPanel.activeSelf)
                    {
                        if (!isStarted)
                        {
                            StartCoroutine(ShowPanel());
                        }
                    }
                }
            }

            if (moveResult)
            {
                if (hintArrow.activeSelf)
                {
                    hintArrow.SetActive(false);
                }

                currentMoves++;
                MovesLabel.text = "Moves: " + currentMoves;
            }
        }
    }

    private bool isStarted = false;
    IEnumerator ShowPanel()
    {
        isStarted = true;

        yield return new WaitForSeconds(1f);

        LevelPassedPanel.SetActive(true);
        isStarted = false;
    }
}
