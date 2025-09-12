using Assets.Game.Scripts.game.VIC.ui.Message;
using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.ChatPanel
{
    public class ChatPanelBehaviour : MonoBehaviour
    {
        public Image img;
        public TextMeshProUGUI txt_content;
        public TextMeshProUGUI txt_name;
        public TextMeshProUGUI txt_time;

        private void Start()
        {
            Clear();
        }
        public void Show(MessagePrototype proto)
        {
            img.enabled = true;
            img.sprite = proto.sp;
            txt_name.text = proto.name;
            txt_content.text = proto.content;
            txt_time.text = proto.timeStr;
        }

        public void Clear()
        {
            img.enabled = false;
            txt_name.text = "";
            txt_content.text = "";
            txt_time.text = "";
        }

        public void OnClick()
        {
            Debug.Log("OnClick");
        }
    }
}