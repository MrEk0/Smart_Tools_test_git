using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerModel
{
    public Vector2ReactiveProperty WorldPosition = new Vector2ReactiveProperty();

    public Vector2Int WorldToQuadPosition()
    {
        int quadX = Mathf.RoundToInt(WorldPosition.Value.x / 2);//get rid of 2
        int quadY = Mathf.RoundToInt(WorldPosition.Value.y / 2);
        return new Vector2Int(quadX, quadY);
    }

}
