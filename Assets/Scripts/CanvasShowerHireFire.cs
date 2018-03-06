using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class CanvasShowerHireFire : MonoBehaviour {

    public GameObject HireFireCanvas;
    private Toggle _toggle;

	void Start ()
    {
        //_objectRect = this.GetComponent<RectTransform>().rect; //Rect isnt the rect of the billboard object.
        _toggle = GetComponent<Toggle>();
	}

    void Update()
    {
        if (HireFireCanvas.activeSelf == false)
            HireFireCanvas.SetActive(true);
        else
            HireFireCanvas.SetActive(false);
    }
}
