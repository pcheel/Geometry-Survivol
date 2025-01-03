using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Zenject;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(Initialization))]
public class Initialization : UpdateSystem 
{
    private PlayerConfig _playerConfig;
    private EnemyConfigs _enemyConfigs;
    private BulletConfig _bulletConfig;

    private Filter _notInitializedPlayer;
    private Filter _notInitializedEnemies;
    private Filter _notInitializedBullets;
    private Filter _notInitializedBorders;
    private Filter _startGameEvent;

    private Stash<Score> _scoreStash;
    private Stash<Movement> _movementStash;
    private Stash<Attacking> _attackingStash;
    private Stash<Health> _healthStash;
    private Stash<DealingCollisionDamage> _dealingCollisionDamageStash;
    private Stash<IsInitializedMarker> _isInitializedStash;
    private Stash<EnemyTypeComponent> _enemyTypeStash;
    private Stash<TransformComponent> _transformStash;
    private Stash<GameObjectComponent> _gameObjectStash;

    private Transform _playerTransform;
    private Transform _cameraTransform;

    private Entity _playerEntity;

    private const float MIN_SPAWN_SHIFT_X = 10f;
    private const float MAX_SPAWN_SHIFT_X = 12f;
    private const float MIN_SPAWN_SHIFT_Y = 6f;
    private const float MAX_SPAWN_SHIFT_Y = 7f;

    [Inject]
    public void Construct(PlayerConfig playerConfig, EnemyConfigs enemyConfigs, BulletConfig bulletConfig)
    {
        _playerConfig = playerConfig;
        _enemyConfigs = enemyConfigs;
        _bulletConfig = bulletConfig;
    }
    public override void OnAwake() 
    {
        _notInitializedPlayer = World.Filter.With<IsInitializableMarker>().With<PlayerMarker>().Without<IsInitializedMarker>().Build();
        _notInitializedEnemies = World.Filter.With<IsInitializableMarker>().With<EnemyTypeComponent>().Without<IsInitializedMarker>().Build();
        _notInitializedBullets = World.Filter.With<IsInitializableMarker>().With<BulletMarker>().Without<IsInitializedMarker>().Build();
        _notInitializedBorders = World.Filter.With<IsInitializableMarker>().With<BorderMarker>().Without<IsInitializedMarker>().Build();
        _startGameEvent = World.Filter.With<StartGameEvent>().Build();
        _cameraTransform = World.Filter.With<CameraMarker>().Build().First().GetComponent<TransformComponent>().transform;

        _scoreStash = World.GetStash<Score>();
        _movementStash = World.GetStash<Movement>();
        _attackingStash = World.GetStash<Attacking>();
        _healthStash = World.GetStash<Health>();
        _dealingCollisionDamageStash = World.GetStash<DealingCollisionDamage>();
        _isInitializedStash = World.GetStash<IsInitializedMarker>();
        _enemyTypeStash = World.GetStash<EnemyTypeComponent>();
        _transformStash = World.GetStash<TransformComponent>();
        _gameObjectStash = World.GetStash<GameObjectComponent>();
    }
    public override void OnUpdate(float deltaTime) 
    {
        if (_startGameEvent.IsNotEmpty())
            _playerTransform = World.Filter.With<PlayerMarker>().Build().First().GetComponent<TransformComponent>().transform;

        InitializePlayer();
        InitializeBullets();
        InitializeEnemies();
        InitializeBorders();
    }

    private EnemyConfig GetEnemyConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Default:
                return _enemyConfigs.firstConfig;
            default:
                return null;
        }
    }
    private void InitializePlayer()
    {
        if (_notInitializedPlayer.IsEmpty())
            return;
        
        _playerEntity = _notInitializedPlayer.First();
        SetHealth(_playerEntity, _playerConfig.health);
        SetMovement(_playerEntity, _playerConfig.speed);
        SetAttacking(_playerEntity, _playerConfig.shotsDelay, _playerConfig.attackDamage);
        SetCollisionDamage(_playerEntity, _playerConfig.collisionDamage);
        _isInitializedStash.Add(_playerEntity);
    }
    private void InitializeEnemies()
    {
        foreach (var entity in _notInitializedEnemies)
        {
            EnemyConfig enemyConfig = GetEnemyConfig(_enemyTypeStash.Get(entity).enemyType);
            SetHealth(entity, enemyConfig.health);
            SetMovement(entity, enemyConfig.speed);
            SetCollisionDamage(entity, enemyConfig.collisionDamage);
            SetStartEnemyPosition(_transformStash.Get(entity).transform);
            _scoreStash.Add(entity, new Score { score = enemyConfig.score});
            _isInitializedStash.Add(entity);
        }
    }
    private void InitializeBullets()
    {
        foreach (var entity in _notInitializedBullets)
        {
            SetMovement(entity, _bulletConfig.bulletSpeed);
            SetCollisionDamage(entity, _attackingStash.Get(_playerEntity).damage);
            _transformStash.Get(entity).transform.position = _playerTransform.position;
            _isInitializedStash.Add(entity);
        }
    }
    private void InitializeBorders()
    {
        foreach (var entity in _notInitializedBorders)
        {
            _gameObjectStash.Get(entity).gameObject.GetComponent<CollisionDetector>().Initialize(entity);
            _isInitializedStash.Add(entity);
        }
    }
    private void SetStartEnemyPosition(Transform enemyTransform)
    {
        Vector3 newPosition = new Vector3();
        float xPosRight = Random.Range(_cameraTransform.position.x + MIN_SPAWN_SHIFT_X, _cameraTransform.position.x + MAX_SPAWN_SHIFT_X);
        float xPosLeft = Random.Range(_cameraTransform.position.x - MIN_SPAWN_SHIFT_X, _cameraTransform.position.x - MAX_SPAWN_SHIFT_X);
        newPosition.x = Random.value > 0.5f ? xPosRight : xPosLeft;
        float yPosUp = Random.Range(_cameraTransform.position.y + MIN_SPAWN_SHIFT_Y, _cameraTransform.position.y + MAX_SPAWN_SHIFT_Y);
        float yPosDown = Random.Range(_cameraTransform.position.y - MIN_SPAWN_SHIFT_Y, _cameraTransform.position.y - MAX_SPAWN_SHIFT_Y);
        newPosition.y = Random.value > 0.5f ? yPosUp : yPosDown;
        enemyTransform.localPosition = newPosition;
    }
    private void SetHealth(Entity entity, int health)
    {
        if (!_healthStash.Has(entity))
            return;

        _healthStash.Get(entity).health = health;
    }
    private void SetMovement(Entity entity, float speed)
    {
        if (!_movementStash.Has(entity))
            return;
            
        ref Movement movement = ref _movementStash.Get(entity);
        movement.rigidbody.gameObject.GetComponent<CollisionDetector>().Initialize(entity);
        movement.speed = speed;
    }
    private void SetAttacking(Entity entity, float shotsDelay, int damage)
    {
        if (!_attackingStash.Has(entity))
            return;

        ref Attacking attacking = ref _attackingStash.Get(entity);
        attacking.shotsDelay = shotsDelay;
        attacking.damage = damage;
    }
    private void SetCollisionDamage(Entity entity, int collisionDamage)
    {
        if (!_dealingCollisionDamageStash.Has(entity))
            return;

        ref DealingCollisionDamage dealingCollisionDamage = ref _dealingCollisionDamageStash.Get(entity);
        dealingCollisionDamage.damage = collisionDamage;
    }
}