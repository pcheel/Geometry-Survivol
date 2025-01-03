using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletSpawner))]
public sealed class BulletSpawner : UpdateSystem 
{
    private ObjectPool<BulletType> _bulletObjectPool;

    private Attacking _playerAttacking;
    private float _timeAfterLastShot;
    private bool _isAllowedToShot;
    private Filter _startGameEvent;
    private Entity _player;
    private Transform _playerTransform;

    private Stash<Attacking> _attackingStash;

    [Inject]
    private void Construct(ObjectPool<BulletType> bulletObjectPool)
    {
        _bulletObjectPool = bulletObjectPool;
    }
    public override void OnAwake() 
    {
        _startGameEvent = World.Filter.With<StartGameEvent>().Build();

        _attackingStash = World.GetStash<Attacking>();
    }

    public override void OnUpdate(float deltaTime) 
    {
        if (_startGameEvent.IsNotEmpty())
        {
            _isAllowedToShot = true;
            _player = World.Filter.With<PlayerMarker>().Build().First();
        }

        if (!_isAllowedToShot)
            return;

        _timeAfterLastShot += deltaTime;
        if (_timeAfterLastShot < _attackingStash.Get(_player).shotsDelay)
            return;

        _timeAfterLastShot = 0f;
        GameObject bulletGO = _bulletObjectPool.GetObject(BulletType.Default);
    }
}