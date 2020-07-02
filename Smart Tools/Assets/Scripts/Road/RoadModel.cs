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

    [Inject]
    private void Construct(DiamondPresenter.Factory factory)
    {
        _factory = factory;
    }

    public void Init(float diamondChance)
    {
        _quads = new Queue<QuadPresenter>();
        _diamonds = new Queue<DiamondPresenter>();
        _startPanelQuads = new List<QuadPresenter>();

        _diamondChance = diamondChance;
        _directionsLength= Enum.GetNames(typeof(RoadDirections)).Length;
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

    public void RearrangeQuad(QuadPresenter quad)
    {
        AddQuad(_quads.Dequeue());
    }

    public Vector2 GetRandomDirection()
    {
        Vector2 lastQuadPos = _lastQuad.GetPosition();
        RoadDirections direction = (RoadDirections)Random.Range(0, _directionsLength);

        switch (direction)
        {
            case RoadDirections.Forward:
                lastQuadPos.y++;
                break;
            case RoadDirections.Right:
                lastQuadPos.x++;
                break;
        }
        return lastQuadPos;
    }

    public void ResetRoad()
    {
        QuadPresenter firstQuad = _quads.Peek();

        Vector2 newQuadPos = GetRandomDirection();
        firstQuad.QuadModel.Position.Value = newQuadPos;
        firstQuad.gameObject.SetActive(true);
        RearrangeQuad(firstQuad);

        if (!CanSpawnDiamond())
            return;

        DiamondPresenter diamond = ResetDiamondPosition();
        if (diamond != null)
        {
            diamond.DiamondModel.Position.Value = newQuadPos;
            diamond.gameObject.SetActive(true);
            diamond.transform.SetParent(firstQuad.transform);
        }
        else
        {
            DiamondPresenter diamondPresenter = _factory.Create(newQuadPos);
            diamondPresenter.transform.SetParent(firstQuad.transform);
            AddDiamond(diamondPresenter);
        }
    }

    public bool CanSpawnDiamond()
    {
        int diamondChance = Random.Range(0, 100);
        return diamondChance < _diamondChance;
    }

    public bool IsThereQuadBeneath(Vector2 quadPosition)
    {
        if (_startPanelQuads.Count != 0)
        {
            foreach (var quad in _startPanelQuads)
            {
                if (quad.GetPosition() == quadPosition)
                {
                    return true;
                }
            }
        }

        foreach (var quad in _quads)
        {
            if (quad.GetPosition() == quadPosition)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsQuadStartPanel(QuadPresenter quad)
    {
        if (_startPanelQuads.Count == 0)
            return false;

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
            return false;
        }
    }

}
