using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EventCleanupSystem))]
public class EventCleanupSystem : CleanupSystem 
{
    private Filter _startGameEvent;
    public override void OnAwake()
    {
        _startGameEvent = World.Filter.With<StartGameEvent>().Build();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (_startGameEvent.IsNotEmpty())
            _startGameEvent.First().Dispose();
    }
}