using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowController : MonoBehaviour
{
    public SpriteGlow.SpriteGlowEffect GlowEffect;
    public float Speed;

    private bool enableGlowing;
    private bool goDown;

    void Update()
    {
        if (enableGlowing)
        {
            if (!goDown)
            {
                GlowEffect.GlowBrightness += Speed;
                if (GlowEffect.GlowBrightness > 8f)
                {
                    goDown = true;
                }
            }
            else
            {
                GlowEffect.GlowBrightness -= Speed;
                if (GlowEffect.GlowBrightness < 5f)
                {
                    goDown = false;
                }
            }

            if (GlowEffect.GlowBrightness > 3f)
            {
                GlowEffect.OutlineWidth = 1;
            }
        }
        else
        {
            if (GlowEffect.GlowBrightness > 5f)
            {
                GlowEffect.GlowBrightness -= 0.1f;
            }
            else
            {
                GlowEffect.GlowBrightness = 0f;
                GlowEffect.OutlineWidth = 0;
            }
        }
    }

    public void EnableGlowing()
    {
        GlowEffect.OutlineWidth = 1;
        GlowEffect.GlowBrightness = 8f;
        enableGlowing = true;
    }

    public void DisableGlowing()
    {
        enableGlowing = false;
    }
}
