using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Reflection;
using System;

public class CameraFilterSystem : MonoBehaviour
{
    public static CameraFilterSystem instance;

    public CameraFilterPack_Atmosphere_Rain cfp_rain { get; private set; }
    public CameraFilterPack_Atmosphere_Rain_Pro cfp_rainPro { get; private set; }
    public CameraFilterPack_Distortion_Dream2 cfp_dream2 { get; private set; }
    public CameraFilterPack_Distortion_ShockWave cfp_shockWave { get; private set; }
    public CameraFilterPack_Distortion_Water_Drop cfp_distortion_water_Drop { get; private set; }
    public CameraFilterPack_AAA_WaterDrop cfp_aaa_water_Drop { get; private set; }
    public CameraFilterPack_Vision_AuraDistortion cfp_aura_distortion { get; private set; }
    public CameraFilterPack_Vision_Drost cfp_drost { get; private set; }

    private void Awake()
    {
        instance = this;
        var trans = Camera.main.transform;
        cfp_rain = trans.GetComponent<CameraFilterPack_Atmosphere_Rain>();
        cfp_rainPro = trans.GetComponent<CameraFilterPack_Atmosphere_Rain_Pro>();
        cfp_dream2 = trans.GetComponent<CameraFilterPack_Distortion_Dream2>();
        cfp_shockWave = trans.GetComponent<CameraFilterPack_Distortion_ShockWave>();
        cfp_distortion_water_Drop = trans.GetComponent<CameraFilterPack_Distortion_Water_Drop>();
        cfp_aaa_water_Drop = trans.GetComponent<CameraFilterPack_AAA_WaterDrop>();
        cfp_aura_distortion = trans.GetComponent<CameraFilterPack_Vision_AuraDistortion>();
        cfp_drost = trans.GetComponent<CameraFilterPack_Vision_Drost>();

    }

    public void Tween<T>(T target, string valueName, float from, float to, float duration)
    {
        StartCoroutine(TweenValue(target, valueName, from, to, duration));
    }

    IEnumerator TweenValue<T>(T obj, string valueName, float from, float to, float duration)
    {
        Type t = obj.GetType();
        float dt = 0;
        while (dt < duration)
        {
            var v = Mathf.Lerp(from, to, dt / duration);
            t.GetField(valueName).SetValue(obj, v);
            yield return null;
            dt += Time.deltaTime;
        }

        t.GetField(valueName).SetValue(obj, to);
    }
}