using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 70f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private Material[] colorMaterials;

    private BlockColor projectileColor;
    private Renderer projectileRenderer;
    private Rigidbody rb;

    void Awake()
    {
        projectileRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Initialize(BlockColor color, Vector3 direction)
    {
        projectileColor = color;
        ApplyMaterial();
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void ApplyMaterial()
    {
        if (projectileRenderer && colorMaterials != null && colorMaterials.Length > (int)projectileColor)
        {
            projectileRenderer.material = colorMaterials[(int)projectileColor];
        }
    }

    public BlockColor GetProjectileColor() => projectileColor;
}