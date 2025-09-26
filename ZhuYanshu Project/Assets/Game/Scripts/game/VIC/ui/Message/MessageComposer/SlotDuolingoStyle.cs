using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui
{
    public class SlotDuolingoStyle : MonoBehaviour
    {
        public ItemDuolingoStyle crtItem;

        public int blankIndex;
        public int answerIndex;

        public void Init(Vector2 p_startPos, int index1, int index2)
        {
            GetComponent<RectTransform>().anchoredPosition = p_startPos;
            blankIndex = index1;
            answerIndex = index2;
        }
    }
}