using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public int health;
    public float speed;
    public float shotsDelay;
    public int collisionDamage;
    public int attackDamage;
}
