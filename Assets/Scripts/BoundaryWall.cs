using UnityEngine;

public class BoundaryWall : MonoBehaviour
{
    [SerializeField] private bool isTransparent = true;
    [SerializeField] private Material glassMaterial;
    [SerializeField] private float transparency = 0.3f;

    private Renderer wallRenderer;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        
        if (isTransparent && glassMaterial != null)
        {
            wallRenderer.material = glassMaterial;
            
            // Set transparency
            Color color = wallRenderer.material.color;
            color.a = transparency;
            wallRenderer.material.color = color;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Projectiles and blocks will bounce off naturally due to physics
        // This script mainly handles visual appearance
        
        // Optional: Add visual feedback when hit
        // For example, flash the wall briefly
    }
}
