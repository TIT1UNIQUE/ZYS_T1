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
        public CanvasGroup sceneCg_WakeUpBoy;
        public CanvasGroup sceneCg_OutsidePuzzle;
        public CanvasGroup sceneCg_WashBoy;
        public CanvasGroup sceneCg_FoodBoy;

        public CanvasGroup endImage_OutsidePuzzle;
        public CanvasGroup puzzleContainer_OutsidePuzzle;
        public CanvasGroup chatFrame_OutsidePuzzle;


        public RectTransform movableBody_OutsidePuzzle;

        public bool puzzleStarting = false;

        IEnumerator StartScene_OutsidePuzzle(float delay)
        {
            puzzleStarting = false;
            yield return new WaitForSeconds(delay + 0.8f);
            Debug.Log("SwitchToOutsidePuzzleScene");
            ToggleCg(sceneCg_girlInBed1, false);
            ToggleCg(sceneCg_washFace, false);
            ToggleCg(sceneCg_dressing, false);

            sceneCg_OutsidePuzzle.gameObject.SetActive(true);
            sceneCg_OutsidePuzzle.interactable = true;
            sceneCg_OutsidePuzzle.blocksRaycasts = true;
            sceneCg_OutsidePuzzle.alpha = 0;
            endImage_OutsidePuzzle.alpha = 0;
            chatFrame_OutsidePuzzle.alpha = 0;
            puzzleContainer_OutsidePuzzle.alpha = 0;
            var words = puzzleContainer_OutsidePuzzle.GetComponentsInChildren<FloatingDraggableWord>();
            foreach (var word in words)
            {
                word.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            float durationFadeIn = 1.5f;
            sceneCg_OutsidePuzzle.DOFade(1, durationFadeIn);

            yield return new WaitForSeconds(durationFadeIn + 0.1f);

            float durationShowPuzzle = 1.0f;
            chatFrame_OutsidePuzzle.DOFade(1, durationShowPuzzle);
            yield return new WaitForSeconds(durationShowPuzzle);
            puzzleStarting = true;

            foreach (var word in words)
            {
                word.StartFloat();
                word.transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 2f).SetDelay(UnityEngine.Random.Range(0.1f, 5f));
            }
            puzzleContainer_OutsidePuzzle.alpha = 1;
        }

        public void OnEndDrag_OutsidePuzzle()
        {
            AnimateMovableBody_OutsidePuzzle();
        }

        public float movableBody_OutsidePuzzle_shakeSize = 1f;
        void AnimateMovableBody_OutsidePuzzle()
        {
            movableBody_OutsidePuzzle.localScale = Vector3.one;
            movableBody_OutsidePuzzle.DOKill();
            movableBody_OutsidePuzzle.DOPunchScale(Vector3.one * movableBody_OutsidePuzzle_shakeSize, 0.6f, 2);
        }

        public GameObject[] OnPuzzleEnd_OutsidePuzzleHides;
        public void OnPuzzleEnd_OutsidePuzzle()
        {
            var allWordsDone = true;
            var words = puzzleContainer_OutsidePuzzle.GetComponentsInChildren<FloatingDraggableWord>();
            foreach (var word in words)
            {
                if (word.enabled)
                {
                    allWordsDone = false;
                    break;
                }

            }
            if (allWordsDone)
            {
                Debug.Log("Puzzle end!!!");
                foreach (var th in OnPuzzleEnd_OutsidePuzzleHides)
                {
                    th.SetActive(false);
                }

                endImage_OutsidePuzzle.DOFade(1, 3).SetDelay(0.5f).OnComplete(
                    () =>
                    {
                        endImage_OutsidePuzzle.transform.DOPunchScale(Vector3.one * 0.03f, 1f, 2).OnComplete(StartPuzzle_Stage2_1);
                    }
                    );
            }
        }

        public void StartPuzzle_Stage2_1()
        {
            ToggleCg(sceneCg_OutsidePuzzle, false);
            ToggleCg(sceneCg_WakeUpBoy, true);
            stage = GameStage.SceneBoyInBed_beforeMobileInteraction;
            StartCoroutine(StartScene_WakeUpBoy());
        }
    }
}