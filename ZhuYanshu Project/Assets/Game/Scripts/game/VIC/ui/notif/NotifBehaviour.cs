using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Game.Scripts.game.VIC.ui.notif
{
    public class NotifBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI titleTxt;
        public TextMeshProUGUI contentTxt;
        public TextMeshProUGUI timeTxt;
        public Image img;
        public float durationFade;
        public float durationDelay;
        public float lifetime = 3.5;

        private void Awake()
        {
        }

        public void Init(NotifPrototype proto)
        {
            titleTxt.text = proto.title;
            contentTxt.text = proto.content;
            img.sprite = proto.sp;
            timeTxt.text = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);

            hide = false;
            StartCoroutine(HideSelf());
        }

        IEnumerator HideSelf()
        {
            yield return new WaitForSeconds(lifetime);
            Hide();
        }

        private bool hide;
        public void Hide()
        {
            if (hide)
                return;

            hide = true;

            var rect = GetComponent<RectTransform>();
            rect.DOKill();
            rect.GetComponent<CanvasGroup>().DOFade(0, durationFade).OnComplete(DestroyCrtNotif).SetDelay(durationDelay);
        }

        void DestroyCrtNotif()
        {
            Destroy(gameObject);
        }
    }
}