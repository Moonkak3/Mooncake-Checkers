using System.Collections.Generic;
using UnityEngine;

public class Vaccinate : Action
{
    private List<GameObject> tokens;

    public Vaccinate(int color)
    {
        tokens = new List<GameObject>();
        for (int x=0; x<Board.GetBoardLength(); x++)
        {
            for (int y=0; y<Board.GetBoardLength(); y++)
            {
                Vector2Int coords = new Vector2Int(x, y);
                GameObject go = Map.GetToken(coords);
                if (go != null && go.GetComponent<Token>() != null 
                    && go.GetComponent<Token>().colorInt == color)
                {
                    tokens.Add(go);
                }
            }
        }
    }

    public void DoAction()
    {
        foreach (GameObject token in tokens)
        {
            token.transform.GetChild(0).gameObject.SetActive(true);
            token.GetComponent<Token>().vaccinated = true;
        }
    }

    public void UndoAction()
    {
        foreach (GameObject token in tokens)
        {
            token.transform.GetChild(0).gameObject.SetActive(false);
            token.GetComponent<Token>().vaccinated = false;
        }
    }
}