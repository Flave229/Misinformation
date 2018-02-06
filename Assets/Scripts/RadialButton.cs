using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RadialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image circle;
        public Image icon;
        public string title;
        public RadialMenu myMenu;
        public float speed = 8f;
        Color defaultColor;

        public void AnimateIn()
        {
            StartCoroutine(AnimateButtonIn());

        }

        IEnumerator AnimateButtonIn()
        {
            transform.localScale = Vector3.zero;
            float timer = 0f;
            while (timer < (1 / speed))
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.one * timer * speed;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }

        public void AnimateOut()
        {
            StartCoroutine(AnimateButtonOut());
        }

        IEnumerator AnimateButtonOut()
        {
            transform.localScale = Vector3.one;
            float timer = 0f;
            while (timer < (1 / speed))
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.one / (timer * speed);
                yield return null;
            }
            transform.localScale = Vector3.zero;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            myMenu.selected = this;
            defaultColor = circle.color;
            circle.color = Color.white;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            myMenu.selected = null;
            circle.color = defaultColor;
        }
    }
}