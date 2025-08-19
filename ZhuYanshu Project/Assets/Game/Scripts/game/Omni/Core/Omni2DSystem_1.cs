using Assets.Game.Scripts.game.Omni.InGameUi;
using com;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Omni
{

    public partial class Omni2DSystem : MonoBehaviour
    {

        public CanvasGroup cg_computer_bg;

        bool _isScreenOn;
        public CanvasGroup cg_office_2d_login;
        public CanvasGroup cg_computer_welcome;
        public RectTransform screenRect;
        public RectTransform screenRect_fullScreenRef;
        public PasswordBehaviour pb;

        public Sticker sticker;
        public Image bootBtnImg;
        public Sprite bootBtnOn;
        public Sprite bootBtnOff;

        void StartGameSetup()
        {
            _isScreenOn = false;
            cg_computer_welcome.alpha = 0;
            cg_computer_bg.alpha = 0;
            bootBtnImg.sprite = bootBtnOff;
        }

        public void OnScreenPowerButtonClicked()
        {
            if (!_isScreenOn)
            {
                _isScreenOn = true;
                bootBtnImg.sprite = bootBtnOn;

                StartCoroutine(ZoomToFullScreen());
            }
        }

        IEnumerator ZoomToFullScreen()
        {
            yield return new WaitForSeconds(0.4f);
            screenRect.DOScale(screenRect_fullScreenRef.localScale.x, 2);
            screenRect.DOAnchorPos(screenRect_fullScreenRef.anchoredPosition, 2);
            yield return new WaitForSeconds(2f);
            cg_computer_welcome.DOKill();
            cg_computer_welcome.DOFade(1, 2).OnComplete(
                () =>
                {
                    cg_computer_bg.alpha = 1;
                    cg_computer_welcome.DOFade(0, 1.5f).SetDelay(1f);
                     sticker.canRemove = true;
                }
                );
            yield return new WaitForSeconds(5.0f);
           
            pb.Boot();
        }

        public void OnLoginSuc()
        {
            pb.TurnOffPasswordScreen();
        }
    }
}