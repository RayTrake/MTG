using System;

public class SolverNode
{
    public SolverNode(long id, SolverNode prev, BaseBoard.EMoveType moveType, byte[] boardData, int totalMoves, int moves)
    {
        BoardId = id;
        Prev = prev;
        MoveType = moveType;
        TotalMoves = totalMoves;
        Moves = moves;

        BoardData = new byte[boardData.Length];
        Array.Copy(boardData, 0, BoardData, 0, boardData.Length);
    }

    public long BoardId;
    public BaseBoard.EMoveType LastMove;
    public BaseBoard.EMoveType MoveType;
    public int TotalMoves;
    public int Moves;
    public bool IsRemoved;
    public SolverNode Next;
    public SolverNode Prev;
    public byte[] BoardData;
}
