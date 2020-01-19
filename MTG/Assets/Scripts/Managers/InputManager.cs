using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public const float MAX_SWIPE_TIME = 0.5f;
    public const float MIN_SWIPE_DISTANCE = 0.17f;

    Vector2 startPos;
    float startTime;

    public BaseBoard.EMoveType GetInput()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                startTime = Time.time;
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (Time.time - startTime > MAX_SWIPE_TIME)
                {
                    return BaseBoard.EMoveType.Count;
                }

                Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                if (swipe.magnitude < MIN_SWIPE_DISTANCE)
                {
                    return BaseBoard.EMoveType.Count;
                }

                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                {
                    if (swipe.x > 0)
                    {
                        return BaseBoard.EMoveType.Right;
                    }
                    else
                    {
                        return BaseBoard.EMoveType.Left;
                    }
                }
                else
                {
                    if (swipe.y > 0)
                    {
                        return BaseBoard.EMoveType.Up;
                    }
                    else
                    {
                        return BaseBoard.EMoveType.Down;
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                return BaseBoard.EMoveType.Left;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                return BaseBoard.EMoveType.Right;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                return BaseBoard.EMoveType.Down;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                return BaseBoard.EMoveType.Up;
            }
        }

        return BaseBoard.EMoveType.Count;
    }
}
