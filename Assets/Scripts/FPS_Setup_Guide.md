# 1인칭 FPS 스타일 게임 설정 가이드

## Unity 씬 구성

### 1. Player 오브젝트 생성
1. Hierarchy → Create Empty → 이름: "Player"
2. Position: (0, 1, -15) - 바운더리 월 밖에 배치
3. Add Component → Character Controller
4. Add Component → Player Controller (스크립트)

### 2. Main Camera 설정
1. Main Camera를 Player의 **자식 오브젝트**로 드래그
2. Camera의 Local Position: (0, 0.6, 0) - 눈 높이
3. Camera의 Local Rotation: (0, 0, 0)

### 3. Shoot Point 생성
1. Player 선택 → 우클릭 → Create Empty
2. 이름: "ShootPoint"
3. Local Position: (0, 0.6, 0.5) - 카메라 앞쪽

### 4. Color Indicator 생성 (선택사항)
1. Main Camera의 자식으로 Create → 3D Object → Cube
2. 이름: "ColorIndicator"
3. Local Position: (0.5, -0.3, 1) - 화면 오른쪽 하단
4. Local Scale: (0.1, 0.1, 0.1)
5. Add Component → Mesh Renderer

### 5. PlayerController Inspector 설정
- **Move Speed**: 5
- **Mouse Sensitivity**: 2
- **Player Camera**: Main Camera 드래그
- **Projectile Prefab**: Projectile Prefab 할당
- **Shoot Point**: ShootPoint 드래그
- **Shoot Cooldown**: 0.5
- **Color Materials**: 5개 색상 Material 배열
- **Color Indicator Renderer**: ColorIndicator의 Mesh Renderer 드래그

### 6. 바운더리 월 배치
블록들을 감싸는 투명한 유리 벽 4개 생성:
- 앞벽: Position (0, 5, 10), Scale (20, 10, 0.1)
- 뒷벽: Position (0, 5, -10), Scale (20, 10, 0.1)
- 왼벽: Position (-10, 5, 0), Scale (0.1, 10, 20)
- 오른벽: Position (10, 5, 0), Scale (0.1, 10, 20)

각 벽에:
- Add Component → Box Collider
- Add Component → Boundary Wall (스크립트)

## 조작법

### 이동
- **W**: 전진
- **S**: 후진
- **A**: 왼쪽 이동
- **D**: 오른쪽 이동

### 시점
- **마우스 이동**: 시점 회전

### 발사
- **마우스 좌클릭**: 발사

### 색상 변경
- **1-5 숫자키**: 특정 색상 선택
- **Q/E**: 색상 순환
- **마우스 휠**: 색상 순환

### 기타
- **ESC**: 마우스 커서 해제

## 게임 구조
- 플레이어는 바운더리 월 **밖**에서 자유롭게 이동
- 1인칭 시점으로 블록들을 조준
- 카메라가 보는 방향으로 발사체 발사
- 발사체 색상과 블록 색상이 일치하면 파괴
