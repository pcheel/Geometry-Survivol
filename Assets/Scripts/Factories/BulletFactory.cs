using UnityEngine;
using Zenject;

public class BulletFactory : MonoBehaviour
{
    private PrefabsConfig _config;

    [Inject]
    public void Construct(PrefabsConfig config)
    {
        _config = config;
    }
    public GameObject Create(BulletType type)
    {
        switch (type)
        {
            case BulletType.Default:
                return Instantiate(_config.defaultBulletPrefab);
            default:
                return null;
        }
    }
}
