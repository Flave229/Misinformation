using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class UIClock : MonoBehaviour
    {
        private Image _clockImage;
        private Text _timerText;
        public int StartingTimeInSeconds;

        private void Awake()
        {
            Timer.Instance().SetStartingTime(StartingTimeInSeconds);
        }

        private void Start()
        {
            // Get all child components and save the clock image and timer text
            foreach (Transform childTransform in transform)
            {
                Image clockImage = childTransform.GetComponent<Image>();
                if (clockImage != null)
                {
                    _clockImage = clockImage;
                    continue;
                }

                Text timerText = transform.Find("TimerText").GetComponent<Text>();
                if (timerText != null)
                {
                    _timerText = timerText;
                    continue;
                }
            }
        }

        private void Update()
        {
            int remainingTime = Timer.Instance().GetRemainingTime();
            double minutesRemaining, secondsRemaining;

            if (remainingTime >= 0.0f)
            {
                minutesRemaining = Math.Floor((double)remainingTime / 60);
                secondsRemaining = Math.Floor(remainingTime - (minutesRemaining * 60));
            }
            else
            {
                minutesRemaining = 0;
                secondsRemaining = 0;
            }

            _timerText.text = minutesRemaining.ToString("00") + ":" + secondsRemaining.ToString("00");
            ChangeClockImage(remainingTime);
        }

        private void ChangeClockImage(double secondsRemaining)
        {
            double timeRemainingFactor = secondsRemaining / Timer.Instance().GetStartingTimeInSeconds();

            if (timeRemainingFactor > 7d / 8d)
                _clockImage.sprite = Resources.Load<Sprite>("UI/Clock_12");
            else if (timeRemainingFactor > 3d / 4d)
                _clockImage.sprite = Resources.Load<Sprite>("UI/Clock_1_30");
            else if (timeRemainingFactor > 5d / 8d)
                _clockImage.sprite = Resources.Load<Sprite>("UI/Clock_3");
            else if (timeRemainingFactor > 1d / 2d)
                _clockImage.sprite = Resources.Load<Sprite>("UI/Clock_4_30");
            else if (timeRemainingFactor > 3d / 8d)
                _clockImage.sprite = Resources.Load<Sprite>("UI/Clock_6");
            else if (timeRemainingFactor > 2d / 8d)
                _clockImage.sprite = Resources.Load<Sprite>("UI/Clock_7_30");
            else if (timeRemainingFactor > 1d / 8d)
                _clockImage.sprite = Resources.Load<Sprite>("UI/Clock_9");
            else if (timeRemainingFactor > 0)
                _clockImage.sprite = Resources.Load<Sprite>("UI/Clock_10_30");
            else
                _clockImage.overrideSprite = Resources.Load<Sprite>("UI/Clock_12");
        }
    }
}
