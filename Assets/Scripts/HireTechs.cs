using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireTechs : MonoBehaviour {

    List<GameObject> listPossibleTechs = new List<GameObject>();
    GameObject _chosenTech;

	// Use this for initialization
	public void Awake()
    {
        _chosenTech = Resources.Load<GameObject>("Player");
        GenerateTechList();
	}
	
    void GenerateTechList()
    {
        GameObject tech = new GameObject();
        tech.AddComponent<Technician>();

        for (int i = 0; i < 3; i++)
        {

            listPossibleTechs.Add(tech);
        }
        for (int i = 0; i < listPossibleTechs.Count; i++)
        {
            Debug.Log(listPossibleTechs[i].GetComponent<Technician>().Salary);
            listPossibleTechs[i].GetComponent<Technician>().RandomiseAttributes();
        }
    }

    public void HireTech()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            _chosenTech.GetComponent<Technician>().SetEquipmentSkill(listPossibleTechs[0].GetComponent<Technician>().GetEquipmentSkill());
            _chosenTech.GetComponent<Technician>().SetMotivationSkill(listPossibleTechs[0].GetComponent<Technician>().GetMotivationSkill());
            _chosenTech.GetComponent<Technician>().SetTranslationSkill(listPossibleTechs[0].GetComponent<Technician>().GetTranslationSkill());//TODO: get the technician attributes onto the chosen tech var;
            GameManager.Instance().TechList.Add(_chosenTech);
        }
        else if (Input.GetKeyDown(KeyCode.F11))
        {
            _chosenTech.GetComponent<Technician>().SetEquipmentSkill(listPossibleTechs[1].GetComponent<Technician>().GetEquipmentSkill());
            _chosenTech.GetComponent<Technician>().SetMotivationSkill(listPossibleTechs[1].GetComponent<Technician>().GetMotivationSkill());
            _chosenTech.GetComponent<Technician>().SetTranslationSkill(listPossibleTechs[1].GetComponent<Technician>().GetTranslationSkill());
            GameManager.Instance().TechList.Add(_chosenTech);
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            _chosenTech.GetComponent<Technician>().SetEquipmentSkill(listPossibleTechs[2].GetComponent<Technician>().GetEquipmentSkill());
            _chosenTech.GetComponent<Technician>().SetMotivationSkill(listPossibleTechs[2].GetComponent<Technician>().GetMotivationSkill());
            _chosenTech.GetComponent<Technician>().SetTranslationSkill(listPossibleTechs[2].GetComponent<Technician>().GetTranslationSkill());
            GameManager.Instance().TechList.Add(_chosenTech);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        HireTech();
    }
}
