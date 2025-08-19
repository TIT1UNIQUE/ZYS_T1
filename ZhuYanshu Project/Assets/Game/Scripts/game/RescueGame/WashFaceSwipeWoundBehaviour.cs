using com;
using DG.Tweening;
using Rescue;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WashFaceSwipeWoundBehaviour : MonoBehaviour
{
    public RectTransform[] wounds;
    public float distanceTolerance = 5;
    public float healAlphaDelta = 0.1f;


    public void OnSwiping(Vector2 pos)
    {
        foreach (var w in wounds)
        {
            var dist = (w.anchoredPosition - pos).magnitude;
            if (dist < distanceTolerance)
            {
                HealWound(w);
            }
        }

        bool allWoundsHealed = true;
        foreach (var w in wounds)
        {
            var img = w.GetComponent<Image>();
            if (img.color.a > 0.08f)
            {
                allWoundsHealed = false;
                break;
            }
        }

        if (allWoundsHealed)
        {
              RescueSystem.instance.OnAllWoundHealed();
           
        }
    }

    private float _soundCd = 0.3f;
    private float _nextSoundCanPlayTime;

    void HealWound(RectTransform wound)
    {
        var img = wound.GetComponent<Image>();
        var c = img.color;
        c.a -= healAlphaDelta;
        c.a = Mathf.Clamp(c.a, 0, 1);
        img.color = c;

        if (Time.time > _nextSoundCanPlayTime)
        {
            _nextSoundCanPlayTime = Time.time + _soundCd;
            SoundSystem.instance.Play("swipe");
        }
    }

}