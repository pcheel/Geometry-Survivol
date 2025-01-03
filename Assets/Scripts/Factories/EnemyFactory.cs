using UnityEngine;
using Zenject;
using System;

public class EnemyFactory : MonoBehaviour
{
    private PrefabsConfig _config;

    [Inject]
    public void Construct(PrefabsConfig config)
    {
        _config = config;
    }
    public GameObject Create(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Default:
                return Instantiate(_config.defaultEnemyPrefab);
            default:
                return null;
        }
    }
}
