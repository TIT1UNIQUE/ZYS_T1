using Assets.Game.Scripts.game.VIC.ui.Message;
using Assets.Game.Scripts.game.VIC.ui.Message.Chat;
using Assets.Game.Scripts.game.VIC.ui.Message.Chat;
using Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.ChatPanel
{
    public class ChatPanelSystem : MonoBehaviour
    {
        public static ChatPanelSystem instance;
        public ChatPanelBehaviour cpb;
        public List<ChatData> chats = new List<ChatData>();

        //public ChatKeyWordsSystem chatKeyWordsSystem;
        public TextComposerDuolingoStyle textComposerDuolingo;
        public Transform personalTransParent;//use to find the personal exist in the scene
        public ChatData currentChatData;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SyncPersona("");
            currentChatData = null;
            textComposerDuolingo.Setup(null);
        }

        public void SyncPersona(string personName)
        {
            //Debug.Log("SyncPersonal " + personName);
            for (int i = 0; i < personalTransParent.childCount; i++)
            {
                var c = personalTransParent.GetChild(i);
                //Debug.Log(c.gameObject.name);
                if (!string.IsNullOrEmpty(personName) && c.gameObject.name.Contains(personName))
                {
                    c.gameObject.SetActive(true);
                }
                else
                {
                    c.gameObject.SetActive(false);
                }
            }
        }

        public void AddMessageOfChat(string chatDataName, MessagePrototype m, bool isNewMessage)
        {
            //Debug.Log("AddMessageOfChat " + chatDataName + ": " + m.name + " " + m.content + " isNewMessage:" + isNewMessage);
            ChatData targetChatData = GetChatData(chatDataName);
            if (targetChatData != null)
            {
                targetChatData.messages.Add(m);
                MessageSystem.instance.RefreshMessageConnection(chatDataName, m, isNewMessage);
            }
            else
            {
                targetChatData = new ChatData();
                // Debug.Log("create ChatData for " + chatDataName + " m " + m.name + ":" + m.content);
                targetChatData.name = chatDataName;
                if (targetChatData.sp == null && m.sp != null)
                {
                    targetChatData.sp = m.sp;
                }

                targetChatData.messages.Add(m);
                chats.Add(targetChatData);
                MessageSystem.instance.CreateConnection(targetChatData, m, isNewMessage);
            }

            if (currentChatData != null && currentChatData.name == chatDataName)
            {
                bool isRemote = false;
                switch (m.briefIsRemote)
                {
                    case MessagePrototype.BriefIsRemoteType.NotSet:
                        isRemote = m.name == currentChatData.name;
                        break;
                    case MessagePrototype.BriefIsRemoteType.Self:
                        m.name = MessageSystem.instance.selfName;
                        m.sp = MessageSystem.instance.selfSp;
                        isRemote = false;
                        break;
                    case MessagePrototype.BriefIsRemoteType.Remote:
                        m.name = currentChatData.name;
                        m.sp = currentChatData.sp;
                        isRemote = true;
                        break;
                }
                textComposerDuolingo.Setup(m.answerProto);
                if (isRemote)
                    cpb.AddRemote(m);
                else
                    cpb.AddSelf(m);
            }
        }

        public ChatData GetChatData(string name)
        {
            foreach (var mc in chats)
            {
                if (mc.messages.Count > 0 && mc.messages[0].name == name)
                {
                    return mc;
                }
            }

            return null;
        }

        public void Show(ChatData cd)
        {
            currentChatData = cd;
            //show all chats of this ChatData
            cpb.Clear();
            foreach (var p in cd.messages)
            {
                if (p.name == cd.name)
                {
                    cpb.AddRemote(p);
                }
                else
                {
                    cpb.AddSelf(p);
                }
            }
            //Debug.Log("show all chats of this ChatData");
            var lastMessage = cd.messages[cd.messages.Count - 1];
            //Debug.Log(lastMessage.content);
            // Debug.Log(lastMessage.name);
            textComposerDuolingo.Setup(lastMessage.answerProto);
            //chatKeyWordsSystem.Show(cd);
            SyncPersona(cd.name);
        }
    }
}