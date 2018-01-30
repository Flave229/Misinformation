using UnityEngine;

namespace Assets.Scripts
{
    public class Technician : MonoBehaviour
    {
        private int _translationSkill;
        private int _equipmentSkill;
        private int _motivation;
        public bool IsActive;

        void Start()
        {
        }
        
        void Update()
        {
        }

        void Awake()
        {
            _translationSkill = Random.Range(0, 8);
            _equipmentSkill = Random.Range(0, 8);
            _motivation = Random.Range(0, 8);
            Debug.Log("Technician");
            Debug.Log("Translator Skill: " + _translationSkill);
            Debug.Log("Equipment Specialist Skill: " + _equipmentSkill);
            Debug.Log("Motivation: " + _motivation);
        }

        public int GetEquipmentSkill()
        {
            return _equipmentSkill;
        }
    }
}
