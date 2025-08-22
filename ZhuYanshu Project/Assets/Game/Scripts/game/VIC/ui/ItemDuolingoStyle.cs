using System.Collections;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui
{
    public class ItemDuolingoStyle : MonoBehaviour
    {
        public Vector2 startPos;
        public string s;
        public TextMeshProUGUI txt;
        public enum State
        {
            None,
            Pending,
            Done,
        }

        public State state;

        public TextComposerDuolingoStyle textComposer;

        public void Init(Vector2 p_startPos, string p_s)
        {
            GetComponent<RectTransform>().anchoredPosition = p_startPos;
            startPos = p_startPos; s = p_s;
            state = State.Pending;
            txt.text = s;
        }

        public void OnClick()
        {
            textComposer.TryMoveMe(this);
        }
    }
}