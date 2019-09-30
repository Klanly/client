using UnityEngine;
using System.Collections;
using MuGame;

public static class Debugger
{
    public static void Log(string str, params object[] args)
    {
        str = string.Format(str, args);
        debug.Log(str);
    }

    public static void LogWarning(string str, params object[] args)
    {
        str = string.Format(str, args);
        Debug.LogWarning(str);
    }

    public static void LogError(string str, params object[] args)
    {
        str = string.Format(str, args);
        Debug.LogError(str);
    }
}
