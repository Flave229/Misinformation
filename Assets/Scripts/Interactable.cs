using UnityEngine;

namespace Assets.Scripts
{
    public class Interactable : MonoBehaviour {

        //public Vector3 mouseLocation;
        [System.Serializable]
        public class Action
        {
            public Color color;
            public Sprite sprite;
            public string title;
            public string objectName;
        }

        public Action[] options;
        private void OnMouseOver()
        {
            if(Input.GetMouseButtonDown(1))
            {
                RadialMenuSpawner.ins.SpawnMenu(this);
            }
        }
    }
}
