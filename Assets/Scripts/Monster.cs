using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100; //체력 함수 선언
    public float Timer = 1.0f;      //타이머 선언
    public int Attackpoint = 10; //공격력 선언
    // 첫 프레임 이전에 한번 실행
    void Start()
    {
        Health = 100; // 첫 프레임 이전에 실행될때 100 체력 추가
  
    }

    // 매번 프레임 때 호출
    void Update()
    {

        CharacterHealthup();
        ChackDeath();
    }

    void CharacterHealthup()
    {
        Timer -= Time.deltaTime; //시간을 매 프레임마다 감소

        if (Timer <= 0)     //만약 타이머 수치가 0이하로 내려갈 경우
        {
            Timer = 1.0f; //타이머 다시 1초
            Health += 20; //1초마다 체력 20 증가
        }
    }

    public void CharacterHit(int Damage) //커스텀 데미지를 받는 함수를 사용
    {
        Health -= Damage; //받은 공격력에 대한 체력을 감소
    }

    void ChackDeath()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }
}
