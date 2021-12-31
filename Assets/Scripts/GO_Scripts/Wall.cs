using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Vector2Int coords;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //tokenPrefab.transform.localPosition = this.transform.localPosition;
    }

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
        switch (Map.currTool)
        {
            case Map.Tool.Delete:
                GameObject hex = Map.mapGenerator.hexTilePrefab;
                GameObject wall = Map.mapGenerator.wallPrefab;
                Board.GetActions().Add(new ChangeTile(coords, wall, hex, (int)Board.PieceTypes.Wall, (int)Board.PieceTypes.Empty));
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.5f);
    }
}
