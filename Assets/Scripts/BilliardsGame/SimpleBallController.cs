using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBallController : MonoBehaviour
{
    [Header("기본 설정")]
    public float power = 10f; // 공을 칠 때의 힘
    public Sprite arrowSprite; // 화살표 스프라이트

    private Rigidbody rb;
    private GameObject arrow; // 화살표 오브젝트
    private bool isDragging = false; // 드래그 중인지 여부
    private Vector3 startPos;

    void SetupBall()                                //공 설정 하기
    {
        rb = GetComponent<Rigidbody>();             //물리 컴포넌트 가져오기
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>(); // Rigidbody가 없으면 추가
        }

        //물리 설정
        rb.mass = 1; // 일반적인 당구공의 질량
        rb.drag = 1; // 공의 마찰력
    }

    public bool IsMoving()
    {
        return rb.velocity.magnitude > 0.2f; // 공이 움직이고 있는지 확인
    }

    void HandleInput()
    {
        if (!SimpleTurnManager.canPlay) return; // 플레이 가능 여부 확인
        if (SimpleTurnManager.anyBallMoveing) return; // 다른 공이 움직이고 있으면 리턴

        if (IsMoving()) return;         //공이 움직이고 있으면 조작 불가

        if (Input.GetMouseButtonDown(0))     //마우스 클릭 시작
        {
            StartDrag(); // 드래그 시작
        }
        if (Input.GetMouseButtonUp(0) && isDragging)     //드레그 중이었는데 마우스 버튼 업 했을 때
        {
            Shoot(); // 공 발사
        }
    }

    void Shoot()
    {
        Vector3 mouseDelta = Input.mousePosition - startPos; // 마우스 이동 거리
        float force = mouseDelta.magnitude * 0.01f * power; // 힘 계산

        if(force < 5) force = 5;

        Vector3 direction = new Vector3(-mouseDelta.x, 0, - mouseDelta.y).normalized; // 방향 계산

        rb.AddForce(direction * force, ForceMode.Impulse); // 힘 적용

        SimpleTurnManager.OnBallHit(); // 턴 매니저에 공이 칠 때 알림

        isDragging = false; // 드래그 상태 해제
        Destroy(arrow); // 화살표 제거
        arrow = null; // 화살표 오브젝트 초기화

        Debug.Log("발사! 힘: " + force);
    }

    void CreateArrow()
    {
        if(arrow != null)
        {
            Destroy(arrow); // 기존 화살표 제거
        }

        arrow = new GameObject("Arrow"); // 새 화살표 오브젝트 생성
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>(); // 스프라이트 렌더러 추가

        sr.sprite = arrowSprite; // 화살표 스프라이트 설정
        sr.color = Color.green; // 화살표 색상 설정
        sr.sortingOrder = 10; // 정렬 순서 설정

        arrow.transform.position = transform.position + Vector3.up; // 공 위치에 화살표 위치 설정
        arrow.transform.localScale = Vector3.one;
    }

    void UpdateArrow()                  //화살표 업데이트
    {
        if (!isDragging || arrow == null) return; // 드래그 중이 아니거나 화살표가 없으면 리턴

        Vector3 mouseDelta = Input.mousePosition - startPos; // 마우스 이동 거리
        float distance = mouseDelta.magnitude;

        float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2f); // 화살표 크기를 힘에 따라 변경
        arrow.transform.localScale = Vector3.one * size; // 화살표 크기 설정

        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();           //화살표 색상을 초록 -> 빨강
        float colorRatio = Mathf.Clamp01(distance * 0.005f); // 거리 비율 계산
        sr.color = Color.Lerp(Color.green, Color.red, colorRatio); // 색상 보간

        if (distance > 10f)
        {
            Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y); // 방향 계산

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // 방향 각도를 변환 시켜주는 공식
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0); // 화살표 회전 설정
        }
    }


    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 화면에서 ray를 쏴서
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject) // 공에 맞으면
            {
                isDragging = true; // 드래그 시작
                startPos = Input.mousePosition; // 시작 위치 저장
                CreateArrow(); // 화살표 생성
                Debug.Log("드래그 시작");
            }
        }
    }


    
    // Start is called before the first frame update
    void Start()
    {
        SetupBall(); // 공 설정
        Debug.Log("공 설정 완료: " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput(); // 입력 처리
        UpdateArrow(); // 화살표 업데이트
    }
}

