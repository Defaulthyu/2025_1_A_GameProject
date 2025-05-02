using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false; // 드래그 중인지 여부
    public Vector3 startPosition; // 드래그 시작 위치
    public Transform startParent; // 드래그 시작 부모

    private GameManager gameManager; // 게임 매니저 참조


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // 드래그 시작 위치 초기화
        startParent = transform.parent; // 드래그 시작 부모 초기화

        gameManager = FindObjectOfType<GameManager>(); // 게임 매니저 찾기
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 위치 가져오기
            mousePos.z = 0; // z축 위치 고정
            transform.position = mousePos; // 카드 위치 업데이트
        }
    }

    void OnMouseDown()
    {
        isDragging = true; // 드래그 시작
        startPosition = transform.position; // 드래그 시작 위치 저장
        startParent = transform.parent; // 드래그 시작 부모 저장
        GetComponent<SpriteRenderer>().sortingOrder = 10; // 드래그 중인 카드의 정렬 순서 변경
    }

    void OnMouseUp()
    {
        isDragging = false; // 드래그 종료
        GetComponent<SpriteRenderer>().sortingOrder = 1; // 드래그 중인 카드가 다른카드보다 앞에 보이도록 한다

        ReturnToOriginalPosition(); // 원래 위치로 돌아가기
    }

    void ReturnToOriginalPosition()
    {
        transform.position = startPosition; // 원래 위치로 돌아가기
        transform.SetParent(startParent); // 원래 부모로 돌아가기

        if(gameManager != null)
        {
            if(startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand(); // 덱에서 카드가 드래그된 경우 덱 셔플
            }
        }
    }

    bool IsOverArea(Transform area)
    {
        if(area == null)
            return false;

        Collider2D areaCollider = area.GetComponent<Collider2D>();
        if(areaCollider == null)
            return false;

        return areaCollider.bounds.Contains(transform.position); // 카드가 영역에 있는지 확인
    }
}

