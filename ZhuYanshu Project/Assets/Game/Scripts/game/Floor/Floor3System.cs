using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using com;

public class Floor3System : MonoBehaviour
{
    public static Floor3System instance;

    public ClickSpeak speakerMan;
    public ClickSpeak speakerWoman;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
    }

    public void OnClickMan()
    {
        speakerMan.Speak();
    }

    public void OnClickWoman()
    {
        speakerWoman.Speak();
    }
}