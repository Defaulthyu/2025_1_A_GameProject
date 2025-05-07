using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false; // �巡�� ������ ����
    public Vector3 startPosition; // �巡�� ���� ��ġ
    public Transform startParent; // �巡�� ���� �θ�

    private GameManager gameManager; // ���� �Ŵ��� ����


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // �巡�� ���� ��ġ �ʱ�ȭ
        startParent = transform.parent; // �巡�� ���� �θ� �ʱ�ȭ

        gameManager = FindObjectOfType<GameManager>(); // ���� �Ŵ��� ã��
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���콺 ��ġ ��������
            mousePos.z = 0; // z�� ��ġ ����
            transform.position = mousePos; // ī�� ��ġ ������Ʈ
        }
    }

    void OnMouseDown()
    {
        isDragging = true; // �巡�� ����
        startPosition = transform.position; // �巡�� ���� ��ġ ����
        startParent = transform.parent; // �巡�� ���� �θ� ����
        GetComponent<SpriteRenderer>().sortingOrder = 10; // �巡�� ���� ī���� ���� ���� ����
    }

    void OnMouseUp()
    {
        isDragging = false; // �巡�� ����
        GetComponent<SpriteRenderer>().sortingOrder = 1; // �巡�� ���� ī�尡 �ٸ�ī�庸�� �տ� ���̵��� �Ѵ�
        
        if(gameManager == null)
        {
            ReturnToOriginalPosition(); // ���� ��ġ�� ���ư���
            return;
        }

        bool wasInMergeArea = startParent == gameManager.mergeArea;     //���� ī�尡 ��� �������� �Դ��� Ȯ��
        
        if(IsOverArea(gameManager.handArea))        //���� ���� ���� ī�带 ���Ҵ��� Ȯ��
        {
            Debug.Log("���� �������� �̵�");

            if(wasInMergeArea)                  //���� ���������Դٸ� MoveToHand �Լ� ȣ��
            {
                for(int i = 0; i< gameManager.mergeCount; i++)      //ī�带 ���� �������� �����ϰ� ���з� �̵�
                {
                    if (gameManager.mergeCards[i] == gameObject)    //�ڵ� �迭�� ���� ���콺 �� �ϴ� ������Ʈ�� ���� ���
                    {
                        for(int j = i; j< gameManager.mergeCount - 1; j++)
                        {
                            gameManager.mergeCards[j] = gameManager.mergeCards[j + 1]; //�ش� ī�带 �����ϰ� �迭 �ڿ��� ������ ��ĭ�� �̵�
                        }
                        gameManager.mergeCards[gameManager.mergeCount - 1] = null;      //�� ���� ī�带 null�� ����
                        gameManager.mergeCount--;               //ī�� ���� ���δ�

                        transform.SetParent(gameManager.handArea);      //���п� ī�� �߰�
                        gameManager.handCards[gameManager.handCount] = gameObject;
                        gameManager.handCount++;

                        gameManager.ArrangeHand();      //���� ����
                        gameManager.ArrangeMerge();
                        break;
                    }
                }
            }
            else
            {
                gameManager.ArrangeHand();      //�̹� ���п� �ִٸ� ���ĸ� ����
            }
        }
        else if(IsOverArea(gameManager.mergeArea))      //���� ���� ���� ī�带 ���Ҵ��� Ȯ��
        {
            if(gameManager.mergeCount >= gameManager.maxMergeSize)  //���� ������ ���� á���� Ȯ��
            {
                Debug.Log("���� ������ ���� á���ϴ�");
                ReturnToOriginalPosition();
            }
            else
            {
                gameManager.MoveCardToMerge(gameObject);        //���� �������� �̵�
            }
        }
        else
        {
            ReturnToOriginalPosition();     //�ƹ� ������ �ƴϸ� ���� ��ġ�� ���ư���
        }

        if(wasInMergeArea)          //���� ������ ���� ��� ��ư ���� ������Ʈ
        {
            if(gameManager.mergeButton != null)
            {
                bool canMerge = (gameManager.mergeCount == 2 || gameManager.mergeCount == 3);
                gameManager.mergeButton.interactable = canMerge;
            }
        }
    }

    void ReturnToOriginalPosition()     //���� ��ġ�� ���ư��� �Լ�
    {
        transform.position = startPosition; // ���� ��ġ�� ���ư���
        transform.SetParent(startParent); // ���� �θ�� ���ư���

        if(gameManager != null)
        {
            if (startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand(); // ������ ī�尡 �巡�׵� ��� �� ����
            }
            if (startParent == gameManager.mergeArea)
            {
                gameManager.ArrangeMerge(); // ������ ī�尡 �巡�׵� ��� �� ����
            }
        }
    }

    bool IsOverArea(Transform area)
    {
        if(area == null)
            return false;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    //���� ���콺 ��ġ�� ��������
        mousePosition.z = 0;                                                            //2D�̱� ������

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);        //���� ĳ��Ʈ ����(���콺 ��ġ���� �Ʒ� ��������)

        foreach(RaycastHit2D hit in hits)                   //����ĳ��Ʈ�� ������ ��� �ݶ��̴� Ȯ��
        {
            if(hit.collider != null && hit.collider.transform == area)      //�ݶ��̴��� ���� ������Ʈ�� ã���ִ� �������� Ȯ��
            {
                Debug.Log(area.name + " ���� ������");
                return true;

            }
        }

        return false;

    }
}

