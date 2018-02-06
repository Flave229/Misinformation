using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Tristan's Comments
/*need to do:
 * stop time when open
 * get new general list
 * get leaving general list
 * add some information about the attack to help player in the right direction
 */
//End of Tristan's Comments

namespace Assets.Scripts
{
    public class MorningReport : MonoBehaviour
    {
        private Daily _dailyManager;
        private readonly List<GameObject> _morningReportComponents = new List<GameObject>();

        private List<GameObject> _oldGeneralList;
        private List<GameObject> _newGeneralList;

        private bool _reportsScreenVisible = true;
       // public Text _fundingFeedback;

        private string _addedGen = "";
        private string _leavingGen = "";

        private Text _fundingFeedback;

        void Start()
        {
            _dailyManager = Object.FindObjectOfType<Daily>();
            _addedGen = "A general is added";

            //_dailyManager.leavingGenerals = new List<General.Name>();
            //_dailyManager.arrivingGenerals = new List<General.Name>();

            //Get all child elements
            foreach (RectTransform rectTransform in GetComponentInChildren<RectTransform>())
            {
                rectTransform.gameObject.SetActive(true);
                if (rectTransform.gameObject.name == "REFNO_Text")
                {
                    UnityEngine.UI.Text REFDATE = rectTransform.gameObject.GetComponent<UnityEngine.UI.Text>();
                    REFDATE.text = GameManager.Instance().GetRefNum() + ", " + GameManager.Instance().GetCurrentDate();
                }
                if (rectTransform.gameObject.name == "Paragraph1_Text(3)")
                {
                
                }
                
                _morningReportComponents.Add(rectTransform.gameObject);
            }

         


                    Show();
        }
    
        void Update()
        {
            if (_reportsScreenVisible == false)
                return;

            if (Input.GetKeyDown(KeyCode.Return) == false)
                return;
        
            _dailyManager.StartDay();
       
                Hide();
        }

        public void Show()
        {
            _reportsScreenVisible = true;
            Time.timeScale = 0;
            Timer.Instance().Pause();
            foreach (GameObject component in _morningReportComponents)
                component.SetActive(true);
            _dailyManager.GenerateGenerals();

            UnityEngine.UI.Text addedGenerals = null;
            UnityEngine.UI.Text leavingGenerals = null;
            foreach (RectTransform rectTransform in GetComponentInChildren<RectTransform>())
            {
                if (rectTransform.gameObject.name == "ArrivingGeneralsList")
                    addedGenerals = rectTransform.gameObject.GetComponent<UnityEngine.UI.Text>();
                else if (rectTransform.gameObject.name == "LeavingGeneralsList")
                    leavingGenerals = rectTransform.gameObject.GetComponent<UnityEngine.UI.Text>();
            }
            addedGenerals.text = "";
            leavingGenerals.text = "";
        
            for (int i = 0; i < _dailyManager.arrivingGenerals.Count; i++)
            {
                addedGenerals.text += _dailyManager.arrivingGenerals[i].FullName() + "\n";
            }
            for (int i = 0; i < _dailyManager.leavingGenerals.Count; i++)
            {
                leavingGenerals.text += _dailyManager.leavingGenerals[i].FullName() + "\n";
            }

            foreach (RectTransform rectTransform in GetComponentInChildren<RectTransform>())
            {
                if (rectTransform.gameObject.name == "FundingText")
                {
                    rectTransform.gameObject.GetComponent<UnityEngine.UI.Text>();

                    if (GameManager.Instance().RecievingFunding && GameManager.Instance().Days != 0)
                    {
                        rectTransform.gameObject.GetComponent<UnityEngine.UI.Text>().text = "You are recieving funding due to diligence and good work!";
                    }
                    else if(!GameManager.Instance().RecievingFunding && GameManager.Instance().Days != 0)
                    {
                        rectTransform.gameObject.GetComponent<UnityEngine.UI.Text>().text = "You have failed to provide information vital to us and have therefore lost funding";
                    }
                }
            }
        }

        public void Hide()
        {
            _reportsScreenVisible = false;
            Time.timeScale = 1;
            Timer.Instance().Play();
            foreach (GameObject component in _morningReportComponents)
                component.SetActive(false);
            SoundManager.Instance().PlaySingle("Page_Turn-Mark DiAngelo");
        }
    }
}