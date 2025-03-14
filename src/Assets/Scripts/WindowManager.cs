
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    void Start()
    {
        // Set window properties
        Screen.fullScreen = false;
        Screen.SetResolution(800, 600, false);
    }

    public void MinimizeWindow()
    {
        #if UNITY_STANDALONE_WIN
        WindowsUtils.MinimizeWindow();
        #endif
    }
}

#if UNITY_STANDALONE_WIN
public static class WindowsUtils
{
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool ShowWindow(System.IntPtr hWnd, int nCmdShow);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    public static void MinimizeWindow()
    {
        ShowWindow(GetActiveWindow(), 2);
    }
}
#endif

