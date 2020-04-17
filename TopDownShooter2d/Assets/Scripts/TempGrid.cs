﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TempGrid
{
    private int width;
    private int height;
    private float cellSize;

    private int[,] gridArray;

    public TempGrid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
        
        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y), 20, Color.white, TextAnchor.MiddleCenter);
                
            }
        }

    }


    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
}


