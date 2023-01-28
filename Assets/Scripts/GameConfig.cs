using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private float stepsPerSecond;
    [SerializeField] private string libPath = "tetris_model/tetris/bin/Debug/netcoreapp2.1/Tetris.dll";
    public float Frequency => 1 / stepsPerSecond; 
    public string LibPath => libPath;
}
