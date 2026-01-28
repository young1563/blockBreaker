using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private Camera playerCamera; // Transform 대신 Camera 타입 사용

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown = 0.3f;

    [Header("Visuals")]
    [SerializeField] private Material[] colorMaterials;
    [SerializeField] private Renderer colorIndicator;

    private BlockColor currentColor = BlockColor.Red;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 mouseDelta;
    private float verticalRotation = 0f;
    private float lastShootTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdatePlayerColor();
    }

    void Update()
    {
        HandleInputs();
        HandleLook();
        if (Mouse.current.leftButton.wasPressedThisFrame) TryShoot();
    }

    void FixedUpdate() => HandleMovement();

    private void HandleInputs()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        var kb = Keyboard.current;
        float x = (kb.dKey.isPressed ? 1 : 0) - (kb.aKey.isPressed ? 1 : 0);
        float y = (kb.wKey.isPressed ? 1 : 0) - (kb.sKey.isPressed ? 1 : 0);
        moveInput = new Vector2(x, y);
        mouseDelta = Mouse.current.delta.ReadValue();

        /*// 숫자키 1~5 색상 전환
        for (int i = 0; i < 5; i++)
            if (kb[$"{i + 1}Key"].wasPressedThisFrame) SetColor((BlockColor)i);*/
    }

    private void HandleMovement()
    {
        Vector3 dir = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        rb.linearVelocity = new Vector3(dir.x * moveSpeed, rb.linearVelocity.y, dir.z * moveSpeed);
    }

    private void HandleLook()
    {
        if (!playerCamera) return;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
        verticalRotation = Mathf.Clamp(verticalRotation - (mouseDelta.y * mouseSensitivity), -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void TryShoot()
    {
        if (Time.time < lastShootTime + shootCooldown) return;
        if (!projectilePrefab || !shootPoint || !playerCamera) return;

        // 화면 중앙 레이캐스트
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 100f) ? hit.point : ray.GetPoint(100f);
        Vector3 fireDir = (targetPoint - shootPoint.position).normalized;

        var projObj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(fireDir));
        if (projObj.TryGetComponent<Projectile>(out var proj))
        {
            proj.Initialize(currentColor, fireDir);
        }
        lastShootTime = Time.time;
    }

    private void SetColor(BlockColor color) { currentColor = color; UpdatePlayerColor(); }

    private void UpdatePlayerColor()
    {
        if (colorIndicator && colorMaterials.Length > (int)currentColor)
            colorIndicator.material = colorMaterials[(int)currentColor];
    }
}