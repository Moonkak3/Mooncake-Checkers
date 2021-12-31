using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : Action
{
    private List<ChangeTile> changes;
    public Earthquake(float percentage)
    {
        changes = new List<ChangeTile>();
        List<Vector2Int> possibleCoords = new List<Vector2Int>();
        for (int x = 0; x < Board.GetBoardLength(); x++)
        {
            for (int z = 0; z < Board.GetBoardLength(); z++)
            {
                Vector2Int coords = new Vector2Int(x, z);
                if (Board.GetSpot(coords) == (int)Board.PieceTypes.Hole)
                {
                    possibleCoords.Add(coords);
                }
            }
        }

        int deleteNum = (int)Mathf.Ceil(percentage * possibleCoords.Count);
        for (int i = 0; i < deleteNum; i++)
        {
            if (possibleCoords.Count == 0) { break; }
            int index = Random.Range(0, possibleCoords.Count);
            ChangeTile ct = new ChangeTile(possibleCoords[index], Map.mapGenerator.invisHexPrefab, Map.mapGenerator.hexTilePrefab,
                    (int)Board.PieceTypes.Hole, (int)Board.PieceTypes.Empty);
            changes.Add(ct);
            possibleCoords.RemoveAt(index);
        }
    }
    public void DoAction()
    {
        foreach (ChangeTile ct in changes)
        {
            ct.DoAction();
        }
    }

    public void UndoAction()
    {
        foreach (ChangeTile ct in changes)
        {
            ct.UndoAction();
        }
    }
}
