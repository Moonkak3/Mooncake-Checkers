using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;
    public GameObject TokenPrefab;
    public GameObject wallPrefab;
    public GameObject invisHexPrefab;
    public GameObject neutralField;
    public GameObject homeField;

    public bool rotating;
    public float separation;
    public AudioSource audioSource;


    private Dictionary<Vector3, Vector2Int> positionToCoords;
    private static Dictionary<int, Color> colorPairing = new Dictionary<int, Color>
        {
            {-1, Color.white},
            { 1, Color.black },
            { 2, Color.magenta },
            { 3, Color.red },
            { 4, Color.green },
            { 5, Color.blue },
            { 6, Color.yellow }
        };


    // Start is called before the first frame update
    void Start()
    {
        createHexTileMap();
        Map.mapGenerator = this;
        rotating = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void createHexTileMap()
    {
        if (Board.NumPlayers == -1)
        {
            Board.SetNumPlayers(2);
        }
        float centerIndex = (Board.GetBoardLength() - 1) / 2.0f;
        float offsetZ = (float) Math.Cos((Math.PI / 180) * 30);
        Map.GOboardTiles = new GameObject[13, 13];
        Map.GOboard = new GameObject[13, 13];
        Map.confetti = GetComponent<ParticleSystem>();

        for (int x=0; x < Board.GetBoardLength(); x++)
        {
            for (int z=0; z < Board.GetBoardLength(); z++)
            {
                Vector2Int coords = new Vector2Int(x, z);
                if (Board.GetSpot(coords) == -1) continue;

                float layoutZ = (z - centerIndex) * separation * offsetZ;
                float layoutX = (x - centerIndex + (z - centerIndex) / 2) * separation;

                GameObject tempHexTile = Instantiate(hexTilePrefab);
                GameObject tempToken = null;
                if (Board.GetStartingSpot(coords) != 0)
                {
                    tempHexTile.transform.parent = homeField.transform;
                    tempHexTile.GetComponent<Renderer>().material.color = (colorPairing[Board.GetStartingSpot(coords)] + Color.white) / 2.0f;
                    tempHexTile.GetComponent<Hexagon>().color = (colorPairing[Board.GetStartingSpot(coords)] + Color.white) / 2.0f;
                    if (Board.turnColors[Board.NumPlayers].Contains(Board.GetSpot(coords))){
                        tempToken = Instantiate(TokenPrefab);
                    }
                }
                else
                {
                    tempHexTile.GetComponent<Hexagon>().color = Map.neutralColor;
                    tempHexTile.GetComponent<Renderer>().material.color = Map.neutralColor;
                    tempHexTile.transform.parent = neutralField.transform;
                }

                tempHexTile.GetComponent<Outline>().enabled = false;
                Hexagon hexScript = tempHexTile.transform.GetComponent<Hexagon>();
                hexScript.coords = coords;
                tempHexTile.GetComponent<SmoothMovement>().MoveTo(new Vector3(layoutX, 0, layoutZ), 0.5f);

                if (tempToken != null)
                {
                    tempToken.transform.GetChild(0).gameObject.SetActive(false);
                    tempToken.transform.parent = tempHexTile.transform;
                    tempToken.GetComponent<Outline>().enabled = false;
                    Token tokenScript = tempToken.GetComponent<Token>();
                    tokenScript.SetColor(Board.GetSpot(coords));
                    tokenScript.coords = coords;
                    tokenScript.GetComponent<SmoothMovement>().MoveTo(Vector3.zero, 0.5f);
                    Map.GOboard[coords.x, coords.y] = tempToken;
                }

                Map.GOboardTiles[coords.x, coords.y] = tempHexTile;
            }
        }
        
    }

    public static GameObject cloneInvisHexPrefab()
    {
        return null;
    }
    
    public static Dictionary<int, Color> GetColorPairing()
    {
        return colorPairing;
    }

    private IEnumerator Rotate(float angle, float duration)
    {

        float startRotation = homeField.transform.eulerAngles.y;
        float endRotation = startRotation + angle;
        float t = 0.0f;

        positionToCoords = new Dictionary<Vector3, Vector2Int>();
        foreach (Transform child in homeField.transform)
        {
            Vector3 rounded = ((Vector3)Vector3Int.RoundToInt(child.position * 100)) / 100f;
            positionToCoords.Add(rounded, child.gameObject.GetComponent<Hexagon>().coords);
        }

        rotating = true;
        while (t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            homeField.transform.eulerAngles = new Vector3(homeField.transform.eulerAngles.x, yRotation,
            homeField.transform.eulerAngles.z);
            yield return null;
        }
        foreach (Transform child in homeField.transform)
        {
            Vector3Int test = Vector3Int.one;

            Vector3 rounded = ((Vector3)Vector3Int.RoundToInt(child.position * 100)) / 100f;
            Vector2Int coords = positionToCoords[rounded];
            child.gameObject.GetComponent<Hexagon>().coords = coords;
            if (child.childCount == 1)
            {
                child.GetChild(0).GetComponent<Token>().coords = coords;
            }
        }
        Map.updateBoards();
        Board.SelectedCoords = new Vector2Int(-1, -1);
        Board.Turn = -1;
        Board.movedTokenPrefab = null;
        rotating = false;

    }
    public void Tornado(float angle, float duration)
    {
        if (rotating) { return; }
        StartCoroutine(Rotate(angle, duration));
    }
}
