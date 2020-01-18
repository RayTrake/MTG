using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelsHolder))]
public class LevelsHolder_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load Levels"))
        {
            string[] data = System.IO.File.ReadAllLines("levels.json");
            LevelsHolder holder = (LevelsHolder)target;
            holder.Levels = new List<LevelsHolder.Level>();

            for (int i = 0; i < data.Length; i++)
            {
                LevelsHolder.Level l = new LevelsHolder.Level(data[i]);
                holder.Levels.Add(l);
            }
        }
    }
}
