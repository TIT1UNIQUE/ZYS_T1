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
        public List<ChatData> messageCaches = new List<ChatData>();

        public ChatKeyWordsSystem chatKeyWordsSystem;
        public TextComposerDuolingoStyle textComposerDuolingo;

        private void Awake()
        {
            instance = this;
        }

        public void AddMessageOfChat(string chatDataName, MessagePrototype m, bool isNewMessage)
        {
            ChatData targetMc = null;
            foreach (var mc in messageCaches)
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
                targetMc.name = chatDataName;
                targetMc.messages.Add(m);
                messageCaches.Add(targetMc);
                MessageSystem.instance.CreateConnection(targetMc, m, isNewMessage);
            }
        }

        public ChatData GetMessageCache(string name)
        {
            foreach (var mc in messageCaches)
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
            chatKeyWordsSystem.Show(cd);
        }
    }

    [System.Serializable]
    public class ChatData
    {
        public string name;
        public List<MessagePrototype> messages = new List<MessagePrototype>();
    }
}