using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageSystem))]
public class DamageSystem : UpdateSystem 
{
    private Filter _damaged;

    private Stash<ApplyDamageEvent> _damagedStash;
    private Stash<Health> _healthStash;
    private Stash<IsDeadMarker> _isDeadStash;
    public override void OnAwake() 
    {
        _damaged = World.Filter.With<ApplyDamageEvent>().With<Health>().Without<IsDeadMarker>().Build();
        _damagedStash = World.GetStash<ApplyDamageEvent>();
        _healthStash = World.GetStash<Health>();
        _isDeadStash = World.GetStash<IsDeadMarker>();
    }

    public override void OnUpdate(float deltaTime) 
    {
        ApplyDamage();
    }

    private void ApplyDamage()
    {
        foreach (var entity in _damaged)
        {
            ref ApplyDamageEvent damage = ref _damagedStash.Get(entity);
            ref Health health = ref _healthStash.Get(entity);
            health.health = health.health - damage.damage <= 0 ? 0 : health.health - damage.damage;
            _damagedStash.Remove(entity);

            if (health.health == 0)
            {
                _isDeadStash.Add(entity);
            }
        }
    }
}