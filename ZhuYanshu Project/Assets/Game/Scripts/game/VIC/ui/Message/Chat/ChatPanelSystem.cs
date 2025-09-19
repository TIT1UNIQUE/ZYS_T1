using Assets.Game.Scripts.game.VIC.ui.Message;
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

        private void Awake()
        {
            instance = this;
        }

        public void AddMessage(MessagePrototype m, bool isNewMessage)
        {
            ChatData targetMc = null;
            foreach (var mc in messageCaches)
            {

                if (mc.messages.Count > 0 && mc.messages[0].name == m.name)
                {
                    mc.messages.Add(m);
                    targetMc = mc;
                     // MessageSystem.instance.renew?reddot??(m);
                }
            }

            if (targetMc == null)
            {
                targetMc = new ChatData();
                targetMc.name = m.name;
                targetMc.messages.Add(m);
                messageCaches.Add(targetMc);
                MessageSystem.instance.CreateConnection(targetMc,m,isNewMessage);
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
            //show all chats of this message cache
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

        }
    }

    public class ChatData
    {
        public string name;
        public List<MessagePrototype> messages = new List<MessagePrototype>();
    }
}