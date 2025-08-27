using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.notif
{
    public class NotifBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI titleTxt;
        public TextMeshProUGUI contentTxt;
        public TextMeshProUGUI timeTxt;
        public Image img;

        public void Init(NotifPrototype proto)
        {
            titleTxt.text = proto.title;
            contentTxt.text = proto.content;
            img.sprite = proto.sp;
            timeTxt.text = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);
        }

    }
}