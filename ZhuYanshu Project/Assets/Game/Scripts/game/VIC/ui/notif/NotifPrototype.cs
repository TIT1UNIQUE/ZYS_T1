using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.notif
{
    [System.Serializable]
    public class NotifPrototype
    {
        public string title;
        public Sprite sp;
        [Multiline]
        public string content;

        public float mimicDelay;
    }
}