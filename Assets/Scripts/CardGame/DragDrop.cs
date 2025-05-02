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

        ReturnToOriginalPosition(); // ���� ��ġ�� ���ư���
    }

    void ReturnToOriginalPosition()
    {
        transform.position = startPosition; // ���� ��ġ�� ���ư���
        transform.SetParent(startParent); // ���� �θ�� ���ư���

        if(gameManager != null)
        {
            if(startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand(); // ������ ī�尡 �巡�׵� ��� �� ����
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

        return areaCollider.bounds.Contains(transform.position); // ī�尡 ������ �ִ��� Ȯ��
    }
}

