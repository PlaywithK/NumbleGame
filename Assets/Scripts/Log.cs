using UnityEngine;

public static class Log
{
    public static void Message(string msg)
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log(msg);
        #endif
    }

    public static void Warning(string msg)
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.LogWarning(msg);
        #endif
    }

    public static void Error(string msg)
    {
        Debug.LogError(msg);
    }
}
