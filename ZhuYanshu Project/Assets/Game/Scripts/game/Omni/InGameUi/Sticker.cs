using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.Omni.InGameUi
{
    public class Sticker : MonoBehaviour
    {
        public RectTransform rect;

        public Vector2 offset;
        public float duration;
        public bool canRemove;
        public void OnClick()
        {
            if (!canRemove)
                return;

            var ap = rect.anchoredPosition;
            rect.DOAnchorPos(ap + offset, duration).SetEase(Ease.InBack).OnComplete(
                () =>
                {
                    this.gameObject.SetActive(false);
                }

                );
        }
    }
}