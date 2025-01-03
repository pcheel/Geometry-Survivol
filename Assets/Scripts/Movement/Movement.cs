using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using TriInspector;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct Movement : IComponent 
{
    public Rigidbody2D rigidbody;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float speed;
}