using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HireTechs : MonoBehaviour {

    List<GameObject> _listPossibleTechs = new List<GameObject>();

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
            }
        }
        HireTechText(textEquipmentList, textTranslationtList, textMotivationList);
    }
	
    void GenerateTechList()
    {
        GameObject tech1 = new GameObject();
        GameObject tech2 = new GameObject();
        GameObject tech3 = new GameObject();
        tech1.AddComponent<Technician>();
        tech2.AddComponent<Technician>();
        tech3.AddComponent<Technician>();
        _listPossibleTechs.Add(tech1);
        _listPossibleTechs.Add(tech2);
        _listPossibleTechs.Add(tech3);

        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            Debug.Log(_listPossibleTechs[i].GetComponent<Technician>().Salary);
            _listPossibleTechs
                [i].GetComponent<Technician>().RandomiseAttributes();
            //Debug.Log("Tech" + i + ": " + _listPossibleTechs[i].GetComponent<Technician>().GetEquipmentSkill());
        }
    }

    public void HireTech(int num)
    {
        GameManager.Instance().TechList.Add(_listPossibleTechs[num]);
    }


    private void HireTechText(List<Text> listEquipment, List<Text> listTranslation, List<Text> listMotivation)
    {
        List<string> equipmentTextList = new List<string>();
        List<int> equipmentIntList = new List<int>();

        List<string> motivationTextList = new List<string>();
        List<int> motivationIntList = new List<int>();

        List<string> translationTextList = new List<string>();
        List<int> translationIntList = new List<int>();

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
        }

        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            listEquipment[i].text = equipmentTextList[i];
            listMotivation[i].text = motivationTextList[i];
            listTranslation[i].text = translationTextList[i];
        }

    }



    // Update is called once per frame
    void Update ()
    {

    }
}
