using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HireFireUI : MonoBehaviour {

    public GameObject PanelCurrent;
    public GameObject PanelHire;
    public GameObject CanvasHireFire;
    private bool _showCanvas;

    void Start()
    {

    }

    private void Awake()
    {

    }

    void Update()
    {
        _showCanvas = CanvasHireFire.activeSelf;

        if (_showCanvas)
        {
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SetActiveCurrent()
    {
        if (PanelCurrent.activeSelf == false)
        {
            PanelCurrent.SetActive(true);
            PanelHire.SetActive(false);
        }
        else
            PanelCurrent.SetActive(false);
    }
    public void SetActiveHire()
    {
        if (PanelHire.activeSelf == false)
        {
            PanelHire.SetActive(true);
            PanelCurrent.SetActive(false);

        }
        else
            PanelHire.SetActive(false);
    }

    public void SetCanvasInactive()
    {
        if (CanvasHireFire.activeSelf == true)
        {
            CanvasHireFire.SetActive(false);
        }
    }

}
