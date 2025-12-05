using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Misc
{
    public class CircularHudPointerDisplayer : MonoBehaviour
    {
        public RectTransform pointer;
        public TextMeshProUGUI currentNumTxt;
        public TextMeshProUGUI kpiNumTxt;
        public TextMeshProUGUI restNumTxt;
        public TextMeshProUGUI percentageNumTxt;

        public float minRatioAngle;
        public float maxRatioAngle;
        public float radius;
        public Vector2 offset;
        public float ratio;
        public float angleOffset;
        public float speed;
        public float pointerAngleOffset;

        void Sync(float r)
        {
            ratio = r;
            var angle = Mathf.Lerp(minRatioAngle, maxRatioAngle, ratio);
            pointer.localEulerAngles = new Vector3(0, 0, pointerAngleOffset - angle);
            var a = Mathf.PI - (angleOffset + angle) * Mathf.Deg2Rad;
            pointer.anchoredPosition = offset + new Vector2(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius);
        }

        void Start()
        {
            Sync(0);
        }

        // public bool test;
        void Update()
        {
            //if (test)
            //{
            //    test = false;
            //    IncreaseTo(1);
            //}
        }

        public void Sync(int current, int max)
        {
            float ratio = (float)current / max;
            currentNumTxt.text = "$" + current;
            kpiNumTxt.text = "$" + max;
            restNumTxt.text = "$" + (max - current) + " left!";
            percentageNumTxt.text = "" + Mathf.FloorToInt(ratio * 100) + "% of KPI";

            IncreaseTo(ratio);
        }

        public void IncreaseTo(float t)
        {
            StartCoroutine(IncreaseIE(t));
        }

        IEnumerator IncreaseIE(float t)
        {
            ratio = 0;
            while (ratio < t)
            {
                ratio += Time.deltaTime * speed;
                if (ratio > t)
                    ratio = t;
                Sync(ratio);
                yield return null;
            }
        }
    }
}