using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Message.Chat
{
    [System.Serializable]
    public class ChatData
    {
        public string name;
        public Sprite sp;
        public List<MessagePrototype> messages = new List<MessagePrototype>();
    }
}