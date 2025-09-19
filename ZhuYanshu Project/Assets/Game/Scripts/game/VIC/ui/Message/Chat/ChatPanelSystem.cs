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
        public List<MessageCache> messageCaches = new List<MessageCache>();

        private void Awake()
        {
            instance = this;
        }

        public void AddMessage(MessagePrototype m)
        {
            MessageCache targetMc = null;
            foreach (var mc in messageCaches)
            {

                if (mc.messages.Count > 0 && mc.messages[0].name == m.name)
                {
                    targetMc = mc;
                }
            }

            if (targetMc == null)
            {
                targetMc = new MessageCache();
                targetMc.name = m.name;
                messageCaches.Add(targetMc);
                MessageSystem.instance.CreateMessageConnection(m);
            }

            targetMc.messages.Add(m);
            // MessageSystem.instance.renew?reddot??(m);
        }

        public MessageCache GetMessageCache(string name)
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

        public void Show(MessagePrototype m)
        {
            MessageCache mc = GetMessageCache(m.name);
            //show all chats of this message cache
            cpb.Clear();
            foreach (var p in mc.messages)
            {
                if (p.name == mc.name)
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

    public class MessageCache
    {
        public string name;
        public List<MessagePrototype> messages = new List<MessagePrototype>();
    }
}