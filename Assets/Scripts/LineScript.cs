using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineScript : MonoBehaviour
{
    public Transform endPoint;
    public LineRenderer lineRenderRef;

    // Update is called once per frame
    void LateUpdate()
    {
        lineRenderRef.SetPosition(1, endPoint.localPosition);
    }
}