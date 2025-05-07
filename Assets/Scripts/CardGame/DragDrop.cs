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
        
        if(gameManager == null)
        {
            ReturnToOriginalPosition(); // 원래 위치로 돌아가기
            return;
        }

        bool wasInMergeArea = startParent == gameManager.mergeArea;     //현재 카드가 어느 영역에서 왔는지 확인
        
        if(IsOverArea(gameManager.handArea))        //손패 영역 위에 카드를 놓았는지 확인
        {
            Debug.Log("손패 영역으로 이동");

            if(wasInMergeArea)                  //머지 영역에서왔다면 MoveToHand 함수 호출
            {
                for(int i = 0; i< gameManager.mergeCount; i++)      //카드를 머지 영역에서 제거하고 손패로 이동
                {
                    if (gameManager.mergeCards[i] == gameObject)    //핸드 배열과 내가 마우스 업 하는 오브젝트와 같은 경우
                    {
                        for(int j = i; j< gameManager.mergeCount - 1; j++)
                        {
                            gameManager.mergeCards[j] = gameManager.mergeCards[j + 1]; //해당 카드를 제거하고 배열 뒤에서 앞으로 한칸씩 이동
                        }
                        gameManager.mergeCards[gameManager.mergeCount - 1] = null;      //맨 뒤의 카드를 null로 설정
                        gameManager.mergeCount--;               //카드 수를 줄인다

                        transform.SetParent(gameManager.handArea);      //손패에 카드 추가
                        gameManager.handCards[gameManager.handCount] = gameObject;
                        gameManager.handCount++;

                        gameManager.ArrangeHand();      //영역 정렬
                        gameManager.ArrangeMerge();
                        break;
                    }
                }
            }
            else
            {
                gameManager.ArrangeHand();      //이미 손패에 있다면 정렬만 수행
            }
        }
        else if(IsOverArea(gameManager.mergeArea))      //머지 영역 위에 카드를 놓았는지 확인
        {
            if(gameManager.mergeCount >= gameManager.maxMergeSize)  //머지 영역이 가득 찼는지 확인
            {
                Debug.Log("머지 영역이 가득 찼습니다");
                ReturnToOriginalPosition();
            }
            else
            {
                gameManager.MoveCardToMerge(gameObject);        //머지 영역으로 이동
            }
        }
        else
        {
            ReturnToOriginalPosition();     //아무 영역도 아니면 원래 위치로 돌아가기
        }

        if(wasInMergeArea)          //머지 영역에 있을 경우 버튼 상태 업데이트
        {
            if(gameManager.mergeButton != null)
            {
                bool canMerge = (gameManager.mergeCount == 2 || gameManager.mergeCount == 3);
                gameManager.mergeButton.interactable = canMerge;
            }
        }
    }

    void ReturnToOriginalPosition()     //원래 위치로 돌아가는 함수
    {
        transform.position = startPosition; // 원래 위치로 돌아가기
        transform.SetParent(startParent); // 원래 부모로 돌아가기

        if(gameManager != null)
        {
            if (startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand(); // 덱에서 카드가 드래그된 경우 덱 셔플
            }
            if (startParent == gameManager.mergeArea)
            {
                gameManager.ArrangeMerge(); // 덱에서 카드가 드래그된 경우 덱 셔플
            }
        }
    }

    bool IsOverArea(Transform area)
    {
        if(area == null)
            return false;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    //현재 마우스 위치를 가져오기
        mousePosition.z = 0;                                                            //2D이기 때문에

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);        //레이 캐스트 생성(마우스 위치에서 아래 방향으로)

        foreach(RaycastHit2D hit in hits)                   //레이캐스트로 감지된 모든 콜라이더 확인
        {
            if(hit.collider != null && hit.collider.transform == area)      //콜라이더의 게임 오브젝트가 찾고있는 영역인지 확인
            {
                Debug.Log(area.name + " 영역 감지됨");
                return true;

            }
        }

        return false;

    }
}

