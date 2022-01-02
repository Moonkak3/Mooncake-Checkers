using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public Vector2Int coords;
    public Color color;
    public bool safeEntryActivated = false;

    private void OnMouseEnter()
    {
        if (!GetComponent<Rigidbody>().isKinematic) { return; }
        GetComponent<Outline>().enabled = true;
    }

    private void OnMouseExit()
    {
        GetComponent<Outline>().enabled = false;
    }

    private void OnMouseDown()
    {
        if (Map.mapGenerator.rotating || GetComponent<SmoothMovement>().moving) { return; }
        if (!GetComponent<Rigidbody>().isKinematic) { return; }
        GameObject hex = Map.mapGenerator.hexTilePrefab;
        switch (Map.currTool)
        {
            case Map.Tool.Cursor:
                if (Board.SelectedCoords == new Vector2Int(-1, -1)) { return; }
                GameObject tokenPrefab = Map.GOboard[Board.SelectedCoords.x, Board.SelectedCoords.y];
                Board.Move(tokenPrefab, coords);
                break;
            case Map.Tool.Add:
                if (transform.childCount != 0 || color != Map.neutralColor || safeEntryActivated) { return; }
                GameObject wall = Map.mapGenerator.wallPrefab;
                Board.GetActions().Add(new ChangeTile(coords, hex, wall, (int) Board.PieceTypes.Empty, (int) Board.PieceTypes.Wall));
                break;
            case Map.Tool.Delete:
                if (transform.childCount != 0 || color != Map.neutralColor || safeEntryActivated) { return; }
                GameObject invisHex = Map.mapGenerator.invisHexPrefab;
                Board.GetActions().Add(new ChangeTile(coords, hex, invisHex, (int) Board.PieceTypes.Empty, (int)Board.PieceTypes.Hole));
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.5f);
    }
}
