using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesManager : MonoBehaviour
{
    public static SpritesManager Instance;

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    public Color[] Colors;

    public SpriteRenderer Block;
    public SpriteRenderer BoardTile;
    public SpriteRenderer[] Items;
    public SpriteRenderer[] Targets;

    public SpriteRenderer GetItem(byte index)
    {
        return Items[index - 1];
    }

    public SpriteRenderer GetTarget(byte index)
    {
        return Targets[index - 1];
    }

    public SpriteRenderer GetRandomTarget()
    {
        return Targets[Random.Range(0, Targets.Length)];
    }
}
