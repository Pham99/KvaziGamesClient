using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;

    private void Awake()
    {
        boardManager.OnPieceSet += HandlePieceSet;
    }

    // Called by TitrizzPlayer to move their piece
    public bool MovePiece(Piece piece, Vector2Int direction)
    {
        return boardManager.MovePiecePublic(piece, direction);
    }

    public void RotatePieceClockwise(Piece piece)
    {
        boardManager.RotatePieceClockwisePublic(piece);
    }

    public void RotatePieceCounterClockwise(Piece piece)
    {
        boardManager.RotatePieceCounterClockwisePublic(piece);
    }

    public void DropPiece(Piece piece)
    {
        boardManager.DropPiecePublic(piece);
    }

    // Called by TitrizzPlayer to get a new piece
    public Piece SpawnNewPieceForPlayer(int playerNumber)
    {
        return boardManager.SpawnNewPiece(playerNumber);
    }

    public void SetPieceInPlace(Piece piece)
    {
        boardManager.SetPieceInPlace(piece);
    }

    public bool IsPieceGrounded(Piece piece)
    {
        return boardManager.IsPieceGrounded(piece);
    }

    public void RemovePiece(Piece piece)
    {
        boardManager.RemovePiece(piece);
    }

    // Handle when a piece is set in place
    private void HandlePieceSet(Piece piece, int playerId)
    {
        // Use the new FindObjectsByType API
        var players = Object.FindObjectsByType<TitrizzPlayer>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            if (player.PlayerNumber == playerId)
            {
                player.OnPieceSet(); // This method should be implemented in TitrizzPlayer
                break;
            }
        }
    }
}
