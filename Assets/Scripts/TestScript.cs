using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris;

public class TestScript : MonoBehaviour
{
    private TetrisCore _tetris = new TetrisCore();
    // Start is called before the first frame update
    void Start()
    {
        _tetris.Rotate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
