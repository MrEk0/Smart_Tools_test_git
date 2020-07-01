using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;




public class RoadPresenter : ReactivePresenter<RoadModel>
{
    public Transform startPanel;
    [SerializeField] int _startQuadSize = 3;
    [SerializeField] int _visibleQuads = 20;
    [SerializeField] float _diamondChance = 20;
    
    private QuadPresenter.Factory _quadFactory;
    private DiamondPresenter.Factory _diamondFactory;
    private RoadModel _roadModel;

    [Inject]
    private void Construct(QuadPresenter.Factory quadFactory, DiamondPresenter.Factory diamondFactory, RoadModel roadModel)
    {
        _quadFactory = quadFactory;
        _diamondFactory = diamondFactory;
        _roadModel = roadModel;
        SetModel(_roadModel);
    }

    private void Awake()
    {
        _roadModel.Init(_diamondChance, _diamondFactory);
        CreateRoad();
    }

    private void CreateRoad()
    {
        GenerateStartPlatform();
        GenerateRoad();
    }

    private void GenerateStartPlatform()
    {
        for (int i = 0; i < _startQuadSize; i++)
        {
            for (int j = 0; j < _startQuadSize; j++)
            {
                QuadPresenter quadPresenter = _quadFactory.Create(new Vector2(i, j));
                //_roadModel.AddQuad(quadPresenter);  
                _roadModel.AddStartPanelQuad(quadPresenter);
                quadPresenter.transform.SetParent(startPanel);            
            }
        }
    }

    private void GenerateRoad()
    {
        Vector2 quadPos = _roadModel.GetLastQuad().GetPosition();

        for (int i = 0; i < _visibleQuads; i++)
        {
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

            QuadPresenter quadPresenter = _quadFactory.Create(new Vector2(quadPos.x, quadPos.y));
            GenerateDiamonds(new Vector2(quadPos.x, quadPos.y), quadPresenter.transform);
            _roadModel.AddQuad(quadPresenter);
            quadPresenter.transform.SetParent(transform);
        }
    }

    private void GenerateDiamonds(Vector2 position, Transform parentTransform)
    {
        int chance= Random.Range(0, 100);
        if (chance > _diamondChance)
            return;

        DiamondPresenter diamondPresenter = _diamondFactory.Create(position);
        diamondPresenter.transform.SetParent(parentTransform);
        _roadModel.AddDiamond(diamondPresenter);
    }
}
