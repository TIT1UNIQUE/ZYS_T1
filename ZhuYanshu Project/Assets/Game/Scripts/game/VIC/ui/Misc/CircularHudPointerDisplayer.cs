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
        void Sync(float r)
        {
            ratio = r;
            var angle = Mathf.Lerp(minRatioAngle, maxRatioAngle, ratio);
            pointer.localEulerAngles = new Vector3(0, 0, pointerAngleOffset - angle);
            var a = Mathf.PI - (angleOffset + angle) * Mathf.Deg2Rad;
            pointer.anchoredPosition = offset + new Vector2(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius);
        }

        public void Sync(int current, int max, bool fromStart)
        {
            float ratio = (float)current / max;
            currentNumTxt.text = "$" + current;
            kpiNumTxt.text = "$" + max;
            var rest = max - current;
            if (rest > 0)
            {
                restNumTxt.text = "$" + rest + " left!";
            }
            else
            {
                restNumTxt.text = "None left!";
            }

            percentageNumTxt.text = "" + Mathf.FloorToInt(ratio * 100) + "% of KPI";

            IncreaseTo(ratio, fromStart);
        }

        public void IncreaseTo(float t, bool fromStart)
        {
            StartCoroutine(IncreaseIE(t, fromStart));
        }

        IEnumerator IncreaseIE(float t, bool fromStart)
        {
            if (fromStart)
            {
                ratio = 0;
            }
            var refinedT = Mathf.Clamp01(t);
            while (ratio < refinedT)
            {
                ratio += Time.deltaTime * speed;
                if (ratio > refinedT)
                    ratio = refinedT;
                Sync(ratio);
                yield return null;
            }
        }
    }
}