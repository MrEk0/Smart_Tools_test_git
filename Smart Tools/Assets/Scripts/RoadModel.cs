using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoadModel
{
    private DiamondPresenter.Factory _factory;
    private Queue<QuadPresenter> quads;
    private Queue<DiamondPresenter> diamonds;
    private QuadPresenter lastQuad;

    private float _diamondChance;

    public void Init(float diamondChance, DiamondPresenter.Factory factory)
    {
        quads = new Queue<QuadPresenter>();
        diamonds = new Queue<DiamondPresenter>();

        _diamondChance = diamondChance;
        _factory = factory;
    }

    public void AddQuad(QuadPresenter quad)
    {
        quads.Enqueue(quad);
        lastQuad = quad;
    }

    public void AddDiamond(DiamondPresenter diamond)
    {
        diamonds.Enqueue(diamond);
    }

    public DiamondPresenter ResetDiamondPosition()
    {
        DiamondPresenter diamond = diamonds.Peek();
        if (!diamond.gameObject.activeInHierarchy)
        {
            AddDiamond(diamonds.Dequeue());
            return diamond;
        }
        else
        {
            return null;
        }
    }

    public bool IsQuadExist()
    {
        return quads.Count!=0;
    }

    public void RearrangeQuad(QuadPresenter quad)
    {
        AddQuad(quads.Dequeue());
    }

    public QuadPresenter GetLastQuad()
    {
        return lastQuad;
    }

    public void ResetRoad()
    {
        QuadPresenter firstQuad = quads.Peek();

        Vector2 quadPos = GetLastQuad().GetPosition();
        RoadDirections direction = (RoadDirections)Random.Range(0, 2);

        switch (direction)
        {
            case RoadDirections.Forward:
                quadPos.y++;
                break;
            case RoadDirections.Right:
                quadPos.x++;
                break;
        }

        firstQuad.QuadModel.Position.Value = quadPos;
        firstQuad.gameObject.SetActive(true);
        RearrangeQuad(firstQuad);

        int diamondChance = Random.Range(0, 100);
        if (diamondChance > _diamondChance)
            return;

        DiamondPresenter diamond = ResetDiamondPosition();
        if (diamond != null)
        {
            diamond.DiamondModel.Position.Value = quadPos;
            diamond.gameObject.SetActive(true);
            diamond.transform.SetParent(firstQuad.transform);
            //diamond.gameObject.SetActive(true);
        }
        else
        {
            DiamondPresenter diamondPresenter = _factory.Create(quadPos);
            Debug.Log("Create");
            diamondPresenter.transform.SetParent(firstQuad.transform);
            AddDiamond(diamondPresenter);
        }
    }

}
