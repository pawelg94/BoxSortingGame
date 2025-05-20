using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour, IBoxManager
{
    public List<Box> activeBoxes = new List<Box>();

    public void RegisterBox(Box box)
    {
        if (!activeBoxes.Contains(box))
            activeBoxes.Add(box);
    }

    public void UnregisterBox(Box box)
    {
        activeBoxes.Remove(box);
    }

    public Box FindClosestUnclaimedBox(Vector2 position)
    {
        Box closest = null;
        float minDistance = float.MaxValue;

        foreach (var box in activeBoxes)
        {
            if (box == null)
                continue;

            float dist = Vector2.Distance(position, box.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = box;
            }
        }
        return closest;
    }

    public List<Box> GetAllBoxes()
    {
        return activeBoxes;
    }
    public void Clear()
    {
        activeBoxes.Clear();
    }
}
