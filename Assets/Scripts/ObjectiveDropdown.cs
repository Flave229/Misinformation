using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    enum DropdownType
    {
        EVENT,
        PLACE,
        DAY,
        MONTH,
        TIME
    }

    class ObjectiveDropdown : MonoBehaviour
    {
        public DropdownType ObjectiveType;

        private void Start()
        {
            List<string> options = new List<string>();
            switch (ObjectiveType)
            {
                case DropdownType.EVENT:
                    options = Objective.Events;
                    break;
                case DropdownType.PLACE:
                    options = Objective.Places;
                    break;
                case DropdownType.DAY:
                    options = Objective.Days.Select(x => x.ToString()).ToList();
                    break;
                case DropdownType.MONTH:
                    options = Objective.Months;
                    break;
                case DropdownType.TIME:
                    options = Objective.Times;
                    break;
            }

            Dropdown dropdown = transform.GetComponent<Dropdown>();
            dropdown.options.AddRange(options.Select(x => new Dropdown.OptionData() { text = x.ToUpper() }));
            dropdown.RefreshShownValue();
        }
    }
}