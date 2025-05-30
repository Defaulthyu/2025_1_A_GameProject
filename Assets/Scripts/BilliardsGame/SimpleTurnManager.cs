using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{

    public static bool canPlay = true; // 플레이 가능 여부
    public static bool anyBallMoveing = false; // 공이 움직이고 있는지 여부

    void CheckAllBalls()
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoveing = false; // 초기화

        foreach(SimpleBallController ball in allBalls)
        {
            if (ball.IsMoving()) // 공이 움직이고 있다면
            {
                anyBallMoveing = true; // 공이 움직이고 있다고 설정
                break; // 더 이상 확인할 필요 없음
            }
        }
    }

    public static void OnBallHit()
    {
        canPlay = false; // 플레이 불가능 상태로 변경
        anyBallMoveing = true; // 공이 움직이고 있다고 설정
        Debug.Log("턴 시작! 공이 멈출 때 까지 기다리세요");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls(); // 모든 공의 상태를 확인
        
        if(!anyBallMoveing && !canPlay) // 모든 공이 멈추면 다시 칠 수 있게 함
        {
            canPlay = true; // 플레이 가능 상태로 변경
            Debug.Log("턴 종료! 다시 칠 수 있습니다");
        }
    }

}
