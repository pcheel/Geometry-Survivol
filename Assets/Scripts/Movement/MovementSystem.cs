using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementSystem))]
public class MovementSystem : FixedUpdateSystem {

    private Filter _movable;
    private Stash<Movement> _movementStash;

    public override void OnAwake() 
    {
        _movable = World.Filter.With<Movement>().Build();

        _movementStash = World.GetStash<Movement>();
    }
    public override void OnUpdate(float deltaTime) 
    {
        foreach (var entity in _movable)
        {
            ref var movement = ref _movementStash.Get(entity);
            movement.rigidbody.linearVelocity = movement.direction * movement.speed;
        }
    }
}