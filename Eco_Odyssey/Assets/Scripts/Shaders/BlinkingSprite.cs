using UnityEngine;

public class BlinkingSprite : MonoBehaviour
{
    [SerializeField] private float blinkDecaySpeed = 10f;

    private SpriteRenderer[] _spriteRenderers;
    private MaterialPropertyBlock _propertyBlock;
    private float _blinkFactor;

    private void Start()
    {
        _spriteRenderers = GetComponents<SpriteRenderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (_blinkFactor <= 0f)
        {
            return;
        }

        _blinkFactor = Mathf.Lerp(_blinkFactor, 0f, blinkDecaySpeed * Time.deltaTime);
        if (_blinkFactor <= 0f)
        {
            _blinkFactor = 0f;
        }

        ApplyBlinkFactor();
    }

    public void Blink()
    {
        _blinkFactor = 1f;
        ApplyBlinkFactor();
    }

    private void ApplyBlinkFactor()
    {
        foreach (var renderer in _spriteRenderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat("_BlinkFactor", _blinkFactor);
            renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
