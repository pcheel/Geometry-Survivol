using UnityEngine;
using Zenject;

public class PlayerFactory : MonoBehaviour
{
    private PrefabsConfig _config;

    [Inject]
    public void Construct(PrefabsConfig config)
    {
        _config = config;
    }
    public GameObject Create()
    {
        return Instantiate(_config.playerPrefab);
    }
}
