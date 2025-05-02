using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject cardPrefab; // ī�� ������
    public Sprite[] cardImages; // ī�� �̹��� �迭

    public Transform deckArea; // ī�� �� ��ġ
    public Transform handArea; // �÷��̾� ī�� ��ġ

    public Button drawButton; // ī�� �̱� ��ư
    public TextMeshProUGUI deckCountText; // ī�� �� ���� �ؽ�Ʈ

    public float cardSpacing = 2.0f; // ī�� ����
    public int maxHandSize = 6; // �ִ� ���� ����

    public GameObject[] handCards; // ���� ī�� �迭
    public int handCount; // ī�� �� ����

    public GameObject[] deckCards; // ī�� �� �迭
    public int deckCount; // ī�� �� ����

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
        //�迭 �ʱ�ȭ
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];

        //�� �ʱ�ȭ �� ����
        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null)
        {
            drawButton.onClick.AddListener(OndrawButtonClicked); // ��ư Ŭ�� �̺�Ʈ ���
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
            int value = prefedinedDeck[i];      //ī�� �� ��������
            //�̹��� �ε��� ���
            int imageIndex = value - 1;     //���� 1���� �����ϹǷ� �ε����� 0����
            if(imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;     //�̹����� �����ϰų� �ε����� �߸��� ��� ù��° �̹��� ���
            }
            //ī�� ������Ʈ ���� (�� ��ġ)
            GameObject newCardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);       //ó������ ��Ȱ��ȭ
            newCardObj.SetActive(false); //ī�� ������Ʈ Ȱ��ȭ
            //ī�� ������Ʈ �ʱ�ȭ
            Card cardComp = newCardObj.GetComponent<Card>();
            if(cardComp != null)
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardObj;      //�迭�� ����
        }

    }

    public void ArrangeHand()
    {
        if(handCount == 0)      //���а� ������ ����
        {
            return;
        }

        float startX = -(handCount - 1) * cardSpacing / 2; //ī�� ���ݿ� ���� ���� ��ġ ����

        for (int i = 0; i < handCount; i++)
        {
            if(handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos; //ī�� ��ġ ����
            }
        }
    }

    public void DrawCardToHand()
    {
        if(handCount >= maxHandSize)      //���а� �ִ� �����̸�
        {
            Debug.Log("���а� ���� á���ϴ�!");
            return;
        }
        if (deckCount <= 0)      //���� ���������
        {
            Debug.Log("���� �� �̻� ī�尡 �����ϴ�.");
            return;
        }
        GameObject drawnCard = deckCards[0]; //������ �� ���� ī�带 ��������

        for (int i = 0; i < deckCount - 1; i++)
        {
            deckCards[i] = deckCards[i + 1]; //������ ī�� �̵�
        }
        deckCount--; //�� ���� ����

        drawnCard.SetActive(true); //ī�� Ȱ��ȭ
        handCards[handCount] = drawnCard; //���п� ī�� �߰�
        handCount++; //���� ���� ����

        drawnCard.transform.SetParent(handArea); //���� ��ġ�� �̵�

        ArrangeHand(); //���� ����
    }

    void OndrawButtonClicked()
    {
        DrawCardToHand();
    }
    
}
