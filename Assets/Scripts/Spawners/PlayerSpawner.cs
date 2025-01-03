using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlayerSpawner))]
public class PlayerSpawner : UpdateSystem 
{
    private Filter _startGameEvent;
    private PlayerFactory _playerFactory;

    [Inject]
    private void Construct(PlayerFactory playerFactory)
    {
        _playerFactory = playerFactory;
    }
    public override void OnAwake() 
    {
        _startGameEvent = World.Filter.With<StartGameEvent>().Build();
    }

    public override void OnUpdate(float deltaTime) 
    {
        if (_startGameEvent.IsEmpty())
            return;

        _playerFactory.Create();
    }
}