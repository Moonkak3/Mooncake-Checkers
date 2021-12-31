using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : Action
{
    public Tornado()
    {
    }
    public void DoAction()
    {
        Map.mapGenerator.Tornado(-60, 1);
        
    }

    public void UndoAction()
    {
        Map.mapGenerator.Tornado(60, 1);
    }

}
