using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public Tile blockTile;
    public Tilemap tilemap;
    public Grid grid;
    public int width = 10;
    public int height = 20;
    public int[,] board;
    public Color color;
    public bool isForBackground = false;
    private Piece currentPiece;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = new int[width, height];
        if (isForBackground)
        {
            FillBoard();
        }
        else
        {
            currentPiece = new Piece(Color.red, new Vector2Int(0, 0));
            InsertPiece(currentPiece);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isForBackground && currentPiece != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MovePiece(currentPiece, new Vector2Int(-1, 0));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MovePiece(currentPiece, new Vector2Int(1, 0));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MovePiece(currentPiece, new Vector2Int(0, 1));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                DropPiece(currentPiece);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RotatePiece(currentPiece);
            }
        }
    }
    public void InsertPiece(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            board[pos.x, pos.y] = 1;
        }
        DrawBoard();
    }
    public void RemovePiece(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            board[pos.x, pos.y] = 0;
        }
    }
    public bool TryMovePiece(Piece piece, Vector2Int direction)
    {
        // Check if the piece can move in the given direction
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            int newX = pos.x + direction.x;
            int newY = pos.y + direction.y;
            if (newX < 0 || newX >= width || newY < 0 || newY >= height)
            {
                Debug.Log("Cannot move to " + newX + ", " + newY);
                return false;
            }
        }
        return true;
    }
    public bool TryRotatePiece(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            int newX = pos.x;
            int newY = pos.y;
            if (newX < 0 || newX >= width || newY < 0 || newY >= height)
            {
                Debug.Log("Cannot rotate to " + newX + ", " + newY);
                return false;
            }
        }
        return true;
    }
    public bool MovePiece(Piece piece, Vector2Int direction)
    {
        if (TryMovePiece(piece, direction))
        {
            RemovePiece(piece);
            piece.position += direction;
            InsertPiece(piece);
            if (TrySetPieceInPlace(piece))
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public void RotatePiece(Piece piece)
    {
        RemovePiece(piece);
        piece.RotateClockWise();
        if (TryRotatePiece(piece))
        {
            RemovePiece(piece);
            InsertPiece(piece);
        }
        else
        {
            piece.RotateCounterClockWise();
            RemovePiece(piece);
            InsertPiece(piece);
        }
        TrySetPieceInPlace(piece);
    }
    public void DropPiece(Piece piece)
    {
        while (MovePiece(piece, new Vector2Int(0, 1))) { }
    }
    public void SetPieceAndMakeNewOne(Piece piece)
    {
        SetPieceInPlace(piece);
        currentPiece = new Piece(Color.blue, new Vector2Int(0, 0));
        InsertPiece(currentPiece);
    }
    public bool TrySetPieceInPlace(Piece piece)
    {
        if (IsPieceOnFloor(piece))
        {
            SetPieceInPlace(piece);
            currentPiece = new Piece(Color.blue, new Vector2Int(0, 0));
            InsertPiece(currentPiece);
            return true;
        }
        else if (IsPieceOnHeap(piece))
        {
            SetPieceInPlace(piece);
            currentPiece = new Piece(Color.blue, new Vector2Int(0, 0));
            InsertPiece(currentPiece);
            return true;
        }
        return false;
    }
    public void DrawBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (board[x, y] == 1)
                {
                    PlaceTile(x, y, blockTile, color);
                }
                else if (board[x, y] == 0)
                {
                    RemoveTile(x, y);
                }
                else if (board[x, y] >= 10)
                {
                    PlaceTile(x, y, blockTile, Color.white);
                }
            }
        }
    }
    public void SetPieceInPlace(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            board[pos.x, pos.y] += 10;
        }
        ClearLines();
        DrawBoard();
    }
    public void PlaceTile(int x, int y, TileBase tile, Color color)
    {
        Vector3Int pos = new Vector3Int(x, y, 0);
        tilemap.SetTile(pos, tile);
        tilemap.SetTileFlags(pos, TileFlags.None);
        tilemap.SetColor(pos, color);
    }

    public void RemoveTile(int x, int y)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), null);
    }

    public bool IsTileOccupied(int x, int y)
    {
        return tilemap.HasTile(new Vector3Int(x, y, 0));
    }

    public void FillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                board[x, y] = 1;
            }
        }
        DrawBoard();
    }

    private bool IsPieceOnFloor(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            if (pos.y >= height - 1)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPieceOnHeap(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            if (board[pos.x, pos.y + 1] >= 10)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (board[x, y] == 0)
            {
                return false;
            }
        }
        return true;
    }
    public bool IsLineEmpty(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (board[x, y] != 0)
            {
                return false;
            }
        }
        return true;
    }
    public void ClearLine(int y)
    {
        for (int i = 0; i < width; i++)
        {
            board[i, y] = 0;
        }
    }
    public void MoveLineDown(int y, int distance)
    {
        for (int x = 0; x < width; x++)
        {
            board[x, y + distance] = board[x, y];
        }
        ClearLine(y);
    }
    public void ClearLines()
    {
        int linesCleared = 0;
        for (int y = height - 1; y >= 0; y--)
        {
            if (IsLineEmpty(y))
            {
                Debug.Log("BREAAAAK");
                break;
            }
            if (IsLineFull(y))
            {
                Debug.Log("WHY ARE YOU GETTING CALLED?");
                ClearLine(y);
                linesCleared++;
            }
            else if (linesCleared > 0)
            {
                Debug.Log("MOOOVEEE");
                MoveLineDown(y, linesCleared);
            }
        }
    }
}
