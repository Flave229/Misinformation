using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        private Daily _daily;
        private bool _pauseMenu;
        private GameObject[] _pauseObjects;

        public MorningReport MorningReport;


        void Start()
        {
            _pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
            MorningReport = Object.FindObjectOfType<MorningReport>();
            _daily = Object.FindObjectOfType<Daily>();
            HidePaused();
        }

        void Update()
        {
            if (Input.GetKeyDown("escape"))
            {
                if (_pauseMenu == true)
                    _pauseMenu = false;

                else _pauseMenu = true;

            }

            if (_pauseMenu)
            {
                Time.timeScale = 0;
                ShowPaused();
            }
            else
            {
                Time.timeScale = 1;
                HidePaused();
            }
        }

        public void LoadByIndex(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
        }

        public void ShowPaused()
        {
            foreach (GameObject g in _pauseObjects)
                g.SetActive(true);
        }

        public void HidePaused()
        {
            foreach (GameObject g in _pauseObjects)
                g.SetActive(false);
        }

        public void ContinueButtonPause()
        {
            if (_pauseMenu)
                _pauseMenu = false;
        }

        public void SubmitClick(DailyReport dailyReport)
        {
            _daily.DestructGenerals();
            dailyReport.Hide();
            dailyReport.CheckValueParity();
            MorningReport.Show();
        }
    }
}