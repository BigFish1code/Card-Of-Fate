using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float offsetSpeed = 0.1f;
    private void Update()
    {
        if(lineRenderer != null)
        {
            var offset = lineRenderer.material.mainTextureOffset;

            offset.x += offsetSpeed * Time.deltaTime;

            lineRenderer.material.mainTextureOffset = offset;
        }
    }
}
