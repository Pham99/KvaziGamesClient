using System;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public int[,] shape;
    public Vector2Int position;
    public int id;
    public int rotationState; // 0: spawn, 1: right, 2: reverse, 3: left
    public TetronimoType type;
    public float lockDelay = 0.5f;
    public float lockTimer = 0f;
    public bool isLocking = false;
    public Piece(int id, Vector2Int position)
    {
        this.position = position;
        this.id = id;
        Array values = Enum.GetValues(typeof(TetronimoType));
        TetronimoType randomTetronimo = (TetronimoType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        type = randomTetronimo;
        shape = TetronimoFactory.tetronimos[randomTetronimo];
        rotationState = 0;
    }
    public List<Vector2Int> GetOccupiedPositions()
    {
        List<Vector2Int> occupiedPositions = new List<Vector2Int>();
        for (int x = 0; x < shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.GetLength(1); y++)
            {
                if (shape[x, y] == 1)
                {
                    occupiedPositions.Add(new Vector2Int(position.x + x, position.y + y));
                }
            }
        }
        return occupiedPositions;
    }
    public void RotateCounterClockWise()
    {
        int[,] newShape = new int[shape.GetLength(1), shape.GetLength(0)];
        for (int x = 0; x < shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.GetLength(1); y++)
            {
                newShape[y, shape.GetLength(0) - 1 - x] = shape[x, y];
            }
        }
        shape = newShape;
        rotationState = (rotationState + 3) % 4;
    }
    public void RotateClockWise()
    {
        int[,] newShape = new int[shape.GetLength(1), shape.GetLength(0)];
        for (int x = 0; x < shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.GetLength(1); y++)
            {
                newShape[shape.GetLength(1) - 1 - y, x] = shape[x, y];
            }
        }
        shape = newShape;
        rotationState = (rotationState + 1) % 4;
    }
}

public enum TetronimoType{
    I,
    J,
    L,
    O,
    S,
    T,
    Z
}

public static class TetronimoFactory{
    public static Dictionary<TetronimoType, int[,]> tetronimos = new Dictionary<TetronimoType, int[,]>() {
        { TetronimoType.I, new int[4,4] {
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 }
            }
        },
        { TetronimoType.J, new int[3,3] {
                { 0, 1, 0 },
                { 0, 1, 0 },
                { 1, 1, 0 }
            }
        },
        { TetronimoType.L, new int[3,3] {
                { 1, 1, 0 },
                { 0, 1, 0 },
                { 0, 1, 0 }
            }
        },
        { TetronimoType.O, new int[2,2] {
                { 1, 1 },
                { 1, 1 }
            }
        },
        { TetronimoType.S, new int[3,3] {
                { 1, 0, 0 },
                { 1, 1, 0 },
                { 0, 1, 0 }
            }
        },
        { TetronimoType.T, new int[3,3] {
                { 0, 1, 0 },
                { 1, 1, 0 },
                { 0, 1, 0 }
            }
        },
        { TetronimoType.Z, new int[3,3] {
                { 0, 1, 0 },
                { 1, 1, 0 },
                { 1, 0, 0 }
            }
        }
    };
}

// SRSData: Wall kick data for SRS (Standard for modern Tetris)
public static class SRSData
{
    // [fromRotation, toRotation][test #] = Vector2Int offset
    // For J, L, S, T, Z (not I or O)
    public static readonly Vector2Int[,] JLSTZ_Offsets = new Vector2Int[8, 5]
    {
        // 0>>R
        { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        // R>>0
        { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,2), new Vector2Int(1,2) },
        // R>>2
        { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,2), new Vector2Int(1,2) },
        // 2>>R
        { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        // 2>>L
        { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(0,-2), new Vector2Int(1,-2) },
        // L>>2
        { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,2), new Vector2Int(-1,2) },
        // L>>0
        { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,2), new Vector2Int(-1,2) },
        // 0>>L
        { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(0,-2), new Vector2Int(1,-2) },
    };
    // For I piece
    public static readonly Vector2Int[,] I_Offsets = new Vector2Int[8, 6]
    {
        // 0>>R
        { new Vector2Int(0,0), new Vector2Int(-2,0), new Vector2Int(1,0), new Vector2Int(-2,-1), new Vector2Int(1,2), new Vector2Int(0,-2) },
        // R>>0
        { new Vector2Int(0,0), new Vector2Int(2,0), new Vector2Int(-1,0), new Vector2Int(2,1), new Vector2Int(-1,-2), new Vector2Int(0,2) },
        // R>>2
        { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(2,0), new Vector2Int(-1,2), new Vector2Int(2,-1), new Vector2Int(0,2) },
        // 2>>R
        { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(-2,0), new Vector2Int(1,-2), new Vector2Int(-2,1), new Vector2Int(0,-2) },
        // 2>>L
        { new Vector2Int(0,0), new Vector2Int(2,0), new Vector2Int(-1,0), new Vector2Int(2,1), new Vector2Int(-1,-2), new Vector2Int(0,2) },
        // L>>2
        { new Vector2Int(0,0), new Vector2Int(-2,0), new Vector2Int(1,0), new Vector2Int(-2,-1), new Vector2Int(1,2), new Vector2Int(0,-2) },
        // L>>0
        { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(-2,0), new Vector2Int(1,-2), new Vector2Int(-2,1), new Vector2Int(0,2) },
        // 0>>2
        { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(2,0), new Vector2Int(-1,2), new Vector2Int(2,-1), new Vector2Int(0,-2) },
    };
    // For O piece (no kicks)
    public static readonly Vector2Int[,] O_Offsets = new Vector2Int[1, 1]
    {
        { new Vector2Int(0,0) }
    };
    // Helper to get the correct offset array index
    public static int GetSRSIndex(int from, int to)
    {
        // 0: spawn, 1: right, 2: reverse, 3: left
        if (from == 0 && to == 1) return 0;
        if (from == 1 && to == 0) return 1;
        if (from == 1 && to == 2) return 2;
        if (from == 2 && to == 1) return 3;
        if (from == 2 && to == 3) return 4;
        if (from == 3 && to == 2) return 5;
        if (from == 3 && to == 0) return 6;
        if (from == 0 && to == 3) return 7;
        return 0;
    }
}