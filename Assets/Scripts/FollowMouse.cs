using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class FollowMouse : MonoBehaviour {

		Vector3 currentMousePos = new Vector3();
		Text text;
		public string tooltipText;
		public bool isEntered {get; set;}

		// Use this for initialization
		void Start () {
			text = this.gameObject.GetComponent<Text> ();
			text.text = "";
			tooltipText = "";
			isEntered = false;
		}
		
		// Update is called once per frame
		void Update () {
			if (isEntered == false) {
				tooltipText = "";
			}
			text.text = tooltipText; 
			currentMousePos = Input.mousePosition;
			currentMousePos.y += 10.0f;
			currentMousePos.x += 150.0f;
			this.transform.position = currentMousePos;
		}
			
		public void UpdateText(string newText)
		{
			isEntered = true;
			tooltipText = newText;
		}
	}
}