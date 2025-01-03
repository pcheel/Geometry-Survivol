using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyConfigs", menuName = "Scriptable Objects/EnemyConfigs")]
public class EnemyConfigs : ScriptableObject
{
    public EnemyConfig firstConfig;
}
