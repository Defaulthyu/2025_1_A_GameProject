using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBallController : MonoBehaviour
{
    [Header("�⺻ ����")]
    public float power = 10f; // ���� ĥ ���� ��
    public Sprite arrowSprite; // ȭ��ǥ ��������Ʈ

    private Rigidbody rb;
    private GameObject arrow; // ȭ��ǥ ������Ʈ
    private bool isDragging = false; // �巡�� ������ ����
    private Vector3 startPos;

    void SetupBall()                                //�� ���� �ϱ�
    {
        rb = GetComponent<Rigidbody>();             //���� ������Ʈ ��������
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>(); // Rigidbody�� ������ �߰�
        }

        //���� ����
        rb.mass = 1; // �Ϲ����� �籸���� ����
        rb.drag = 1; // ���� ������
    }

    public bool IsMoving()
    {
        return rb.velocity.magnitude > 0.2f; // ���� �����̰� �ִ��� Ȯ��
    }

    void HandleInput()
    {
        if (!SimpleTurnManager.canPlay) return; // �÷��� ���� ���� Ȯ��
        if (SimpleTurnManager.anyBallMoveing) return; // �ٸ� ���� �����̰� ������ ����

        if (IsMoving()) return;         //���� �����̰� ������ ���� �Ұ�

        if (Input.GetMouseButtonDown(0))     //���콺 Ŭ�� ����
        {
            StartDrag(); // �巡�� ����
        }
        if (Input.GetMouseButtonUp(0) && isDragging)     //�巹�� ���̾��µ� ���콺 ��ư �� ���� ��
        {
            Shoot(); // �� �߻�
        }
    }

    void Shoot()
    {
        Vector3 mouseDelta = Input.mousePosition - startPos; // ���콺 �̵� �Ÿ�
        float force = mouseDelta.magnitude * 0.01f * power; // �� ���

        if(force < 5) force = 5;

        Vector3 direction = new Vector3(-mouseDelta.x, 0, - mouseDelta.y).normalized; // ���� ���

        rb.AddForce(direction * force, ForceMode.Impulse); // �� ����

        SimpleTurnManager.OnBallHit(); // �� �Ŵ����� ���� ĥ �� �˸�

        isDragging = false; // �巡�� ���� ����
        Destroy(arrow); // ȭ��ǥ ����
        arrow = null; // ȭ��ǥ ������Ʈ �ʱ�ȭ

        Debug.Log("�߻�! ��: " + force);
    }

    void CreateArrow()
    {
        if(arrow != null)
        {
            Destroy(arrow); // ���� ȭ��ǥ ����
        }

        arrow = new GameObject("Arrow"); // �� ȭ��ǥ ������Ʈ ����
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>(); // ��������Ʈ ������ �߰�

        sr.sprite = arrowSprite; // ȭ��ǥ ��������Ʈ ����
        sr.color = Color.green; // ȭ��ǥ ���� ����
        sr.sortingOrder = 10; // ���� ���� ����

        arrow.transform.position = transform.position + Vector3.up; // �� ��ġ�� ȭ��ǥ ��ġ ����
        arrow.transform.localScale = Vector3.one;
    }

    void UpdateArrow()                  //ȭ��ǥ ������Ʈ
    {
        if (!isDragging || arrow == null) return; // �巡�� ���� �ƴϰų� ȭ��ǥ�� ������ ����

        Vector3 mouseDelta = Input.mousePosition - startPos; // ���콺 �̵� �Ÿ�
        float distance = mouseDelta.magnitude;

        float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2f); // ȭ��ǥ ũ�⸦ ���� ���� ����
        arrow.transform.localScale = Vector3.one * size; // ȭ��ǥ ũ�� ����

        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();           //ȭ��ǥ ������ �ʷ� -> ����
        float colorRatio = Mathf.Clamp01(distance * 0.005f); // �Ÿ� ���� ���
        sr.color = Color.Lerp(Color.green, Color.red, colorRatio); // ���� ����

        if (distance > 10f)
        {
            Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y); // ���� ���

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // ���� ������ ��ȯ �����ִ� ����
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0); // ȭ��ǥ ȸ�� ����
        }
    }


    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ȭ�鿡�� ray�� ����
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject) // ���� ������
            {
                isDragging = true; // �巡�� ����
                startPos = Input.mousePosition; // ���� ��ġ ����
                CreateArrow(); // ȭ��ǥ ����
                Debug.Log("�巡�� ����");
            }
        }
    }


    
    // Start is called before the first frame update
    void Start()
    {
        SetupBall(); // �� ����
        Debug.Log("�� ���� �Ϸ�: " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput(); // �Է� ó��
        UpdateArrow(); // ȭ��ǥ ������Ʈ
    }
}

