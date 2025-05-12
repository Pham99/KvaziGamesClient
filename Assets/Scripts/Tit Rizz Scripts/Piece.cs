using System;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public int[,] shape;
    public Color color;
    public Vector2Int position;
    public Piece(Color color, Vector2Int position)
    {
        this.color = color;
        this.position = position;
        Array values = Enum.GetValues(typeof(TetronimoType));
        TetronimoType randomTetronimo = (TetronimoType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        shape = TetronimoFactory.tetronimos[randomTetronimo];
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
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 }
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