using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Undo : MonoBehaviour
{
    private Button btn;
    public float waitTime = 0.5f;
    private Coroutine buttonDisabled = null;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(handleClick);
    }

    private void Update()
    {
        GetComponent<Image>().color = (MapGenerator.GetColorPairing()[Board.Turn] + Color.grey) / 2;
    }

    private void handleClick()
    {
        if (buttonDisabled != null)
            return;
        buttonDisabled = StartCoroutine(DisableButtonForSeconds(waitTime));
        Board.GetActions().Undo();
    }
    private IEnumerator DisableButtonForSeconds(float seconds)
    {
        // disable the button 
        btn.interactable = false;

        // wait for our amount of time to re-enable
        yield return new WaitForSeconds(seconds);

        // re-enable the button
        btn.interactable = true;

        // reset our reference to be called again
        buttonDisabled = null;
    }
}
