using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Product
{
    [System.Serializable]
    public class VipLevel
    {
        public int lv;
        public int maxTokens;
        public RectTransform label;
        public float scaleFactor = 0.6f;
    }
}