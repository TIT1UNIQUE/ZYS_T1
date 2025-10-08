using Assets.Game.Scripts.game.VIC.ui.Message;
using Assets.Game.Scripts.game.VIC.ui.Message.Chat;
using Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer;
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
            for (int i = 0; i < personalTransParent.childCount; i++)
            {
                var c = personalTransParent.GetChild(i);
                c.gameObject.SetActive(false);
            }

            currentChatData = null;
            textComposerDuolingo.Setup(null);
        }

        public void AddMessageOfChat(string chatDataName, MessagePrototype m, bool isNewMessage)
        {
            ChatData targetChatData = null;
            foreach (var chat in chats)
            {

                if (chat.messages.Count > 0 && chat.messages[0].name == chatDataName)
                {
                    chat.messages.Add(m);
                    targetChatData = chat;
                    MessageSystem.instance.RefreshMessageConnection(chatDataName, m, isNewMessage);
                }
            }

            if (targetChatData == null)
            {
                targetChatData = new ChatData();
                Debug.Log("create ChatData for " + chatDataName);
                for (int i = 0; i < personalTransParent.childCount; i++)
                {
                    var c = personalTransParent.GetChild(i);
                    if (c.gameObject.name.Contains(chatDataName))
                    {
                        targetChatData.personalParent = c.gameObject;
                        Debug.Log("personal found");
                        break;
                    }
                }

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

            var lastMessage = cd.messages[cd.messages.Count - 1];
            textComposerDuolingo.Setup(lastMessage.answerProto);
            //chatKeyWordsSystem.Show(cd);
            for (int i = 0; i < personalTransParent.childCount; i++)
            {
                var c = personalTransParent.GetChild(i);
                c.gameObject.SetActive(c.gameObject == cd.personalParent);
            }
        }
    }

    [System.Serializable]
    public class ChatData
    {
        public string name;
        public Sprite sp;
        [HideInInspector]
        public GameObject personalParent;
        public List<MessagePrototype> messages = new List<MessagePrototype>();
    }
}