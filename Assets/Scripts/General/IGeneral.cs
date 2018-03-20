using System.Collections.Generic;
using Assets.Scripts.AI;
using UnityEngine;
using Event = Assets.Scripts.EventSystem.Event;

namespace Assets.Scripts.General
{
    public interface IGeneral
    {
        void UpdateTrustValue(int trustDifference);
        void UpdateKnowledgeValue(int knowledgeDifference);
        void Informed(List<GameObject> otherGeneralsKnownList);
        List<GameObject> knowenListeringDevices();
        void ConsumeEvent(Event subscribeEvent, object eventPacket);
        void SubscribeToEvents();
        int GetKnowledge();
        int GetTrust();
        int GetPerception();
        NeedStatus GetNeed(NeedType needType);
        NeedType GetLowestNeed();
        void SatisfyBiggestNeed();
        void GeneralObjectiveKnowlage();
        string GetObjectivePlace();
        string GetObjectiveEvent();
        string GetObjectiveTime();
        Character2D GetCharacter();
    }
}