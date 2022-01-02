using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : Action
{
    private GameObject lastMovedGO;
    private int prevTurn;
    private JumpBoost jumpBoost;
    private bool walked;

    public Turn(GameObject lastMovedGO, int prevTurn, bool walked)
    {
        this.lastMovedGO = lastMovedGO;
        this.prevTurn = prevTurn;
        this.walked = walked;

        if (lastMovedGO.GetComponent<Token>().jumpBoosted)
        {
            jumpBoost = new JumpBoost(lastMovedGO);
        }
    }
    public void DoAction()
    {
        Board.SelectedCoords = new Vector2Int(-1, -1);
        Board.Turn = -1;
        Board.movedTokenPrefab = null;
        Board.walked = false;
        if (jumpBoost != null) { jumpBoost.UndoAction(); }
    }

    public void UndoAction()
    {
        Board.SelectedCoords = lastMovedGO.GetComponent<Token>().coords;
        Board.movedTokenPrefab = lastMovedGO;
        Board.Turn = prevTurn;
        Board.walked = walked;
        if (jumpBoost != null) { jumpBoost.DoAction(); }
    }
}
