using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLayout : MonoBehaviour
{
    public static Vector3 GetPositionInLayout(int index, int count, float width, float padding){
        float t = count * width + (count - 1) * padding;
        float start = -(t / 2);

        float x = ((float)index + 0.5f) * width + (index * padding);
        x += start;
        return new Vector3(x, 0f, 0f); 
    }
}
