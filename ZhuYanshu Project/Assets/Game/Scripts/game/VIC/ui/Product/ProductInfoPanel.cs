using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.Product
{
    public class PruductSystem : MonoBehaviour
    {
        public static PruductSystem instance;

        public VipLevel[] vipLevels;
        public int vipLevel;
        public int tokens;



        private void Awake()
        {
            instance = this;
        }

        public void SetVipLevel(int lv)
        {
            var vl = GetVipLevel(lv);
            if (vl == null)
            {
                Debug.LogWarning("no this vip level");
                return;
            }

            vipLevel = lv;
            SetVipLevelLabel(vl);
            SetVipToken(vl);
        }

        public VipLevel crtVipLevel
        {
            get
            {
                return GetVipLevel(vipLevel);
            }
        }

        public VipLevel GetVipLevel(int lv)
        {
            foreach (var l in vipLevels)
            {
                if (l.lv == lv)
                    return l;
            }
            return null;
        }

        public void SetVipLevelLabel(VipLevel vipLevel)
        {
            foreach (var vl in vipLevels)
            {
                vl.label.gameObject.SetActive(false);
            }

            vipLevel.label.gameObject.SetActive(true);
            vipLevel.label.DOKill();
            vipLevel.label.transform.localScale = Vector3.zero;
            vipLevel.label.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        public void SetVipToken(VipLevel vipLevel)
        {
            tokens = vipLevel.maxTokens;
            SyncTokens();
        }

        public int AmountOfSomeTokens()
        {
            var lv1MaxTokens = vipLevels[0].maxTokens;
            var goodNum = lv1MaxTokens * 0.11f;
            return (int)(goodNum * Random.Range(0.6f, 1.25f));
        }

        public void ConsumeSomeTokens()
        {
            tokens -= AmountOfSomeTokens();
            SyncTokens();
        }

        public Scrollbar tokenBar;
        public TextMeshProUGUI tokenTxt;

        void SyncTokens()
        {
            var max = crtVipLevel.maxTokens;
            float ratio = ((float)max - tokens) / (float)max;
            var size = Mathf.Clamp(0.05f, 1, ratio);
            tokenBar.size = size;

            tokenTxt.text = "" + tokens;
        }
    }
}