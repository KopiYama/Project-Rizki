using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject workspace;
    public List<GameObject> buttons;
    public float offsetX;

    private List<GameObject> instantiatedButtons = new List<GameObject>();

    void Start()
    {
        buttons.Clear();
    }

    private void PositionButtons()
    {
        for (int i = 0; i < instantiatedButtons.Count; i++)
        {
            RectTransform rectTransform = instantiatedButtons[i].GetComponent<RectTransform>();
            float buttonWidth = rectTransform.rect.width;
            float xPos = -627 + (buttonWidth * i) + (offsetX * i);
            rectTransform.localPosition = new Vector3(xPos, 0, 0);
        }
    }

    public void ClickButton(GameObject id)
    {
        buttons.Clear();
        buttons.Add(id);

        // Instantiate buttons as children of the workspace and disable their Button components
        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject buttonInstance = Instantiate(buttons[i], workspace.transform);
            buttonInstance.GetComponent<Button>().enabled = false;
            instantiatedButtons.Add(buttonInstance);
        }

        // Position buttons accordingly
        PositionButtons();
    }
}
