using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class LevelsHolder : MonoBehaviour
{
    static char Splitter = ';';

    [System.Serializable]
    public class Level
    {
        public int Moves;
        public int PlayerRecord;
        public long UniqueId;
        public byte[] LevelData;

        public Level()
        {

        }

        public Level(string data)
        {
            Deserialize(data);
        }

        public string Serialize(StringBuilder sb = null)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
            }
            else
            {
                sb.Length = 0;
            }

            sb.Append(Moves);
            sb.Append(Splitter);
            sb.Append(PlayerRecord);
            sb.Append(Splitter);
            sb.Append(UniqueId);
            sb.Append(Splitter);
            sb.Append(LevelData.Length);
            sb.Append(Splitter);

            for (int i = 0; i < LevelData.Length; i++)
            {
                sb.Append(LevelData[i]);

                if (i != LevelData.Length - 1)
                {
                    sb.Append(Splitter);
                }
            }

            return sb.ToString();
        }

        public void Deserialize(string data)
        {
            string[] opts = data.Split(Splitter);
            int.TryParse(opts[0], out Moves);
            int.TryParse(opts[1], out PlayerRecord);
            long.TryParse(opts[2], out UniqueId);

            int arrayLength = 0;
            int.TryParse(opts[3], out arrayLength);

            LevelData = new byte[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                byte.TryParse(opts[4 + i], out LevelData[i]);
            }
        }
    }

    public List<Level> Levels;
}
