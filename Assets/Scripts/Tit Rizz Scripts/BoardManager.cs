using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public Tile blockTile;
    public Tilemap tilemap;
    public int width = 16;
    public int height = 20;
    public int[,] board = new int[16, 20]; // 0 = empty, 1-9 = player pieces, 10+ = set pieces
    public PauseMenu gameManager; // Reference to the PauseMenu for game over handling
    public ParticlePool dustParticlePool;
    public ParticlePool clearParticlePool;
    public bool gameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    void Update()
    {
        if (gameOver)
        {
            gameManager.GameOver();
            return;
        }
    }

    public delegate void PieceSetHandler(Piece piece, int playerId);
    public event PieceSetHandler OnPieceSet;

    // Add method to spawn a new piece for a player
    public Piece SpawnNewPiece(int playerNumber)
    {
        int pieceWidth = 4; // Max width for I piece
        int spawnY = height - 2;
        System.Random rand = new System.Random();
        int maxAttempts = width * 2; // Try all possible x and y positions
        for (int y = spawnY; y >= 0; y--)
        {
            // Try random x first, then sweep left and right
            int startX = rand.Next(0, width - pieceWidth + 1);
            for (int offset = 0; offset < width; offset++)
            {
                int x = (startX + offset) % (width - pieceWidth + 1);
                Piece newPiece = new Piece(playerNumber, new Vector2Int(x, y));
                if (IsValidPosition(newPiece))
                {
                    InsertPiece(newPiece);
                    return newPiece;
                }
            }
        }
        // If no valid spot found, game over
        gameOver = true;
        Debug.Log("Game Over: No valid spawn position for new piece.");
        return null;
    }

    public void InsertPiece(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            Debug.Log("piece:" + piece + " pos:" + pos + " id:" + piece.id);
            board[pos.x, pos.y] = piece.id;
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

    private bool TryMovePiece(Piece piece, Vector2Int direction)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            int newX = pos.x + direction.x;
            int newY = pos.y + direction.y;
            if (newX < 0 || newX >= width || newY < 0 || newY >= height)
            {
                Debug.Log("Cannot move to " + newX + ", " + newY);
                return false;
            }
            // Block if cell is occupied by another piece (not self)
            if (board[newX, newY] != 0 && board[newX, newY] != piece.id)
            {
                Debug.Log("Blocked by another piece at " + newX + ", " + newY);
                return false;
            }
        }
        return true;
    }
    private bool TryRotatePiece(Piece piece)
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
    private bool MovePiece(Piece piece, Vector2Int direction)
    {
        if (TryMovePiece(piece, direction))
        {
            RemovePiece(piece);
            piece.position += direction;
            InsertPiece(piece);
            return true;
        }
        return false;
    }
    private void RotatePieceClockwise(Piece piece)
    {
        RemovePiece(piece);
        int oldRotation = piece.rotationState;
        int newRotation = (oldRotation + 1) % 4;
        int srsIndex = SRSData.GetSRSIndex(oldRotation, newRotation);
        int[,] originalShape = piece.shape;
        Vector2Int originalPosition = piece.position;
        piece.RotateClockWise();
        Vector2Int[,] offsets;
        if (piece.type == TetronimoType.I)
            offsets = SRSData.I_Offsets;
        else if (piece.type == TetronimoType.O)
            offsets = SRSData.O_Offsets;
        else
            offsets = SRSData.JLSTZ_Offsets;
        bool rotated = false;
        for (int i = 0; i < offsets.GetLength(1); i++)
        {
            Vector2Int offset = offsets[srsIndex, i];
            // No y-inversion needed now
            piece.position = originalPosition + offset;
            if (IsValidPosition(piece))
            {
                rotated = true;
                break;
            }
        }
        if (!rotated)
        {
            piece.RotateCounterClockWise();
            piece.position = originalPosition;
        }
        InsertPiece(piece);
    }
    private void RotatePiece(Piece piece)
    {
        TryRotateWithSRS(piece, true);
    }
    private void RotatePieceCounterClockwise(Piece piece)
    {
        TryRotateWithSRS(piece, false);
    }

    private bool TryRotateWithSRS(Piece piece, bool clockwise)
    {
        RemovePiece(piece);
        int oldRotation = piece.rotationState;
        int newRotation = clockwise ? (oldRotation + 1) % 4 : (oldRotation + 3) % 4;
        int srsIndex = SRSData.GetSRSIndex(oldRotation, newRotation);
        Vector2Int originalPosition = piece.position;
        if (clockwise)
            piece.RotateClockWise();
        else
            piece.RotateCounterClockWise();
        Vector2Int[,] offsets;
        if (piece.type == TetronimoType.I)
            offsets = SRSData.I_Offsets;
        else if (piece.type == TetronimoType.O)
            offsets = SRSData.O_Offsets;
        else
            offsets = SRSData.JLSTZ_Offsets;
        bool rotated = false;
        for (int i = 0; i < offsets.GetLength(1); i++)
        {
            Vector2Int offset = offsets[srsIndex, i];
            piece.position = originalPosition + offset;
            if (IsValidPosition(piece))
            {
                rotated = true;
                break;
            }
        }
        if (!rotated)
        {
            if (clockwise)
                piece.RotateCounterClockWise();
            else
                piece.RotateClockWise();
            piece.position = originalPosition;
        }
        InsertPiece(piece);
        return rotated;
    }

    private bool IsValidPosition(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
                return false;
            // Block if cell is occupied by another piece (not self)
            if (board[pos.x, pos.y] != 0 && board[pos.x, pos.y] != piece.id)
                return false;
        }
        return true;
    }

    private void DropPiece(Piece piece)
    {
        // Only move down as far as valid, do not set in place if blocked by another active piece
        while (TryMovePiece(piece, new Vector2Int(0, -1)))
        {
            MovePiece(piece, new Vector2Int(0, -1));
        }
        // After dropping, check if grounded (on floor or set piece)
        if (IsPieceGrounded(piece))
        {
            SetPieceInPlace(piece);
        }
        else
        {
            // If not grounded, do not set in place (should never happen, but safety)
            InsertPiece(piece);
        }
    }
    public void SetPieceAndMakeNewOne(Piece piece)
    {
        SetPieceInPlace(piece);
        int spawnX = width / 2 - 1;
        int spawnY = height - 2;
        Piece newPiece = new Piece(piece.id, new Vector2Int(spawnX, spawnY));
        InsertPiece(newPiece);
    }
    public void DrawBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int cellValue = board[x, y];
                if (cellValue == 0)
                {
                    RemoveTile(x, y);
                }
                else if (cellValue < 10)
                {
                    PlaceTile(x, y, blockTile, PLayerColours.GetColour(cellValue));
                }
                else if (board[x, y] >= 10)
                {
                    PlaceTile(x, y, blockTile, PLayerColours.GetColour(cellValue - 10) * 0.8f); // Slightly faded color for set pieces
                }
            }
        }
    }
    public void SetPieceInPlace(Piece piece)
    {
        List<Vector2Int> contactPositions = new List<Vector2Int>();
        // First, check for heap/floor contact before marking as set
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            if (pos.y == 0 || (pos.y > 0 && board[pos.x, pos.y - 1] >= 10))
            {
                contactPositions.Add(pos);
            }
        }
        // Now mark the cells as set
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            board[pos.x, pos.y] = piece.id + 10;
        }
        ClearLines();
        DrawBoard();
        SpawnContactParticles(contactPositions, piece.id);
        OnPieceSet?.Invoke(piece, piece.id);
    }

    private void SpawnContactParticles(List<Vector2Int> positions, int pieceId)
    {
        Color color = PLayerColours.GetColour(pieceId);
        foreach (var pos in positions)
        {
            Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(pos.x, pos.y, 0));
            worldPos.x += 0.5f;
            var particle = dustParticlePool.GetParticle(worldPos, Quaternion.Euler(-90, 0, 0));
            var main = particle.GetComponent<ParticleSystem>().main;
            main.startColor = color;
            dustParticlePool.ReturnParticle(particle, main.duration + main.startLifetime.constantMax);
        }
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

    public bool IsPieceGrounded(Piece piece)
    {
        foreach (Vector2Int pos in piece.GetOccupiedPositions())
        {
            if (pos.y == 0 || (pos.y > 0 && board[pos.x, pos.y - 1] >= 10))
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
        for (int x = 0; x < width; x++)
        {
            board[x, y] = 0;
        }
    }
        public void ClearLineWithParticles(int y)
    {
        List<Vector2Int> clearedPositions = new List<Vector2Int>();
        for (int x = 0; x < width; x++)
        {
            clearedPositions.Add(new Vector2Int(x, y));
            board[x, y] = 0;
        }
        SpawnLineClearParticles(clearedPositions);
    }

    private void SpawnLineClearParticles(List<Vector2Int> positions)
    {
        foreach (var pos in positions)
        {
            Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(pos.x, pos.y, 0));
            var particle = clearParticlePool.GetParticle(worldPos, Quaternion.Euler(-90, 0, 0));
            var main = particle.GetComponent<ParticleSystem>().main;
            clearParticlePool.ReturnParticle(particle, main.duration + main.startLifetime.constantMax);
        }
    }
    public void MoveLineDown(int y, int distance)
    {
        for (int x = 0; x < width; x++)
        {
            board[x, y - distance] = board[x, y];
        }
        ClearLine(y);
    }
    public void ClearLines()
    {
        int linesCleared = 0;
        for (int y = 0; y < height; y++) // bottom to top
        {
            if (IsLineEmpty(y))
            {
                break;
            }
            if (IsLineFull(y))
            {
                ClearLineWithParticles(y);
                linesCleared++;
            }
            else if (linesCleared > 0)
            {
                MoveLineDown(y, linesCleared);
            }
        }
    }

    // Add public wrappers for BoardController
    public bool MovePiecePublic(Piece piece, Vector2Int direction)
    {
        return MovePiece(piece, direction);
    }
    public void RotatePieceCounterClockwisePublic(Piece piece)
    {
        RotatePiece(piece);
    }
    public void RotatePieceClockwisePublic(Piece piece)
    {
        RotatePieceCounterClockwise(piece);
    }
    public void DropPiecePublic(Piece piece)
    {
        DropPiece(piece);
    }
    
}
