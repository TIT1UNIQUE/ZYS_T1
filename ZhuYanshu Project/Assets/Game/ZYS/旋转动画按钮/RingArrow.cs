using System.Collections;
using UnityEngine;

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
        private void Start()
        {
            Burst();
        }

        void Burst()
        {
            _crtAnimationTime = 0;
            StartCoroutine(Anime());
        }

        IEnumerator Anime()
        {
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