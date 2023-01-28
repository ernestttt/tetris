using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private float stepsPerSecond;
    public float Frequency => 1 / stepsPerSecond;
}
