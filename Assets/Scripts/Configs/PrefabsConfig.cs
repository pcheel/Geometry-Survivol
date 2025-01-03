using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsConfig", menuName = "Scriptable Objects/PrefabsConfig")]
public class PrefabsConfig : ScriptableObject
{
    public GameObject playerPrefab;
    public GameObject defaultEnemyPrefab;
    public GameObject defaultBulletPrefab;
}
