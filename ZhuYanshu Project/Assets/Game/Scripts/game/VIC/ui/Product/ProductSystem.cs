using Assets.Game.Scripts.game.VIC.ui.Misc;
using DG.Tweening;
using System.Collections;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.Product
{
    public class ProductSystem : MonoBehaviour
    {
        public static ProductSystem instance;

        public ProductInfoPanel productInfoPanel;
        public PersonalInfoFrameBehaviour personalInfoFrame;

        public SimplePopup vip3UpPopup;
        public SimplePopup jobDonePopup;

        public int vipLevel;
        public int tokens;

        private bool _isUpgradeCoroutineRunning;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            foreach (var vl in productInfoPanel.vipLevels)
            {
                vl.label.gameObject.SetActive(false);
                vl.labelChat.gameObject.SetActive(false);
            }

            _isUpgradeCoroutineRunning = false;
            isArduinoDeviceConnected = false;

            vip3UpPopup.Hide();
            jobDonePopup.Hide();
            SetVipLevel(1);
            ConsumeSomeTokens();
        }

        public void SetVipLevel(int lv)
        {
            Debug.Log("SetVipLevel " + lv);
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
        { get { return GetVipLevel(vipLevel); } }

        public VipLevel GetVipLevel(int lv)
        {
            foreach (var l in productInfoPanel.vipLevels)
            {
                if (l.lv == lv)
                    return l;
            }
            return null;
        }

        public void SetVipLevelLabel(VipLevel vipLevel)
        {
            Debug.Log("SetVipLevelLabel " + vipLevel.lv);

            foreach (var vl in productInfoPanel.vipLevels)
            {
                vl.label.gameObject.SetActive(false);
                vl.labelChat.gameObject.SetActive(false);
            }

            vipLevel.labelChat.gameObject.SetActive(true);

            vipLevel.label.gameObject.SetActive(true);
            vipLevel.label.DOKill();
            vipLevel.label.transform.localScale = Vector3.zero;
            vipLevel.label.DOScale(Vector3.one * vipLevel.scaleFactor, 0.5f).SetEase(Ease.OutBack);
        }

        public void SetVipToken(VipLevel vipLevel)
        {
            tokens = vipLevel.maxTokens;
            productInfoPanel.SyncTokens(tokens, crtVipLevel.maxTokens);
        }

        public int AmountOfSomeTokens()
        {
            var lv1MaxTokens = productInfoPanel.vipLevels[0].maxTokens;
            var goodNum = lv1MaxTokens * 0.1f;
            return (int)(goodNum * Random.Range(0.6f, 1.25f));
        }

        public void ConsumeSomeTokens()
        {
            tokens -= AmountOfSomeTokens();
            productInfoPanel.SyncTokens(tokens, crtVipLevel.maxTokens);
        }

        public void PayTokens()
        {
            ConsumeSomeTokens();
            com.SoundSystem.instance.Play("pay");
        }

        public void AddSomeSpending()
        {
            int v = Random.Range(9, 18);
            personalInfoFrame.AddSpending(v);
            com.SoundSystem.instance.Play("pay");
        }

        public void ShowVipUpPopup()
        {
            if (!isArduinoDeviceConnected)
            {
                Debug.LogWarning("ArduinoDevice not Connected, cannot upgrade vip");
                return;
            }

            if (_isUpgradeCoroutineRunning)
                return;
            vip3UpPopup.Show();
        }

        public void OnClickVipUpPopup()
        {
            vip3UpPopup.Hide();
            UpgradeVip();
        }

        void UpgradeVip()
        {
            _isUpgradeCoroutineRunning = true;
            StartCoroutine(UpgradeVipIE());
        }

        IEnumerator UpgradeVipIE()
        {
            var crtVipLevelNum = vipLevel;
            productInfoPanel.tokenTxt.text = "Upgrading...";
            com.SoundSystem.instance.Play("pay");
            yield return new WaitForSeconds(0.25f);

            tokens = 0;//test
            var max = crtVipLevel.maxTokens;
            productInfoPanel.SyncTokensSimple(tokens, max);
            yield return new WaitForSeconds(0.25f);

            var delta = max - tokens;
            int addValue = (int)(delta * 0.05f + 1);
            while (tokens < max)
            {
                tokens += addValue;
                if (tokens > max)
                {
                    tokens = max;
                }
                //yield return null;
                yield return new WaitForSeconds(0.05f);
                productInfoPanel.SyncTokensSimple(tokens, max);
            }
            com.SoundSystem.instance.Play("ding");
            SetVipLevel(crtVipLevelNum + 1);

            yield return new WaitForSeconds(0.2f);
            _isUpgradeCoroutineRunning = false;
        }

        public void ShowJobDonePopup()
        {
            int income = 500 * Random.Range(4, 11);
            jobDonePopup.attributeValue = income;
            jobDonePopup.SetTextWithAttribute();
            jobDonePopup.Show();
        }

        public void OnClickJobDonePopup()
        {
            jobDonePopup.Hide();
            JobDone(jobDonePopup.attributeValue);
        }

        void JobDone(int income)
        {
            StartCoroutine(JobDoneIE(income));
        }

        IEnumerator JobDoneIE(int income)
        {
            com.SoundSystem.instance.Play("pay");
            personalInfoFrame.AddIncome(income);
            yield return new WaitForSeconds(1f);

            var max = crtVipLevel.maxTokens;
            productInfoPanel.SyncTokensSimple(tokens, max);

            productInfoPanel.tokenTxt.text = "Adding...";
            com.SoundSystem.instance.Play("tap");

            var delta = 5000;
            int addValue = (int)(delta * 0.1f);
            var final = tokens + delta;

            productInfoPanel.SyncTokensSimple(tokens, max);

            while (tokens < final)
            {
                tokens += addValue;
                if (tokens > final)
                {
                    tokens = final;
                }
                //yield return null;
                yield return new WaitForSeconds(0.05f);
                productInfoPanel.SyncTokensSimple(tokens, max);
            }
            com.SoundSystem.instance.Play("ding");
            productInfoPanel.SyncTokens(final, max);

        }

        public bool isArduinoDeviceConnected { get; private set; }
        public void ToggleArduinoDevice()
        {
            isArduinoDeviceConnected = !isArduinoDeviceConnected;
            if (isArduinoDeviceConnected)
            {
                Debug.LogWarning("ArduinoDevice Connected");
                productInfoPanel.ToggleIcon(true);
            }
            else
            {
                Debug.LogWarning("ArduinoDevice unconnected");
                productInfoPanel.ToggleIcon(false);
            }
        }
    }
}