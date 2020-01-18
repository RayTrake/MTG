using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Generator : MonoBehaviour
{
#if UNITY_EDITOR
    public int Size;
    public int Items;
    public int Colors;

    public int MinBlocks;
    public int MaxBlocks;

    public int MinMoves;
    public int MaxMoves;
    public int LevelsCount;

    public string jsonFilename;

    public LevelsHolder Holder;

    private BoardSolver solver;

    private byte[] boardData;
    private int blocksOffset;
    private int itemsOffset;
    private int targetOffset;

    private List<LevelsHolder.Level> currentLevels = new List<LevelsHolder.Level>();

    private void Start()
    {
        int squaredSize = Size * Size;

        int whileGuard = 0;
        while (currentLevels.Count < LevelsCount)
        {
            if (whileGuard > LevelsCount * 100)
            {
                break;
            }

            boardData = new byte[squaredSize * 3 + 3];

            boardData[0] = (byte)Size;
            boardData[1] = (byte)Items;
            boardData[2] = (byte)Colors;

            blocksOffset = 3;
            itemsOffset = squaredSize + 3;
            targetOffset = squaredSize * 2 + 3;

            int totalBlocksCount = Random.Range(MinBlocks, MaxBlocks);
            var possibleBlocks = Enumerable.Range(0, squaredSize).ToList();
            for (int i = 0; i < totalBlocksCount; i++)
            {
                int rndIndex = Random.Range(0, possibleBlocks.Count);
                int blockIndex = possibleBlocks[rndIndex];
                possibleBlocks.RemoveAt(rndIndex);

                boardData[blocksOffset + blockIndex] = 1;
            }

            int itemsSpawned = 0;
            var possibleItems = Enumerable.Range(0, squaredSize).ToList();
            for (int i = 0; i < possibleItems.Count; i++)
            {
                if (itemsSpawned >= Items)
                {
                    continue;
                }

                int rndIndex = Random.Range(0, possibleItems.Count);
                int itemIndex = possibleItems[rndIndex];
                if (boardData[blocksOffset + itemIndex] == 0 &&
                    boardData[itemsOffset + itemIndex] == 0)
                {
                    possibleItems.Remove(rndIndex);
                    boardData[itemsOffset + itemIndex] = 1;
                    itemsSpawned++;
                }
            }

            int targetsSpawned = 0;
            var possibleTargets = Enumerable.Range(0, squaredSize).ToList();
            for (int i = 0; i < possibleTargets.Count; i++)
            {
                if (targetsSpawned >= Items)
                {
                    continue;
                }

                int rndIndex = Random.Range(0, possibleTargets.Count);
                int itemIndex = possibleTargets[rndIndex];
                if (boardData[blocksOffset + itemIndex] == 0 &&
                    boardData[targetOffset + itemIndex] == 0)
                {
                    possibleTargets.Remove(rndIndex);
                    boardData[targetOffset + itemIndex] = 1;
                    targetsSpawned++;
                }
            }

            GameBoard gb = new GameBoard();
            gb.Init(boardData);

            long uniqueId = gb.GetBoardUniqueId();
            if (currentLevels.Any(x => x.UniqueId == uniqueId))
            {
                continue;
            }

            solver = new BoardSolver();
            var result = solver.GetMoves(gb.Clone());
            if (result.Count > MinMoves &&
                result.Count <= MaxMoves)
            {
                LevelsHolder.Level level = new LevelsHolder.Level();
                level.Moves = result.Count;
                level.LevelData = new byte[squaredSize * 3 + 3];
                System.Array.Copy(gb.data, 0, level.LevelData, 3, gb.data.Length);
                level.LevelData[0] = (byte)Size;
                level.LevelData[1] = (byte)Items;
                level.LevelData[2] = (byte)Colors;

                level.UniqueId = gb.GetBoardUniqueId();

                currentLevels.Add(level);
            }

            whileGuard++;
        }

        currentLevels = currentLevels.OrderBy(x => x.Moves).ToList();

        List<string> levels = new List<string>();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < currentLevels.Count; i++)
        {
            levels.Add(currentLevels[i].Serialize(sb));
        }

        System.IO.File.WriteAllLines(jsonFilename, levels);
    }
#endif
}
