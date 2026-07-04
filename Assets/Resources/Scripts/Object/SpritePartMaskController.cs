using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public sealed class SpritePartMaskController : MonoBehaviour
{
    public enum MaskMode
    {
        ShowOnlyInsideMask,
        HideInsideMask
    }

    [SerializeField] private Transform targetRoot;
    [SerializeField] private List<SpriteRenderer> targetRenderers = new List<SpriteRenderer>();
    [SerializeField] private Sprite maskSprite;
    [SerializeField] private MaskMode maskMode = MaskMode.ShowOnlyInsideMask;
    [SerializeField, Range(0f, 1f)] private float alphaCutoff = 0.1f;
    [SerializeField] private Vector3 maskLocalPosition;
    [SerializeField] private Vector3 maskLocalScale = Vector3.one;
    [SerializeField] private bool collectChildrenOnApply = true;
    [SerializeField] private bool restoreMaskInteractionOnDisable = true;

    private const string GeneratedMaskName = "__sprite_part_mask";
    private readonly Dictionary<SpriteRenderer, SpriteMaskInteraction> originalInteractions = new Dictionary<SpriteRenderer, SpriteMaskInteraction>();
    private SpriteMask spriteMask;

    private void Reset()
    {
        targetRoot = transform;
        CollectChildRenderers();
    }

    private void OnEnable()
    {
        Apply();
    }

    private void OnValidate()
    {
        Apply();
    }

    private void OnDisable()
    {
        if (!restoreMaskInteractionOnDisable)
        {
            return;
        }

        foreach (KeyValuePair<SpriteRenderer, SpriteMaskInteraction> pair in originalInteractions)
        {
            if (pair.Key != null)
            {
                pair.Key.maskInteraction = pair.Value;
            }
        }
    }

    [ContextMenu("Apply Mask")]
    public void Apply()
    {
        if (collectChildrenOnApply)
        {
            CollectChildRenderers();
        }

        EnsureMask();
        ConfigureMask();
        ApplyRendererMaskMode();
    }

    [ContextMenu("Collect Child Renderers")]
    public void CollectChildRenderers()
    {
        Transform root = targetRoot != null ? targetRoot : transform;
        targetRenderers.Clear();
        root.GetComponentsInChildren(true, targetRenderers);
    }

    [ContextMenu("Clear Mask")]
    public void ClearMask()
    {
        foreach (SpriteRenderer targetRenderer in targetRenderers)
        {
            if (targetRenderer != null)
            {
                targetRenderer.maskInteraction = SpriteMaskInteraction.None;
            }
        }
    }

    private void EnsureMask()
    {
        if (spriteMask != null)
        {
            return;
        }

        Transform existingMask = transform.Find(GeneratedMaskName);
        GameObject maskObject = existingMask != null ? existingMask.gameObject : new GameObject(GeneratedMaskName);
        maskObject.transform.SetParent(transform, false);
        spriteMask = maskObject.GetComponent<SpriteMask>();
        if (spriteMask == null)
        {
            spriteMask = maskObject.AddComponent<SpriteMask>();
        }
    }

    private void ConfigureMask()
    {
        spriteMask.sprite = maskSprite;
        spriteMask.alphaCutoff = alphaCutoff;
        spriteMask.transform.localPosition = maskLocalPosition;
        spriteMask.transform.localRotation = Quaternion.identity;
        spriteMask.transform.localScale = maskLocalScale;

        if (targetRenderers.Count == 0)
        {
            spriteMask.isCustomRangeActive = false;
            return;
        }

        int minOrder = int.MaxValue;
        int maxOrder = int.MinValue;
        int sortingLayerId = targetRenderers[0] != null ? targetRenderers[0].sortingLayerID : 0;

        foreach (SpriteRenderer targetRenderer in targetRenderers)
        {
            if (targetRenderer == null)
            {
                continue;
            }

            sortingLayerId = targetRenderer.sortingLayerID;
            minOrder = Mathf.Min(minOrder, targetRenderer.sortingOrder);
            maxOrder = Mathf.Max(maxOrder, targetRenderer.sortingOrder);
        }

        if (minOrder == int.MaxValue)
        {
            spriteMask.isCustomRangeActive = false;
            return;
        }

        spriteMask.isCustomRangeActive = true;
        spriteMask.frontSortingLayerID = sortingLayerId;
        spriteMask.frontSortingOrder = maxOrder + 1;
        spriteMask.backSortingLayerID = sortingLayerId;
        spriteMask.backSortingOrder = minOrder - 1;
    }

    private void ApplyRendererMaskMode()
    {
        SpriteMaskInteraction interaction = maskMode == MaskMode.ShowOnlyInsideMask
            ? SpriteMaskInteraction.VisibleInsideMask
            : SpriteMaskInteraction.VisibleOutsideMask;

        foreach (SpriteRenderer targetRenderer in targetRenderers)
        {
            if (targetRenderer == null || targetRenderer.GetComponent<SpriteMask>() != null)
            {
                continue;
            }

            if (!originalInteractions.ContainsKey(targetRenderer))
            {
                originalInteractions.Add(targetRenderer, targetRenderer.maskInteraction);
            }

            targetRenderer.maskInteraction = interaction;
        }
    }
}
