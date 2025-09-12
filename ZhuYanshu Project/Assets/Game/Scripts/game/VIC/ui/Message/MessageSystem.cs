using Assets.Game.Scripts.game.VIC.ui.notif;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Message
{
    public class MessageSystem : MonoBehaviour
    {
        public static MessageSystem instance;

        public List<MessageBehaviour> messages = new List<MessageBehaviour>();

        public MessageBehaviour messagePrefab;
        public NotifSystem notifSystem;

        private void Awake()
        {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
            foreach (var 默认消息 in 默认的几个消息)
            {
                CreateMessage(默认消息, false);
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
                CreateMessage(开头的消息, true);
            }
        }

        public MessageBehaviour CreateMessage(MessagePrototype p, bool isNewMessage)
        {
            var mb = Instantiate(messagePrefab, messagePrefab.transform.parent);
            mb.transform.SetAsFirstSibling();

            mb.gameObject.SetActive(true);
            mb.Show(p, isNewMessage);

            if (isNewMessage)
            {
                notifSystem.Add(p);
            }
            return mb;
        }

        public MessagePrototype[] 开头的几个消息;
        public MessagePrototype[] 默认的几个消息;
    }
}