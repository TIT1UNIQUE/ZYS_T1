using Assets.Game.Scripts.game.RescueGame;
using com;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Rescue
{
    public partial class RescueSystem : MonoBehaviour
    {
        [SerializeField] Transform mobile_boy;
        [SerializeField] Image boy_before;
        [SerializeField] Image boy_after;

        IEnumerator StartScene_WakeUpBoy()
        {
            boyInBed_1.SetActive(true);
            boyInBed_2.SetActive(false);
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(MobileVibrateLoop_boy());
            StartCoroutine(DelayAction(4.5f, ZoomInToShowMobile_boy));
        }

        IEnumerator MobileVibrateLoop_boy()
        {
            yield return new WaitForSeconds(2);

            while (stage == GameStage.SceneBoyInBed_PendingMobileInteraction
                ||
                stage == GameStage.SceneBoyInBed_beforeMobileInteraction)
            {
                MobileVibrate_boy();
                yield return new WaitForSeconds(4);
            }
        }

        public void OnClickMobile_boy()
        {
            if (stage != GameStage.SceneBoyInBed_PendingMobileInteraction)
                return;

            ToggleOffMobileAlarm_boy();
        }

        [SerializeField] Transform zoomParent_boy;
        [SerializeField] Transform zoomParentZoomInRectTrans_boy;
        void ZoomInToShowMobile_boy()
        {
            zoomParent_boy.DOMove(zoomParentZoomInRectTrans_boy.transform.position, zoomParentZoomInDuration).SetEase(Ease.InOutCubic);
            zoomParent_boy.DOScale(1.5f, zoomParentZoomInDuration).SetEase(Ease.InOutCubic).OnComplete(
                () =>
                {
                    stage = GameStage.SceneBoyInBed_PendingMobileInteraction;
                }
                );
        }

        void ZoomOutToShowBed_boy()
        {
            zoomParent_boy.DOMove(zoomParentOrginalPos, zoomParentZoomInDuration).SetEase(Ease.InOutCubic);
            zoomParent_boy.DOScale(1, zoomParentZoomInDuration).SetEase(Ease.InOutCubic).OnComplete(
                () =>
                {
                    SwitchInBedImage_boy();
                    StartCoroutine(DelayAction(2.0f, SwitchToWashScene_boy));
                }
                );
        }

        [SerializeField] GameObject boyInBed_1;
        [SerializeField] GameObject boyInBed_2;

        void SwitchInBedImage_boy()
        {
            boyInBed_1.SetActive(false);
            boyInBed_2.SetActive(true);
            boyInBed_2.GetComponent<UiImageAnimation>().Play(0);
        }

        void MobileVibrate_boy()
        {
            mobileVibSound.Play();
            mobile_boy.DOShakePosition(2.5f, mobileVibStrength, mobileVibVibrato, fadeOut: false);
        }

        void ToggleOffMobileAlarm_boy()
        {
            SoundSystem.instance.Play("switchoff");
            mobileVibSound.Stop();
            mobile_boy.DOKill();
            stage = GameStage.SceneBoyInBed_AfterMobileInteraction;
            ZoomOutToShowBed_boy();
        }

        void SwitchToWashScene_boy()
        {
            SwitchToWashScene(true);
            boy_before.gameObject.SetActive(true);
            boy_before.color = Color.white;
            boy_after.gameObject.SetActive(false);
            ClearWhiteDots();
        }

        [SerializeField] int _needEatTime = 3;
        [SerializeField] MedicineBottle mb;
        public void OnEatPill()
        {
            _needEatTime -= 1;
            Debug.Log("OnEatPill _needEatTime " + _needEatTime);
            if (_needEatTime == 0)
            {
                mb.enabled = false;
                mb.GetComponent<Image>().DOFade(0, 2);
                boy_before.DOColor(new Color(1, 1, 1, 0), 2).OnComplete(() => { boy_before.gameObject.SetActive(false); });

                boy_after.gameObject.SetActive(true);
                boy_after.color = new Color(1, 1, 1, 0);
                boy_after.DOColor(new Color(1, 1, 1, 1), 2).OnComplete(() =>
                {
                    StartCoroutine(DelayAction(4f, SwitchToBoyEatScene));
                });
            }
        }

        void SwitchToBoyEatScene()
        {
            ToggleCg(sceneCg_WashBoy, false);
            ToggleCg(sceneCg_FoodBoy, true);
            ShowBoyFoodScene();
        }
    }
}