using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.Omni.Mission
{
    [CreateAssetMenu]
    public class MissionPackagePrototype : ScriptableObject
    {
        public string senderName;
        public string title;
        public string id;
        public MissionPrototype[] protos;
    }
}