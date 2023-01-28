using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class MenuScript
{
    private static GameConfig GameConfig
    {
        get
        {
            if (_gameConfig == null)
            {
                string guid = AssetDatabase.FindAssets("t:" + nameof(GameConfig)).First();
                if (!string.IsNullOrEmpty(guid))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    _gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>(path);
                }
            }

            return _gameConfig;
        }
    }

    private static GameConfig _gameConfig;
    
    [MenuItem("Custom/Update library")]
    private static void UpdateLibrary()
    {
        string sourceFileName = $"{Application.dataPath}/../{GameConfig.LibPath}";
        string destinationPath = $"{Application.dataPath}/Scripts/Libs/TetrisCore";

        string projectDir = $"{sourceFileName}";
        
        if (Directory.Exists(destinationPath))
        {
            Directory.Delete(destinationPath, true);
        }
        
        // build dll
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = $"/C cd /d {projectDir} && dotnet build -o {destinationPath}";
        
        process.StartInfo = startInfo;
        process.Start();
        
        
        while (!process.StandardOutput.EndOfStream)
        {
            string line = process.StandardOutput.ReadToEnd();
            Debug.Log(line);
        }
    }
}
