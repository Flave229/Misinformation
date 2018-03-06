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
        GenerateTechList();
        List<GameObject> canvasEquipmentGameObjects = new List<GameObject>();
        canvasEquipmentGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Finish"));
        List<GameObject> canvasMotivationGameObjects = new List<GameObject>();
        canvasMotivationGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Respawn"));
        List<GameObject> canvasTranslationGameObjects = new List<GameObject>();
        canvasTranslationGameObjects.AddRange(GameObject.FindGameObjectsWithTag("EditorOnly"));
        HireTechText(canvasEquipmentGameObjects, canvasMotivationGameObjects, canvasTranslationGameObjects);
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
            _listPossibleTechs[i].GetComponent<Technician>().RandomiseAttributes();
            Debug.Log("Tech" + i + ": " + _listPossibleTechs[i].GetComponent<Technician>().GetEquipmentSkill());
        }
    }

    public void HireTech1()
    {
        GameManager.Instance().TechList.Add(_listPossibleTechs[0]);
    }

    public void HireTech2()
    {
        GameManager.Instance().TechList.Add(_listPossibleTechs[1]);
    }

    public void HireTech3()
    {
        GameManager.Instance().TechList.Add(_listPossibleTechs[2]);
    }

    private void HireTechText(List<GameObject> listEquipment, List<GameObject> listMotivation, List<GameObject> listTranslation)
    {
        //Equipment
        List<string> equipmentTextList = new List<string>();
        List<int> equipmentIntList = new List<int>();
        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            equipmentIntList.Add(_listPossibleTechs[i].GetComponent<Technician>().GetEquipmentSkill());
            equipmentTextList.Add(equipmentIntList[i].ToString());
        }

        //Motivation
        List<string> motivationTextList = new List<string>();
        List<int> motivationIntList = new List<int>();
        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            motivationIntList.Add(_listPossibleTechs[i].GetComponent<Technician>().GetMotivationSkill());
            motivationTextList.Add(motivationIntList[i].ToString());
        }

        //Translation
        List<string> translationTextList = new List<string>();
        List<int> translationIntList = new List<int>();
        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            translationIntList.Add(_listPossibleTechs[i].GetComponent<Technician>().GetTranslationSkill());
            translationTextList.Add(equipmentIntList[i].ToString());
        }
        for (int i = 0; i < _listPossibleTechs.Count; i++)
        {
            listEquipment[i].GetComponent<Text>().text = equipmentTextList[i];
            listMotivation[i].GetComponent<Text>().text = motivationTextList[i];
            listTranslation[i].GetComponent<Text>().text = translationTextList[i];
        }

    }

    // Update is called once per frame
    void Update ()
    {

    }
}
