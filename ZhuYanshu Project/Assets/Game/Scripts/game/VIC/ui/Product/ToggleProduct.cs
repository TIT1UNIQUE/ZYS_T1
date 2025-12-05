using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.VIC.ui.Misc
{
    public class ToggleProduct : MonoBehaviour
    {
        public Scrollbar toggleBar;
        public Image toggleBarImg;
        public Color toggleBarImgStartColor;
        public Color toggleBarImgEndColor;

        public CanvasGroup cgProgBar;
        public float toggleBarSpeed;
        public Text progText;
        public float offAlpha;
        public bool isOn { get; private set; }

        private void Start()
        {
            toggleBar.value = 1;
            toggleBarImg.color = toggleBarImgEndColor;
            isOn = true;
            cgProgBar.alpha = 1;
        }

        public void Toggle()
        {
            if (isOn)
            {
                ToggleOff();
            }
            else
            {
                ToggleOn();
            }

        }

        public void ToggleOn()
        {
            isOn = true;
            com.SoundSystem.instance.Play("do");
            StartCoroutine(ToggleBarOnIE());
        }

        public void ToggleOff()
        {
            isOn = false;
            com.SoundSystem.instance.Play("do");
            StartCoroutine(ToggleBarOffIE());
        }

        IEnumerator ToggleBarOnIE()
        {
            toggleBarImg.DOKill();
            toggleBarImg.DOColor(toggleBarImgEndColor, 0.35f);
            cgProgBar.DOFade(1, 0.25f).SetDelay(0.25f);

            while (toggleBar.value < 1)
            {
                if (toggleBar.value > 1)
                {
                    toggleBar.value = 1;
                }
                toggleBar.value += Time.deltaTime * toggleBarSpeed;
                yield return null;
            }
        }

        IEnumerator ToggleBarOffIE()
        {
            toggleBarImg.DOKill();
            toggleBarImg.DOColor(toggleBarImgStartColor, 0.35f);
            cgProgBar.DOFade(offAlpha, 0.25f).SetDelay(0.25f);


            while (toggleBar.value > 0)
            {
                if (toggleBar.value < 0)
                {
                    toggleBar.value = 0;
                }
                toggleBar.value -= Time.deltaTime * toggleBarSpeed;
                yield return null;
            }
        }
    }
}