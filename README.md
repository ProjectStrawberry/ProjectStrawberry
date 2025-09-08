# Doki Doki Ducky

<img width="440" height="590" alt="image" src="https://github.com/user-attachments/assets/5c7f275b-1de4-487a-9456-6656baec2d54" />

팀 스파르타 내일배움캠프 유니티 11기

팀 스트로베리 2038 (개발 팀: 20조 / 기획 팀: 38조)

장르: 2D 액션 플랫포머

제작 툴: Unity 2022.3.17f1, Visual Studio, Rider

제작 기간: 25/09/01 ~ 25/09/05

| 팀원 | 역할 |
| ---------- | ---------------------------------------------- |
| 주용진 | 일반 몬스터, 보스 몬스터 구현 |
| 김나경 | 플랫폼 및 생성로직, UI 구현 |
| 조영종 | 플레이어 및 카메라 구현 |
| 송성현 | 시스템 전반, 플레이어, 몬스터 및 전투 기획 |
| 김정민 | 사운드 디자인 및 레벨 디자인 |

<br />

# 목차

1. 시스템 & 컨텐츠 기획
2. 핵심 재미 요소
3. 핵심적인 로직 및 기능

<br />

# 시스템 & 컨텐츠 기획

**플레이어 조작법**
| KeyBoard    | Description                                    |
| ---------- | ---------------------------------------------- |
| WASD | 상하좌우 이동                |
| C | 점프 및 2단 점프 |
| X | 근접 공격 |
| Z | 원거리 공격 |
| V | 체력 회복 |

<br />

**레벨 디자인**
<img width="1000" height="700" alt="image" src="https://github.com/user-attachments/assets/ffc47058-38ef-4294-ae22-291e33ee1b26" />


# 핵심 재미 요소

### 레퍼런스 게임
**Hollow Knight - 탐험 및 카메라 레퍼런스**
![kamera-idong](https://github.com/user-attachments/assets/c3fb4fd8-17ef-4be5-ba1b-56a552f27be7)

<br />

**Blasphemous - 보스 및 맵 패턴**

![MaLNIc](https://github.com/user-attachments/assets/6d6789d1-f685-4bf1-9a1b-9cb2a5987a2e)

### 탐험 확장성
1단 점프로는 닿지 못할 높이의 플랫폼을 2단 점프로 쉽게 넘어버리며 탐험을 누릴 수 있는 영역이 확장된다.
![2025-09-07-14-50-29](https://github.com/user-attachments/assets/d0f1fa7c-bd9c-4f9a-a1b5-8824d30f0096)

### 단순하고 쉬운 일반 몬스터와의 전투
단순한 공격패턴을 가지고 있는 2종류의 일반 몬스터를 배치하여, 플레이어는 쉽게 전투를 익히며 전진하는 쾌감을 느낄 수 있습니다.

![Skeleton_Attack](https://github.com/user-attachments/assets/fde93c92-9fa8-412c-a702-f68f45b8484f)
<img width="220" height="200" alt="image (1)" src="https://github.com/user-attachments/assets/25efc3d9-c0d3-408a-a68d-603952364989" />

### 다양한 패턴의 보스
단순한 몬스터와 달리 다양한 패턴이 있는 보스전으로 도전 의식을 유발해 긴장감과 재미를 제공합니다.
![Animation2](https://github.com/user-attachments/assets/d0581176-a8be-4666-8920-0451513e4978)
![Animation3](https://github.com/user-attachments/assets/05e90957-8641-4001-9533-4e7d3928f6ff)

### 난이도별 차이점
2가지 난이도 구성으로 사용자에게 클리어 성취감, 도전 정신, 보스 전투의 집중도와 긴장감을 유도합니다.
<img width="400" height="450" alt="image" src="https://github.com/user-attachments/assets/fd8d24d3-8d3d-4af6-a66a-efff6e1633a5" />

# 핵심적인 로직 및 기능
### 플레이어 기능, 동작 및 전투 시스템 구현
+ 인풋 시스템 - Input Unity Events로 플레이어 조작 구현
+ 수치적으로 표현 가능한 플레리어 데이터들을 Scriptable Object로 일괄 관리
+ 애니메이션 플래그 처리와 즉각 플레이를 혼합하여 애니메이팅 처리
+ 애니메이션 이벤트 시스템을 통해 기능별 효과음 적용
![Animation](https://github.com/user-attachments/assets/85f9df7d-87d8-4688-8082-2273b7feca2d)

### 몬스터 및 보스 몬스터 FSM 구성
+ 몬스터 및 보스 몬스터에게 FSM과 각각의 상태들을 정의하고 할당
+ IState 인터페이스를 기반으로 한 StateMachine 추상 클래스, StateMachine 추상 클래스를 상속받는 몬스터들의 StateMachine 클래스
+ 몬스터들은 현재 상황에 따라 알맞은 상태로 변경하면서 행동 메소드들을 실행
<img width="1000" height="600" alt="ray-so-export (1)" src="https://github.com/user-attachments/assets/c5b65e06-463f-4fed-beb8-1a9d060c3568" />

<br />

### 몬스터 공격 애니메이션
+ 몬스터의 공격 애니메이션과 공격 메서드의 타이밍을 정확히 맞춰야 하는 문제 발생
+ 애니메이션 이벤트를 통해 공격 애니메이션과 공격 메서드의 타이밍을 동기화
<img width="535" height="378" alt="image" src="https://github.com/user-attachments/assets/41a85f52-4ffa-4fa5-ade9-d4e0a1ba27de" />

<br />

### 플랫폼 패턴 기반 무한 생성 밑 하강
+ 베이스 무버로 이동을 구현함으로써 확장성 높은 기능으로 구현
+ 플랫폼 패턴은 Scriptable Object로 데이터 정의, 새로운 패턴을 쉽게 생성 및 확장 가능
+ 오브젝트 풀리을 적용하여 메모리 최적화
<img width="1000" height="600" alt="image" src="https://github.com/user-attachments/assets/b872ad25-6aa0-44de-bf85-449691042d99" />
