using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.notif
{
    public class NotifSystem : MonoBehaviour
    {
        public static NotifSystem instance;

        public RectTransform startAnchor;
        public RectTransform endAnchor;
        public float durationMove;
        public Ease ease;

        public NotifPrototype[] notifs_test;
        public NotifBehaviour prefab;

        private NotifBehaviour crtNotif;

        private void Awake()
        {
            instance = this;
        }

        public void StartMimic()
        {

            StartCoroutine(CreateMimicNotifIE());
        }

        IEnumerator CreateMimicNotifIE()
        {
            foreach (var n in notifs_test)
            {
                yield return new WaitForSeconds(n.mimicDelay);
                Add(n);
            }
        }

        void Add(NotifPrototype p)
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

        void HideCrtNotif()
        {
            if (crtNotif == null)
                return;

            crtNotif.Hide();
        }
    }
}