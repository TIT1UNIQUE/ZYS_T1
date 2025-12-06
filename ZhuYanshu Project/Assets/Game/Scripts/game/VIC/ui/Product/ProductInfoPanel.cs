using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.Product
{
    public class ProductInfoPanel : MonoBehaviour
    {
        public VipLevel[] vipLevels;
        public Scrollbar tokenBar;
        public TextMeshProUGUI tokenTxt;
        public Image icon;
        public RectTransform iconParent;
        float _tokenTxtScale;

        private void Start()
        {
            _tokenTxtScale = tokenTxt.transform.localScale.x;
            icon.color = new Color(0, 0, 0, 0.3f);
            iconParent.transform.localScale = Vector3.one * 0.8f;
        }

        public void SyncTokens(int tokens, int max)
        {
            float ratio = ((float)max - tokens) / (float)max;
            var size = Mathf.Clamp(ratio, 0.0f, 0.98f);
            //Debug.Log("SyncTokens size " + size);
            tokenBar.size = 1 - size;
            tokenTxt.text = "" + tokens + " tokens left";
            tokenTxt.transform.localScale = Vector3.one * _tokenTxtScale;
            tokenTxt.transform.DOPunchScale(Vector3.one * 0.11f, 0.2f, 4, 0.5f);
        }

        public void ToggleIcon(bool b)
        {
            if (b)
            {
                icon.color = Color.white;
                iconParent.transform.DOScale(Vector3.one, 0.25f);
            }
            else
            {
                icon.color = new Color(1, 1, 1, 0.35f);
                iconParent.transform.DOScale(Vector3.one * 0.8f, 0.25f);
            }
        }

        public void SyncTokensSimple(int tokens, int max)
        {
            float ratio = ((float)max - tokens) / (float)max;
            var size = Mathf.Clamp(ratio, 0.0f, 0.98f);
            //Debug.Log("SyncTokens size " + size);
            tokenBar.size = 1 - size;
            //tokenTxt.text = "" + tokens + " tokens left";
        }
    }
}