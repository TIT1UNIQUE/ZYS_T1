using com;
using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Rescue
{
    public partial class RescueSystem : MonoBehaviour
    {
        public Transform dotParent;

        public float dotInterval = 0.1f;

        public AudioSource[] sfxsSwitching;


        void SwitchToWashScene()
        {
            SwitchToWashScene(false);
        }

        void ClearWhiteDots()
        {
            var childCount = dotParent.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var c = dotParent.GetChild(i);
                c.gameObject.SetActive(false);
            }
        }

        void SwitchToWashScene(bool isBoy = false)
        {
            foreach (var s in sfxsSwitching)
            {
                s.Stop();
                s.Play();
                s.DOKill();
                var v = s.volume;
                s.volume = 0;
                s.DOFade(v, 1);
            }

            var childCount = dotParent.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var delay = i * dotInterval + 0.3f;
                var c = dotParent.GetChild(i);
                StartCoroutine(
                    DelayAction(delay, () => { c.gameObject.SetActive(true); }
                ));
            }

            var waitedTime = childCount * dotInterval;
            StartCoroutine(DelayAction(waitedTime, () =>
               {
                   foreach (var s in sfxsSwitching)
                   {
                       var v = s.volume;
                       s.DOFade(0, 1).OnComplete(() => { s.volume = v; });
                   }

                   if (isBoy)
                   {
                       ToggleCg(sceneCg_WakeUpBoy, false);
                       sceneCg_WashBoy.alpha = 0;
                       sceneCg_WashBoy.interactable = true;
                       sceneCg_WashBoy.blocksRaycasts = true;
                       sceneCg_WashBoy.DOKill();
                       sceneCg_WashBoy.DOFade(1, 2);
                   }
                   else
                   {
                       ToggleCg(sceneCg_girlInBed1, false);
                       sceneCg_washFace.alpha = 0;
                       sceneCg_washFace.interactable = true;
                       sceneCg_washFace.blocksRaycasts = true;
                       sceneCg_washFace.DOKill();
                       sceneCg_washFace.DOFade(1, 2);
                   }
               }));
        }

        public void OnAllWoundHealed()
        {
            if (_woundAllHealed)
                return;
            _woundAllHealed = true;
            StartCoroutine(WoundHealedCoroutine());
        }

        public CanvasGroup girlWoundBefore;
        public CanvasGroup girlWoundAfter;
        private bool _woundAllHealed;

        IEnumerator WoundHealedCoroutine()
        {
            girlWoundBefore.DOFade(0, 2);
            yield return new WaitForSeconds(0.8f);
            girlWoundAfter.gameObject.SetActive(true);
            girlWoundAfter.alpha = 0;
            girlWoundAfter.DOFade(1, 2);
            yield return new WaitForSeconds(4.5f);
            SwitchToDressingScene();
        }

        public void SwitchToDressingScene()
        {
            Debug.Log("SwitchToDressingScene");
            sceneCg_washFace.gameObject.SetActive(false);
            ToggleCg(sceneCg_dressing, true);
            ClearWhiteDots();
        }

        public void FocusViewOnPickedCloth(Vector2 localAnchoredPos)
        {
            var duration = 3.5f;
            var rect = sceneCg_dressing.GetComponent<RectTransform>();
            rect.DOScale(1.4f, duration);
            rect.DOAnchorPos(-localAnchoredPos, duration);
            StartCoroutine(StartScene_OutsidePuzzle(duration));
        }
    }
}