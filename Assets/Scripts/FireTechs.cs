using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireTechs : MonoBehaviour {

    List<GameObject> _techList = new List<GameObject>();

    private void Awake()
    {
        _techList = GameManager.Instance().TechList;
        List<Text> canvasTextGameObjects = new List<Text>();
        canvasTextGameObjects.AddRange(Object.FindObjectsOfType<Text>());

        List<Text> textEquipmentList = new List<Text>();
        List<Text> textTranslationtList = new List<Text>();
        List<Text> textMotivationList = new List<Text>();
        List<Text> textSalaryList = new List<Text>();
        //TODO: The tech attributes are displayed in the wrong order...
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
            }
        }
        FireTechText(textEquipmentList, textTranslationtList, textMotivationList, textSalaryList);
    }

    public void FireTech(int num)
    {
        _techList.RemoveAt(num);
    }

    private void FireTechText(List<Text> listEquipment, List<Text> listTranslation, List<Text> listMotivation, List<Text> listSalary)
    {
        List<string> equipmentTextList = new List<string>();
        List<int> equipmentIntList = new List<int>();

        List<string> motivationTextList = new List<string>();
        List<int> motivationIntList = new List<int>();

        List<string> translationTextList = new List<string>();
        List<int> translationIntList = new List<int>();

        List<string> salaryTextList = new List<string>();
        List<int> salaryIntList = new List<int>();

        for (int i = 0; i < _techList.Count; i++)
        {
            //Equipment
            equipmentIntList.Add(_techList[i].GetComponent<Technician>().GetEquipmentSkill());
            equipmentTextList.Add(equipmentIntList[i].ToString());
            Debug.Log("Tech" + i + "Equipment: " + _techList[i].GetComponent<Technician>().GetEquipmentSkill());

            //Translation
            translationIntList.Add(_techList[i].GetComponent<Technician>().GetTranslationSkill());
            translationTextList.Add(translationIntList[i].ToString());
            Debug.Log("Tech" + i + "Translation: " + _techList[i].GetComponent<Technician>().GetTranslationSkill());

            //Motivation
            motivationIntList.Add(_techList[i].GetComponent<Technician>().GetMotivationSkill());
            motivationTextList.Add(motivationIntList[i].ToString());
            Debug.Log("Tech" + i + "Motivation: " + _techList[i].GetComponent<Technician>().GetMotivationSkill());

            salaryIntList.Add(_techList[i].GetComponent<Technician>().Salary);
            salaryTextList.Add(salaryIntList[i].ToString());
        }

        for (int i = 0; i < _techList.Count; i++)
        {
            listEquipment[i].text = equipmentTextList[i];
            listMotivation[i].text = motivationTextList[i];
            listTranslation[i].text = translationTextList[i];
            listSalary[i].text = salaryTextList[i];

        }

    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
