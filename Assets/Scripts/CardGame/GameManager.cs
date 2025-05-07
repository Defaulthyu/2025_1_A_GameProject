using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject cardPrefab; // 카드 프리팹
    public Sprite[] cardImages; // 카드 이미지 배열

    public Transform deckArea; // 카드 덱 위치
    public Transform handArea; // 플레이어 카드 위치

    public Button drawButton; // 카드 뽑기 버튼
    public TextMeshProUGUI deckCountText; // 카드 덱 개수 텍스트

    public float cardSpacing = 2.0f; // 카드 간격
    public int maxHandSize = 6; // 최대 손패 개수

    public GameObject[] handCards; // 손패 카드 배열
    public int handCount; // 카드 덱 개수

    public GameObject[] deckCards; // 카드 덱 배열
    public int deckCount; // 카드 덱 개수

    public int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1,
        2,2,2,2,2,2,
        3,3,3,3,
        4,4
    };

    public Transform mergeArea;     //머지 영역 추가
    public Button mergeButton;      //머지 버튼 추가
    public int maxMergeSize = 3;    //최대 머지 수

    public GameObject[] mergeCards; //머지 영역 배열
    public int mergeCount;          //현재 머지 영역에 있는 카드 수

    
    // Start is called before the first frame update
    void Start()
    {
        //배열 초기화
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        //덱 초기화 및 셔플
        InitializeDeck();
        ShuffleDeck();

        if (drawButton != null)
        {
            drawButton.onClick.AddListener(OndrawButtonClicked); // 버튼 클릭 이벤트 등록
        }

        if (mergeButton != null)
        {
            mergeButton.onClick.AddListener(OnmergeButtonClicked); // 버튼 클릭 이벤트 등록
            mergeButton.interactable = false;                       //처음에는 머지 버튼 비활성화
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deckCount ; i++)
        {
            int j = Random.Range(i,deckCount);

            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }

    void InitializeDeck()
    {
        deckCount = prefedinedDeck.Length;

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];      //카드 값 가져오기
            //이미지 인덱스 계산
            int imageIndex = value - 1;     //값은 1부터 시작하므로 인덱스는 0부터
            if(imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;     //이미지가 부족하거나 인덱스가 잘못된 경우 첫번째 이미지 사용
            }
            //카드 오브젝트 생성 (덱 위치)
            GameObject newCardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);       //처음에는 비활성화
            newCardObj.SetActive(false); //카드 오브젝트 활성화
            //카드 컴포넌트 초기화
            Card cardComp = newCardObj.GetComponent<Card>();
            if(cardComp != null)
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardObj;      //배열에 저장
        }

    }
    //손패 정렬 함수
    public void ArrangeHand()
    {
        if(handCount == 0)      //손패가 없으면 리턴
        {
            return;
        }

        float startX = -(handCount - 1) * cardSpacing / 2; //카드 간격에 따라 시작 위치 조정

        for (int i = 0; i < handCount; i++)
        {
            if(handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos; //카드 위치 조정
            }
        }
    }

    //머지 영역에서 카드 정렬 함수
    public void ArrangeMerge()
    {
        if (mergeCount == 0)      //머지 영역에 카드가 없으면 리턴
        {
            return;
        }

        float startX = -(mergeCount - 1) * cardSpacing / 2; //카드 중앙 정렬을 위해 오프셋 계산

        for (int i = 0; i < mergeCount; i++)     //각 카드 위치 조정
        {
            if (mergeCards[i] != null)
            {
                Vector3 newPos = mergeArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                mergeCards[i].transform.position = newPos; //카드 위치 조정
            }
        }
    }

    public void DrawCardToHand()
    {
        if(handCount + mergeCount >= maxHandSize)      //손패+머지 영역을 합쳐서 최대 카드 수 하고 비교
        {
            Debug.Log("카드수가 최대 입니다 공간을 확보하세요");
            return;
        }
        if (deckCount <= 0)      //덱이 비어있으면
        {
            Debug.Log("덱에 더 이상 카드가 없습니다.");
            return;
        }
        GameObject drawnCard = deckCards[0]; //덱에서 맨 위에 카드를 가져오기

        for (int i = 0; i < deckCount - 1; i++)
        {
            deckCards[i] = deckCards[i + 1]; //덱에서 카드 이동
        }
        deckCount--; //덱 개수 감소

        drawnCard.SetActive(true); //카드 활성화
        handCards[handCount] = drawnCard; //손패에 카드 추가
        handCount++; //손패 개수 증가

        drawnCard.transform.SetParent(handArea); //손패 위치로 이동

        ArrangeHand(); //손패 정렬
    }

    public void MoveCardToMerge(GameObject card)        //카드를 머지 영역으로 이동 [카드를 인수로 받는다]
    {
        if (mergeCount >= maxMergeSize)      //머지 영역이 가득 찼는지 확인
        {
            Debug.Log("머지 영역이 가득 찼습니다!");
            return;
        }


        for (int i = 0; i < handCount; i++) //카드가 손패에 있는지 확인하고 제거
        {
            if (handCards[i] == card)
            {
                for(int j = i; j < handCount - 1; j++)      //카드를 제거하고 배열 정리
                {
                    handCards[j] = handCards[j + 1];
                }
                handCards[handCount - 1] = null;    //핸드를 null 값을 넣는다
                handCount--;                        //카운트를 줄인다

                ArrangeHand(); //손패 정렬
                break;      //for 문을 빠져 나온다

            }
        }

        mergeCards[mergeCount] = card;      // 머지 영역에 카드 추가
        mergeCount++;

        card.transform.SetParent(mergeArea);    //머지 영역을 부모로 둔다
        ArrangeMerge();                         //머지 영역 정렬
        UpdateMergeButtonState();               //머지 버튼 상태 업데이트
       
    }

    //머지 버튼 상태 업데이트
    void UpdateMergeButtonState()
    {
        if(mergeButton != null)        //머지 버튼이 있을 경우
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);    //머지 영역에 카드가 2개 또는 3개만 있을때만 활성화
        }
    }

    //머지 영역의 카드들을 합처서 새 카드 생성
    void MergeCards()
    {
        if(mergeCount != 2 && mergeCount != 3)
        {
            Debug.Log("머지를 하려면 카드가 2개 또는 3개 필요합니다!");
            return;
        }

        int firstCardValue = mergeCards[0].GetComponent<Card>().cardValue;      //첫번째 카드에 있는 값을 가져온다
        for(int i = 1; i < mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();     //머지 배열에 있는 카드들을 수환하면서 검사
            if(card == null || card.cardValue != firstCardValue)    //null 값이거나 카드 값이 다른경우
            {
                Debug.Log("같은 숫자의 카드로만 머지 할 수 있습니다");//로그를 남기고 함수 종료
                return;                                               //함수 종료
            }
        }

        int newValue = firstCardValue + 1;      //머지된 새 카드 값 계산
        if(newValue > cardImages.Length)        //새로운 카드가 생성될때 최대 값 검사
        {
            Debug.Log("최대 카드 값에 도달 했습니다.");
            return;
        }

        for (int i = 0; i < mergeCount; i++)        //머지 영역의 카드들을 비활성화
        {
            if (mergeCards[i] == null)              //머지 영역의 배열들을 순환해서
            {
                mergeCards[i].SetActive(false);     //카드들을 비활성화
            }
        }

        GameObject newCard = Instantiate(cardPrefab, mergeArea.position, Quaternion.identity); //새 카드 생성

        Card newCardTemp = newCard.GetComponent<Card>();        
        if(newCardTemp != null)
        {
            int imageIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImages[imageIndex]);
        }

        //머지 영역 비우기
        for (int i = 0; i < maxMergeSize; i++)      //머지 배열 순환하면서 null 값
        {
            mergeCards[i] = null;
        }
        mergeCount = 0;         //머지 카운트 0으로 초기화

        UpdateMergeButtonState();  //머지 버튼 상태 업데이트

        handCards[handCount] = newCard;     //새로 만들어진 카드를 손패로 이동
        handCount++;                        //핸드 카운트 증가
        newCard.transform.SetParent(handArea); //새로 만들어진 카드의 위치를 핸드 영역에 자식으로 놓는다

        ArrangeHand();                      //손패 정렬
    }

    //머지 버튼 클릭시 머지 영역 카드 합성
    void OnmergeButtonClicked()
    {
        MergeCards(); 
    }

    void OndrawButtonClicked()
    {
        DrawCardToHand();
    }
    
}
