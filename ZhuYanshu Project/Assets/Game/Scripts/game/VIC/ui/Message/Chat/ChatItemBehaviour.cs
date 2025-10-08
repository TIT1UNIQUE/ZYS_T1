using Assets.Game.Scripts.game.VIC.ui.Message;
using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using System.Collections;
using TMPro;
using UnityEngine;
using Assets.Game.Scripts.game.VIC.ui.ChatPanel;
using UnityEngine.UI;


namespace Assets.Game.Scripts.game.VIC.ui.ChatPanel
{
    public class ChatItemBehaviour : MonoBehaviour
    {
        public Image img;
        public TextMeshProUGUI txt_content;
        public TextMeshProUGUI txt_name;
        public TextMeshProUGUI txt_time;

        MessagePrototype proto;

        public void Show(MessagePrototype proto)
        {
            this.proto = proto;

            img.sprite = proto.sp;
            txt_name.text = proto.name;
            txt_content.text = proto.content;
            txt_time.text = proto.timeStr;
        }

        public void OnClick()
        {
             com.SoundSystem.instance.Play("tap");
        }
    }
}