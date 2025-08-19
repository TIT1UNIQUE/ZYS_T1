using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class LiftDoor
{
    public RectTransform door;

    public float openX;
    public float closeX;
    public float duration;

    public bool stopped { get; private set; }
    public bool closeOrToClose { get; private set; }
    public bool closedAndStopped { get { return stopped & closeOrToClose; } }
    public bool openAndStopped { get { return stopped & !closeOrToClose; } }
    public void Set(bool toOpen, bool instant)
    {
        closeOrToClose = !toOpen;
        var targetX = toOpen ? openX : closeX;
        if (instant)
        {
            door.DOKill();
            var pos = door.anchoredPosition;
            pos.x = targetX;
            door.anchoredPosition = pos;
            stopped = true;
        }
        else
        {
            stopped = false;
            door.DOKill();
            door.DOAnchorPosX(targetX, duration).SetEase(Ease.InOutCubic).OnComplete(() => { stopped = true; });
        }
    }
}