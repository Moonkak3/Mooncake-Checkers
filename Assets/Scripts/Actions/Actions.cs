using System.Collections.Generic;

public class Actions
{

    private List<Action> actions;
    private int actionIndex;

    public Actions()
    {
        actions = new List<Action>();
        actionIndex = -1;
    }

    public void Add(Action action)
    {
        if (Map.mapGenerator.rotating || Token.rising) { return; }
        actions = new List<Action>(actions.GetRange(0, actionIndex + 1));
        actions.Add(action);
        actionIndex++;
        actions[actionIndex].DoAction();
    }

    public void Undo()
    {
        if (Map.mapGenerator.rotating || Token.rising) { return; }
        if (actionIndex < 0)
        {
            return;
        }
        actions[actionIndex].UndoAction();
        actionIndex--;
    }

    public void Redo()
    {
        if (Map.mapGenerator.rotating) { return; }
        if (actionIndex + 1 >= actions.Count)
        {
            return;
        }
        actions[actionIndex + 1].DoAction();
        actionIndex++;
    }
}
