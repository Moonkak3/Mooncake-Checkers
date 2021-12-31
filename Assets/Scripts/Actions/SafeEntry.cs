using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeEntry : Action
{
    private List<Vector2Int> safeEntries;
    
    public SafeEntry(int numSafeEntries)
    {
        safeEntries = new List<Vector2Int>();

        List<Vector2Int> possibleCoords = new List<Vector2Int>();
        for (int x = 0; x < Board.GetBoardLength(); x++)
        {
            for (int z = 0; z < Board.GetBoardLength(); z++)
            {
                Vector2Int coords = new Vector2Int(x, z);
                if (Board.GetSpot(coords) == (int)Board.PieceTypes.Empty)
                {
                    Hexagon hexagon = Map.GetTile(coords).GetComponent<Hexagon>();
                    if (hexagon.color != Map.homeColor && !hexagon.safeEntryActivated)
                    {
                        possibleCoords.Add(coords);
                    }
                }
            }
        }

        for (int i=0; i<numSafeEntries; i++)
        {
            if (possibleCoords.Count == 0) { break; }
            int index = Random.Range(0, possibleCoords.Count);
            safeEntries.Add(possibleCoords[index]);
            possibleCoords.RemoveAt(index);
        }
    }

    public void DoAction()
    {
        foreach (Vector2Int coords in safeEntries)
        {
            Map.GetTile(coords).GetComponent<Renderer>().material.color = Map.vaccinatedColor;
            Map.GetTile(coords).GetComponent<Hexagon>().safeEntryActivated = true;
            Board.SetSpot(coords, (int)Board.PieceTypes.SafeEntry);
        }
    }

    public void UndoAction()
    {
        foreach (Vector2Int coords in safeEntries)
        {
            Map.GetTile(coords).GetComponent<Renderer>().material.color = Map.GetTile(coords).GetComponent<Hexagon>().color;
            Map.GetTile(coords).GetComponent<Hexagon>().safeEntryActivated = false;
            Board.SetSpot(coords, (int)Board.PieceTypes.Empty);
        }
    }
}
