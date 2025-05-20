using UnityEngine;
using DG.Tweening;

public class BoxDropBehaviour : MonoBehaviour
{
    [Header("Drop Settings")]
    public float dropHeight = 1.5f;
    public float dropDuration = 0.5f;

    [Header("Scale Settings")]
    public float startScale = 1.3f;
    public float endScale = 1.0f;

    [Header("Landing Animation")]
    public bool enableLandingAnimation = true;
    public float squashScaleY = 0.9f;
    public float squashDuration = 0.1f;

    private Vector3 groundPosition;

    public void DropTo(Vector3 targetPosition)
    {
        groundPosition = targetPosition;

        transform.position = groundPosition + new Vector3(0f, dropHeight, 0f);
        transform.localScale = Vector3.one * startScale;

        Sequence dropSequence = DOTween.Sequence();
        dropSequence.Append(transform.DOMove(groundPosition, dropDuration).SetEase(Ease.OutQuad));
        dropSequence.Join(transform.DOScale(endScale, dropDuration).SetEase(Ease.OutQuad));

        dropSequence.OnComplete(() =>
        {
            if (enableLandingAnimation)
            {
                Sequence squash = DOTween.Sequence();
                squash.Append(transform.DOScale(new Vector3(1.1f, squashScaleY, 1f), squashDuration).SetEase(Ease.OutQuad));
                squash.Append(transform.DOScale(Vector3.one * endScale, squashDuration).SetEase(Ease.OutQuad));
            }
        });
    }

}
