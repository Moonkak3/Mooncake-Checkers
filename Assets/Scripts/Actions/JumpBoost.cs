using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoost : Action
{
    private static Vector3 scale = new Vector3(1 / 1.4f, 1 / 1.4f, 1 / 1.4f);
    private GameObject token;
    public JumpBoost(GameObject token)
    {
        this.token = token;
    }
    public void DoAction()
    {
        token.GetComponent<Token>().jumpBoosted = true;
        token.transform.localScale = Vector3.one;
    }

    public void UndoAction()
    {
        int color = token.GetComponent<Token>().colorInt;
        foreach (GameObject go in Map.GOboard)
        {
            if (go != null && go.GetComponent<Token>().colorInt == color)
            {
                go.GetComponent<Token>().jumpBoosted = false;
                go.transform.localScale = scale;
            }
        }
    }
}
