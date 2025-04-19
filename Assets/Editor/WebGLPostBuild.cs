using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class WebGLPostBuild
{
    [PostProcessBuild]
    public static void CopySignalR(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.WebGL)
            return;

        string sourcePath = Path.Combine(Application.dataPath, "Plugins", "WebGL", "signalr.js"); // Adjust path if needed
        string destinationPath = Path.Combine(pathToBuiltProject, "Build", "signalr.js");
        Debug.Log(sourcePath);
        Debug.Log(destinationPath);

        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, destinationPath, true);
            Debug.Log("SignalR.js copied to WebGL build folder.");
        }
        else
        {
            Debug.LogWarning("SignalR.js not found! Make sure it's in Assets/SignalR.");
        }
    }
}
