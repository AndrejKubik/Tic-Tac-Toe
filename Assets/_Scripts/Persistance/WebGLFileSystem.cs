using System.Runtime.InteropServices;
using UnityEngine;

public static class WebGLFileSystem
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SyncFiles();
#endif

    public static void Sync()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFiles();
#endif
    }
}