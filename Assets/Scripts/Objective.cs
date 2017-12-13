using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Objective : MonoBehaviour
    {
        public static List<string> Places = new List<string>();
        public static List<string> Events = new List<string>();
        public static List<int> Days = new List<int>();
        public static List<string> Months = new List<string>();
        public static List<string> Times = new List<string>();

        public static string Place = "#undeclared#";
        public static string Event = "#undeclared#";
        public static string Time = "#undeclared#";
        public static int Day = 1;
        public static string Month = "#undeclared#";
        public static string Hour = "#undeclared#";
    
        public string pplace = "#undeclared#";                  //only used to see the vaules in editor, debug only
        public string pevent = "#undeclared#";                  //only used to see the vaules in editor, debug only
        public string ptime = "#undeclared#";                   //only used to see the vaules in editor, debug only
    
        System.Random randomNumber = new System.Random();

        public void Awake()
        {
            Months = new List<string>
            {
                "January",
                "Feburary",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };

            for (int i = 0; i < 24; i++)
                Times.Add(i.ToString("00") + ":00");

            NewDayUpdate();
        }
    
        public void Start ()
        {
        }
    
        public void NewDayUpdate()
        {
            PlaceUpdate();
            EventUpdate();
            TimeUpdate();

            pevent = Event;         //only used to see the vaules in editor, debug only
            pplace = Place;         //only used to see the vaules in editor, debug only
            ptime = Time;           //only used to see the vaules in editor, debug only
        }

        private void PlaceUpdate()
        {
            LinkedList<string> placeList = new LinkedList<string>();
        
            try
            {
                TextAsset placeData = Resources.Load<TextAsset>("Text/locations");
                string placeFileContents = System.Text.Encoding.Default.GetString(placeData.bytes);
                Places = placeFileContents.Split('\n').Select(x => x.Replace("\r", "").Trim()).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Could not read the file for locations", e);
            }

            if (Places.Count != 0)
            {
                int placeNum = randomNumber.Next(Places.Count);
                Place = Places.ElementAt(placeNum);
            }
            else
            {
                Console.WriteLine("Error!!");
            }
        }

        private void EventUpdate()
        {
            LinkedList<string> eventList = new LinkedList<string>();

            try
            {
                TextAsset eventData = Resources.Load<TextAsset>("Text/objective_events");
                string eventFileContents = System.Text.Encoding.Default.GetString(eventData.bytes);
                Events = eventFileContents.Split('\n').Select(x => x.Replace("\r", "").Trim()).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Could not read the file for Events", e);
            }

            if (Events.Count != 0)
            {
                int eventNum = randomNumber.Next(Events.Count);
                Event = Events.ElementAt(eventNum);
            }
            else
            {
                Console.WriteLine("Error!!");
            }
        }

        private void TimeUpdate()
        {
            int hour =  randomNumber.Next(25);
            int monthIndex = randomNumber.Next(0, 12);
            int year = (randomNumber.Next(6)) + 1940;
            int daysOnMonth = DateTime.DaysInMonth(year, monthIndex);

            Days.Clear();
            for (int i = 0; i < daysOnMonth; i++)
                Days.Add(i);

            Time = hour + ":00" + " on " + Day + " of " + Months[monthIndex] + " " + year;
            Month = Months[monthIndex];
            Hour = hour.ToString("00") + ":00";
        }
    }
}