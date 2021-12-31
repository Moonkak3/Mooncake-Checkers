using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleHex : MonoBehaviour
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
            case Map.Tool.Add:
                if (transform.childCount != 0) { return; }
                GameObject hex = Map.mapGenerator.hexTilePrefab;
                GameObject invisHex = Map.mapGenerator.invisHexPrefab;
                Board.GetActions().Add(new ChangeTile(coords, invisHex, hex, 
                    (int)Board.PieceTypes.Hole, (int)Board.PieceTypes.Empty));
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.5f);
    }
}
