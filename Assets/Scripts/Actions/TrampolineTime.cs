using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTime : Action
{
    private List<JumpBoost> jumpBoosts;
    public TrampolineTime()
    {
        jumpBoosts = new List<JumpBoost>();
        foreach (GameObject go in Map.GOboard)
        {
            if (go != null) { jumpBoosts.Add(new JumpBoost(go)); }
        }
    }
    void Action.DoAction()
    {
        foreach (JumpBoost jb in jumpBoosts)
        {
            jb.DoAction();
        }
    }

    void Action.UndoAction()
    {
        foreach (JumpBoost jb in jumpBoosts)
        {
            jb.UndoAction();
        }
    }

}
