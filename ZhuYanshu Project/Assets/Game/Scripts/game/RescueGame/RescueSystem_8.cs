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
        public CanvasGroup sceneCg_Rescue;

        [SerializeField] RectTransform rescueBigImage;
        [SerializeField] float rescueBigImageScale1 = 0.78f;
        [SerializeField] float rescueBigImageScale2 = 0.9f;
        [SerializeField] float rescueBigImageScaleTime = 5f;

        [SerializeField] RectTransform pullHand;
        [SerializeField] RectTransform pullHandWater;

        [SerializeField] float pullHandYMax;
        [SerializeField] float pullHandYMin;
        [SerializeField] float pullHandFallSpeed;
        [SerializeField] float pullHandBoost;

        [SerializeField] CameraFilterPack_Atmosphere_Rain_Pro cfp_rainPro;

        [SerializeField] CameraFilterPack_Atmosphere_Rain cfp_rain;
        IEnumerator StartScene_Rescue(float delay)
        {
            pullHand.gameObject.SetActive(false);
            pullHandWater.gameObject.SetActive(false);
            rescueBigImage.localScale = Vector3.one * rescueBigImageScale1;
            cfp_rainPro.enabled = true;
            yield return new WaitForSeconds(delay);
            SoundSystem.instance.Play("thunder");
            rescueBigImage.DOScale(rescueBigImageScale2, rescueBigImageScaleTime);
            yield return new WaitForSeconds(rescueBigImageScaleTime + 2f);
            rescueBigImage.GetComponent<Image>().DOFade(0, 2);
            yield return new WaitForSeconds(2);
            cfp_rainPro.enabled = false;
            StartPullHandPuzzle();
        }

        private bool _isInPullHandPuzzle;
        void StartPullHandPuzzle()
        {
            rescueBigImage.gameObject.SetActive(false);
            pullHandWater.gameObject.SetActive(true);
            pullHand.gameObject.SetActive(true);
            _isInPullHandPuzzle = true;
            StartCoroutine(PullHandPuzzleProcess());
        }

        IEnumerator PullHandPuzzleProcess()
        {
            cfp_rain.enabled = true;
            while (pullHand.anchoredPosition.y < pullHandYMax)
            {
                //not done
                var pos = pullHand.anchoredPosition;
                pos.y -= pullHandFallSpeed * Time.deltaTime;
                if (pos.y < pullHandYMin)
                {
                    pos.y = pullHandYMin;
                }
                pullHand.anchoredPosition = pos;
                yield return null;
            }
            cfp_rain.enabled = false;
            SoundSystem.instance.Play("outwater");

            _isInPullHandPuzzle = false;
            OnPuzzleEnd_Rescue();
        }

        public void OnClickPullHand()
        {
            if (!_isInPullHandPuzzle)
                return;

            var y = pullHand.anchoredPosition.y;
            pullHand.DOKill();
            pullHand.DOAnchorPosY(y + pullHandBoost, 0.25f);
        }

        public void OnPuzzleEnd_Rescue()
        {
            pullHand.GetComponentInChildren<Image>().DOFade(0, 3).SetDelay(2);
            pullHandWater.GetComponent<Image>().DOFade(0, 2);

            StartCoroutine(DelayAction(5, StartPuzzle_DialogTwoPerson));
        }

        public void StartPuzzle_DialogTwoPerson()
        {
            ToggleCg(sceneCg_Rescue, false);
            ToggleCg(sceneCg_DialogTwoPerson, true);
            stage = GameStage.SceneBoyInBed_AfterMobileInteraction;
            StartCoroutine(StartScene_DialogTwoPerson(0.6f));
        }
    }
}