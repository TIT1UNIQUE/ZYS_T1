using Assets.Game.Scripts.game.VIC.ui.Message;
using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.ChatPanel
{
    public class ChatPanelBehaviour : MonoBehaviour
    {
        public ChatItemBehaviour prefab_itemLeft;
        public ChatItemBehaviour prefab_itemRight;
        public Transform chatParent;

        public List<ChatItemBehaviour> currentChats = new List<ChatItemBehaviour>();
        private void Start()
        {
            Clear();
        }
        public void AddSelf(MessagePrototype proto)
        {
            var cb = Instantiate(prefab_itemRight, chatParent);
            cb.gameObject.SetActive(true);
            cb.Show(proto);

            currentChats.Add(cb);
        }
        public void AddRemote(MessagePrototype proto)
        {
            var cb = Instantiate(prefab_itemLeft, chatParent);
            cb.gameObject.SetActive(true);
            cb.Show(proto);

            currentChats.Add(cb);
        }

        public void Clear()
        {
            foreach (var c in currentChats)
            {
                Destroy(c.gameObject);
            }
            currentChats.Clear();
        }
    }
}