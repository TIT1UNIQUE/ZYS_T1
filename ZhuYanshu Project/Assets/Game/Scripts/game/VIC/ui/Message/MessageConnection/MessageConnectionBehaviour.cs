using Assets.Game.Scripts.game.VIC.ui.ChatPanel;
using DG.Tweening;
using LeTai.Asset.TranslucentImage;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.Message
{
    public class MessageConnectionBehaviour : MonoBehaviour
    {
        public Image img;
        public TextMeshProUGUI txt_content;
        public TextMeshProUGUI txt_name;
        public TextMeshProUGUI txt_time;

        public MessagePrototype coverMsg { get; private set; }
        public ChatData chatData { get; private set; }

        public void Init(ChatData c)
        {
            chatData = c;
        }

        public void SetMessage(MessagePrototype m)
        {
            this.coverMsg = m;
            if (m.name == chatData.name)
            {
                txt_content.text = m.content;
                img.sprite = m.sp;
                txt_name.text = m.name;
            }
            else
            {
                txt_content.text = m.name + ": " + m.content;
            }

            txt_time.text = m.timeStr;
        }

        public void Hide()
        {


        }

        public void OnClick()
        {
            //Debug.Log("OnClick");
            ChatPanelSystem.instance.Show(chatData);
        }

        public TranslucentImage tImg;
        public float endAlpha;
        public float startAlpha;

        public RectTransform rect_innerContainer;
        public float startAnchoredX;
        public float endAnchoredX;

        public float duration_ShowAnimation;
        public CanvasGroup cg;

        public void PlayNoAnimation()
        {
            //Debug.Log("PlayNoAnimation");
            cg.alpha = 1;

        }
        public void PlayRefreshAnimation()
        {
            // Debug.Log("PlayRefreshAnimation");
            //毛玻璃 alpha变化
            tImg.DOKill();
            var c = tImg.color;
            c.a = startAlpha;
            tImg.color = c;

            tImg.DOFade(endAlpha, duration_ShowAnimation);
        }

        public void PlayShowAnimation()
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