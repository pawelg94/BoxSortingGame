using UnityEngine;

public class Box : MonoBehaviour
{
    public enum BoxColor { Red, Blue }
    public BoxColor boxColor;

    public BoxNodeReference boxNodeReference;
    public BoxDropBehaviour dropBehaviour;

    [HideInInspector] public bool IsTaken;

    private void OnEnable()
    {
        GameManager.Instance.BoxManager.RegisterBox(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.BoxManager.UnregisterBox(this);
    }

    public void Initialize(BoxColor color)
    {
        boxColor = color;
        GetComponent<SpriteRenderer>().color = (color == BoxColor.Red) ? Color.red : Color.blue;
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
