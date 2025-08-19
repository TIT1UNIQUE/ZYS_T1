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
        public CanvasGroup sceneCg_ClockNarrative;

        IEnumerator StartScene_ClockNarrative(float delay)
        {
            var cg = boyClock.GetComponent<CanvasGroup>();
            cg.blocksRaycasts = true;
            cg.interactable = true;
            cg.alpha = 0;

            var cg2 = girlClock.GetComponent<CanvasGroup>();
            cg2.blocksRaycasts = false;
            cg2.interactable = false;
            cg2.alpha = 0;

            foreach (var bg in boyClockBeforeActiveGroup)
                bg.SetActive(true);
            foreach (var bg in boyClockAfterActiveGroup)
                bg.SetActive(false);

            foreach (var bg in girlClockBeforeActiveGroup)
                bg.SetActive(false);
            foreach (var bg in girlClockAfterActiveGroup)
                bg.SetActive(false);

            yield return new WaitForSeconds(delay);
            cg.DOFade(1, 2f);
        }

        public void OnPuzzleEnd_ClockNarrative()
        {
            waterLoopSound.Stop();
            StartPuzzle_Rescue();
        }

        public void StartPuzzle_ClockNarrative()
        {
            ToggleCg(sceneCg_ClockNarrative, false);
        }

        [SerializeField] ClockBehaviour boyClock;
        [SerializeField] ClockBehaviour girlClock;
        [SerializeField] GameObject[] boyClockBeforeActiveGroup;
        [SerializeField] GameObject[] boyClockAfterActiveGroup;
        [SerializeField] GameObject[] girlClockBeforeActiveGroup;
        [SerializeField] GameObject[] girlClockAfterActiveGroup;
        [SerializeField] AudioSource waterLoopSound;


        public void ReachBoyClockEnd()
        {
            boyClock.enabled = false;
            var cg = boyClock.GetComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
            cg.interactable = false;
            cg.DOFade(0, 2).OnComplete(ShowDragBoyToPool);
        }

        public void ReachGirlClockEnd()
        {
            girlClock.enabled = false;
            var cg = girlClock.GetComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
            cg.interactable = false;
            cg.DOFade(0, 2).OnComplete(ShowAfterGirlClockPuzzle);
        }

        void ShowDragBoyToPool()
        {
            foreach (var bg in boyClockBeforeActiveGroup)
                bg.SetActive(false);
            foreach (var bg in boyClockAfterActiveGroup)
                bg.SetActive(true);
        }

        void ShowAfterGirlClockPuzzle()
        {
            StartCoroutine(ShowGirlManga());
        }

        IEnumerator ShowGirlManga()
        {
            waterLoopSound.Play();
            yield return new WaitForSeconds(1);

            float fadeTime = 2.6f;
            foreach (var n in girlClockAfterActiveGroup)
            {
                n.SetActive(true);
                var image = n.GetComponent<Image>();
                image.color = new Color(1, 1, 1, 0);
                image.DOKill();
                image.DOFade(1, fadeTime);

                yield return new WaitForSeconds(fadeTime + 0.6f);
            }
            yield return new WaitForSeconds(1);
            OnPuzzleEnd_ClockNarrative();
        }

        public void OnDragToPoolPuzzleFinished()
        {
            StartCoroutine(DelayAction(2f, PrepareGirlClockPuzzle));
        }

        void PrepareGirlClockPuzzle()
        {
            var cg2 = girlClock.GetComponent<CanvasGroup>();
            cg2.blocksRaycasts = true;
            cg2.interactable = true;
            cg2.alpha = 0;
            cg2.DOFade(1, 2f);
            foreach (var bg in boyClockBeforeActiveGroup)
                bg.SetActive(false);
            foreach (var bg in boyClockAfterActiveGroup)
                bg.SetActive(false);

            foreach (var bg in girlClockBeforeActiveGroup)
                bg.SetActive(true);
            foreach (var bg in girlClockAfterActiveGroup)
                bg.SetActive(false);
        }

        public void StartPuzzle_Rescue()
        {
            ToggleCg(sceneCg_ClockNarrative, false);
            ToggleCg(sceneCg_Rescue, true);
            stage = GameStage.SceneBoyInBed_AfterMobileInteraction;
            StartCoroutine(StartScene_Rescue(0.6f));
        }
    }
}