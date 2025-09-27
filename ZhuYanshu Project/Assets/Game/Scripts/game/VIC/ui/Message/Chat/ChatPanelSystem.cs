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
            ChatData targetMc = null;
            foreach (var mc in chats)
            {

                if (mc.messages.Count > 0 && mc.messages[0].name == chatDataName)
                {
                    mc.messages.Add(m);
                    targetMc = mc;
                    MessageSystem.instance.RefreshMessageConnection(chatDataName, m, isNewMessage);
                }
            }

            if (targetMc == null)
            {
                targetMc = new ChatData();
                Debug.Log("create ChatData for " + chatDataName);
                for (int i = 0; i < personalTransParent.childCount; i++)
                {
                    var c = personalTransParent.GetChild(i);
                    if (c.gameObject.name.Contains(chatDataName))
                    {
                        targetMc.personalParent = c.gameObject;
                        Debug.Log("personal found");
                        break;
                    }
                }

                targetMc.name = chatDataName;
                targetMc.sp = m.sp;
                targetMc.messages.Add(m);
                chats.Add(targetMc);
                MessageSystem.instance.CreateConnection(targetMc, m, isNewMessage);
            }

            if (currentChatData != null && currentChatData.name == chatDataName)
            {
                textComposerDuolingo.Setup(m.answerProto);
                if (m.name == currentChatData.name)
                {
                    cpb.AddRemote(m);
                }
                else
                {
                    cpb.AddSelf(m);
                }
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
        public GameObject personalParent;
        public List<MessagePrototype> messages = new List<MessagePrototype>();
    }
}