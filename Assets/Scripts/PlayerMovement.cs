using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;    //�̵��ӵ� ���� ����
    public float jumpForce = 5f;    //������ �� ���� �ش�

    public bool isGrounded = true;  //���� �ִ��� üũ �ϴ� ���� (T/F)
    public Rigidbody rb;    //�÷��̾��� ��ü�� ȣ��

    public int coinCount = 0;
    public int totalCoins;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(totalCoins);
        totalCoins = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //�ӵ��� ���� �̵�
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //���� �Է�
        if (Input.GetButton("Jump") && isGrounded)  //�� ���� �����Ҷ� -> �����̽� ��ư�� �������� �� isGrounded�� True �϶�
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //�������� ������ ����ŭ ��ü�� �ش�
            isGrounded = false;                                     //������ �ϴ¼��� ������ �������� ������ false
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //���� ����
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"���� ���� : {coinCount} / {totalCoins}");
        }
        
        //������ ������ ���� �α� ���
        if(other.gameObject.tag == "Door" && coinCount == totalCoins)   //��� ������ ȹ�� �Ŀ� ������ ���� ���� ����
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �߰� ����
        }
    }
}
