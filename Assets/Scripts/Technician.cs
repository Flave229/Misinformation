using Assets.Scripts.FileIO;
using Assets.Scripts.General;
using Assets.Scripts.Progression;
using UnityEngine;

namespace Assets.Scripts
{
    public class Technician : MonoBehaviour
    {
        public Skill TranslationSkill { get; private set; }
        public Skill EquipmentSkill { get; private set; }
        public Skill Motivation { get; private set; }

        public bool IsActive;
		public int Salary;
        public Name Name;
        public string FullName;

        void Start()
        {
        }
        
        void Update()
        {
        }

        public void SetSkills(int translationLevel, int equipmentLevel, int motivationLevel)
        {
            TranslationSkill = new Skill(translationLevel);
            EquipmentSkill = new Skill(equipmentLevel);
            Motivation = new Skill(motivationLevel);
            UpdateSalary();
        }

        public void Awake()
        {
            Name = NameGenerator.GenerateTechnicianName();
            RandomiseAttributes();
            UpdateSalary();
            Debug.Log("Technician");
            Debug.Log("Translator Skill: " + TranslationSkill);
            Debug.Log("Equipment Specialist Skill: " + EquipmentSkill);
            Debug.Log("Motivation: " + Motivation);
            FullName = Name.FullName();
        }

		public void UpdateSalary()
		{
			Salary = 100 + (TranslationSkill.CurrentLevel * 20) + (EquipmentSkill.CurrentLevel * 20) + (Motivation.CurrentLevel * 10);
		}

        public int GetEquipmentSkill()
        {
            return EquipmentSkill.CurrentLevel;
        }

        public int GetTranslationSkill()
        {
            return TranslationSkill.CurrentLevel;
        }

        public int GetMotivationSkill()
        {
            return Motivation.CurrentLevel;
        }
        
        public void RandomiseAttributes()
        {
            TranslationSkill = new Skill(Random.Range(0, 6));
            EquipmentSkill = new Skill(Random.Range(0, 6));
            Motivation = new Skill(Random.Range(0, 8));
            UpdateSalary();
        }
    }
}