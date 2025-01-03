using ModestTree.Util;
using System;
using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    [SerializeField] private PrefabsConfig _prefabsConfig;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private EnemyConfigs _enemyConfigs;
    [SerializeField] private PlayerFactory _playerFactory;
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private BulletFactory _bulletFactory;
    [SerializeField] private ScoreView _scoreView;
    public override void InstallBindings()
    {
        BindConfigs();
        BindFactories();
        BindPools();
        BindSpawners();
        BindScore();
    }

    private void BindConfigs()
    {
        Container.Bind<PrefabsConfig>().FromInstance(_prefabsConfig);
        Container.Bind<PlayerConfig>().FromInstance(_playerConfig);
        Container.Bind<BulletConfig>().FromInstance(_bulletConfig);
        Container.Bind<EnemyConfigs>().FromInstance(_enemyConfigs);
    }
    private void BindFactories()
    {
        Container.Bind<PlayerFactory>().FromInstance(_playerFactory);
        Container.Bind<EnemyFactory>().FromInstance(_enemyFactory);
        Container.Bind<BulletFactory>().FromInstance(_bulletFactory);
    }
    private void BindSpawners()
    {
        Container.Bind<PlayerSpawner>().AsSingle();
    }
    private void BindPools()
    {
        Container.Bind<ObjectPool<EnemyType>>()
            .AsSingle()
            .WithArguments<Func<EnemyType, GameObject>, EnemyType>(x => _enemyFactory.Create(x), EnemyType.Default);
        Container.Bind<ObjectPool<BulletType>>()
            .AsSingle()
            .WithArguments<Func<BulletType, GameObject>, BulletType>(x => _bulletFactory.Create(x), BulletType.Default);
    }
    private void BindScore()
    {
        Container.Bind<ScoreModel>().AsSingle();
        Container.Bind<ScoreView>().FromInstance(_scoreView);
        Container.Bind<ScorePresenter>().AsSingle();
    }
}