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
            CurrentLevel = currentLevel;
        }

        public void AddExperience(float experiencePoints)
        {
            _currentExperience += experiencePoints;
            CurrentLevel = Mathf.FloorToInt(Mathf.Pow(_currentExperience / 1000, 1 / 1.5f));

            if (CurrentLevel >= _maxLevel)
                CurrentLevel = _maxLevel;
        }

        public ExperienceProgress GetProgressTowardsNextLevel()
        {
            float lowestExperienceForThisLevel = Mathf.Ceil(Mathf.Pow(CurrentLevel, 1.5f) * 1000);
            float lowestExperienceForNextLevel = Mathf.Ceil(Mathf.Pow(CurrentLevel + 1, 1.5f) * 1000);
            return new ExperienceProgress
            {
                CurrentExperience = Mathf.FloorToInt(_currentExperience),
                CurrentLevel = CurrentLevel,
                ExperienceForNextLevel = CurrentLevel < _maxLevel ? Mathf.CeilToInt(Mathf.Pow(CurrentLevel + 1, 1.5f) * 1000) : 0,
                PercentageToNextLevel = CurrentLevel < _maxLevel ? Mathf.FloorToInt(((_currentExperience - lowestExperienceForThisLevel) / (lowestExperienceForNextLevel - lowestExperienceForThisLevel)) * 100) : 100
            };
        }
    }
}