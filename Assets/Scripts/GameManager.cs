using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static PlayerAttitude PlayerAttitude;
}

public enum PlayerAttitude
{
    Upward,
    Downward,
    Rightward,
    Leftward,
    Idle
}
