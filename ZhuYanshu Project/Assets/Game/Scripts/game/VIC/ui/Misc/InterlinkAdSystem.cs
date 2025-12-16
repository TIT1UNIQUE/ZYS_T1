using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Misc
{
    public class InterlinkAdSystem : MonoBehaviour
    {
        public static InterlinkAdSystem instance;

        public CanvasGroup cg;
        public RectTransform ad1Panel;
        public RectTransform ad2Panel;

        private float scale_ad1Panel;
        private float scale_ad2Panel;
        public GameObject btn1;
        public GameObject btn2;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            scale_ad1Panel = ad1Panel.localScale.x;
            scale_ad2Panel = ad2Panel.localScale.x;
            Hide();
        }

        public void PopupAd()
        {
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            cg.interactable = true;

            ad1Panel.gameObject.SetActive(true);
            ad2Panel.gameObject.SetActive(false);
            com.SoundSystem.instance.Play("tap");
            ad1Panel.DOKill();
            ad1Panel.localScale = Vector3.zero;
            ad1Panel.DOScale(scale_ad1Panel, 0.4f).SetEase(Ease.OutBounce).OnComplete(
           () => { btn1.SetActive(true); });
        }

        public void OnClickAd1()
        {
            btn1.SetActive(false);
            btn2.SetActive(false);

            ad1Panel.gameObject.SetActive(false);
            ad2Panel.gameObject.SetActive(true);
            com.SoundSystem.instance.Play("pay");
            ad2Panel.DOKill();
            ad2Panel.localScale = Vector3.zero;
            ad2Panel.DOScale(scale_ad2Panel, 0.4f).SetDelay(0.15f).SetEase(Ease.OutBounce).OnComplete(
           () => { btn2.SetActive(true); });
        }

        public void OnClickAd2()
        {
            Hide();
        }

        public void Hide()
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            cg.interactable = false;
            ad1Panel.gameObject.SetActive(false);
            ad2Panel.gameObject.SetActive(false);
            btn1.SetActive(false);
            btn2.SetActive(false);
        }
    }
}