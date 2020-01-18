using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSolver
{
    public List<BaseBoard.EMoveType> GetMoves(BaseBoard board)
    {
        List<BaseBoard.EMoveType> result = new List<BaseBoard.EMoveType>();

        if (board.IsSolved())
        {
            return result;
        }

        SolverNode solvedNode = Find(board);
        if (solvedNode != null)
        {
            result.Add(solvedNode.LastMove);
            while (solvedNode != null && solvedNode.Prev != null)
            {
                result.Add(solvedNode.MoveType);
                solvedNode = solvedNode.Prev;
            }
        }

        return result;
    }

    public BaseBoard.EMoveType FindNextMove(BaseBoard board)
    {
        if (board.IsSolved())
        {
            return BaseBoard.EMoveType.Count;
        }

        SolverNode solvedNode = Find(board);
        if (solvedNode != null)
        {
            return GetNextMove(solvedNode);
        }

        return BaseBoard.EMoveType.Count;
    }

    private BaseBoard.EMoveType GetNextMove(SolverNode node)
    {
        BaseBoard.EMoveType result = BaseBoard.EMoveType.Count;

        if (node.Prev == null)
        {
            result = node.LastMove;
        }
        else
        {
            result = node.MoveType;
            while (node != null && node.Prev != null)
            {
                result = node.MoveType;
                node = node.Prev;
            }
        }

        return result;
    }

    private SolverNode Find(BaseBoard board)
    {
        SolverNode initialNode = new SolverNode(board.GetBoardUniqueId(), null, BaseBoard.EMoveType.Count, board.data, 1, 0);

        var container = new SolverNodeContainer();
        container.Add(initialNode);

        SolverNode searchNode = null;
        while (container.MoveNext())
        {
            SolverNode node = container.Current();
            if (node.Moves > 30)
            {
                return null;
            }

            bool flag = searchNode != node;
            for (int i = 0; i < 4; i++)
            {
                if (flag)
                {
                    Array.Copy(node.BoardData, 0, board.data, 0, node.BoardData.Length);
                }

                searchNode = null;
                flag = board.Move((BaseBoard.EMoveType)i, false);
                if (flag)
                {
                    if (board.IsSolved())
                    {
                        node.LastMove = (BaseBoard.EMoveType)i;
                        return node;
                    }

                    long boardId = board.GetBoardUniqueId();
                    int movesCount = node.Moves + 1;

                    if (!container.Contains(boardId))
                    {
                        searchNode = new SolverNode(boardId, node, (BaseBoard.EMoveType)i, board.data, movesCount/* + board.GetMinMovesToSolution()*/, movesCount);
                        container.Add(searchNode);
                    }
                }
            }
        }

        return null;
    }
}
