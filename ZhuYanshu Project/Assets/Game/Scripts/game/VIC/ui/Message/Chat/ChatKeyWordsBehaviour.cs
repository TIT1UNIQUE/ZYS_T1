using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Message.Chat
{
    public class ChatKeyWordsBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI txt;
        public RectTransform rect;

        public void Init(string s)
        {
            txt.text = s;
            //fit size size container
        }
    }
}