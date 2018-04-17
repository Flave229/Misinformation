using Assets.Scripts;
using Assets.Scripts.Progression;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FireTechs : MonoBehaviour {

    List<Text> canvasTextGameObjects = new List<Text>();
    List<Image> canvasImageGameObjects = new List<Image>();
    public int _currentExpEquipment;
    public int _currentExpTranslation;
    public int _currentExpMotivation;

    private void Start()
    {
        canvasTextGameObjects.AddRange(Object.FindObjectsOfType<Text>());
        canvasImageGameObjects.AddRange(Object.FindObjectsOfType<Image>());
    }

    public void OnActive()
    {
        List<Text> textEquipmentList = new List<Text>();
        List<Text> textTranslationtList = new List<Text>();
        List<Text> textMotivationList = new List<Text>();
        List<Text> textSalaryList = new List<Text>();
        List<Text> textNameList = new List<Text>();

        for (int j = 1; j < 4; j++)
        {
            for (int i = 0; i < canvasTextGameObjects.Count; i++)
            {
                if (canvasTextGameObjects[i].name == "FireEquipmentText" + j)
                {
                    textEquipmentList.Add(canvasTextGameObjects[i]);
                }

                if (canvasTextGameObjects[i].name == "FireTranslationText" + j)
                {
                    textTranslationtList.Add(canvasTextGameObjects[i]);
                }

                if (canvasTextGameObjects[i].name == "FireMotivationText" + j)
                {
                    textMotivationList.Add(canvasTextGameObjects[i]);
                }
                if (canvasTextGameObjects[i].name == "FireWage" + j)
                {
                    textSalaryList.Add(canvasTextGameObjects[i]);
                }
                if (canvasTextGameObjects[i].name == "TechName" + j)
                {
                    textNameList.Add(canvasTextGameObjects[i]);
                }
            }
        }
        FireTechText(textEquipmentList, textTranslationtList, textMotivationList, textSalaryList, textNameList);
        ExpBar();
    }

    public void FireTech(int num)
    {
        GameManager.Instance().FireTechList.Add(GameManager.Instance().TechList[num]);
        GameManager.Instance().TechList.RemoveAt(num);
        Resources.FindObjectsOfTypeAll<Daily>().ToList().First()._prevTechs -= 1;
    }

    private void FireTechText(List<Text> listEquipment, List<Text> listTranslation, List<Text> listMotivation, List<Text> listSalary, List<Text> listName)
    {
        List<string> equipmentTextList = new List<string>();
        List<int> equipmentIntList = new List<int>();

        List<string> motivationTextList = new List<string>();
        List<int> motivationIntList = new List<int>();

        List<string> translationTextList = new List<string>();
        List<int> translationIntList = new List<int>();

        List<string> salaryTextList = new List<string>();
        List<int> salaryIntList = new List<int>();

        List<string> nameTextList = new List<string>();

        for (int i = 0; i < GameManager.Instance().TechList.Count; i++)
        {
            //Equipment
            equipmentIntList.Add(GameManager.Instance().TechList[i].GetComponent<Technician>().GetEquipmentSkill());
            equipmentTextList.Add(equipmentIntList[i].ToString());
            Debug.Log("Tech" + i + "Equipment: " + GameManager.Instance().TechList[i].GetComponent<Technician>().GetEquipmentSkill());

            //Translation
            translationIntList.Add(GameManager.Instance().TechList[i].GetComponent<Technician>().GetTranslationSkill());
            translationTextList.Add(translationIntList[i].ToString());
            Debug.Log("Tech" + i + "Translation: " + GameManager.Instance().TechList[i].GetComponent<Technician>().GetTranslationSkill());

            //Motivation
            motivationIntList.Add(GameManager.Instance().TechList[i].GetComponent<Technician>().GetMotivationSkill());
            motivationTextList.Add(motivationIntList[i].ToString());
            Debug.Log("Tech" + i + "Motivation: " + GameManager.Instance().TechList[i].GetComponent<Technician>().GetMotivationSkill());

            salaryIntList.Add(GameManager.Instance().TechList[i].GetComponent<Technician>().Salary);
            salaryTextList.Add(salaryIntList[i].ToString());

            nameTextList.Add(GameManager.Instance().TechList[i].GetComponent<Technician>().FullName);
        }

        for (int i = 0; i < GameManager.Instance().TechList.Count; i++)
        {
            listEquipment[i].text = equipmentTextList[i];
            listMotivation[i].text = motivationTextList[i];
            listTranslation[i].text = translationTextList[i];
            listSalary[i].text = salaryTextList[i];
            listName[i].text = nameTextList[i];
        }

    }

    private void ExpBar()
    {
        List<Image> imageEquipmentList = new List<Image>();
        List<Image> imageTranslationList = new List<Image>();
        List<Image> imageMotivationList = new List<Image>();
         
        for (int j = 1; j < 4; j++)
        {
            for (int i = 0; i < canvasImageGameObjects.Count; i++)
            {
                if (canvasImageGameObjects[i].name == "ExpBarEquipment" + j)
                {
                    imageEquipmentList.Add(canvasImageGameObjects[i]);
                }

                if (canvasImageGameObjects[i].name == "ExpBarTranslation" + j)
                {
                    imageTranslationList.Add(canvasImageGameObjects[i]);
                }

                if (canvasImageGameObjects[i].name == "ExpBarMotivationText" + j)
                {
                    imageMotivationList.Add(canvasImageGameObjects[i]);
                }
            }
        }
        List<ExperienceProgress> expProg = new List<ExperienceProgress>();
        List<GameObject> techs = GameManager.Instance().TechList;
        for (int i = 0; i < techs.Count; ++i)
        {
            imageEquipmentList[i].rectTransform.localScale = new Vector3((float)techs[i].GetComponent<Technician>().EquipmentSkill.GetProgressTowardsNextLevel().PercentageToNextLevel / 100, 1, 1);
        //imageMotivationList[j].rectTransform.localScale = new Vector3((float)techs[i].GetComponent<Technician>().Motivation.GetProgressTowardsNextLevel().PercentageToNextLevel / 100, 1, 1);
            imageTranslationList[i].rectTransform.localScale = new Vector3((float)techs[i].GetComponent<Technician>().TranslationSkill.GetProgressTowardsNextLevel().PercentageToNextLevel / 100, 1, 1);
        }
}

    // Update is called once per frame
    void Update ()
    {
        //_expBar.rectTransform.localScale = new Vector3(_currentExp, 1, 1);
	}
}
