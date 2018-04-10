using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HireTechs : MonoBehaviour {

    List<GameObject> _listPossibleTechs = new List<GameObject>();
    public Technician SelectedTech;

	// Use this for initialization
	public void Awake()
    {
        _listPossibleTechs.Clear();
        GenerateTechList();
        List<Text> canvasTextGameObjects = new List<Text>();
        canvasTextGameObjects.AddRange(Object.FindObjectsOfType<Text>());

        List<Text> textEquipmentList = new List<Text>();
        List<Text> textTranslationtList = new List<Text>();
        List<Text> textMotivationList = new List<Text>();
        List<Text> textSalaryList = new List<Text>();
        List<Text> textNameList = new List<Text>();
        //TODO: The tech attributes are displayed in the wrong order...
        for (int j = 1; j < 4; j++)
        {
            for (int i = 0; i < canvasTextGameObjects.Count; i++)
            {
                if (canvasTextGameObjects[i].name == "HireEquipmentText" + j) 
                {
                    textEquipmentList.Add(canvasTextGameObjects[i]);
                }

                if (canvasTextGameObjects[i].name == "HireTranslationText" + j)
                {
                    textTranslationtList.Add(canvasTextGameObjects[i]);
                }

                if (canvasTextGameObjects[i].name == "HireMotivationText" + j)
                {
                    textMotivationList.Add(canvasTextGameObjects[i]);
                }
                if (canvasTextGameObjects[i].name == "HireWage" + j)
                {
                    textSalaryList.Add(canvasTextGameObjects[i]);
                }
                if (canvasTextGameObjects[i].name == "HireTechName" + j)
                {
                    textNameList.Add(canvasTextGameObjects[i]);
                }
            }
        }
        HireTechText(textEquipmentList, textTranslationtList, textMotivationList, textSalaryList, textNameList);
    }
	
    void GenerateTechList()
    {
        GameObject tech1 = new GameObject("tech1");
        GameObject tech2 = new GameObject("tech2");
        GameObject tech3 = new GameObject("tech3");
        tech1.AddComponent<Technician>();
        tech2.AddComponent<Technician>();
        tech3.AddComponent<Technician>();
        _listPossibleTechs.Add(tech1);
        _listPossibleTechs.Add(tech2);
        _listPossibleTechs.Add(tech3);

        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            _listPossibleTechs[i].GetComponent<Technician>().RandomiseAttributes();
        }
    }

    public void HireTech(int num)
    {
        GameManager.Instance().TechList.Add(_listPossibleTechs[num]);
        SelectedTech = _listPossibleTechs[num].GetComponent<Technician>();
    }


    private void HireTechText(List<Text> listEquipment, List<Text> listTranslation, List<Text> listMotivation, List<Text> listSalary, List<Text> listName)
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

        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            //Equipment
            equipmentIntList.Add(_listPossibleTechs[i].GetComponent<Technician>().GetEquipmentSkill());
            equipmentTextList.Add(equipmentIntList[i].ToString());
            Debug.Log("Tech" + i + "Equipment: " + _listPossibleTechs[i].GetComponent<Technician>().GetEquipmentSkill());

            //Translation
            translationIntList.Add(_listPossibleTechs[i].GetComponent<Technician>().GetTranslationSkill());
            translationTextList.Add(translationIntList[i].ToString());
            Debug.Log("Tech" + i + "Translation: " + _listPossibleTechs[i].GetComponent<Technician>().GetTranslationSkill());

            //Motivation
            motivationIntList.Add(_listPossibleTechs[i].GetComponent<Technician>().GetMotivationSkill());
            motivationTextList.Add(motivationIntList[i].ToString());
            Debug.Log("Tech" + i + "Motivation: " + _listPossibleTechs[i].GetComponent<Technician>().GetMotivationSkill());

            salaryIntList.Add(_listPossibleTechs[i].GetComponent<Technician>().Salary);
            salaryTextList.Add(salaryIntList[i].ToString());

            nameTextList.Add(_listPossibleTechs[i].GetComponent<Technician>().FullName);
        }

        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            listEquipment[i].text = equipmentTextList[i];
            listMotivation[i].text = motivationTextList[i];
            listTranslation[i].text = translationTextList[i];
            listSalary[i].text = salaryTextList[i];
            listName[i].text = nameTextList[i];
        }

    }



    // Update is called once per frame
    void Update ()
    {

    }
}
