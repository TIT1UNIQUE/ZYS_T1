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
        public CanvasGroup sceneCg_OutsidePuzzle_boy;

        public CanvasGroup endImage_OutsidePuzzleBoy;
        public CanvasGroup puzzleContainer_OutsidePuzzleBoy;
        //public CanvasGroup chatFrame_OutsidePuzzleBoy;


        public RectTransform movableBody_OutsidePuzzleBoy;
        public bool puzzleStarting_boy = false;

        IEnumerator StartScene_OutsidePuzzle_boy(float delay)
        {
            puzzleStarting_boy = false;

            puzzleContainer_OutsidePuzzleBoy.alpha = 0;
            var words = puzzleContainer_OutsidePuzzleBoy.GetComponentsInChildren<FloatingDraggableWord>();
            foreach (var word in words)
            {
                word.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            yield return new WaitForSeconds(delay + 0.8f);
            Debug.Log("SwitchToOutsidePuzzleScene boy");
            ToggleCg(sceneCg_FoodBoy, false);
            ToggleCg(sceneCg_WashBoy, false);
            ToggleCg(sceneCg_WakeUpBoy, false);

            sceneCg_OutsidePuzzle_boy.gameObject.SetActive(true);
            sceneCg_OutsidePuzzle_boy.interactable = true;
            sceneCg_OutsidePuzzle_boy.blocksRaycasts = true;
            sceneCg_OutsidePuzzle_boy.alpha = 0;
            endImage_OutsidePuzzleBoy.alpha = 0;
            //chatFrame_OutsidePuzzleBoy.alpha = 0;

            float durationFadeIn = 1.5f;
            sceneCg_OutsidePuzzle_boy.DOFade(1, durationFadeIn);

            yield return new WaitForSeconds(durationFadeIn + 0.1f);

            float durationShowPuzzle = 1.0f;
            //chatFrame_OutsidePuzzleBoy.DOFade(1, durationShowPuzzle);
            yield return new WaitForSeconds(durationShowPuzzle);
            puzzleStarting_boy = true;

            foreach (var word in words)
            {
                word.StartFloat();
                word.transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 2f).SetDelay(UnityEngine.Random.Range(0.1f, 5f));
            }
            puzzleContainer_OutsidePuzzleBoy.alpha = 1;
        }

        public void OnEndDrag_OutsidePuzzle_boy()
        {
            AnimateMovableBody_OutsidePuzzle_boy();
        }

        public float movableBody_OutsidePuzzle_shakeSize_boy = 1f;
        void AnimateMovableBody_OutsidePuzzle_boy()
        {
            movableBody_OutsidePuzzleBoy.localScale = Vector3.one;
            movableBody_OutsidePuzzleBoy.DOKill();
            movableBody_OutsidePuzzleBoy.DOPunchScale(Vector3.one * movableBody_OutsidePuzzle_shakeSize_boy, 0.6f, 2);
        }

        public GameObject[] OnPuzzleEnd_OutsidePuzzleHides_boy;
        public void OnPuzzleEnd_OutsidePuzzle_boy()
        {
            var allWordsDone = true;
            var words = puzzleContainer_OutsidePuzzleBoy.GetComponentsInChildren<FloatingDraggableWord>();
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
                Debug.Log("boy go Outside Puzzle end!!!");
                foreach (var th in OnPuzzleEnd_OutsidePuzzleHides_boy)
                {
                    th.SetActive(false);
                }

                endImage_OutsidePuzzleBoy.DOFade(1, 3).SetDelay(0.5f).OnComplete(
                    () =>
                    {
                        puzzleContainer_OutsidePuzzleBoy.transform.DOPunchScale(Vector3.one * 0.03f, 1f, 2).OnComplete(
                            StartPuzzle_Stage3
                            );
                    }
                    );
            }
        }

        public void StartPuzzle_Stage3()
        {
            ToggleCg(sceneCg_OutsidePuzzle_boy, false);
            ToggleCg(sceneCg_ClockNarrative, true);
            stage = GameStage.SceneBoyInBed_AfterMobileInteraction;
            StartCoroutine(StartScene_ClockNarrative(0.6f));
        }
    }
}