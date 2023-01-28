using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TetrisInstaller : MonoInstaller<TetrisInstaller>
{
    [SerializeField] private GameConfig _gameConfig;
    public override void InstallBindings()
    {
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<TetrisManager>().AsSingle().NonLazy();
    }
}
