using UnityEngine;
using Zenject;
using Scellecs.Morpeh;

public class EntryPoint : MonoBehaviour
{
    private ObjectPool<EnemyType> _enemyPool;
    private ObjectPool<BulletType> _bulletPool;

    [Inject]
    private void Construct(ObjectPool<EnemyType> enemyPool, ObjectPool<BulletType> bulletPool)
    {
        _enemyPool = enemyPool;
        _bulletPool = bulletPool;
    }
    private void Start()
    {
        _enemyPool.CreatePool();
        _bulletPool.CreatePool();
        StartGame();
    }
    private void StartGame()
    {
        Entity startGameEntity = World.Default.CreateEntity();
        startGameEntity.AddComponent<StartGameEvent>();
    }
}
