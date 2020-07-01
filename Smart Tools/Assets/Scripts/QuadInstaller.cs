using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuadInstaller : Installer<QuadInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<QuadModel>().AsSingle();
    }
}
