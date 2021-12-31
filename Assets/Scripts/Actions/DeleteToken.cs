using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteToken : Action
{
    private GameObject token;
    private Vector2Int coords;

    public DeleteToken(GameObject token, Vector2Int coords)
    {
        this.token = token;
        this.coords = coords;
    }
    public void DoAction()
    {
        token.SetActive(false);
        Board.SetSpot(coords, 0);
    }

    public void UndoAction()
    {
        token.SetActive(true);
        Board.SetSpot(coords, token.GetComponent<Token>().colorInt);
    }
}
