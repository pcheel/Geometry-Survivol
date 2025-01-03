using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlayerInput))]
public class PlayerInput : UpdateSystem 
{
    private GameInput _gameInput;
    private Filter _player;
    private Stash<Movement> _movementStash;
    public override void OnAwake() 
    {
        _gameInput = new GameInput();
        _gameInput.Enable();
        _player = World.Filter.With<PlayerMarker>().Build();
        _movementStash = World.GetStash<Movement>();
    }

    public override void OnUpdate(float deltaTime) 
    {
        foreach(var entity in _player)
        {
            _movementStash.Get(entity).direction = _gameInput.Gameplay.Movement.ReadValue<Vector2>();
        }
    }
}