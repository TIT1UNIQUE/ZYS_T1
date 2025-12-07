using Assets.Game.Scripts.game.VIC.ui.ChatPanel;
using Assets.Game.Scripts.game.VIC.ui.Message.Chat;
using Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer;
using Assets.Game.Scripts.game.VIC.ui.notif;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Message
{
    public class MessageSystem : MonoBehaviour
    {
        public static MessageSystem instance;

        public List<MessageConnectionBehaviour> connections = new List<MessageConnectionBehaviour>();

        public MessageConnectionBehaviour messagePrefab;
        public NotifSystem notifSystem;
        public Sprite selfSp;
        public string selfName;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            foreach (var 默认消息 in 默认消息列表)
            {
                foreach (var m in 默认消息.messages)
                {
                    AddMessageOfChat(默认消息.name, m, false);
                }
            }
        }

        public void 初始化message系统()
        {
            StartCoroutine(开头弹出几个消息IE());
        }

        IEnumerator 开头弹出几个消息IE()
        {
            yield return new WaitForSeconds(1f);

            foreach (var 开头的消息 in 开头接受的消息列表)
            {
                foreach (var m in 开头的消息.chatData.messages)
                {
                    yield return new WaitForSeconds(开头的消息.preDelay);
                    AddMessageOfChat(开头的消息.chatData.name, m, true);
                }
            }
        }

        public void InsertBossRandomMessage()
        {
            var i = UnityEngine.Random.Range(0, bossRandomMessages.messages.Count);
            var m = bossRandomMessages.messages[i];
            AddMessageOfChat(bossRandomMessages.name, m, true);
        }

        public void AddMessageOfChat_now_myReply(string content, MessageComposerPrototype answerProto)
        {
            var chat = ChatPanelSystem.instance.currentChatData;
            MessagePrototype msg = new MessagePrototype();
            msg.name = selfName;
            msg.sp = selfSp;
            msg.content = content;
            msg.sdt = DateTime.Now;
            msg.answerProto = answerProto;
            //Debug.Log("AddMessageOfChat_now_myReply " + content);
            ChatPanelSystem.instance.AddMessageOfChat(chat.name, msg, true);
        }

        public void AddMessageOfChat_now(string chatDataName, MessagePrototype m, MessageComposerPrototype answerProto)
        {
            var chat = ChatPanelSystem.instance.GetChatData(chatDataName);
            if (m.briefIsRemote == MessagePrototype.BriefIsRemoteType.Remote)
            {
                m.name = chat.name;
                m.sp = chat.sp;
            }
            else if (m.briefIsRemote == MessagePrototype.BriefIsRemoteType.Self)
            {
                m.name = selfName;
                m.sp = selfSp;
            }
            m.answerProto = answerProto;
            m.sdt = DateTime.Now;
            //Debug.Log("AddMessageOfChat_now " + m.name + ":" + m.content);
            AddMessageOfChat(chatDataName, m, true);
        }

        public void AddMessageOfChat(string chatDataName, MessagePrototype p, bool isNewMessage)
        {
            if (p.content == "")
            {
                if (ChatPanelSystem.instance.currentChatData != null && ChatPanelSystem.instance.currentChatData.name == chatDataName)
                {
                    ChatPanelSystem.instance.textComposerDuolingo.Setup(p.answerProto);
                }
            }
            else
            {
                ChatPanelSystem.instance.AddMessageOfChat(chatDataName, p, isNewMessage);
            }
            if (isNewMessage)
                notifSystem.Add(chatDataName, p);
        }

        public void RefreshMessageConnection(string name, MessagePrototype p, bool isNewMessage)
        {
            //Debug.Log("RefreshMessageConnection chat " + name + ": " + p.name + " " + p.content);
            foreach (var con in connections)
            {
                if (con.chatData.name == name)
                {
                    con.SetMessage(p);
                    if (isNewMessage)
                        con.PlayRefreshAnimation();
                }
            }
        }

        public MessageConnectionBehaviour CreateConnection(ChatData c, MessagePrototype p, bool withAnim)
        {
            var mb = Instantiate(messagePrefab, messagePrefab.transform.parent);
            mb.transform.SetAsFirstSibling();

            mb.gameObject.SetActive(true);
            mb.Init(c);
            mb.SetMessage(p);
            if (withAnim)
                mb.PlayShowAnimation();
            else
                mb.PlayNoAnimation();

            connections.Add(mb);
            return mb;
        }

        public ChatData[] 默认消息列表;
        public MimicChatData[] 开头接受的消息列表;

        public ChatData bossRandomMessages;
    }

    [System.Serializable]
    public class MimicChatData
    {
        public float preDelay;
        public ChatData chatData;
    }
}