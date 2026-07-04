using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public sealed class SpriteOutlineMask : MonoBehaviour
{
    private static readonly int OutlineMaskId = Shader.PropertyToID("_OutlineMask");
    private static readonly int UseOutlineMaskId = Shader.PropertyToID("_UseOutlineMask");
    private static readonly int OutlineMaskCutoffId = Shader.PropertyToID("_OutlineMaskCutoff");

    [SerializeField] private Texture2D outlineMask;
    [SerializeField, Range(0f, 1f)] private float maskCutoff = 0.5f;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;

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
        if (TryGetRenderer())
        {
            spriteRenderer.GetPropertyBlock(GetPropertyBlock());
            propertyBlock.SetFloat(UseOutlineMaskId, 0f);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    private void Apply()
    {
        if (!TryGetRenderer())
        {
            return;
        }

        spriteRenderer.GetPropertyBlock(GetPropertyBlock());

        if (outlineMask != null)
        {
            propertyBlock.SetTexture(OutlineMaskId, outlineMask);
            propertyBlock.SetFloat(UseOutlineMaskId, 1f);
            propertyBlock.SetFloat(OutlineMaskCutoffId, maskCutoff);
        }
        else
        {
            propertyBlock.SetFloat(UseOutlineMaskId, 0f);
        }

        spriteRenderer.SetPropertyBlock(propertyBlock);
    }

    private bool TryGetRenderer()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        return spriteRenderer != null;
    }

    private MaterialPropertyBlock GetPropertyBlock()
    {
        propertyBlock ??= new MaterialPropertyBlock();
        return propertyBlock;
    }
}
