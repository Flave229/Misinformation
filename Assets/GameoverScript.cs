using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameoverScript : MonoBehaviour
    {
    float TimerTypewriter;

    public float letterPause = 0.2f;
    //string message;
    public Text gameOverText;
    public bool isFinished { get; private set; }
    public bool hasDisplayed { get; private set; }

    private void Start()
    {

            //message = "Game Over";
            gameOverText.text = "";
            //gameOverText.text = message;

            hasDisplayed = false;

    }
        private void Update()
        {
            TypeTextNonIEnum("Game Over \n   Press \"Space\" to return to menu");
        }

        public void TypeTextNonIEnum(string message)
        {
            TimerTypewriter += Time.deltaTime;
            if (TimerTypewriter == letterPause)
            {
                foreach (char letter in message.ToCharArray())
                {
                    gameOverText.text += letter;
                }
                TimerTypewriter = 0;
            }

            isFinished = true;
        }


        public IEnumerator TypeText(string message)
    {
            foreach (char letter in message.ToCharArray())
            {
            gameOverText.text += letter;
            yield return new WaitForSeconds(letterPause);
            }

        isFinished = true;
    }


  }
}