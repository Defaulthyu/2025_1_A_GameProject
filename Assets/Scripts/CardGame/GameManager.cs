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

    // Start is called before the first frame update
    void Start()
    {
        //배열 초기화
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];

        //덱 초기화 및 셔플
        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null)
        {
            drawButton.onClick.AddListener(OndrawButtonClicked); // 버튼 클릭 이벤트 등록
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

    public void DrawCardToHand()
    {
        if(handCount >= maxHandSize)      //손패가 최대 개수이면
        {
            Debug.Log("손패가 가득 찼습니다!");
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

    void OndrawButtonClicked()
    {
        DrawCardToHand();
    }
    
}
