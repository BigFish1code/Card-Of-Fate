using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAroow : MonoBehaviour
{
    private LineRenderer lineRenderer;
    //贝塞尔曲线
    public int pointsCount;
    public float arcModifier;
    //......
    private Vector3 mousePos;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

        SetArrowPosition();
    }

    public void SetArrowPosition()
    {
        Vector3 cardPosition = transform.position; //卡牌位置
        Vector3 direction = mousePos - cardPosition;//从卡牌指向鼠标方向
        Vector3 normallizeDirection = direction.normalized;//方向归一
        //计算垂直于卡牌到鼠标方向的向量
        Vector3 perpendicular = new(-normallizeDirection.y, normallizeDirection.x, normallizeDirection.z);
        //设置控制点偏移量
        Vector3 offset = perpendicular * arcModifier;//调整该值改变曲线形状
        Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset;//控制点
        lineRenderer.positionCount = pointsCount;//设置LineRenderer的点数
       
        for (int i = 0; i < pointsCount; i++)
        {
            float t = i/(float)(pointsCount - 1);
            Vector3 point = CalculateQuadraticBezierPoint(t, cardPosition, controlPoint, mousePos); 
            lineRenderer.SetPosition(i, point);
        }
    }
    Vector3 CalculateQuadraticBezierPoint(float t,Vector3 p0,Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}
