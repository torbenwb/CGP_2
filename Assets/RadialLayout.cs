using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialLayout
{
    public static void GetPlacement(int i, int n, float w, float p, float r){
        float c = 2 * Mathf.PI * r;
        float t = (n * w) + ((n - 1) * p);
        float a = (t / c) * 360f;
        Vector3 v = Vector3.up;
        v = Quaternion.Euler(0f, 0f, -(a / 2)) * v;
    }
   
}
