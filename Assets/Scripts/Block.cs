using UnityEngine;

// Enum을 클래스 밖으로 빼서 전역적으로 사용 가능하게 합니다.
public enum BlockColor { Red, Blue, Green, Yellow, Purple }

public class Block : MonoBehaviour
{
    [SerializeField] private BlockColor blockColor;
    [SerializeField] private Material[] colorMaterials;
    [SerializeField] private float destroyDelay = 0.1f;
    [SerializeField] private GameObject destroyEffectPrefab;

    private Renderer blockRenderer;

    void Awake()
    {
        blockRenderer = GetComponent<Renderer>();
        // 블록이 물리 엔진에 의해 밀려나지 않도록 설정
        if (TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;
    }

    void Start() => UpdateVisuals();

    public BlockColor GetBlockColor() => blockColor;

    private void UpdateVisuals()
    {
        if (blockRenderer && colorMaterials != null && colorMaterials.Length > (int)blockColor)
        {
            blockRenderer.material = colorMaterials[(int)blockColor];
        }
    }

    // Block.cs 클래스 내부에 추가
    public void SetBlockColor(BlockColor color)
    {
        this.blockColor = color;
        UpdateVisuals(); // 색상이 바뀌었으므로 머티리얼 갱신
    }
    public void DestroyBlock()
    {
        if (destroyEffectPrefab) Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        // GameManager.Instance?.AddScore(10); // GameManager가 있다면 주석 해제
        Destroy(gameObject, destroyDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            if (projectile.GetProjectileColor() == blockColor)
            {
                DestroyBlock();
            }
        }
    }
}