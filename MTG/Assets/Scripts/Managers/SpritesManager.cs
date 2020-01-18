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

    public GameObject Item;
    public GameObject Block;
    public GameObject Empty;
    public GameObject Target;
}
