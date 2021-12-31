using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public static bool rising;

    public int colorInt;
    private new Renderer renderer;
    public Vector2Int coords;
    public bool vaccinated;
    public bool jumpBoosted;

    private void Awake()
    {
        rising = false;
        transform.hasChanged = false;
        renderer = GetComponent<Renderer>();
        jumpBoosted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coords != Board.SelectedCoords)
        {
            if (transform.parent.GetComponent<Hexagon>().safeEntryActivated)
            {
                transform.parent.GetComponent<Renderer>().material.color = Map.vaccinatedColor;
            }
            else
            {
                transform.parent.GetComponent<Renderer>().material.color = transform.parent.GetComponent<Hexagon>().color;
            }
            GetComponent<SmoothMovement>().MoveTo(Vector3.zero, 0.2f);
        }
    }

    public void SetColor(int colorInt)
    {
        this.colorInt = colorInt;
        renderer.material.color = MapGenerator.GetColorPairing()[colorInt];
    }

    private void OnMouseEnter()
    {
        if (Board.Turn != colorInt && Board.Turn != -1)
        {
            return;
        }
        GetComponent<Outline>().enabled = true;
    }

    private void OnMouseExit()
    {
        GetComponent<Outline>().enabled = false;
    }

    public void OnMouseDown()
    {
        if (Map.mapGenerator.rotating || rising) { return; }
        switch (Map.currTool)
        {
            case Map.Tool.Cursor:
                if (Board.Turn != colorInt && Board.Turn != -1)
                {
                    return;
                }
                if (Board.SelectedCoords == coords)
                {
                    transform.parent.GetComponent<Renderer>().material.color = transform.parent.GetComponent<Hexagon>().color;
                    Board.SelectedCoords = new Vector2Int(-1, -1);
                    MoveTo(Vector3.zero, 0.2f);
                }
                else
                {
                    transform.parent.GetComponent<Renderer>().material.color *= 0.7f;
                    Board.SelectedCoords = coords;
                    if (jumpBoosted)
                    {
                        MoveTo(new Vector3(0, 4, 0), 0.2f);
                    }
                    else
                    {
                        MoveTo(new Vector3(0, 2, 0), 0.2f);
                    }
                }
                break;

            case Map.Tool.Add:
                if (jumpBoosted) { break; };
                Board.GetActions().Add(new JumpBoost(gameObject));
                if (Board.SelectedCoords == coords)
                {
                    MoveTo(new Vector3(0, 4, 0), 0.2f);
                }
                break;

            //case Map.Tool.Delete:
            //    Board.GetActions().Add(new DeleteToken(gameObject, coords));
            //    break;
            case Map.Tool.Vaccine:
                Board.GetActions().Add(new Vaccinate(colorInt));
                break;
        }
    }

    private IEnumerator MoveTo(Vector3 startCoords, Vector3 endCoords, float time)
    {
        rising = true;
        float currentTime = 0;
        while (Vector3.Distance(transform.localPosition, endCoords) > 0)
        {
            currentTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startCoords, endCoords, currentTime / time);
            yield return null;
        }
        rising = false;
    }

    public void MoveTo(Vector3 endCoords, float time)
    {
        StartCoroutine(MoveTo(transform.localPosition, endCoords, time));
    }
}
