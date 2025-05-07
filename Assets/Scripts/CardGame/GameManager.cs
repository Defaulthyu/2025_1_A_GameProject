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

    public Transform mergeArea;     //���� ���� �߰�
    public Button mergeButton;      //���� ��ư �߰�
    public int maxMergeSize = 3;    //�ִ� ���� ��

    public GameObject[] mergeCards; //���� ���� �迭
    public int mergeCount;          //���� ���� ������ �ִ� ī�� ��

    
    // Start is called before the first frame update
    void Start()
    {
        //�迭 �ʱ�ȭ
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        //�� �ʱ�ȭ �� ����
        InitializeDeck();
        ShuffleDeck();

        if (drawButton != null)
        {
            drawButton.onClick.AddListener(OndrawButtonClicked); // ��ư Ŭ�� �̺�Ʈ ���
        }

        if (mergeButton != null)
        {
            mergeButton.onClick.AddListener(OnmergeButtonClicked); // ��ư Ŭ�� �̺�Ʈ ���
            mergeButton.interactable = false;                       //ó������ ���� ��ư ��Ȱ��ȭ
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
    //���� ���� �Լ�
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

    //���� �������� ī�� ���� �Լ�
    public void ArrangeMerge()
    {
        if (mergeCount == 0)      //���� ������ ī�尡 ������ ����
        {
            return;
        }

        float startX = -(mergeCount - 1) * cardSpacing / 2; //ī�� �߾� ������ ���� ������ ���

        for (int i = 0; i < mergeCount; i++)     //�� ī�� ��ġ ����
        {
            if (mergeCards[i] != null)
            {
                Vector3 newPos = mergeArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                mergeCards[i].transform.position = newPos; //ī�� ��ġ ����
            }
        }
    }

    public void DrawCardToHand()
    {
        if(handCount + mergeCount >= maxHandSize)      //����+���� ������ ���ļ� �ִ� ī�� �� �ϰ� ��
        {
            Debug.Log("ī����� �ִ� �Դϴ� ������ Ȯ���ϼ���");
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

    public void MoveCardToMerge(GameObject card)        //ī�带 ���� �������� �̵� [ī�带 �μ��� �޴´�]
    {
        if (mergeCount >= maxMergeSize)      //���� ������ ���� á���� Ȯ��
        {
            Debug.Log("���� ������ ���� á���ϴ�!");
            return;
        }


        for (int i = 0; i < handCount; i++) //ī�尡 ���п� �ִ��� Ȯ���ϰ� ����
        {
            if (handCards[i] == card)
            {
                for(int j = i; j < handCount - 1; j++)      //ī�带 �����ϰ� �迭 ����
                {
                    handCards[j] = handCards[j + 1];
                }
                handCards[handCount - 1] = null;    //�ڵ带 null ���� �ִ´�
                handCount--;                        //ī��Ʈ�� ���δ�

                ArrangeHand(); //���� ����
                break;      //for ���� ���� ���´�

            }
        }

        mergeCards[mergeCount] = card;      // ���� ������ ī�� �߰�
        mergeCount++;

        card.transform.SetParent(mergeArea);    //���� ������ �θ�� �д�
        ArrangeMerge();                         //���� ���� ����
        UpdateMergeButtonState();               //���� ��ư ���� ������Ʈ
       
    }

    //���� ��ư ���� ������Ʈ
    void UpdateMergeButtonState()
    {
        if(mergeButton != null)        //���� ��ư�� ���� ���
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);    //���� ������ ī�尡 2�� �Ǵ� 3���� �������� Ȱ��ȭ
        }
    }

    //���� ������ ī����� ��ó�� �� ī�� ����
    void MergeCards()
    {
        if(mergeCount != 2 && mergeCount != 3)
        {
            Debug.Log("������ �Ϸ��� ī�尡 2�� �Ǵ� 3�� �ʿ��մϴ�!");
            return;
        }

        int firstCardValue = mergeCards[0].GetComponent<Card>().cardValue;      //ù��° ī�忡 �ִ� ���� �����´�
        for(int i = 1; i < mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();     //���� �迭�� �ִ� ī����� ��ȯ�ϸ鼭 �˻�
            if(card == null || card.cardValue != firstCardValue)    //null ���̰ų� ī�� ���� �ٸ����
            {
                Debug.Log("���� ������ ī��θ� ���� �� �� �ֽ��ϴ�");//�α׸� ����� �Լ� ����
                return;                                               //�Լ� ����
            }
        }

        int newValue = firstCardValue + 1;      //������ �� ī�� �� ���
        if(newValue > cardImages.Length)        //���ο� ī�尡 �����ɶ� �ִ� �� �˻�
        {
            Debug.Log("�ִ� ī�� ���� ���� �߽��ϴ�.");
            return;
        }

        for (int i = 0; i < mergeCount; i++)        //���� ������ ī����� ��Ȱ��ȭ
        {
            if (mergeCards[i] == null)              //���� ������ �迭���� ��ȯ�ؼ�
            {
                mergeCards[i].SetActive(false);     //ī����� ��Ȱ��ȭ
            }
        }

        GameObject newCard = Instantiate(cardPrefab, mergeArea.position, Quaternion.identity); //�� ī�� ����

        Card newCardTemp = newCard.GetComponent<Card>();        
        if(newCardTemp != null)
        {
            int imageIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImages[imageIndex]);
        }

        //���� ���� ����
        for (int i = 0; i < maxMergeSize; i++)      //���� �迭 ��ȯ�ϸ鼭 null ��
        {
            mergeCards[i] = null;
        }
        mergeCount = 0;         //���� ī��Ʈ 0���� �ʱ�ȭ

        UpdateMergeButtonState();  //���� ��ư ���� ������Ʈ

        handCards[handCount] = newCard;     //���� ������� ī�带 ���з� �̵�
        handCount++;                        //�ڵ� ī��Ʈ ����
        newCard.transform.SetParent(handArea); //���� ������� ī���� ��ġ�� �ڵ� ������ �ڽ����� ���´�

        ArrangeHand();                      //���� ����
    }

    //���� ��ư Ŭ���� ���� ���� ī�� �ռ�
    void OnmergeButtonClicked()
    {
        MergeCards(); 
    }

    void OndrawButtonClicked()
    {
        DrawCardToHand();
    }
    
}
