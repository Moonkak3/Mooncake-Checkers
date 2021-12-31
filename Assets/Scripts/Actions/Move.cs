using UnityEngine;

public class Move : Action
{
    private GameObject movedTokenPrefab;
    private JumpBoost jumpBoost;
    private Vector2Int start, end;
    private bool firstMove;

    public Move(GameObject movedTokenPrefab, Vector2Int start, Vector2Int end, bool firstMove)
    {
        this.movedTokenPrefab = movedTokenPrefab;
        this.start = start;
        this.end = end;
        this.firstMove = firstMove;
    }

    public void DoAction()
    {
        Map.MoveToken(start, end);
        Board.SetSpot(end, Board.GetSpot(start));
        Board.SetSpot(start, 0);
        Board.Turn = movedTokenPrefab.GetComponent<Token>().colorInt;
        Board.movedTokenPrefab = movedTokenPrefab;
    }

    public void UndoAction()
    {
        Map.MoveToken(end, start);
        Board.SetSpot(start, Board.GetSpot(end));
        Board.SetSpot(end, 0);
        if (firstMove)
        {
            Board.Turn = -1;
        }
    }
}