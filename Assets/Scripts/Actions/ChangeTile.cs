using System.Collections;
using UnityEngine;

public class ChangeTile : Action
{
    private Vector2Int coords;
    private GameObject oldTile;
    private GameObject newTile;
    private int oldValue;
    private int newValue;
    public ChangeTile(Vector2Int coords, GameObject oldTile, GameObject newTile, int oldValue, int newValue)
    {
        this.coords = coords;
        this.oldTile = oldTile;
        this.newTile = newTile;
        this.oldValue = oldValue;
        this.newValue = newValue;
    }
    public void DoAction()
    {
        Map.SetTile(coords, MonoBehaviour.Instantiate(newTile));
        Board.SetSpot(coords, newValue);
    }

    public void UndoAction()
    {
        Map.SetTile(coords, MonoBehaviour.Instantiate(oldTile));
        Board.SetSpot(coords, oldValue);
    }
}