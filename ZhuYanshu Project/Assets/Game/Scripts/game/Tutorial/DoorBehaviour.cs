using DG.Tweening;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public float endX;//���� property
    public float duration;//���� property
    public Ease ease;
    public SpriteRenderer sr;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        //transform.DOMoveX(endX, duration).SetEase(ease);
        //transform.DOShakePosition(5, 10, 10);
        sr.DOColor(color, 10);
    }
}
