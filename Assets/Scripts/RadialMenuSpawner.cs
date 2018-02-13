using UnityEngine;

namespace Assets.Scripts
{
    public class RadialMenuSpawner : MonoBehaviour {

        public static RadialMenuSpawner ins;
        public RadialMenu menuPrefab;
        public Vector3 mouseLocation;

        void Awake()
        {
            ins = this;
        }
	
        public void SpawnMenu(Interactable obj)
        {
            RadialMenu newMenu = Instantiate(menuPrefab) as RadialMenu;
            newMenu.transform.SetParent(transform, false);
            newMenu.transform.position = Input.mousePosition;
            newMenu.mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newMenu.DrawingLine = true;
            newMenu.SpawnButtons(obj);
        }
    }
}
