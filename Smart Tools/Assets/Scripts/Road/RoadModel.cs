using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Zenject;

public enum RoadDirections
{
    Right,
    Forward
}

public class RoadModel
{
    private DiamondPresenter.Factory _factory;
    private Queue<QuadPresenter> _quads;
    private Queue<DiamondPresenter> _diamonds;
    private QuadPresenter _lastQuad;

    private List<QuadPresenter> _startPanelQuads;

    private float _diamondChance;
    private int _directionsLength;

    public void Init(float diamondChance, DiamondPresenter.Factory factory)
    {
        _quads = new Queue<QuadPresenter>();
        _diamonds = new Queue<DiamondPresenter>();
        _startPanelQuads = new List<QuadPresenter>();

        _diamondChance = diamondChance;
        _directionsLength= Enum.GetNames(typeof(RoadDirections)).Length;
        _factory = factory;
    }

    public void AddQuad(QuadPresenter quad)
    {
        _quads.Enqueue(quad);
        _lastQuad = quad;
    }

    public void AddStartPanelQuad(QuadPresenter quad)
    {
        _startPanelQuads.Add(quad);
        _lastQuad = quad;
    }

    public void AddDiamond(DiamondPresenter diamond)
    {
        _diamonds.Enqueue(diamond);
    }

    public DiamondPresenter ResetDiamondPosition()
    {
        DiamondPresenter diamond = _diamonds.Peek();
        if (!diamond.gameObject.activeInHierarchy)
        {
            AddDiamond(_diamonds.Dequeue());
            return diamond;
        }
        else
        {
            return null;
        }
    }

    public bool IsQuadExist()
    {
        return _quads.Count!=0;
    }

    public void RearrangeQuad(QuadPresenter quad)
    {
        AddQuad(_quads.Dequeue());
    }

    public QuadPresenter GetLastQuad()
    {
        return _lastQuad;
    }

    public void ResetRoad()//refactoring
    {
        QuadPresenter firstQuad = _quads.Peek();

        Vector2 quadPos = GetLastQuad().GetPosition();
        RoadDirections direction = (RoadDirections)Random.Range(0, _directionsLength);

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
        }
        else
        {
            DiamondPresenter diamondPresenter = _factory.Create(quadPos);
            diamondPresenter.transform.SetParent(firstQuad.transform);
            AddDiamond(diamondPresenter);
        }
    }

    public bool IsThereQuadBeneath(Vector2 quadPosition)//improve
    {
        foreach (var quad in _quads)
        {
            if(quad.QuadModel.Position.Value==quadPosition)
            {
                return true;
            }
        }

        if (_startPanelQuads.Count != 0)
        {
            foreach (var quad in _startPanelQuads)
            {
                if (quad.QuadModel.Position.Value == quadPosition)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsQuadStartPanel(QuadPresenter quad)//improve
    {
        //if (_startPanelQuads.Count != 0)
        //    return false;

        if (_startPanelQuads.Contains(quad))
        {
            return true;
        }
        else
        {
            foreach (var quadPanel in _startPanelQuads)
            {
                quadPanel.gameObject.SetActive(false);
            }
            _startPanelQuads.Clear();
            //Debug.Log(_startPanelQuads.Count);
            return false;
        }
    }

}
