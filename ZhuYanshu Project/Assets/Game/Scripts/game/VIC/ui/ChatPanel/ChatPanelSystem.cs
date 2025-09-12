using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.ChatPanel
{
    public class ChatPanelSystem : MonoBehaviour
    {
        public static ChatPanelSystem instance;
        public ChatPanelBehaviour cpb;

        private void Awake()
        {
            instance = this;
        }

    }
}