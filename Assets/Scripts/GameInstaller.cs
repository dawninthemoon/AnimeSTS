using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<RoomHandler>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<IEncounterable>().FromComponentsInHierarchy().AsSingle().NonLazy();
    }
}