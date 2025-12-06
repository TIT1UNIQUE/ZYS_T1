using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Misc
{
    public class SimplePopup : MonoBehaviour
    {
        public int attributeValue;

        public RectTransform rect;

        [Multiline]
        public string prefix;

        [Multiline]
        public string suffix;

        public TextMeshProUGUI text;

        public void SetTextWithAttribute()
        {
            text.text = prefix + attributeValue + suffix;
        }

        public void Show()
        {
            rect.localScale = Vector3.one;
            gameObject.SetActive(true);
            rect.DOPunchScale(Vector3.one * 0.15f, 0.15f, 4, 0.5f);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}