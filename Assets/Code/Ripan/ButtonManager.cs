using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ButtonManager : MonoBehaviour
{
    private int acuan = 0;
    private int id_button = 0;
    private int hitung = 0;

    public GameObject workspace;
    public List<GameObject> buttons;
    public List<Button> onOff;
    public float offsetX;

    public GameObject[] bottoms;

    public GameObject endButton;

    private List<GameObject> instantiatedButtons = new List<GameObject>();
    private List<string> labelPuzzle = new List<string>();

    private string path;

    void Start()
    {
        for(int i=0; i< bottoms.Length; i++)
        {
            bottoms[i].SetActive(false);
        }

        bottoms[0].SetActive(true);
    }

    private void PositionButtons()
    {
        for (int i = 0; i < instantiatedButtons.Count; i++)
        {
            RectTransform rectTransform = instantiatedButtons[i].GetComponent<RectTransform>();
            float buttonWidth = rectTransform.rect.width;
            float xPos = -487 + (buttonWidth * i) + (offsetX * i);
            rectTransform.localPosition = new Vector3(xPos, 92, 0);
        }
    }

    public void ClickButton(GameObject id)
    {
        buttons.Clear();
        buttons.Add(id);

        acuan++;

        if(acuan % 2 != 0)
        {
            bottoms[0].SetActive(false);

            if(id_button == 0)
            {
                bottoms[1].SetActive(true);
            }
        }

        else
        {
            hitung++;
            for(int i=0; i< bottoms.Length; i++)
            {
                bottoms[i].SetActive(false);
            }

            bottoms[0].SetActive(true);
        }

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

    public void GetIdButton(int id)
    {
        id_button = id;
    }

    public void ResetLabel()
    {
        buttons.Clear();
        labelPuzzle.Clear();

        // Destroy instantiated buttons and clear the list
        foreach (GameObject buttonInstance in instantiatedButtons)
        {
            Destroy(buttonInstance);
        }
        instantiatedButtons.Clear();

        for(int i=0; i<onOff.Count; i++)
        {
            onOff[i].interactable = true;
        }
    }

    public void LabelButton(string label)
    {
        labelPuzzle.Add(label);
    }

    public void PrintLabel()
    {
        string nilai = "";
        labelPuzzle.Add("S");
        GameObject buttonInstance = Instantiate(endButton, workspace.transform);
        instantiatedButtons.Add(buttonInstance);

        PositionButtons();

        for(int i=0; i<onOff.Count; i++)
        {
            onOff[i].interactable = false;
        }

        path = Application.dataPath + "/MyTextFile.txt";

        foreach(var label in labelPuzzle)
        {
            nilai += label;
        }

        WriteText(nilai);
    }

    private void WriteText(string content)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(content);
        writer.Close();

        //Re-import the file to update the reference in the editor
        UnityEditor.AssetDatabase.ImportAsset(path); 

        //Print the text from the file
        string text = File.ReadAllText(path);
        Debug.Log(text);
    }
}
