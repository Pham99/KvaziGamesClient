using Mygame.NetInput;
using UnityEngine;

public class TitrizzPlayer : MonoBehaviour
{
    [SerializeField]
    public string Id { get; private set; }
    [SerializeField]
    public string Name { get; private set; }
    [SerializeField]
    public int PlayerNumber { get; set; } // 1-8
    private InputMap netInput;
    private BoardController boardController;
    public Piece CurrentPiece { get; private set; }
    private float lockDelay = 0.5f;
    private float lockTimer = 0f;
    private bool isLocking = false;

    public void Init(string id, string name, int playerNumber, BoardController boardController)
    {
        Id = id;
        Name = name;
        PlayerNumber = playerNumber;
        this.boardController = boardController;
        netInput = NetInput.GetInputMap(Id);
        CurrentPiece = boardController.SpawnNewPieceForPlayer(PlayerNumber);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (netInput == null || CurrentPiece == null)
        {
            return;
        }
        // Input handling
        if (netInput.GetKeyDown(NetKeyCode.Left))
        {
            boardController.MovePiece(CurrentPiece, new Vector2Int(-1, 0));
        }
        if (netInput.GetKeyDown(NetKeyCode.Right))
        {
            boardController.MovePiece(CurrentPiece, new Vector2Int(1, 0));
        }
        if (netInput.GetKeyDown(NetKeyCode.Down))
        {
            boardController.MovePiece(CurrentPiece, new Vector2Int(0, -1));
        }
        if (netInput.GetKeyDown(NetKeyCode.A))
        {
            boardController.DropPiece(CurrentPiece);
            // Only set in place if grounded (touching floor or set piece)
            if (boardController.IsPieceGrounded(CurrentPiece))
            {
                boardController.SetPieceInPlace(CurrentPiece); // Instantly set in place
            }
            isLocking = false;
            lockTimer = 0f;
            return;
        }
        if (netInput.GetKeyDown(NetKeyCode.X))
        {
            boardController.RotatePieceClockwise(CurrentPiece);
        }
        if (netInput.GetKeyDown(NetKeyCode.Up))
        {
            boardController.RotatePieceClockwise(CurrentPiece);
        }
        if (netInput.GetKeyDown(NetKeyCode.Y))
        {
            boardController.RotatePieceCounterClockwise(CurrentPiece);
        }

        // Lock delay and grounded check
        if (IsPieceGrounded())
        {
            if (!isLocking)
            {
                isLocking = true;
                lockTimer = 0f;
            }
            else
            {
                lockTimer += Time.deltaTime;
                if (lockTimer >= lockDelay)
                {
                    boardController.SetPieceInPlace(CurrentPiece);
                    isLocking = false;
                    lockTimer = 0f;
                }
            }
        }
        else
        {
            isLocking = false;
            lockTimer = 0f;
        }
    }

    private bool IsPieceGrounded()
    {
        return boardController.IsPieceGrounded(CurrentPiece);
    }

    public void OnPieceSet()
    {
        CurrentPiece = boardController.SpawnNewPieceForPlayer(PlayerNumber);
    }
}
