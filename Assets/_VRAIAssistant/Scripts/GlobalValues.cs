using System;
using UnityEngine;

public class GlobalValues : Singleton<GlobalValues>
{
    public bool _isAIRunning;

    const float PLAYER_HEIGHT = 1.7F;
    const float UI_HEIGHT = 1.3F;
    const float FADE_SPEED = .7F;
    public static float PlayerHeight => PLAYER_HEIGHT;
    public static float UIHeight => UI_HEIGHT;
    public static float FadeSpeed => FADE_SPEED;

    public void SetIsAIRunning(bool isRunning)
    {
        _isAIRunning = isRunning;
    }
}
