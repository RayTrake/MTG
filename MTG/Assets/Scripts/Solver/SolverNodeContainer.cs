using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolverNodeContainer
{
    private SolverNode headNode;
    private Dictionary<long, SolverNode> nodes = new Dictionary<long, SolverNode>();

    public bool MoveNext()
    {
        return headNode != null;
    }

    public bool Contains(long boardId)
    {
        return nodes.ContainsKey(boardId);
    }

    private void Remove(SolverNode item)
    {
        nodes.Remove(item.BoardId);
        item.IsRemoved = true;
    }

    public bool Add(SolverNode addNode)
    {
        if (nodes.ContainsKey(addNode.BoardId))
        {
            SolverNode node = nodes[addNode.BoardId];
            if (node.TotalMoves <= addNode.TotalMoves)
            {
                return false;
            }

            Remove(node);
        }

        nodes.Add(addNode.BoardId, addNode);
        if (headNode == null)
        {
            headNode = addNode;
        }
        else if (headNode.Next == null && addNode.TotalMoves <= headNode.TotalMoves)
        {
            addNode.Next = headNode;
            headNode = addNode;
        }
        else
        {
            SolverNode nextNode = headNode;
            while (nextNode.Next != null && nextNode.Next.TotalMoves < addNode.TotalMoves)
            {
                nextNode = nextNode.Next;
            }

            addNode.Next = nextNode.Next;
            nextNode.Next = addNode;
        }

        return true;
    }

    public SolverNode Current()
    {
        if (headNode == null)
        {
            return null;
        }

        while (headNode.IsRemoved)
        {
            headNode = headNode.Next;
            if (headNode == null)
            {
                return null;
            }
        }

        SolverNode node = headNode;
        nodes.Remove(node.BoardId);

        headNode = headNode.Next;
        return node;
    }
}
