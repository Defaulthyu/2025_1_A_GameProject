using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;    //이동속도 변수 설정
    public float jumpForce = 5f;    //점프의 힘 값을 준다

    public bool isGrounded = true;  //땅에 있는지 체크 하는 변수 (T/F)
    public Rigidbody rb;    //플레이어의 강체를 호출

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
        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //속도로 직접 이동
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //점프 입력
        if (Input.GetButton("Jump") && isGrounded)  //두 값을 만족할때 -> 스페이스 버튼을 눌렀을때 외 isGrounded가 True 일때
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //위쪽으로 설정한 힘만큼 강체에 준다
            isGrounded = false;                                     //점프를 하는순간 땅에서 떨어졌기 때문에 false
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
        //코인 수집
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인 수집 : {coinCount} / {totalCoins}");
        }
        
        //목적지 도착시 종료 로그 출력
        if(other.gameObject.tag == "Door" && coinCount == totalCoins)   //모든 코인을 획득 후에 문으로 가면 게임 종료
        {
            Debug.Log("게임 클리어");
            //게임 완료 로직 추가 가능
        }
    }
}
