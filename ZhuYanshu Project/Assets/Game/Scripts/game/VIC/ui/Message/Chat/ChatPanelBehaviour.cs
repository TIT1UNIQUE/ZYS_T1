using Assets.Game.Scripts.game.VIC.ui.Message;
using Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer;
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
        public ChatItemBehaviour prefab_itemRemote;
        public ChatItemBehaviour prefab_itemSelf;
        public Transform chatParent;

        public List<ChatItemBehaviour> currentChats = new List<ChatItemBehaviour>();
        public TextComposerDuolingoStyle textComposer;

        private void Start()
        {
            Clear();
        }
        public void AddSelf(MessagePrototype proto)
        {
            var cb = Instantiate(prefab_itemSelf, chatParent);
            cb.gameObject.SetActive(true);
            cb.Show(proto);

            currentChats.Add(cb);
        }
        public void AddRemote(MessagePrototype proto)
        {
            var cb = Instantiate(prefab_itemRemote, chatParent);
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