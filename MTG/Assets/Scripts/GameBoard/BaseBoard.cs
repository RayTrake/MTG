using System;
using UnityEngine;

public class BaseBoard
{
    public enum EMoveType
    {
        Left,
        Right,
        Up,
        Down,
        Count
    }

    public byte[] data;

    protected int size;
    protected int itemsCount;
    protected int colorCount;
    protected int solveCount;

    protected int blockOffset;
    protected int itemsOffset;
    protected int targetsOffset;

    public void Init(byte[] inData)
    {
        size = inData[0];
        itemsCount = inData[1];
        colorCount = inData[2];

        int squaredSize = size * size;
        data = new byte[squaredSize * 3];
        Array.Copy(inData, 3, data, 0, squaredSize * 3);

        blockOffset = 0;
        itemsOffset = squaredSize;
        targetsOffset = squaredSize * 2;
    }

    public bool Move(EMoveType moveType, bool moveVisual = false)
    {
        bool result = false;
        solveCount = 0;

        switch (moveType)
        {
            case EMoveType.Left:
                {
                    for (int h = 0; h < size; h++)
                    {
                        for (int w = 0; w < size; w++)
                        {
                            int index = h * size + w;
                            byte currentItem = data[itemsOffset + index];
                            if (SkipMove(w == 0, currentItem, index))
                            {
                                continue;
                            }

                            int newIndex = h * size + w - 1;
                            if (TryMove(currentItem, index, newIndex, moveVisual))
                            {
                                result = true;
                            }
                        }
                    }
                }
                break;

            case EMoveType.Right:
                {
                    for (int h = 0; h < size; h++)
                    {
                        for (int w = size - 1; w >= 0; w--)
                        {
                            int index = h * size + w;
                            byte currentItem = data[itemsOffset + index];
                            if (SkipMove(w == size - 1, currentItem, index))
                            {
                                continue;
                            }

                            int newIndex = h * size + w + 1;
                            if (TryMove(currentItem, index, newIndex, moveVisual))
                            {
                                result = true;
                            }
                        }
                    }
                }
                break;

            case EMoveType.Down:
                {
                    for (int h = size - 1; h >= 0; h--)
                    {
                        for (int w = 0; w < size; w++)
                        {
                            int index = h * size + w;
                            byte currentItem = data[itemsOffset + index];
                            if (SkipMove(h == size - 1, currentItem, index))
                            {
                                continue;
                            }

                            int newIndex = (h + 1) * size + w;
                            if (TryMove(currentItem, index, newIndex, moveVisual))
                            {
                                result = true;
                            }
                        }
                    }
                }
                break;

            case EMoveType.Up:
                {
                    for (int h = 0; h < size; h++)
                    {
                        for (int w = 0; w < size; w++)
                        {
                            int index = h * size + w;
                            byte currentItem = data[itemsOffset + index];
                            if (SkipMove(h == 0, currentItem, index))
                            {
                                continue;
                            }

                            int newIndex = (h - 1) * size + w;
                            if (TryMove(currentItem, index, newIndex, moveVisual))
                            {
                                result = true;
                            }
                        }
                    }
                }
                break;
        }

        return result;
    }

    public bool IsSolved()
    {
        return solveCount == itemsCount;
    }

    private bool SkipMove(bool primaryCheck, byte currentItem, int index)
    {
        bool needSkip = false;

        byte currentBlock = data[blockOffset + index];
        if (primaryCheck ||
            currentItem == 0 ||
            currentBlock > 0)
        {
            if (currentItem > 0 &&
                currentItem == data[targetsOffset + index])
            {
                solveCount++;
            }

            needSkip = true;
        }

        return needSkip;
    }

    private bool TryMove(byte currentItem, int index, int newIndex, bool moveVisual)
    {
        byte neighbourItem = data[itemsOffset + newIndex];
        byte neighbourBlock = data[blockOffset + newIndex];
        byte neighbourTarget = data[targetsOffset + newIndex];

        if (neighbourItem == 0 &&
            neighbourBlock == 0)
        {
            data[itemsOffset + newIndex] = currentItem;
            data[itemsOffset + index] = 0;

            if (moveVisual)
            {
                OnMove(index, newIndex);
            }

            if (currentItem == neighbourTarget)
            {
                solveCount++;
            }

            return true;
        }
        else
        {
            if (currentItem > 0 &&
                currentItem == data[targetsOffset + index])
            {
                solveCount++;
            }
        }

        return false;
    }

    public int GetCellId(int x, int y)
    {
        int index = y * size + x;

        if (data[blockOffset + index] > 0)
        {
            return 1;
        }
        else
        {
            return data[itemsOffset + index] * 10 + data[targetsOffset + index];
        }
    }

    public long GetBoardUniqueId()
    {
        long result = 0;
        for (int h = 0; h < size; h++)
        {
            for (int w = 0; w < size; w++)
            {
                int uniqueId = GetCellId(w, h);
                result = result * 6 + uniqueId;
            }
        }

        return result;
    }

    public virtual void OnMove(int from, int to)
    {
    }

    public BaseBoard Clone()
    {
        BaseBoard bb = new BaseBoard();
        bb.itemsCount = itemsCount;
        bb.size = size;
        bb.data = data;

        bb.blockOffset = blockOffset;
        bb.itemsOffset = itemsOffset;
        bb.targetsOffset = targetsOffset;

        return bb;
    }
}
