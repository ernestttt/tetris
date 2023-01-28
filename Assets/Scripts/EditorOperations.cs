using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MenuScript
{
    private static GameConfig GameConfig
    {
        get
        {
            if (_gameConfig == null)
            {
                string guid = AssetDatabase.FindAssets("t:" + typeof(GameConfig).Name).First();
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
        string destinationFileName = $"{Application.dataPath}/Scripts/Libs/TetrisCore.dll";

        if (File.Exists(destinationFileName))
        {
            File.Delete(destinationFileName);
        }
        File.Copy(sourceFileName, destinationFileName);
    }
}
