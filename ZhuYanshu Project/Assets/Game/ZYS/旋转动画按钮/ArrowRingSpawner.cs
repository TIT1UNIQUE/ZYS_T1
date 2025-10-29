using Assets.Game.ZYS.旋转动画按钮;
using UnityEngine;

[System.Serializable]
public class ArrowRing
{
    public int count = 10;           // 当前圆环箭头数量
    public float radius = 200f;      // 半径
    public float startAngle = 0f;    // 起始角度（度数）
    //public float deltaAngle = 360f;  // 覆盖的角度范围（例如 90 表示四分之一圈）
    public float startRotation = 0f;
    public float animationDelay;
}

public class ArrowRingSpawner : MonoBehaviour
{
    public RingArrow arrowPrefab;       // 箭头Prefab
    public ArrowRing[] rings;            // 多层圆环配置
    public bool faceCenter = true;       // 箭头是否朝向中心
    public Transform parentTrans;

    void Start()
    {
        //SpawnAllRings();
    }

    public void SpawnAllRings()
    {
        foreach (var ring in rings)
        {
            // 每个箭头的角度间隔
            float step = 360f / ring.count;
            // float step = 360 / (ring.count - 1);
            for (int i = 0; i < ring.count; i++)
            {
                float angleDeg = ring.startAngle + step * i;
                SpawnArrow(ring, angleDeg, ring.startRotation + i * step);
            }
        }
    }

    void SpawnArrow(ArrowRing ring, float angleDeg, float rot)
    {
        RingArrow arrow = Instantiate(arrowPrefab, parentTrans);
        arrow.transform.localRotation = Quaternion.Euler(0, 0, rot);
        arrow.Setup(angleDeg, ring.radius, ring.animationDelay);
        arrow.gameObject.SetActive(true);
    }
}