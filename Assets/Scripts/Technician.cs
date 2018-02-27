using UnityEngine;

namespace Assets.Scripts
{
    public class Technician : MonoBehaviour
    {
        private int _translationSkill;
        private int _equipmentSkill;
        private int _motivation;
        public bool IsActive;
		public int Salary;

        void Start()
        {        
			Salary = 100 + (_translationSkill * 20) + (_equipmentSkill * 20) + (_motivation * 10);
        }
        
        void Update()
        {
        }

        public void Awake()
        {
            RandomiseAttributes();
            Debug.Log("Technician");
            Debug.Log("Translator Skill: " + _translationSkill);
            Debug.Log("Equipment Specialist Skill: " + _equipmentSkill);
            Debug.Log("Motivation: " + _motivation);
        }


		public void UpdateSalary()
		{
			Salary = 100 + (_translationSkill * 20) + (_equipmentSkill * 20) + (_motivation * 10);
		}

        public int GetEquipmentSkill()
        {
            return _equipmentSkill;
        }

        public int GetTranslationSkill()
        {
            return _translationSkill;
        }

        public int GetMotivationSkill()
        {
            return _motivation;
        }

        public void SetEquipmentSkill(int equipmentSkill)
        {
            _equipmentSkill = equipmentSkill;
        }

        public void SetTranslationSkill(int translationSkill)
        {
            _translationSkill = translationSkill;
        }

        public void SetMotivationSkill(int motivation)
        {
            _motivation = motivation;
        }

        public void RandomiseAttributes()
        {
            _translationSkill = Random.Range(0, 8);
            _equipmentSkill = Random.Range(0, 8);
            _motivation = Random.Range(0, 8);
        }
    }
}
