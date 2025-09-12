using Assets.Game.Scripts.game.VIC.ui.Message;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.notif
{
    public class NotifSystem : MonoBehaviour
    {
        public RectTransform startAnchor;
        public RectTransform endAnchor;
        public float durationMove;
        public Ease ease;

        public NotifBehaviour prefab;

        private NotifBehaviour crtNotif;

        public void Add(MessagePrototype p)
        {
            HideCrtNotif();

            var newNotif = Instantiate(prefab, prefab.transform.parent);
            newNotif.gameObject.SetActive(true);
            newNotif.Init(p);

            var rect = newNotif.GetComponent<RectTransform>();
            rect.anchoredPosition = startAnchor.anchoredPosition;
            rect.DOAnchorPos(endAnchor.anchoredPosition, durationMove).SetEase(ease);

            crtNotif = newNotif;
        }

        public void HideCrtNotif()
        {
            if (crtNotif == null)
                return;

            crtNotif.Hide();
        }
    }
}