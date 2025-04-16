using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5f;    //�̵��ӵ� ���� ����
    public float jumpForce = 5f;    //������ �� ���� �ش�
    public float turnSpeed = 10f;     //ȸ�� �ӵ�

    [Header("���� ���� ����")]
    public float fallMultiplier = 2.5f;     //�ϰ� �߷� ����
    public float lowJumpMultiplier = 2.0f;  //ª�� ���� ����

    [Header("���� ���� ����")]
    public float coyoteTime = 2.5f;         //���� ���� �ð�
    public float coyoteTimeCounter;         //���� Ÿ�̸�
    public bool realGrouned = true;         //���� ���� ����

    [Header("�۶��̴� ����")]
    public GameObject gliderObject;         //�۶��̴� ������Ʈ
    public float gliderFallSpeed = 1.0f;    //�۶��̴� ���ϼӵ�
    public float gliderMoveSpeed = 7.0f;    //
    public float gliderMaxTime = 5.0f;      //�ִ� ���ð�
    public float gliderTimeLeft;            //���� ���ð�
    public bool isGliding = false;          //�۶��̵� ������ ����

    

    public bool isGrounded = true;  //���� �ִ��� üũ �ϴ� ���� (T/F)
    public Rigidbody rb;    //�÷��̾��� ��ü�� ȣ��

    public int coinCount = 0;
    public int totalCoins;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(totalCoins);
        totalCoins = 10;
        coyoteTimeCounter = 0;

        //�۶��̴� ������Ʈ �ʱ�ȭ
        if(gliderObject != null)
        {
            gliderObject.SetActive(false);      //���� �� ��Ȱ��ȭ

        }

        gliderTimeLeft = gliderMaxTime;         //�۶��̴� �ð� �ʱ�ȭ

        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���� ����ȭ
        UpdateGroundedState();


        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //�̵� ���� ����
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        //�Է��� ���� ���� ȸ��
        if(movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        //GŰ�� �۶��̴� ���� (������ ���ȸ� Ȱ��ȭ)
        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)        //GŰ�� �����鼭 ���� ���� �ʰ� �۶��̴� ���� �ð��� ������ (3���� ����)
        {
            if(!isGliding)                                                      //�۶��̴� Ȱ��ȭ(������ �ִµ���)
            {
                //�۶��̴� Ȱ��ȭ �Լ� (�Ʒ� ����)
                EnableGlider();
            }

            //�۶��̴� ��� �ð� ����
            gliderTimeLeft -= Time.deltaTime;
            
            //�۶��̴� �ð��� �� �Ǹ� ��Ȱ��ȭ
            if(gliderTimeLeft <= 0)
            {
                //�۶��̴� ��Ȱ��ȭ �Լ� (�Ʒ� ����)
                DisableGlider();
            }
        }
        else if(isGliding)
        {
            //GŰ�� ���� �۶��̴� ��Ȱ��ȭ
            DisableGlider();
        }

        else
        {
            //�ӵ��� ���� �̵�
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);
            //���� ���� ���� ����
            if (rb.velocity.y < 0)
            {
                //�ϰ� �� ���� ��ȭ
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            //���� �Է�
            if (Input.GetButton("Jump") && isGrounded)  //�� ���� �����Ҷ� -> �����̽� ��ư�� �������� �� isGrounded�� True �϶�
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //�������� ������ ����ŭ ��ü�� �ش�
                isGrounded = false;                                     //������ �ϴ¼��� ������ �������� ������ false
                realGrouned = false;
                coyoteTimeCounter = 0;                                  //�ڿ��� Ÿ�� ��� ����
            }

            //���鿡 ������ �۶��̴� �ð� ȸ�� �� �۶��̴� ��Ȱ��ȭ
            if(isGliding)
            {
                DisableGlider();
            }

            //���� ���� �� �ð� ȸ��
            gliderTimeLeft = gliderMaxTime;
        }




        
    }

    //�۶��̴� Ȱ��ȭ �Լ�
    void EnableGlider()
    {
        isGliding = true;

        //�۶��̴� ������Ʈ ǥ��
        if(gliderObject != null)
        {
            gliderObject.SetActive(true);
        }
        //�ϰ� �ӵ� �ʱ�ȭ
        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);

    }

    //�۶��̴� ��Ȱ��ȭ �Լ�
    void DisableGlider()
    {
        isGliding = false;

        //�۶��̴� ������Ʈ �����
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        //��� �����ϵ��� �߷� ����
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    void ApplyGliderMovement(float horizontal, float vertical)//����� ������ �Լ��� �μ��� �޴´�
    {
        //�۶��̴� ȿ��: õõ�� �������� ���� �������� �� ������ �̵�

        Vector3 gliderVelocity = new Vector3(
            horizontal * gliderMoveSpeed,   //X��
            -gliderFallSpeed,               //Y�� ������ �ӵ��� õõ�� �ϰ�
            vertical * gliderMoveSpeed      //Z��
        );

        rb.velocity = gliderVelocity;
            
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = true;
        }

    }


    void OnCollisionStay(Collision collision)           //�浹 ó�� �Լ�
    {
        if(collision.gameObject.CompareTag("Ground"))   //�浹�� �Ͼ ��ü�� Tag�� Ground ���
        {
            realGrouned = true;                         //���� �浹�ϸ� true�� �����Ѵ�
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //���� ����
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"���� ���� : {coinCount} / {totalCoins}");
        }

        //������ ������ ���� �α� ���
        if (other.gameObject.tag == "Door" && coinCount >= totalCoins)   //��� ������ ȹ�� �Ŀ� ������ ���� ���� ����
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �߰� ����
        }


    }
    //���� ���� ������Ʈ �Լ�
    void UpdateGroundedState()
    {
        if(realGrouned)         //���� ���鿡 ������ �ڿ��� Ÿ�� ����
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            //�����δ� ���鿡 ������ �ڿ���� Ÿ�� ���� ������ ������ �������� �Ǵ�.
            if(coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;        //�ð��� ���������� ���� ��Ų��
                isGrounded = true;

            }
            else
            {
                isGrounded = false;                         //Ÿ���� ������ false

            }
        }
    }
}
