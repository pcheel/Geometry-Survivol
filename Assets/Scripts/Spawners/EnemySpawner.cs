using Scellecs.Morpeh.Systems;
using UnityEngine;
using Zenject;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EnemySpawner))]
public class EnemySpawner : UpdateSystem 
{
    private bool _isAllowdToSpawn;
    private float _timeAfterLastSpawn;
    private ObjectPool<EnemyType> _enemyPool;
    private Transform _cameraTransform;
    private Filter _startGameEvent;

    private const float SPAWN_TIME = 5f;

    [Inject]
    private void Construct(ObjectPool<EnemyType> enemyPool)
    {
        _enemyPool = enemyPool;
    }

    public override void OnAwake() 
    {
        _cameraTransform = World.Filter.With<CameraMarker>().With<TransformComponent>().Build().First().GetComponent<TransformComponent>().transform;
        _startGameEvent = World.Filter.With<StartGameEvent>().Build();
    }

    public override void OnUpdate(float deltaTime) 
    {
        if (_startGameEvent.IsNotEmpty())
        {
            _isAllowdToSpawn = true;
        }

        if (!_isAllowdToSpawn)
            return;

        _timeAfterLastSpawn += deltaTime;
        if (_timeAfterLastSpawn < SPAWN_TIME)
            return;

        _timeAfterLastSpawn = 0f;
        _enemyPool.GetObject(EnemyType.Default);
    }
    }