using UnityEngine;

public class NpcCarryVisual : MonoBehaviour
{
    [SerializeField] private GameObject carryVisualObject;
    private SpriteRenderer visualRenderer;

    private void Awake()
    {
        if (carryVisualObject != null)
            visualRenderer = carryVisualObject.GetComponent<SpriteRenderer>();
    }

    public void ShowBox(Box.BoxColor color)
    {
        if (carryVisualObject == null || visualRenderer == null) return;

        visualRenderer.color = color == Box.BoxColor.Red ? Color.red : Color.blue;
        carryVisualObject.SetActive(true);
    }

    public void HideBox()
    {
        if (carryVisualObject != null)
            carryVisualObject.SetActive(false);
    }
}
