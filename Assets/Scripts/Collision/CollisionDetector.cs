using UnityEngine;
using Scellecs.Morpeh;

public class CollisionDetector : MonoBehaviour
{
    private Entity _listner;
    private Stash<CollisionEvent> _eventStash;

    public Entity listner => _listner;

    public void Initialize(Entity listner)
    {
        _listner = listner;
        _eventStash = World.Default.GetStash<CollisionEvent>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionDetector otherCollisionDetector = collision.gameObject.GetComponent<CollisionDetector>();
        if (otherCollisionDetector == null)
            return;

        Entity collisionEventEntity = World.Default.CreateEntity();
        ref CollisionEvent collisionEvent = ref _eventStash.Add(collisionEventEntity);
        collisionEvent.first = _listner;
        collisionEvent.second = otherCollisionDetector.listner;
    }
}
