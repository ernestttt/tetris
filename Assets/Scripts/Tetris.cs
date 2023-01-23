using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tetris : MonoBehaviour
{
    private const int width = 10;
    private const int height = 20;
    private byte[,] field = new byte[width, height];

    [SerializeField] private Transform square;

    private void Start()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                field[x, y] = (byte)Random.Range(0, 2);
            }
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (field[x, y] == 1)
                {
                    Transform obj = Instantiate(square, transform);
                    obj.localPosition = new Vector3(x, y);
                    Gizmos.
                }
            }
        }
    }
}
