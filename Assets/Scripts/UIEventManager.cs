using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventManager : MonoBehaviour
{
    public GameObject HexMap;
    public Button btnNextTurn;

    private GameObject ActiveDisplay;

    void Start()
    {
        btnNextTurn.onClick.AddListener(HexMap.GetComponent<HexMap>().NextTurn);
        ActiveDisplay = null;
    }


    public void DeactivatePanels()
    {
        if (ActiveDisplay != null)
        {
            ActiveDisplay.transform.position = new Vector3(-300, 0, 0);
            ActiveDisplay = null;
        }
    }

    public void ShowHexDataPanel(HexDataSim hexData)
    {
        DeactivatePanels();
        // Show this panel
        ActiveDisplay = GameObject.Find("panelHexInspector").gameObject;
        ActiveDisplay.transform.position = new Vector3(0, 0, 0);

        // Get display panels
        GameObject hexDataPanel = GameObject.Find("displayCurrentFood").gameObject;
        Text displayCurrentFood = hexDataPanel.GetComponent<Text>();

        // Update displays
        displayCurrentFood.text = hexData.GetCurrentFood().ToString();
    }
}
