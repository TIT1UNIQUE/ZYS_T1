using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.game.Omni.Mission.TextInfo
{
    public class TextInfoMissionSystem : MonoBehaviour
    {
        public GameObject view;

        bool currentMissionDone;

        private void Awake()
        {
            view.SetActive(false);
        }

        /// <summary>
        /// 首次开始这个任务
        /// </summary>
        public void ResetMission()
        {
            view.SetActive(true);

            RefreshFinishBtn();
            foreach (var cm in restaurantCheckmarks)
            {
                cm.SetActive(false);
            }

            keyword_burger.SetActive(false);
            keyword_burgers.SetActive(false);
            keyword_20min.SetActive(false);
            keyword_15min.SetActive(false);
            keyword_0fee.SetActive(false);
            keyword_199fee.SetActive(false);
            keyword_15_25_perperson.SetActive(false);
            keyword_20_25_perperson.SetActive(false);

            CheckKeywordsTriggerClueLv3();

            txt_clueLv4.text = "0/3";
            // txt_clueLv4.text = "need 3 checked";

            foreach (var g in submitSucToShows)
                g.SetActive(false);
            foreach (var g in submitFailToShows)
                g.SetActive(false);

            ToggleFinishedButton(false);

            if (currentMissionDone)
            {
                foreach (var g in submitSucToShows)
                    g.SetActive(true);
                foreach (var g in submitSucToHides)
                    g.SetActive(false);
            }
        }

        public void Hide()
        {
            view.SetActive(false);
        }

        /// <summary>
        /// 失败了重新开始 去掉餐厅的勾勾
        /// </summary>
        public void RetryMission()
        {
            foreach (var g in submitFailToShows)
                g.SetActive(false);
            //    foreach (var g in submitFailToHides)
            // g.SetActive(false);
        }

        public GameObject[] submitSucToShows;
        public GameObject[] submitSucToHides;
        public GameObject[] submitFailToShows;
        public GameObject[] submitFailToHides;
        /// <summary>
        /// 点击finish触发（至少勾选了3个餐厅出现可以点击的finish）
        /// </summary>
        public void SubmitMission()
        {
            bool result = true;
            int checkedCount = 0;
            foreach (var cm in restaurantCheckmarks)
            {
                if (cm.activeSelf)
                {
                    checkedCount++;
                    if (!correctRestaurantCheckmarks.Contains(cm))
                    {
                        result = false;//选了不该选的
                    }
                }
            }
            if (checkedCount != correctRestaurantCheckmarks.Length)
            {
                result = false;//选的数量不对
            }

            Debug.Log("SubmitMission" + result);
            if (result)
            {
                currentMissionDone = true;
                foreach (var g in submitSucToShows)
                    g.SetActive(true);
                foreach (var g in submitSucToHides)
                    g.SetActive(false);
                return;
            }

            foreach (var g in submitFailToShows)
                g.SetActive(true);
            foreach (var g in submitFailToHides)
                g.SetActive(false);
        }

        public GameObject finishBtn_ok;
        public GameObject finishBtn_notOk;

        public GameObject[] correctRestaurantCheckmarks;
        public GameObject[] restaurantCheckmarks;

        int GetRestaurantCheckedCount()
        {
            int checkedCount = 0;
            foreach (var cm in restaurantCheckmarks)
            {
                if (cm.activeSelf) checkedCount++;
            }
            return checkedCount;
        }

        void RefreshFinishBtn()
        {
            int checkedCount = GetRestaurantCheckedCount();
            txt_clueLv4.text = checkedCount + "/3";
            ToggleFinishedButton(checkedCount >= 3);
        }

        void ToggleFinishedButton(bool ok)
        {
            finishBtn_ok.SetActive(ok);
            finishBtn_notOk.SetActive(!ok);
        }

        public void OnRestaurantChecked(GameObject checkmark)
        {
            //Debug.Log(checkmark);
            int checkedCount = GetRestaurantCheckedCount();

            if (checkmark.activeSelf)
            {
                checkmark.SetActive(false);
            }
            else
            {
                if (checkedCount >= 3)
                    return;
                checkmark.SetActive(true);
            }

            RefreshFinishBtn();
        }

        public GameObject keyword_burger;
        public GameObject keyword_burgers;
        public GameObject keyword_20min;
        public GameObject keyword_15min;
        public GameObject keyword_0fee;
        public GameObject keyword_199fee;
        public GameObject keyword_15_25_perperson;
        public GameObject keyword_20_25_perperson;

        public GameObject clueLv3_burger;
        public GameObject clueLv3_min;
        public GameObject clueLv3_fee;
        public GameObject clueLv3_perperson;

        public TextMeshProUGUI txt_clueLv3;
        public TextMeshProUGUI txt_clueLv4;

        public void OnKeywordChecked(GameObject kw)
        {
            if (!kw.activeSelf)
            {
                kw.SetActive(true);
            }
            CheckKeywordsTriggerClueLv3();
        }

        void CheckKeywordsTriggerClueLv3()
        {
            int triggered = 0;
            clueLv3_burger.SetActive(keyword_burger.activeSelf && keyword_burgers.activeSelf);
            clueLv3_min.SetActive(keyword_20min.activeSelf && keyword_15min.activeSelf);
            clueLv3_fee.SetActive(keyword_0fee.activeSelf && keyword_199fee.activeSelf);
            clueLv3_perperson.SetActive(keyword_15_25_perperson.activeSelf && keyword_20_25_perperson.activeSelf);

            if (clueLv3_burger.activeSelf) triggered++;
            if (clueLv3_min.activeSelf) triggered++;
            if (clueLv3_fee.activeSelf) triggered++;
            if (clueLv3_perperson.activeSelf) triggered++;


            txt_clueLv3.text = triggered + "/4";
        }
    }
}