using System.Collections.Generic;
using UnityEngine;

public interface IBoxManager
{
    public void RegisterBox(Box box);
    public void UnregisterBox(Box box);
    Box FindClosestUnclaimedBox(Vector2 position);
    public List<Box> GetAllBoxes();
}