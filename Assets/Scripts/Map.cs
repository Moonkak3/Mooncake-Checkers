using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map
{
    public static GameObject[,] GOboardTiles { get; set; }
    public static GameObject[,] GOboard { get; set; }
    public static ParticleSystem confetti;
    public static Color homeColor = new Color(0.8f, 0.9f, 0.95f);
    public static Color neutralColor = Color.white;
    public static Color vaccinatedColor = new Color(0.0f, 0.5f, 0.3f);
    public static MapGenerator mapGenerator;
    public enum Tool
    {
        Cursor,
        Add,
        Delete,
        Vaccine
    }
    public static Tool currTool;
    
    public static void MoveToken(Vector2Int startCoords, Vector2Int endCoords)
    {

        GameObject tokenPrefab = GOboard[startCoords.x, startCoords.y];
        GameObject hexPrefab = GOboardTiles[endCoords.x, endCoords.y];
        Hexagon hexagon = tokenPrefab.transform.parent.GetComponent<Hexagon>();
        Color originalColor;
        if (hexagon.safeEntryActivated)
        {
            originalColor = vaccinatedColor;
        }
        else
        {
            originalColor = hexagon.color;
        }
        tokenPrefab.transform.parent.GetComponent<Renderer>().material.color = originalColor;
        tokenPrefab.transform.parent = hexPrefab.transform;
        tokenPrefab.GetComponent<Token>().coords = endCoords;

        GOboard[endCoords.x, endCoords.y] = GOboard[startCoords.x, startCoords.y];
        GOboard[startCoords.x, startCoords.y] = null;

        Token tokenScript = tokenPrefab.GetComponent<Token>();
        tokenScript.OnMouseDown();

        hexPrefab.transform.parent.parent.GetComponent<MapGenerator>().audioSource.Play();

    }

    public static GameObject GetTile(Vector2Int coords)
    {
        return GOboardTiles[coords.x, coords.y];
    }

    public static void SetTile(Vector2Int coords, GameObject tile)
    {
        Vector3 pos = GOboardTiles[coords.x, coords.y].transform.localPosition;
        pos = new Vector3(pos.x, 0, pos.z);
        tile.SetActive(true);
        tile.GetComponent<Rigidbody>().isKinematic = true;
        tile.transform.parent = GOboardTiles[coords.x, coords.y].transform.parent;
        tile.transform.localPosition = pos + Vector3.up;
        tile.transform.localRotation = GOboardTiles[coords.x, coords.y].transform.localRotation;

        if (tile.GetComponent<InvisibleHex>() != null)
        {
            tile.GetComponent<InvisibleHex>().coords = coords;
        }
        else if (tile.GetComponent<Wall>() != null)
        {
            tile.GetComponent<Wall>().coords = coords;
        }
        else if (tile.GetComponent<Hexagon>() != null)
        {
            tile.GetComponent<Hexagon>().coords = coords;
            tile.GetComponent<Hexagon>().color = neutralColor;
        }
        tile.GetComponent<SmoothMovement>().MoveTo(pos, 0.5f);

        GOboardTiles[coords.x, coords.y].GetComponent<Rigidbody>().isKinematic = false;
        GOboardTiles[coords.x, coords.y] = tile;
    }

    public static GameObject GetToken(Vector2Int coords)
    {
        return GOboard[coords.x, coords.y];
    } 

    public static void playConfetti()
    {
        confetti.Play();
    }

    public static void updateBoards()
    {
        GameObject[,] newGOTiles = new GameObject[13, 13];
        foreach (GameObject go in GOboardTiles)
        {
            if (go == null) { continue; }

            Vector2Int coords = new Vector2Int(-1, -1);
            if (go.GetComponent<InvisibleHex>() != null)
            {
                coords = go.GetComponent<InvisibleHex>().coords;
                Board.SetSpot(coords, (int)Board.PieceTypes.Hole);
            }
            else if (go.GetComponent<Wall>() != null)
            {
                coords = go.GetComponent<Wall>().coords;
                Board.SetSpot(coords, (int)Board.PieceTypes.Wall);
            }
            else if (go.GetComponent<Hexagon>() != null)
            {
                coords = go.GetComponent<Hexagon>().coords;
                if (go.GetComponent<Hexagon>().safeEntryActivated)
                {
                    Board.SetSpot(coords, (int)Board.PieceTypes.SafeEntry);
                }
                else
                {
                    Board.SetSpot(coords, (int)Board.PieceTypes.Empty);
                }
            }
            if (coords != new Vector2Int(-1, -1))
            {
                newGOTiles[coords.x, coords.y] = go;
            }
        }
        GOboardTiles = newGOTiles;

        GameObject[,] newGOs = new GameObject[13, 13];
        foreach (GameObject go in GOboard)
        {
            if (go == null) { continue; }
            Token token = go.GetComponent<Token>();
            newGOs[token.coords.x, token.coords.y] = go;
            Board.SetSpot(token.coords, token.colorInt);
        }
        GOboard = newGOs;
    }
}
