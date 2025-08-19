using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.Omni.Mission
{
    public class MissionData
    {
        public MissionPrototype proto;

        public enum State
        {
            Waiting,
            PendingInNotification,
            Unread,
            Read,
            Done,
        }

        public State state = State.Waiting;
        
        public string missionPackageId;
    }
}