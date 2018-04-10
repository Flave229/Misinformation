using Assets.Scripts.FileIO;
using Assets.Scripts.General;
using Assets.Scripts.Progression;
using UnityEngine;

namespace Assets.Scripts
{
    public class Technician : MonoBehaviour
    {
        private Skill _translationSkill;
        private Skill _equipmentSkill;
        private Skill _motivation;
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
            _translationSkill = new Skill(translationLevel);
            _equipmentSkill = new Skill(equipmentLevel);
            _motivation = new Skill(motivationLevel);
            UpdateSalary();
        }

        public void Awake()
        {
            Name = NameGenerator.GenerateTechnicianName();
            RandomiseAttributes();
            UpdateSalary();
            Debug.Log("Technician");
            Debug.Log("Translator Skill: " + _translationSkill);
            Debug.Log("Equipment Specialist Skill: " + _equipmentSkill);
            Debug.Log("Motivation: " + _motivation);
            FullName = Name.FullName();
        }

		public void UpdateSalary()
		{
			Salary = 100 + (_translationSkill.CurrentLevel * 20) + (_equipmentSkill.CurrentLevel * 20) + (_motivation.CurrentLevel * 10);
		}

        public int GetEquipmentSkill()
        {
            return _equipmentSkill.CurrentLevel;
        }

        public int GetTranslationSkill()
        {
            return _translationSkill.CurrentLevel;
        }

        public int GetMotivationSkill()
        {
            return _motivation.CurrentLevel;
        }

        public void RandomiseAttributes()
        {
            _translationSkill = new Skill(Random.Range(0, 8));
            _equipmentSkill = new Skill(Random.Range(0, 8));
            _motivation = new Skill(Random.Range(0, 8));
            UpdateSalary();
        }
    }
}
