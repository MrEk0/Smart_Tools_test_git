using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] GameObject quadPrefab;
    [SerializeField] GameObject diamondPrefab;

    public override void InstallBindings()
    {
        Container.Bind<PlayerModel>().AsSingle();
        Container.Bind<RoadModel>().AsSingle();
        Container.Bind<QuadModel>().AsTransient();
        Container.Bind<PointsModel>().AsSingle();
        Container.Bind<DiamondModel>().AsTransient();
        Container.Bind<GameOverModel>().AsSingle();
        Container.Bind<QuadTools>().AsSingle();

        Container.BindFactory<Vector2, QuadPresenter, QuadPresenter.Factory>().FromComponentInNewPrefab(quadPrefab);
        Container.BindFactory<Vector2, DiamondPresenter, DiamondPresenter.Factory>().FromComponentInNewPrefab(diamondPrefab);
    }
}
