using Assets.Game.Scripts.game.VIC.ui.ChatPanel;
using Assets.Game.Scripts.game.VIC.ui.notif;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Message
{
    public class MessageSystem : MonoBehaviour
    {
        public static MessageSystem instance;

        public List<MessageConnectionBehaviour> messages = new List<MessageConnectionBehaviour>();

        public MessageConnectionBehaviour messagePrefab;
        public NotifSystem notifSystem;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            foreach (var 默认消息 in 默认的几个消息)
            {
                AddMessage(默认消息, false);
            }
        }

        public void 初始化message系统()
        {
            StartCoroutine(开头弹出几个消息IE());
        }

        IEnumerator 开头弹出几个消息IE()
        {
            yield return new WaitForSeconds(1f);

            foreach (var 开头的消息 in 开头的几个消息)
            {
                yield return new WaitForSeconds(1);
                AddMessage(开头的消息, true);
            }
        }

        public void AddMessage(MessagePrototype p, bool isNewMessage)
        {
            //var mc = ChatPanelSystem.instance.GetMessageCache(p.name);
            ChatPanelSystem.instance.AddMessage(p);

            if (isNewMessage)
                notifSystem.Add(p);
        }

        public MessageConnectionBehaviour CreateMessageConnection(MessagePrototype p)
        {
            var mb = Instantiate(messagePrefab, messagePrefab.transform.parent);
            mb.transform.SetAsFirstSibling();

            mb.gameObject.SetActive(true);
            mb.Show(p, true);
            ChatPanelSystem.instance.AddMessage(p);

            return mb;
        }

        public MessagePrototype[] 开头的几个消息;
        public MessagePrototype[] 默认的几个消息;
    }
}