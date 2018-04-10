﻿using Assets.Scripts.General;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Conversation
{
    public class ConversationManager
    {
        public bool TrueEvent;
        public bool TruePlace;

        private bool General1InRoomWithLD;
        private bool General2InRoomWithLD;

        private static List<string> _textStatment = new List<string>();
        private static List<string> _textStatmentInform = new List<string>();

        private static List<string> _textRespondPositive = new List<string>();
        private static List<string> _textRespondUnsure = new List<string>();
        private static List<string> _textRespondNeutral = new List<string>();

        private static List<string> _textRespondNegative = new List<string>();
        private static List<string> _textRespondNegativeEvent = new List<string>();
        private static List<string> _textRespondNegativePlace = new List<string>();

        private static List<string> _textRespondInform = new List<string>();
        private static List<string> _textRespondInformCanNot = new List<string>();
        private static List<string> _textRespondInform2 = new List<string>();

        private readonly System.Random _randomNumber = new System.Random();

        private static ConversationManager _instance;

        /** replacement codes
         *  *XXXX where X is letter, this gives the abilty to give a simi human readable code and have a easy uesed stander to replace with data
         *  -> *EVNT for EVENT
         *  -> *LOCN for LOCATION
         *  -> *TIME for TIME
         *  -> *RAND for RANDOM
         */

        private ConversationManager()
        {
            try
            {
                _textStatment = LoadResponsesFrom("Text/Coversaion/coversaionText-Statment");
                _textRespondPositive = LoadResponsesFrom("Text/Coversaion/coversaionText-Responce-posvitive");
                _textRespondUnsure = LoadResponsesFrom("Text/Coversaion/coversaionText-Responce-negative");
                _textRespondNeutral = LoadResponsesFrom("Text/Coversaion/coversaionText-Responce-negative");
                _textRespondNegative = LoadResponsesFrom("Text/Coversaion/coversaionText-Responce-negative");
                _textRespondNegativeEvent = LoadResponsesFrom("Text/Coversaion/coversaionText-Responce-negativeEvents");
                _textRespondNegativePlace = LoadResponsesFrom("Text/Coversaion/conversaionText-Responce-negativePlace");

                //need thise files
                _textStatmentInform = LoadResponsesFrom("Text/Coversaion/coversaionText-StatmentInform");
                _textRespondInform = LoadResponsesFrom("Text/Coversaion/coversaionText-Respond-Inform");
                _textRespondInformCanNot = LoadResponsesFrom("Text/Coversaion/coversaionText-Respond-InformCanNot");
                _textRespondInform2 = LoadResponsesFrom("Text/Coversaion/coversaionText-Respond-Inform2");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public static ConversationManager Instance()
        {
            return _instance ?? (_instance = new ConversationManager());
        }

        List<string> LoadResponsesFrom(string fileName)
        {
            var textAsset = Resources.Load<TextAsset>(fileName);
            var responses = System.Text.Encoding.Default.GetString(textAsset.bytes);
            return responses.Split('\n').Select(x => x.Replace("\r", "").Trim()).ToList();
        }

        public Dictionary<IGeneral, string> ConversationGenerator(IGeneral general1, IGeneral general2)
        {
            Dictionary<IGeneral, string> conversation = new Dictionary<IGeneral, string>();
            GeneralConversationData general1ConversationData = InitialiseConversationData(general1);
            GeneralConversationData general2ConversationData = InitialiseConversationData(general2);

            //checks if the generals know they are in a room with a Listening Device
            General1InRoomWithLD = isGeneralInRoomWithListeningDevice(general1);
            General2InRoomWithLD = isGeneralInRoomWithListeningDevice(general2);

            //decises what type of conversation to have
            if (General1InRoomWithLD == false)
            {
                if (general1.knowenListeringDevices().Count != 0)
                {
                    int inform = _randomNumber.Next(0, 1);      //the second number is used to chagne the chance of informing, (1 is 50%, 2 is 33% and so on)
                    if (inform == 0)
                    {
                        return InformConversation(general1, general2, conversation, general1ConversationData, general2ConversationData);
                    }
                }
            }

            //need a if statment for the convo type
            return ObjectiveConversation(general1, general2, conversation, general1ConversationData, general2ConversationData);
        }
        
        //Conversation where they talk objectives, this may include informing about Listening Devices
        private Dictionary<IGeneral, string> ObjectiveConversation(IGeneral general1, IGeneral general2, Dictionary<IGeneral, string> conversation,
            GeneralConversationData general1ConversationData, GeneralConversationData general2ConversationData)
        {
            //are the generals lieing?
            if (_textStatment.Count != 0)
                conversation.Add(general1, TextFill(_textStatment.ElementAt(_randomNumber.Next(_textStatment.Count)), general1ConversationData));
            else
                conversation.Add(general1, "Error: Load didn't work");

            // TODO: Need new code to generate the response type, may need more types of responses to handle this
            var responseType = GeneralCompare(general1ConversationData, general2ConversationData, general1, general2);

            string tempText = "";
            
            switch (responseType)
            {
                case 0:
                    tempText = _textRespondPositive.ElementAt(_randomNumber.Next(_textRespondPositive.Count));
                    break;
                case 3:
                    tempText = _textRespondNeutral.ElementAt(_randomNumber.Next(_textRespondNeutral.Count));
                    break;
                case 10:
                case 17:                                                                                                    //rand
                case 11:
                    tempText = _textRespondNegativeEvent.ElementAt(_randomNumber.Next(_textRespondNegativeEvent.Count));
                    break;   //event
                case 12:
                    tempText = _textRespondNegativePlace.ElementAt(_randomNumber.Next(_textRespondNegativePlace.Count));
                    break;   //place
                case 13:
                    tempText = _textRespondNegative.ElementAt(_randomNumber.Next(_textRespondNegative.Count));
                    break;           //event\place
                case 20:
                case 27:                                                                                                    //rand
                case 21:                                                                                                    //event
                case 22:                                                                                                    //place
                case 23:
                    tempText = _textRespondUnsure.ElementAt(_randomNumber.Next(_textRespondUnsure.Count));
                    break;           //event\place
                case 100:
                    tempText = _textRespondInform2.ElementAt(_randomNumber.Next(_textRespondInform2.Count));
                    General2Inform(general1, general2);
                    break;
                default:
                    tempText = "ERROR!!";
                    break;
            }

            if(responseType != 100)
            {
                if(general2.knowenListeringDevices().Count != 0)
                {
                    int inform = _randomNumber.Next(0, 1);
                    if (inform == 1)
                    {
                        General2Inform(general1, general2);
                    }

                }
            }

            conversation.Add(general2, TextFill(tempText, general2ConversationData));
            return conversation;
        }

        //Conversation where they openly inform one and other about Listening Devices
        private Dictionary<IGeneral, string> InformConversation(IGeneral general1, IGeneral general2, Dictionary<IGeneral, string> conversation,
            GeneralConversationData general1ConversationData, GeneralConversationData general2ConversationData)
        {

            if (_textStatmentInform.Count != 0)
                conversation.Add(general1, TextFill(_textStatmentInform.ElementAt(_randomNumber.Next(_textStatmentInform.Count)), general1ConversationData));
            else
                conversation.Add(general1, "Error: Load didn't work");

            general2.Informed(general1.knowenListeringDevices());
            if (general2.knowenListeringDevices().Count == 0)
            {
                if (_textRespondInform.Count != 0)
                    conversation.Add(general2, TextFill(_textRespondInform.ElementAt(_randomNumber.Next(_textRespondInform.Count)), general2ConversationData));
                else
                    conversation.Add(general2, "Error: Load didn't work");
            }
            else
            {
                if (_textStatmentInform.Count != 0)
                    conversation.Add(general2, TextFill(_textStatmentInform.ElementAt(_randomNumber.Next(_textStatmentInform.Count)), general2ConversationData));
                else
                    conversation.Add(general2, "Error: Load didn't work");

                general1.Informed(general2.knowenListeringDevices());
            }

            return conversation;
        }
        
        private void General2Inform(IGeneral general1, IGeneral general2)
        {
            if(general1.knowenListeringDevices().Count != 0)
            {
                general2.Informed(general1.knowenListeringDevices());
            }
            general1.Informed(general2.knowenListeringDevices());
        }

        private GeneralConversationData InitialiseConversationData(IGeneral general1)
        {
            GeneralConversationData conversationData = new GeneralConversationData();
            conversationData.Truthfulness = CalculateGeneralTruthfulness(general1);
            CalculateEventPlaceTime(general1, conversationData);

            return conversationData;
        }
        
        private int GeneralCompare(GeneralConversationData general1ConversationData, GeneralConversationData general2ConversationData, IGeneral general1, IGeneral general2)
        {
            int response = -1;
            string tempEvent;
            string tempPlace;
            //string tempTime = "";     //need to be added later

            //bool trueEvent = false;
            //bool truePlace = false;
            //bool trueTime = false;    //need to be added later

            if (general2ConversationData.Truthfulness >= 10)
            {
                tempEvent = Objective.Event;
                tempPlace = Objective.Place;
            }
            else
            {
                tempEvent = general2ConversationData.Event;
                tempPlace = general2ConversationData.Place;
            }

            if (tempEvent.Equals(general1ConversationData.Event))
                TrueEvent = true;
            if (tempPlace.Equals(general1ConversationData.Place))
                TruePlace = true;

            if (isGeneralInRoomWithListeningDevice(general1))
            {
                general1ConversationData.Lying = true;
            }
            else
            {
                switch (general1ConversationData.Truthfulness)
                {
                    case 11:
                    case 1:
                        general1ConversationData.Lying = true;
                        break;
                    case 0:
                    case 10:
                        general1ConversationData.Lying = false;
                        break;
                }
            }

            if (isGeneralInRoomWithListeningDevice(general2))
            {
                TrueEvent = !TrueEvent;
                TruePlace = !TruePlace;
                general2ConversationData.Lying = true;
            }
            else
            {
                switch (general2ConversationData.Truthfulness)
                {
                    case 11:
                        TrueEvent = !TrueEvent;
                        TruePlace = !TruePlace;
                        general2ConversationData.Lying = true;
                        break;
                    case 1:
                        general2ConversationData.Lying = true;
                        break;
                    case 0:
                    case 10:
                        general2ConversationData.Lying = false;
                        break;
                }
            }

            if (TrueEvent && TruePlace)
            {                 //error where if one is not true then the reponce is positive, need to debug
                response = 0;
            }
            else if (TrueEvent && !TruePlace)
            {
                response = 12;
            }
            else if (!TrueEvent && TruePlace)
            {
                response = 11;
            }
            else if (!TrueEvent && !TruePlace)
            {
                response = 3;
            }
            
            if(general2ConversationData.Lying == true)
            {
                List<GameObject> general1KnowenDeives = general1.knowenListeringDevices();
                List<GameObject> general2KnowenDeives = general2.knowenListeringDevices();

                bool match = true;

                for(int i = 0; i < general1KnowenDeives.Count; ++i)
                {
                    for (int j = 0; j < general2KnowenDeives.Count; ++j)
                    {
                        if(general1KnowenDeives[i] != general2KnowenDeives[j])
                        {
                            match = false;
                            j += general2KnowenDeives.Count;
                            i += general1KnowenDeives.Count;
                        }
                    }
                }

                if (match == false)
                {
                    int num = _randomNumber.Next(10);
                    if (num < 5)
                    {
                        response = 100;
                    }
                }

            }

                return response;
        }
        
        private string TextFill(string temp, GeneralConversationData conversationData)
        {
            string filled = "";
            temp = temp.Trim();

            while (temp != "")
            {
                if (!temp.Contains("*"))
                {
                    filled = filled + temp;
                    temp = "";
                }
                else
                {
                    if (temp.IndexOf("*") != 0)
                    {
                        filled = filled + temp.Substring(0, temp.IndexOf("*"));
                        temp = temp.Substring(temp.IndexOf("*"));
                    }
                    else
                    {
                        string type = temp.Substring(1, 4);
                        temp = temp.Substring(5);
                        switch (type)
                        {
                            case "EVNT":
                                filled = filled + conversationData.Event;
                                break;
                            case "LOCN":
                                filled = filled + conversationData.Place;
                                break;
                            case "TIME":
                                filled = filled + conversationData.Time;
                                break;
                            case "RAND":
                                switch (_randomNumber.Next(3))
                                {
                                    case 0:
                                        filled = filled + conversationData.Event + "ing";
                                        break;
                                    case 1:
                                        filled = filled + conversationData.Place;
                                        break;
                                    case 2:
                                        filled = filled + conversationData.Time;
                                        break;
                                }
                                break;
                            default:
                                filled += "ERROR!!";
                                break;
                        }
                    }
                }
            }

            return filled;
        }
        
        void CalculateEventPlaceTime(IGeneral general, GeneralConversationData conversationData)
        {
            string eventString;
            string place;
            string time;

            var truthful = conversationData.Truthfulness;

            if (truthful >= 10)
            {
                eventString = Objective.Event;
                place = Objective.Place;
                time = Objective.Time;
            }
            else
            {
                List<string> events = Objective.Events;
                eventString = events.ElementAt(_randomNumber.Next(events.Count));
                List<string> places = Objective.Places;
                place = places.ElementAt(_randomNumber.Next(places.Count));
                time = Objective.Time;
            }

            if (truthful == 11 || truthful == 1)
            {
                List<string> events = Objective.Events;
                string trueEvent = eventString;
                while (trueEvent.Equals(eventString))
                {
                    eventString = events.ElementAt(_randomNumber.Next(events.Count));
                }
                List<string> places = Objective.Places;
                string truePlace = place;
                while (truePlace.Equals(place))
                {
                    place = places.ElementAt(_randomNumber.Next(places.Count));
                }
            }
            
            conversationData.Event = eventString;
            conversationData.Place = place;
            conversationData.Time = time;
        }
        
        //Truthfulness to be read int value to be read as >=10 knows the truth, (after -10 if know truth) if  == 1 then will lie
        int CalculateGeneralTruthfulness(IGeneral general)
        {
            int truthfulness = 0;

            if (_randomNumber.Next(10) <= general.GetKnowledge())
                truthfulness += 10;

            if (_randomNumber.Next(10) <= general.GetTrust())
                truthfulness += 1;
            
            return truthfulness;
        }

        private bool isGeneralInRoomWithListeningDevice(IGeneral general)
        {
            Room room = general.GetCharacter().CurrentRoom;
            
            List<GameObject> LD = general.knowenListeringDevices();

            for (int i = 0; i < LD.Count; ++i)
            {
                if (LD[i].GetComponent<ListeningDevice>().CurrentRoom == room)
                {
                    return true;
                }
            }
            
            return false;
        }
        
    }
}