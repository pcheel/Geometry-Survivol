using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CollisionEventHandler))]
public class CollisionEventHandler : UpdateSystem 
{
    private Filter _collisionEvents;

    private Stash<CollisionEvent> _collisionEventStash;
    private Stash<IsDeadMarker> _isDeadMarkerStash;
    private Stash<BulletMarker> _bulletMarkerStash;
    private Stash<BorderMarker> _borderMarkerStash;
    private Stash<PlayerMarker> _playerMarkerStash;
    private Stash<EnemyMarker> _enemyMarkerStash;
    private Stash<DealingCollisionDamage> _dealingCollisionDamageStash;
    private Stash<ApplyDamageEvent> _applyDamageEventStash;

    public override void OnAwake() 
    {
        _collisionEvents = World.Filter.With<CollisionEvent>().Build();

        _collisionEventStash = World.GetStash<CollisionEvent>();
        _isDeadMarkerStash = World.GetStash<IsDeadMarker>();
        _bulletMarkerStash = World.GetStash<BulletMarker>();
        _borderMarkerStash = World.GetStash<BorderMarker>();
        _playerMarkerStash = World.GetStash<PlayerMarker>();
        _enemyMarkerStash = World.GetStash<EnemyMarker>();
        _dealingCollisionDamageStash = World.GetStash<DealingCollisionDamage>();
        _applyDamageEventStash = World.GetStash<ApplyDamageEvent>();
    }

    public override void OnUpdate(float deltaTime) 
    {
        if (_collisionEvents.IsEmpty())
            return;

        foreach(var entity in _collisionEvents)
        {
            ref CollisionEvent collisionEvent = ref _collisionEventStash.Get(entity);
            if (_bulletMarkerStash.Has(collisionEvent.first) && _borderMarkerStash.Has(collisionEvent.second))
            {
                if (!_isDeadMarkerStash.Has(collisionEvent.first))
                    _isDeadMarkerStash.Add(collisionEvent.first);
            }
            else if (_enemyMarkerStash.Has(collisionEvent.first) && _bulletMarkerStash.Has(collisionEvent.second))
            {
                ref ApplyDamageEvent applyDamageEvent = ref _applyDamageEventStash.Add(collisionEvent.first);
                applyDamageEvent.damage = _dealingCollisionDamageStash.Get(collisionEvent.second).damage;
                _isDeadMarkerStash.Add(collisionEvent.second);
            }
            else if (_playerMarkerStash.Has(collisionEvent.first) && _enemyMarkerStash.Has(collisionEvent.second)
                || _enemyMarkerStash.Has(collisionEvent.first) && _playerMarkerStash.Has(collisionEvent.second))
            {
                ref ApplyDamageEvent applyDamageEvent = ref _applyDamageEventStash.Add(collisionEvent.first);
                applyDamageEvent.damage = _dealingCollisionDamageStash.Get(collisionEvent.second).damage;
            }
        }

        ClearCollisionEvents();
    }

    private void ClearCollisionEvents()
    {
        int length = _collisionEvents.GetLengthSlow();
        for (int i = 0; i < length; i++)
        {
            _collisionEvents.First().Dispose();
        }
    }
}