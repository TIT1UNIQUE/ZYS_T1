using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.Misc
{
    public class PersonalInfoFrameBehaviour : MonoBehaviour
    {
        public RectTransform targetFrame;
        //Personal info frame 
        public float expandDuration1;
        public float expandDuration2;

        private float expandedSizeW;
        private float expandedSizeH;

        public float shrinkedSizeW;
        public float shrinkedSizeH;

        private bool isExpanded;
        public CanvasGroup cgContent;
        public GameObject icon;
        public Button iconBtn;

        public CircularHudPointerDisplayer hpd_income;
        public CircularHudPointerDisplayer hpd_kpi;

        void Start()
        {
            cgContent.alpha = 0;
            isExpanded = false;
            expandedSizeW = targetFrame.sizeDelta.x;
            expandedSizeH = targetFrame.sizeDelta.y;
            targetFrame.sizeDelta = new Vector2(shrinkedSizeW, shrinkedSizeH);
        }

        public void ShowIcon()
        {
            icon.SetActive(true);
            iconBtn.enabled = true;
        }

        public void OnClickIcon()
        {
            if (isExpanded)
            {
                StartCoroutine(ShrinkIE());
            }
            else
            {
                StartCoroutine(ExpandIE());
            }
        }

        IEnumerator ExpandIE()
        {
            iconBtn.enabled = false;
            targetFrame.DOKill();
            var size1 = new Vector2(expandedSizeW, shrinkedSizeH);
            var size2 = new Vector2(expandedSizeW, expandedSizeH);
            targetFrame.DOSizeDelta(size1, expandDuration1).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(expandDuration1 - 0.15f);
            targetFrame.DOSizeDelta(size2, expandDuration2).SetEase(Ease.OutCubic); ;
            yield return new WaitForSeconds(expandDuration2);

            SyncKpi();
            SyncIncome();

            cgContent.DOKill();
            cgContent.DOFade(1, 0.5f);
            iconBtn.enabled = true;
            isExpanded = true;
        }

        IEnumerator ShrinkIE()
        {
            iconBtn.enabled = false;
            targetFrame.DOKill();
            cgContent.DOKill();
            cgContent.DOFade(0, 0.25f);
            //yield return new WaitForSeconds(0.25f);
            var size1 = new Vector2(expandedSizeW, shrinkedSizeH);
            var size2 = new Vector2(shrinkedSizeW, shrinkedSizeH);
            targetFrame.DOSizeDelta(size1, expandDuration1).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(expandDuration1 - 0.15f);
            targetFrame.DOSizeDelta(size2, expandDuration2).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(expandDuration2);
            iconBtn.enabled = true;
            isExpanded = false;
        }

        public void SyncKpi()
        {
            hpd_kpi.IncreaseTo(1);
        }


        public void SyncIncome()
        {
            hpd_income.IncreaseTo(1);
        }
    }
}