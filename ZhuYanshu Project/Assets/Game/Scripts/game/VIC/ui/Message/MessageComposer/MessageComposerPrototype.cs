using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer
{
    [CreateAssetMenu]
    public class MessageComposerPrototype : ScriptableObject
    {
        [Multiline]
        public string rawText;
        //<color=#AA00FF><i>wq</i></color>ha______h<size=200%>hs</size>eqwe<color=#FFAAAA>wq</color>Your best enemy!!!!-<sprite index=0><size=200%><sprite index=1></size><sprite index=2><sprite index=3><sprite index=4><sprite index=5><sprite index=6><sprite index=7><sprite index=8><sprite index=9><sprite index=10><sprite index=11><sprite index=12>

        public string[] options;

        public MessageBlank[] blanks;

        public string 备注;
        public MessagePrototype reply;
    }

    [System.Serializable]
    public struct MessageBlank
    {
        public Vector2 anchorPos;
        public int correctAnswerIndex;
    }
}