using UnityEngine;

namespace Assets.Scripts
{
    public class Technician : MonoBehaviour {

        enum TranslatorSkill
        {
            Translator1,
            Translator2,
            Translator3,
            Translator4,
            Translator5,
            Translator6,
            Translator7,
            Translator8,
            Translator9,
            Translator10,
            NumberOfTypes
        };

        enum EquipmentSpecialistSkill
        {
            EquipmentSpec1,
            EquipmentSpec2,
            EquipmentSpec3,
            EquipmentSpec4,
            EquipmentSpec5,
            EquipmentSpec6,
            EquipmentSpec7,
            EquipmentSpec8,
            EquipmentSpec9,
            EquipmentSpec10,
            NumberOfTypes
        };

        enum MotivationSkill
        {
            Motivation1,
            Motivation2,
            Motivation3,
            Motivation4,
            Motivation5,
            Motivation6,
            Motivation7,
            Motivation8,
            Motivation9,
            Motivation10,
            NumberOfTypes
        };

        TranslatorSkill TranslatorSkillLvl;
        EquipmentSpecialistSkill EquipSpecSkillLvl;
        MotivationSkill MotivationLvl;
        bool testBoolDebugLog = true; // test value for debug log 

        //random speed between 0.25 and 0.4


        // Use this for initialization
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update() {

            if (testBoolDebugLog)
            {
                Debug.Log("Technician");
                Debug.Log("Translator Skill: " + System.Array.IndexOf(System.Enum.GetValues(TranslatorSkillLvl.GetType()), TranslatorSkillLvl));
                // System.Console.WriteLine("Translator Skill: " + TranslatorSkillLvl); // Didn't display in Log  - maybe writing to a file - decided to use the debug log as it is easier to read and find. - R.Walters 
                Debug.Log("Equipment Specialist Skill: " + System.Array.IndexOf(System.Enum.GetValues(EquipSpecSkillLvl.GetType()), EquipSpecSkillLvl));
                Debug.Log("Motivation: " + System.Array.IndexOf(System.Enum.GetValues(MotivationLvl.GetType()), MotivationLvl));
                testBoolDebugLog = false;
            }
        }

        void Awake() // Replaces the generate skills method I had intended to use as this works as necessary and in the same manner - Generates before or is called before Start or Update - make considerations for that. - R.Walters
        {
            //GenerateSkills(TranslatorSkillLvl, EquipSpecSkillLvl, MotivationLvl);
            TranslatorSkillLvl = (TranslatorSkill)Random.Range(0, System.Enum.GetValues(typeof(TranslatorSkill)).Length - 1);
            EquipSpecSkillLvl = (EquipmentSpecialistSkill)Random.Range(0, System.Enum.GetValues(typeof(EquipmentSpecialistSkill)).Length - 1);
            MotivationLvl = (MotivationSkill)Random.Range(0, System.Enum.GetValues(typeof(MotivationSkill)).Length - 1);
            if (gameObject.GetComponent("Character2D") && gameObject.GetComponent("Technician"))
            {
            
            }
        }



        // void GenerateSkills(TranslatorSkill TranslatorSkillLvlG, EquipmentSpecialistSkill EquipSpecSkillLvlG, MotivationSkill MotivationLvlG) //May restore this function if it is more helpful or easier to work with than Awake for RNG for Skills. - R.Walters
        // {

        // }
    }
}
