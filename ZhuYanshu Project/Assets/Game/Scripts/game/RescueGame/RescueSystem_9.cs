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
        public CanvasGroup sceneCg_DialogTwoPerson;
        [SerializeField] AudioSource drippingSound;

        [SerializeField] CanvasGroup dialogTwoPersonCg_1;
        [SerializeField] CanvasGroup dialogTwoPersonCg_2;
        [SerializeField] CanvasGroup dialogTwoPersonCg_3;
        [SerializeField] CanvasGroup dialogTwoPersonCg_4;
        [SerializeField] CanvasGroup dialogTwoPersonCg_5;
        [SerializeField] CanvasGroup dialogTwoPersonCg_6;
        IEnumerator StartScene_DialogTwoPerson(float delay)
        {
            var cf = CameraFilterSystem.instance.cfp_aura_distortion;
            cf.enabled = true;
            CameraFilterSystem.instance.Tween(cf, "Twist", 1.5f, 0.12f, 2);

            dialogTwoPersonCg_1.alpha = 0;
            dialogTwoPersonCg_2.alpha = 0;
            dialogTwoPersonCg_3.alpha = 0;
            dialogTwoPersonCg_4.alpha = 0;
            dialogTwoPersonCg_5.alpha = 0;
            dialogTwoPersonCg_6.alpha = 0;
            var words1 = dialogTwoPersonCg_1.GetComponentsInChildren<FloatingDraggableWord>();
            foreach (var word in words1)
            {
                word.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            var words2 = dialogTwoPersonCg_2.GetComponentsInChildren<FloatingDraggableWord>();
            foreach (var word in words2)
            {
                word.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            drippingSound.Play();
            SoundSystem.instance.Play("breathing");
            yield return new WaitForSeconds(1.5f);
            //SoundSystem.instance.Play("cough", 0.3f);
            yield return new WaitForSeconds(delay);
            dialogTwoPersonCg_1.DOFade(1, 2);

            yield return new WaitForSeconds(1);
            foreach (var word in words1)
            {
                word.StartFloat();
                word.transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 2f).SetDelay(UnityEngine.Random.Range(0.1f, 2f));
            }
        }


        public void OnEndDrag_dialogTwoPerson_1()
        {

        }

        public void OnEndDrag_dialogTwoPerson_2()
        {

        }

        public void OnClickDialogTwoPerson_lastDialogs1()
        {
            dialogTwoPersonCg_3.DOFade(0, 1);
            dialogTwoPersonCg_3.interactable = false;
            dialogTwoPersonCg_3.blocksRaycasts = false;
            StartCoroutine(DelayAction(1.5f, () =>
              {
                  dialogTwoPersonCg_4.DOFade(1, 2).OnComplete(() => { dialogTwoPersonCg_4.blocksRaycasts = true; dialogTwoPersonCg_4.interactable = true; });
              }));
        }

        public void OnClickDialogTwoPerson_lastDialogs2()
        {
            dialogTwoPersonCg_4.DOFade(0, 1);
            dialogTwoPersonCg_4.interactable = false;
            dialogTwoPersonCg_4.blocksRaycasts = false;
            StartCoroutine(DelayAction(1.5f, () =>
              {
                  dialogTwoPersonCg_5.DOFade(1, 2).OnComplete(() => { dialogTwoPersonCg_5.blocksRaycasts = true; dialogTwoPersonCg_5.interactable = true; });
              }));
        }

        public void OnClickDialogTwoPerson_lastDialogs3()
        {
            dialogTwoPersonCg_5.DOFade(0, 1);
            dialogTwoPersonCg_5.interactable = false;
            dialogTwoPersonCg_5.blocksRaycasts = false;
            StartCoroutine(DelayAction(1.5f, () =>
              {
                  dialogTwoPersonCg_6.DOFade(1, 2).OnComplete(() => { dialogTwoPersonCg_6.blocksRaycasts = true; dialogTwoPersonCg_6.interactable = true; });
              }));
        }

        public void OnClickDialogTwoPerson_lastDialogs4()
        {
            dialogTwoPersonCg_6.DOFade(0, 1);
            StartCoroutine(DelayAction(2f, () =>
            {
                OnPuzzleEnd_DialogTwoPerson();
            }));
        }

        public void OnDialogTwoPersonPuzzle1End()
        {
            var allWordsDone = true;
            var words = dialogTwoPersonCg_1.GetComponentsInChildren<FloatingDraggableWord>();
            foreach (var word in words)
            {
                if (word.enabled)
                {
                    allWordsDone = false;
                    break;
                }

            }
            if (!allWordsDone)
                return;

            dialogTwoPersonCg_1.DOFade(0, 2);
            dialogTwoPersonCg_1.interactable = false;

            dialogTwoPersonCg_2.DOFade(1, 2).SetDelay(2);
            dialogTwoPersonCg_2.interactable = true;

            var words2 = dialogTwoPersonCg_2.GetComponentsInChildren<FloatingDraggableWord>();
            foreach (var word in words2)
            {
                word.StartFloat();
                word.transform.GetChild(0).GetComponent<Image>().DOFade(1, 2);
            }
        }

        public void OnDialogTwoPersonPuzzle2End()
        {
            var allWordsDone = true;
            var words = dialogTwoPersonCg_2.GetComponentsInChildren<FloatingDraggableWord>();
            foreach (var word in words)
            {
                if (word.enabled)
                {
                    allWordsDone = false;
                    break;
                }

            }
            if (!allWordsDone)
                return;
            dialogTwoPersonCg_2.DOFade(0, 2);
            dialogTwoPersonCg_2.interactable = false;

            StartCoroutine(DelayAction(2.5f, () =>
            {
                dialogTwoPersonCg_3.DOFade(1, 2).OnComplete(() => { dialogTwoPersonCg_3.blocksRaycasts = true; dialogTwoPersonCg_3.interactable = true; });
            }));
        }

        public void OnPuzzleEnd_DialogTwoPerson()
        {
            drippingSound.Stop();
            StartCoroutine(DelayAction(1.0f, StartPuzzle_FinalOfRescue));
        }

        public void StartPuzzle_FinalOfRescue()
        {
            ToggleCg(sceneCg_DialogTwoPerson, false);
            ToggleCg(sceneCg_FinalOfRescue, true);
            stage = GameStage.SceneBoyInBed_AfterMobileInteraction;
            StartCoroutine(StartScene_FinalOfRescue(0.6f));
        }
    }
}