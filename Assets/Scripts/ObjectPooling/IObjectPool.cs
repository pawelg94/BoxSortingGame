using UnityEngine;

public interface IObjectPool
{
    public Box Spawn(Box.BoxColor color, Vector2 position);
    void Despawn(Box box);
}
