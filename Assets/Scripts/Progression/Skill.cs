using UnityEngine;

namespace Assets.Scripts.Progression
{
    public class Skill
    {
        public int CurrentLevel;

        private int _maxLevel;
        private float _maxExperience;
        private float _currentExperience;

        public Skill(int currentLevel)
        {
            _maxLevel = 10; // Hardcoded cap, can adjust this by passing into constructor if necessary.
            _maxExperience = Mathf.Pow(_maxLevel, 1.5f) * 1000;
            _currentExperience = Mathf.Pow(currentLevel, 1.5f) * 1000;
        }

        public void AddExperience(float experiencePoints)
        {
            _currentExperience += experiencePoints;
            CurrentLevel = Mathf.FloorToInt(Mathf.Pow(_currentExperience / 1000, 1 / 1.5f));
        }
    }
}
