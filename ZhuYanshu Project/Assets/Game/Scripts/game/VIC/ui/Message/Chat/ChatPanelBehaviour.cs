using Assets.Game.Scripts.game.VIC.ui.Message;
using Assets.Game.Scripts.game.VIC.ui.Message.Chat;
using Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer;
using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using System;
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
            ScrollDown();
        }
        public void AddRemote(MessagePrototype proto)
        {
            var cb = Instantiate(prefab_itemRemote, chatParent);
            cb.gameObject.SetActive(true);
            cb.Show(proto);

            currentChats.Add(cb);
            ScrollDown();
        }

        public void Clear()
        {
            foreach (var c in currentChats)
            {
                Destroy(c.gameObject);
            }
            currentChats.Clear();
        }

        public void OnClickSubmit()
        {
            Debug.Log("OnClickSubmit");
            var r = textComposer.currentMcp.reply;
            var d = textComposer.currentMcp.replyDelay;
            MessageSystem.instance.AddMessageOfChat_now_myReply(textComposer.GetFinalString());
            com.SoundSystem.instance.Play("do");
            //Debug.Log(r);
            if (r != null)
            {
                //Debug.Log(r.content);
                var replyerName = ChatPanelSystem.instance.currentChatData.name;
                Debug.Log("replyerName " + replyerName + " content " + r.content);
                StartCoroutine(
                    MainScreenSystem.instance.DelayActionIE(
                     d,
                        () =>
                        {
                            if (r.content == "")
                            {
                                ChatPanelSystem.instance.textComposerDuolingo.Setup(r.answerProto);
                            }
                            else
                            {
                                MessageSystem.instance.AddMessageOfChat_now_remote(replyerName, r);
                            }
                        }
                       )
                    );
            }
        }

        public Canvas canvas;
        public ScrollRect scrollRect;
        public void ScrollDown()
        {
            Canvas.ForceUpdateCanvases();   // make sure layouts rebuilt
            scrollRect.DOVerticalNormalizedPos(0, 0.35f);
        }
    }
}