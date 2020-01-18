using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : BaseBoard
{
    public class MoveTask
    {
        public ItemVisual Item;
        public Vector2 To;
        public bool ReachTarget;
    }

    public class ItemVisual
    {
        public Transform TransformObject;
        public GlowController GlowControllerInstance;
    }

    List<MoveTask> moveTasks;
    Vector2[] positions;
    ItemVisual[] itemsVisual;

    public GameObject board;

    public void DrawBoard()
    {
        float worldWidth = GetScreenToWorldWidth;
        float worldHeight = GetScreenToWorldHeight;

        float aspectRatioMax = Mathf.Min(worldWidth, worldHeight);

        float percents = aspectRatioMax * 0.1f;
        float scale = (aspectRatioMax - percents) / size;
        float startFrom = aspectRatioMax * 0.5f - scale * 0.5f - percents * 0.5f;

        moveTasks = new List<MoveTask>();

        board = new GameObject("board");
        board.transform.position = Vector3.zero;

        positions = new Vector2[size * size];
        itemsVisual = new ItemVisual[size * size];

        for (int h = 0; h < size; h++)
        {
            for (int w = 0; w < size; w++)
            {
                int index = h * size + w;
                Vector2 currentPosition = new Vector2(startFrom * -1 + scale * w, startFrom - scale * h);
                positions[index] = currentPosition;

                ///////////////////////// TEMPORARY
                var obj = GameObject.Instantiate(SpritesManager.Instance.BoardTile, board.transform);
                obj.transform.position = currentPosition;
                obj.transform.localScale = Vector3.one * scale;
                ///////////////////////////////////

                if (data[targetsOffset + index] > 0)
                {
                    var target = GameObject.Instantiate(SpritesManager.Instance.GetRandomTarget(), board.transform);
                    target.color = SpritesManager.Instance.Colors[data[targetsOffset + index] - 1];
                    target.transform.position = currentPosition;
                    target.transform.localScale = Vector3.one * scale * 0.95f;
                }

                if (data[blockOffset + index] > 0)
                {
                    var block = GameObject.Instantiate(SpritesManager.Instance.Block, board.transform);
                    block.transform.position = currentPosition;
                    block.transform.localScale = Vector3.one * scale * 0.95f;
                }

                if (data[itemsOffset + index] > 0)
                {
                    var item = GameObject.Instantiate(SpritesManager.Instance.GetItem(data[itemsOffset + index]), board.transform);
                    item.transform.position = currentPosition;
                    item.transform.localScale = Vector3.one * scale * 0.8f;

                    itemsVisual[index] = new ItemVisual();
                    itemsVisual[index].TransformObject = item.transform;
                    itemsVisual[index].GlowControllerInstance = item.GetComponent<GlowController>();

                    if (data[targetsOffset + index] == data[itemsOffset + index])
                    {
                        itemsVisual[index].GlowControllerInstance.EnableGlowing();
                    }
                }
            }
        }
    }

    public bool MovementsComplete()
    {
        return moveTasks.Count == 0;
    }

    public override void OnMove(int from, int to, bool reachTarget)
    {
        var temp = itemsVisual[from];
        itemsVisual[to] = temp;
        itemsVisual[from] = null;

        AddMoveTask(itemsVisual[to], positions[to], reachTarget);
    }

    void AddMoveTask(ItemVisual t, Vector2 to, bool reachTarget)
    {
        moveTasks.Add(new MoveTask()
        {
            Item = t,
            To = to,
            ReachTarget = reachTarget
        });
    }

    public void Update()
    {
        if (moveTasks != null &&
            moveTasks.Count > 0)
        {
            for (int i = moveTasks.Count - 1; i >= 0; i--)
            {
                moveTasks[i].Item.TransformObject.position = Vector2.MoveTowards(moveTasks[i].Item.TransformObject.position, moveTasks[i].To, 0.3f);
                if ((Vector2)moveTasks[i].Item.TransformObject.position == moveTasks[i].To)
                {
                    if (moveTasks[i].ReachTarget)
                    {
                        moveTasks[i].Item.GlowControllerInstance.EnableGlowing();
                    }
                    else
                    {
                        moveTasks[i].Item.GlowControllerInstance.DisableGlowing();
                    }

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
}