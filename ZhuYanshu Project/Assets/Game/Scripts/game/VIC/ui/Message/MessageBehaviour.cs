using Assets.Game.Scripts.game.VIC.ui.ChatPanel;
using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.Message
{
    public class MessageBehaviour : MonoBehaviour
    {
        public Image img;
        public TextMeshProUGUI txt_content;
        public TextMeshProUGUI txt_name;
        public TextMeshProUGUI txt_time;
        MessagePrototype proto;

        public void Show(MessagePrototype proto, bool withAnim)
        {
            this.proto = proto;
            Show(proto.sp, proto.name, proto.content, proto.timeStr, withAnim);
        }

        public void Show(Sprite sp, string name, string content, string timeStr, bool withAnim)
        {
            img.sprite = sp;
            txt_name.text = name;
            txt_content.text = content;
            txt_time.text = timeStr;

            if (withAnim)
                PlayShowAnimation();
            else
                DefaultShow();
        }

        public void Hide()
        {


        }

        public void OnClick()
        {
            Debug.Log("OnClick");
            ChatPanelSystem.instance.cpb.Show(this.proto);
        }

        public TranslucentImage tImg;
        public float endAlpha;
        public float startAlpha;

        public RectTransform rect_innerContainer;
        public float startAnchoredX;
        public float endAnchoredX;

        public float duration_ShowAnimation;
        public CanvasGroup cg;

        void DefaultShow()
        {
            //Debug.Log("ShowContext");
            cg.alpha = 1;
        }

        void PlayShowAnimation()
        {
            //Debug.Log("PlayShowAnimation");
            cg.alpha = 0;

            //毛玻璃 alpha变化
            tImg.DOKill();
            var c = tImg.color;
            c.a = startAlpha;
            tImg.color = c;

            tImg.DOFade(endAlpha, duration_ShowAnimation);

            //里面的图 anchor position变化
            rect_innerContainer.DOKill();
            var a = rect_innerContainer.anchoredPosition;
            a.x = startAnchoredX;
            rect_innerContainer.anchoredPosition = a;
            rect_innerContainer.DOAnchorPosX(endAnchoredX, duration_ShowAnimation).OnComplete(ShowContext);
        }

        void ShowContext()
        {
            cg.DOFade(1, 0.35f);
        }
    }
}