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
        public CircularHudPointerDisplayer hpd_spending;

        void Start()
        {
            InitIncomeAndSpending();
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

            SyncIncome(income_current, income_kpi, true);
            SyncSpending(spending_current, spending_kpi, true);

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
        public int income_current_startMin;
        public int income_current_startMax;
        public int income_kpi_startMin;
        public int income_kpi_startMax;

        public int income_current;
        public int income_kpi;

        public int spending_current_startMin;
        public int spending_current_startMax;
        public int spending_kpi_startMin;
        public int spending_kpi_startMax;

        public int spending_current;
        public int spending_kpi;

        public void InitIncomeAndSpending()
        {
            income_current = Random.Range(income_current_startMin, income_current_startMax);
            income_kpi = Random.Range(income_kpi_startMin, income_kpi_startMax);
            income_kpi = ((int)((float)income_kpi / 500f)) * 500;

            spending_current = Random.Range(spending_current_startMin, spending_current_startMax);
            spending_kpi = Random.Range(spending_kpi_startMin, spending_kpi_startMax);
            spending_kpi = ((int)((float)spending_kpi / 10f)) * 10;

            SyncIncome(income_current, income_kpi, true);
            SyncSpending(spending_current, spending_kpi, true);
        }

        public void AddIncome(int delta)
        {
            income_current += delta;
            SyncIncome(income_current, income_kpi, false);

        }

        public void AddSpending(int delta)
        {
            spending_current += delta;
            SyncSpending(spending_current, spending_kpi, false);
        }

        public void SyncSpending(int current, int kpi, bool fromStart)
        {
            hpd_spending.Sync(current, kpi, fromStart);
        }


        public void SyncIncome(int current, int kpi, bool fromStart)
        {
            hpd_income.Sync(current, kpi, fromStart);
        }
    }
}