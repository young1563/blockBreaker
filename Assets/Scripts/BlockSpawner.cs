using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private int gridDepth = 10;
    [SerializeField] private float blockSize = 1f;
    [SerializeField] private float blockSpacing = 0.05f; // 약간의 간격을 기본값으로 추천

    [Header("Color Distribution")]
    [SerializeField] private bool randomColors = true;

    void Start()
    {
        SpawnBlocksInGrid();
    }

    void SpawnBlocksInGrid()
    {
        if (blockPrefab == null)
        {
            Debug.LogError("Block prefab not assigned!");
            return;
        }

        int spawnedCount = 0;
        float spacing = blockSize + blockSpacing;

        // 그리드 중앙 정렬을 위한 시작 위치 계산
        Vector3 startPosition = transform.position - new Vector3(
            (gridWidth - 1) * spacing / 2f,
            (gridHeight - 1) * spacing / 2f,
            (gridDepth - 1) * spacing / 2f
        );

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    Vector3 position = startPosition + new Vector3(x * spacing, y * spacing, z * spacing);

                    // 1. 블록 생성 및 부모 설정
                    GameObject blockObj = Instantiate(blockPrefab, position, Quaternion.identity);
                    blockObj.transform.SetParent(this.transform);

                    // 2. Block 컴포넌트 접근 및 색상 설정
                    if (blockObj.TryGetComponent<Block>(out Block block))
                    {
                        if (randomColors)
                        {
                            // 전역 Enum인 BlockColor를 직접 사용합니다.
                            int colorCount = System.Enum.GetValues(typeof(BlockColor)).Length;
                            BlockColor randomColor = (BlockColor)Random.Range(0, colorCount);

                            // [수정 포인트] 이제 에러 없이 작동합니다.
                            // Block.cs에 public void SetBlockColor(BlockColor color) 메서드가 있어야 합니다.
                            block.SetBlockColor(randomColor);
                        }
                    }

                    spawnedCount++;
                }
            }
        }
        Debug.Log($"<color=cyan>Spawned {spawnedCount} blocks in grid.</color>");
    }

    // 에디터 뷰에서 생성 영역 미리보기
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        float spacing = blockSize + blockSpacing;
        Vector3 gridSize = new Vector3(gridWidth * spacing, gridHeight * spacing, gridDepth * spacing);
        Gizmos.DrawWireCube(transform.position, gridSize);
    }
}