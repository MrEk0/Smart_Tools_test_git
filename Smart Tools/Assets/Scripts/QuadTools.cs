using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTools
{
    private float quadSize = 2f;
    private Vector2 previousQuadPos=new Vector2Int();

    public void SetQuadSize(float size)
    {
        quadSize = size;
    }

    public Vector2Int WorldToQuadPosition(Vector2 worldPos)
    {
        int quadX = Mathf.RoundToInt(worldPos.x / quadSize);
        int quadY = Mathf.RoundToInt(worldPos.y / quadSize);
        return new Vector2Int(quadX, quadY);
    }

    public Vector3 QuadToWorldPosition(Vector2 quadPos)
    {
        int quadX = Mathf.RoundToInt(quadPos.x * quadSize);
        int quadY = Mathf.RoundToInt(quadPos.y * quadSize);
        return new Vector3(quadX, 0, quadY);
    }

    public bool IsQuadPosChanged(Vector2 currentPos)
    {
        // Vector2Int currentPos=WorldToQuadPosition(worldPos);
        //Debug.Log("prev "+previousQuadPos);
        //Debug.Log("current "+currentPos);
        if (currentPos==previousQuadPos)
        {
            return false;
        }
        else
        {
            previousQuadPos = currentPos;
            //Debug.Log("Cahnged");
            return true;
        }
    }
}
