using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private bool isFirstPerson = true;
    
    // 1인칭 시점에서는 PlayerController가 카메라를 제어하므로
    // 이 스크립트는 필요하지 않습니다.
    // 카메라는 Player의 자식 오브젝트로 배치하면 됩니다.

    void Start()
    {
        if (isFirstPerson)
        {
            // 1인칭 모드에서는 이 스크립트가 비활성화됩니다
            enabled = false;
        }
    }
}
