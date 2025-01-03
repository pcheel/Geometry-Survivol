using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementDirectionController))]
public class MovementDirectionController : UpdateSystem 
{
    private GameInput _gameInput;
    private Rigidbody2D _playerRigidbody;

    private Filter _toPlayerMovements;
    private Filter _playerMovement;
    private Filter _toPointMovement;
    private Filter _startGameEvent;

    private Stash<Movement> _movementStash;
    private Stash<ToPointMovement> _toPointMovementStash;

    public override void OnAwake() 
    {
        _gameInput = new GameInput();
        _gameInput.Enable();
        _playerMovement = World.Filter.With<PlayerMarker>().Build();
        _toPlayerMovements = World.Filter.With<ToPlayerMovementMarker>().Build();
        _toPointMovement = World.Filter.With<ToPointMovement>().Build();
        _startGameEvent = World.Filter.With<StartGameEvent>().Build();

        _movementStash = World.GetStash<Movement>();  
        _toPointMovementStash = World.GetStash<ToPointMovement>();
    }
    public override void OnUpdate(float deltaTime) 
    {
        if (_startGameEvent.IsNotEmpty())
        {
            _playerRigidbody = _movementStash.Get(_playerMovement.First()).rigidbody;
        }

        if (_playerMovement.IsEmpty())
            return;

        CalculatePlayerDirection();
        CalculateDirectionToPlayer();
        CalculateDirectionToPoint();
    }

    private void CalculatePlayerDirection()
    {
        _movementStash.Get(_playerMovement.First()).direction = _gameInput.Gameplay.Movement.ReadValue<Vector2>();
    }
    private void CalculateDirectionToPlayer()
    {
        foreach (var entity in _toPlayerMovements)
        {
            ref Movement toPlayerMovement = ref _movementStash.Get(entity);
            toPlayerMovement.direction = (_playerRigidbody.position - toPlayerMovement.rigidbody.position).normalized;
        }
    }
    private void CalculateDirectionToPoint()
    {
        foreach (var entity in _toPointMovement)
        {
            ref ToPointMovement toPointMovement = ref _toPointMovementStash.Get(entity);
            if (toPointMovement.isInitialized)
                continue;

            ref Movement movement = ref _movementStash.Get(entity);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(_gameInput.Gameplay.MousePosition.ReadValue<Vector2>());
            movement.direction = (mousePosition - _playerRigidbody.position).normalized;
            toPointMovement.isInitialized = true;
        }
    }
}