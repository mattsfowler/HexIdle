using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mouseRay, out hit))
            {
                GameObject hex = hit.transform.parent.gameObject;
                GameObject.Find("UIOverlay").GetComponent<UIEventManager>().ShowHexDataPanel(hex.GetComponent<HexDataSim>());
            }
            else
            {
                GameObject.Find("UIOverlay").GetComponent<UIEventManager>().DeactivatePanels();
            }
        }
    }
}
