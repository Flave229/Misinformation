using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.General;
using UnityEngine;

namespace Assets.Scripts.FileIO
{
    public static class NameGenerator
    {
        private static List<string> _takenFirstNames = new List<string>();
        private static List<string> _takenLastNames = new List<string>();

        public static Name GenerateGeneralName()
        {
            TextAsset firstNameData = Resources.Load<TextAsset>("Text/first_names");
            string firstNameFileContents = System.Text.Encoding.Default.GetString(firstNameData.bytes);
            List<string> firstNameString = firstNameFileContents.Split('\n').Select(x => x.Replace("\r", "").Trim()).ToList();

            TextAsset lastNameData = Resources.Load<TextAsset>("Text/last_names");
            string lastNameFileContents = System.Text.Encoding.Default.GetString(lastNameData.bytes);
            List<string> lastNameString = lastNameFileContents.Split('\n').Select(x => x.Replace("\r", "").Trim()).ToList();

            string chosenFirstName = firstNameString[new System.Random().Next(0, firstNameString.Count)];

            while (_takenFirstNames.Any(x => x == chosenFirstName))
            {
                chosenFirstName = firstNameString[new System.Random().Next(0, firstNameString.Count)];
            }

            _takenFirstNames.Add(chosenFirstName);

            string chosenLastName = lastNameString[new System.Random().Next(0, lastNameString.Count)];

            while (_takenLastNames.Any(x => x == chosenLastName))
            {
                chosenLastName = lastNameString[new System.Random().Next(0, lastNameString.Count)];
            }

            _takenLastNames.Add(chosenLastName);

            return new Name
            {
                FirstName = chosenFirstName,
                LastName = chosenLastName
            };
        }

        public static Name GenerateTechnicianName()
        {
            TextAsset firstNameData = Resources.Load<TextAsset>("Text/first_names_tech");
            string firstNameFileContents = System.Text.Encoding.Default.GetString(firstNameData.bytes);
            List<string> firstNameString = firstNameFileContents.Split('\n').Select(x => x.Replace("\r", "").Trim()).ToList();

            TextAsset lastNameData = Resources.Load<TextAsset>("Text/last_names_tech");
            string lastNameFileContents = System.Text.Encoding.Default.GetString(lastNameData.bytes);
            List<string> lastNameString = lastNameFileContents.Split('\n').Select(x => x.Replace("\r", "").Trim()).ToList();

            string chosenFirstName = firstNameString[new System.Random().Next(0, firstNameString.Count)];

            while (_takenFirstNames.Any(x => x == chosenFirstName))
            {
                chosenFirstName = firstNameString[new System.Random().Next(0, firstNameString.Count)];
            }

            _takenFirstNames.Add(chosenFirstName);

            string chosenLastName = lastNameString[new System.Random().Next(0, lastNameString.Count)];

            while (_takenLastNames.Any(x => x == chosenLastName))
            {
                chosenLastName = lastNameString[new System.Random().Next(0, lastNameString.Count)];
            }

            _takenLastNames.Add(chosenLastName);

            return new Name
            {
                FirstName = chosenFirstName,
                LastName = chosenLastName
            };
        }

        public static void RemoveNameFromPool(Name name)
        {
            _takenFirstNames.Remove(name.FirstName);
            _takenLastNames.Remove(name.LastName);
        }
    }
}
