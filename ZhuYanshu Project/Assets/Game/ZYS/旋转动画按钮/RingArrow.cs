using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.ZYS.旋转动画按钮
{
    public class RingArrow : MonoBehaviour
    {
        public RectTransform rect;

        private float _angleDeg;
        private float _radius;
        public float animationTime;
        private float _crtAnimationTime;
        public AnimationCurve curve;
        private float _delay;

        public bool fadeOut;
        public float fadeDelay;
        public float fadeDuration;
        public AnimationCurve curveFadeOut;
        private Image img;

        private void Start()
        {
            img = GetComponent<Image>();
            Burst();
        }

        void Burst()
        {
            StartCoroutine(AnimeIE());
            if (fadeOut)
            {
                StartCoroutine(FadeOutIE());
            }
        }

        IEnumerator AnimeIE()
        {
            _crtAnimationTime = 0;
            yield return new WaitForSeconds(_delay);

            while (_crtAnimationTime < animationTime)
            {
                _crtAnimationTime += Time.deltaTime;
                if (_crtAnimationTime > animationTime)
                {
                    _crtAnimationTime = animationTime;
                }

                var crtRadius = _radius * curve.Evaluate(_crtAnimationTime / animationTime);
                SyncPosition(crtRadius);
                yield return null;
            }
        }
        IEnumerator FadeOutIE()
        {
            yield return new WaitForSeconds(fadeDelay);
            _crtAnimationTime = 0;
            //img.DOFade(0,fadeDuration);
            rect.DOScale(0, fadeDuration);
            while (_crtAnimationTime < fadeDuration)
            {
                _crtAnimationTime += Time.deltaTime;
                if (_crtAnimationTime > fadeDuration)
                {
                    _crtAnimationTime = fadeDuration;
                }

                var crtRadius = _radius * curveFadeOut.Evaluate(_crtAnimationTime / fadeDuration);
                SyncPosition(crtRadius);
                yield return null;
            }

            //img.color = new Color(1, 1, 1, 0);
            Destroy(gameObject);
        }

        public void Setup(float angleDeg, float radius, float delay)
        {
            _delay = delay;
            _angleDeg = angleDeg;
            _radius = radius;
        }

        public void SyncPosition(float r)
        {

            float rad = _angleDeg * Mathf.Deg2Rad;
            float x = r * Mathf.Cos(rad);
            float y = r * Mathf.Sin(rad);
            Vector3 pos = new Vector3(x, y, 0);
            rect.anchoredPosition = pos;
        }
    }
}