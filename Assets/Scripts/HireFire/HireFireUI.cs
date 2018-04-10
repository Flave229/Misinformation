using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HireFireUI : MonoBehaviour {

    public GameObject PanelCurrent;
    public GameObject PanelHire;
    public GameObject CanvasHireFire;
    public GameObject UIClock;
    private bool _showCanvas;


    void Update()
    {
        _showCanvas = CanvasHireFire.activeSelf;

        if (_showCanvas)
        {
            Time.timeScale = 0;
            //UIClock.SetActive(false);
            Assets.Scripts.Timer.Instance().Pause();    //pauses the time
        }
        else
        {
            Time.timeScale = 1;
            //UIClock.SetActive(true);
        }
    }

    public void SetActiveCurrent()
    {
        if (PanelCurrent.activeSelf == false)
        {
            PanelCurrent.SetActive(true);
            PanelHire.SetActive(false);
            Resources.FindObjectsOfTypeAll<FireTechs>().ToList().First().GetComponent<FireTechs>().OnActive();
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
            Assets.Scripts.Timer.Instance().Play();     //unpauses the time
        }
    }

}
