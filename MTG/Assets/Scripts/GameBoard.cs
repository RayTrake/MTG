using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : BaseBoard
{
    public class MoveTask
    {
        public Transform Item;
        public Vector2 To;
    }

    List<MoveTask> moveTasks;
    Vector2[] positions;
    Transform[] itemsVisual;

    public void DrawBoard()
    {
        float worldWidth = GetScreenToWorldWidth;
        float worldHeight = GetScreenToWorldHeight;

        float aspectRatioMax = Mathf.Min(worldWidth, worldHeight);

        float percents = aspectRatioMax * 0.1f;
        float scale = (aspectRatioMax - percents) / size;
        float startFrom = aspectRatioMax * 0.5f - scale * 0.5f - percents * 0.5f;

        moveTasks = new List<MoveTask>();

        GameObject board = new GameObject("board");
        board.transform.position = Vector3.zero;

        positions = new Vector2[size * size];
        itemsVisual = new Transform[size * size];

        for (int h = 0; h < size; h++)
        {
            for (int w = 0; w < size; w++)
            {
                int index = h * size + w;
                Vector2 currentPosition = new Vector2(startFrom * -1 + scale * w, startFrom - scale * h);
                positions[index] = currentPosition;

                ///////////////////////// TEMPORARY
                var obj = GameObject.Instantiate(SpritesManager.Instance.Empty, board.transform);
                obj.transform.position = currentPosition;
                obj.transform.localScale = Vector3.one * scale;
                ///////////////////////////////////

                if (data[targetsOffset + index] > 0)
                {
                    var goal = GameObject.Instantiate(SpritesManager.Instance.Target, board.transform);
                    goal.transform.position = currentPosition;
                    goal.transform.localScale = Vector3.one * scale;
                }

                if (data[blockOffset + index] > 0)
                {
                    var block = GameObject.Instantiate(SpritesManager.Instance.Block, board.transform);
                    block.transform.position = currentPosition;
                    block.transform.localScale = Vector3.one * scale;
                }

                if (data[itemsOffset + index] > 0)
                {
                    var item = GameObject.Instantiate(SpritesManager.Instance.Item, board.transform);
                    item.transform.position = currentPosition;
                    item.transform.localScale = Vector3.one * scale * 0.8f;

                    itemsVisual[index] = item.transform;
                }
            }
        }
    }

    public override void OnMove(int from, int to)
    {
        var temp = itemsVisual[from];
        itemsVisual[to] = temp;
        itemsVisual[from] = null;

        AddMoveTask(itemsVisual[to], positions[to]);
    }

    void AddMoveTask(Transform t, Vector2 to)
    {
        moveTasks.Add(new MoveTask()
        {
            Item = t,
            To = to
        });
    }

    public void Update()
    {
        if (moveTasks != null &&
            moveTasks.Count > 0)
        {
            for (int i = moveTasks.Count - 1; i >= 0; i--)
            {
                moveTasks[i].Item.position = Vector2.MoveTowards(moveTasks[i].Item.position, moveTasks[i].To, 0.3f);
                if ((Vector2)moveTasks[i].Item.position == moveTasks[i].To)
                {
                    moveTasks.RemoveAt(i);
                }
            }
        }
    }

    public static float GetScreenToWorldHeight
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var height = edgeVector.y * 2;
            return height;
        }
    }

    public static float GetScreenToWorldWidth
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2;
            return width;
        }
    }

    public void SaveBoard()
    {

    }

    // TEMPORARY
    public void InitTest()
    {
        int sqrSize = 3 * 3;

        var d = new byte[sqrSize * 3 + 3];
        d[0] = (byte)3;
        d[1] = (byte)3;
        d[2] = (byte)3;

        d[sqrSize + 3 + 0] = 1;
        d[sqrSize + 3 + 1] = 1;
        d[sqrSize + 3 + 2] = 1;

        d[sqrSize * 2 + 3 + 6] = 1;
        d[sqrSize * 2 + 3 + 7] = 1;
        d[sqrSize * 2 + 3 + 8] = 1;

        Init(d);
    }
}