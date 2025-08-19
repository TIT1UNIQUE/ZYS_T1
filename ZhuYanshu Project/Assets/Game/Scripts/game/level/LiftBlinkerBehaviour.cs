using com;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LiftBlinkerBehaviour : MonoBehaviour
{
    public Image img;
    private float _timer;

    public float durationMin;
    public float durationMax;
    public float intervalMin;
    public float intervalMax;
    // Use this for initialization
    void Start()
    {
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer <= 0)
        {
            if (img.enabled)
            {
                img.enabled = false;
                _timer = Random.Range(intervalMin, intervalMax);
            }
            else
            {
                img.enabled = true;
                _timer = Random.Range(durationMin, durationMax);
                SoundSystem.instance.Play("blink");
            }
        }
        _timer -= Time.deltaTime;
    }
}