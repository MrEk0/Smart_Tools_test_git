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
        //QuadInstaller.Install(Container);

        Container.BindFactory<Vector2, QuadPresenter, QuadPresenter.Factory>().FromComponentInNewPrefab(quadPrefab);
        Container.BindFactory<Vector2, DiamondPresenter, DiamondPresenter.Factory>().FromComponentInNewPrefab(diamondPrefab);
        //Container.BindFactory<Vector2, QuadPresenter, QuadPresenter.Factory>()
        //    .FromMonoPoolableMemoryPool(x => x.WithInitialSize(20).FromSubContainerResolve()
        //    .ByNewPrefabInstaller<QuadInstaller>(quadPrefab)
        //    .UnderTransformGroup("RoadPresenter"));
    }
}
