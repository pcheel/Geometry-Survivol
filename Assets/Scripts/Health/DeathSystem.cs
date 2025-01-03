using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DeathSystem))]
public class DeathSystem : UpdateSystem 
{
    private Filter _deads;

    private Stash<Score> _scoreStash;
    private Stash<TransformComponent> _transformStash;

    private ScorePresenter _scorePresenter;

    [Inject]
    public void Construct(ScorePresenter scorePresenter)
    {
        _scorePresenter = scorePresenter;
    }
    public override void OnAwake() 
    {
        _deads = World.Filter.With<IsDeadMarker>().Build();
        _scoreStash = World.GetStash<Score>();
        _transformStash = World.GetStash<TransformComponent>();
    }

    public override void OnUpdate(float deltaTime) 
    {
        foreach(var entity in _deads)
        {
            entity.RemoveComponent<IsDeadMarker>();
            entity.RemoveComponent<IsInitializedMarker>();
            _transformStash.Get(entity).transform.position = new Vector3(20f, 20f);
            _transformStash.Get(entity).transform.gameObject.SetActive(false);
            if (_scoreStash.Has(entity))
            {
                _scorePresenter.AddScore(_scoreStash.Get(entity).score);
                entity.RemoveComponent<Score>();
            }
        }
    }
}