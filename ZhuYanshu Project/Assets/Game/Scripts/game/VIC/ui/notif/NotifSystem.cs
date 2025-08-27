using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.notif
{
    public class NotifSystem : MonoBehaviour
    {
        public static NotifSystem instance;

        public Vector2 startAnchorPos;
        public Vector2 endAnchorPos;
        public float durationMove;
        public float durationFade;
        public float durationDelay;
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
            FadeCrtNotif();

            var newNotif = Instantiate(prefab, prefab.transform.parent);
            newNotif.Init(p);
            newNotif.gameObject.SetActive(true);

            var rect = newNotif.GetComponent<RectTransform>();
            rect.anchoredPosition = startAnchorPos;
            rect.DOAnchorPos(endAnchorPos, durationMove).SetEase(ease);

            crtNotif = newNotif;
        }

        void FadeCrtNotif()
        {
            if (crtNotif == null)
                return;

            var rect = crtNotif.GetComponent<RectTransform>();
            rect.DOKill();
            rect.DOAnchorPos(startAnchorPos, durationMove).SetDelay(durationDelay);
            rect.GetComponent<CanvasGroup>().DOFade(0, durationFade).OnComplete(DestroyCrtNotif).SetDelay(durationDelay);
        }

        void DestroyCrtNotif()
        {
            Destroy(crtNotif.gameObject);
            crtNotif = null;
        }
    }
}