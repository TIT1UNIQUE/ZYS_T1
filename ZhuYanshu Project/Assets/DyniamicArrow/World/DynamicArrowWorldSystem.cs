using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通过三阶贝塞尔曲线绘制选择箭头 - 支持外部控制版本
/// </summary>
public class DynamicArrowWorldSystem : MonoBehaviour
{
    [Header("引用 可以拖拽")]
    [SerializeField] private Transform arrowHeadPrefab;
    [SerializeField] private Transform arrowNodePrefab;
    [SerializeField] private Transform origin;
    [SerializeField] private Camera projectionCamera;
    [SerializeField] private LayerMask layerMask; // Assign the layer mask to include the Box Collider's layer
    [Header("箭头配置")]
    [SerializeField] private int arrowNodeNum = 18;
    [SerializeField] private float headRotationOffset = 90;

    [Header("贝塞尔曲线参数 可以用默认值")]
    [Range(0f, 1f)]
    [SerializeField] private float controlPoint1LengthRatio = 0.6f; // P1为P0上方的点，P0到P1占P0到P3的y轴高度差的比例
    [Range(0f, 1f)]
    [SerializeField] private float controlPoint2LengthRatio = 0.4f; // P2为P3左右侧的点，P2到P3占P0到P3的x轴横向长度差的比例
    [SerializeField] private float refControlPoint1Height = 800f;
    [SerializeField] private float controlPoint1Height = -200;
    [SerializeField] private float controlPoint2Height = -250;
    [SerializeField] private float controlPoint3Height = -80;

    [Header("流动动画")]
    [SerializeField] private bool enableFloatingAnim = true;
    [SerializeField] private float floatingSpeed = 0.08f;
    [SerializeField] private float floatingThresholdMax = 0.5f;

    private List<Transform> arrowNodes = new List<Transform>();
    private List<Vector3> controlPoints = new List<Vector3>();

    private void Awake()
    {
        // 创建节点
        for (int i = 0; i < arrowNodeNum; ++i)
        {
            var node = Instantiate(arrowNodePrefab, arrowNodePrefab.parent);
            arrowNodes.Add(node);
            node.gameObject.name = "node " + arrowNodes.Count;
        }

        // 创建箭头头部
        var head = Instantiate(arrowHeadPrefab, arrowHeadPrefab.parent);
        arrowNodes.Add(head);
        head.gameObject.SetActive(true);
        head.gameObject.name = "head " + arrowNodes.Count;

        // 初始化控制点
        for (int i = 0; i < 4; ++i)
            controlPoints.Add(Vector3.zero);
    }

    private void Update()
    {
        if (arrowNodes.Count == 0) return;

        UpdateWithMouseControl();
    }

    /// <summary>
    /// 使用鼠标控制的更新（原有逻辑）
    /// </summary>
    private void UpdateWithMouseControl()
    {
        // Get the mouse position in screen space
        Ray ray = projectionCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // If the ray hits the Box Collider, print the hit point
            //Debug.Log("Hit point: " + hit.point);

            // Optionally, draw a line in the Scene view to visualize the ray
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            // If no hit, draw a line to infinity
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);
            return;
        }

        // 计算控制点
        CalculateControlPoints(origin.position, hit.point);

        // 更新节点
        UpdateArrowNodes();
    }

    /// <summary>
    /// 计算控制点
    /// </summary>
    private void CalculateControlPoints(Vector3 startPosition, Vector3 targetPosition)
    {
        controlPoints[0] = startPosition; // P0: 起点
        controlPoints[3] = targetPosition; // P3: 终点

        Vector3 direction = targetPosition - startPosition;
        float distance = direction.magnitude;
        float noCurveDist = 0.1f;

        if (distance <= noCurveDist)
        {
            // 距离太小时，退化为直线
            controlPoints[1] = Vector3.Lerp(controlPoints[0], controlPoints[3], 0.4f);
            controlPoints[2] = Vector3.Lerp(controlPoints[0], controlPoints[3], 0.6f);
        }
        else
        {
            // 计算垂直向量
            //Vector3 perpendicular = new Vector3(-direction.y, direction.x).normalized;
            var lenY = Mathf.Abs(controlPoints[3].y - controlPoints[0].y);
            var lenXSigned = controlPoints[3].x - controlPoints[0].x;

            // 计算控制点位置
            controlPoints[1] = controlPoints[0] + Vector3.up * lenY * controlPoint1LengthRatio;
            var y1 = (refControlPoint1Height + controlPoints[1].y) * 0.5f;
            controlPoints[1] = new Vector3(controlPoints[1].x, y1, controlPoint1Height);
            controlPoints[2] = controlPoints[3] + Vector3.left * lenXSigned * controlPoint2LengthRatio;
            var y2 = (controlPoints[1].y + controlPoints[2].y) * 0.5f;
            controlPoints[2] = new Vector3(controlPoints[2].x, y2, controlPoint2Height);
            controlPoints[3] = new Vector3(controlPoints[3].x, controlPoints[3].y, controlPoint3Height);
        }
    }


    /// <summary>
    /// 更新箭头节点 - 改为公共方法
    /// </summary>
    public void UpdateArrowNodes()
    {
        for (int i = 0; i < arrowNodes.Count; ++i)
        {
            var node = arrowNodes[i];
            node.gameObject.SetActive(true);
            float t = CalculateTValue(i);//   return (float)index / (arrowNodes.Count - 1);
            float div = 1f / (arrowNodes.Count - 1);
            var t_floating = t;
            if (enableFloatingAnim && i != arrowNodes.Count - 1)
            {
                t_floating += Time.time * floatingSpeed;
                while (t_floating > t + div * floatingThresholdMax)
                    t_floating -= div;
            }

            // 计算贝塞尔曲线位置
            Vector3 pos = CalculateBezierPoint(t_floating);

            // 验证计算结果
            if (float.IsNaN(pos.x) || float.IsNaN(pos.y))
            {
                pos = Vector3.Lerp(controlPoints[0], controlPoints[3], t);
            }

            // 设置位置
            node.position = pos;

            // 设置旋转
            if (i > 0)
            {
                Vector3 dir = node.position - arrowNodes[i - 1].position;
                if (dir.magnitude > 0.001f)
                {
                    node.rotation = Quaternion.LookRotation(-Vector3.forward, dir);
                }
            }
        }

        if (arrowNodes.Count >= 2)
        {
            //arrowNodes[0].eulerAngles = arrowNodes[1].eulerAngles;
            arrowNodes[arrowNodes.Count - 1].eulerAngles += new Vector3(0, 0, headRotationOffset);
        }

    }

    private float CalculateTValue(int index)
    {
        return (float)index / (arrowNodes.Count - 1);
    }

    private Vector3 CalculateBezierPoint(float t)
    {
        float oneMinusT = 1f - t;
        return Mathf.Pow(oneMinusT, 3) * controlPoints[0] +
               3 * Mathf.Pow(oneMinusT, 2) * t * controlPoints[1] +
               3 * oneMinusT * Mathf.Pow(t, 2) * controlPoints[2] +
               Mathf.Pow(t, 3) * controlPoints[3];
    }

    //estimate the arc length as the average between the chord and the control net. In practice:
    float GetEstimatedBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var chord = (p3 - p0).magnitude;
        var cont_net = (p0 - p1).magnitude + (p2 - p1).magnitude + (p3 - p2).magnitude;
        return (cont_net + chord) / 2;
    }
}