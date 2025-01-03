using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectPool<T>
{
    private readonly Func<T, GameObject> _newObjectConstructor;
    private readonly T _type;

    private const int STARTING_NUMBERS = 5;

    public Dictionary<T, List<GameObject>> _members;

    public ObjectPool (Func <T, GameObject> newObjectConstructor, T type)
    {
        _newObjectConstructor = newObjectConstructor;
        _type = type;
    }
    public void CreatePool()
    {
        _members = new Dictionary<T, List<GameObject>>();
        _members.Add(_type, new List<GameObject>());
        for (int i = 0; i < STARTING_NUMBERS; i++)
        {
            GameObject newMember = CreateMember(_type);
            newMember.SetActive(false);
        }
    }
    public GameObject GetObject(T type)
    {
        foreach (var member in _members[type])
        {
            if (!member.activeSelf)
            {
                member.SetActive(true);
                return member;
            }
        }
        return CreateMember(type);
    }

    private GameObject CreateMember(T type)
    {
        GameObject newMember = _newObjectConstructor.Invoke(type);
        _members[type].Add(newMember);
        return newMember;
    }
}
