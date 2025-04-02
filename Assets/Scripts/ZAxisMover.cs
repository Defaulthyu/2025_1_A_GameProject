using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisMover : MonoBehaviour     //z축으로 이동 하는 클래스
{
    public float speed = 5.0f;  //이동 속도
    public float timer = 5.0f;   //타이머 설정

    // Update is called once per frame
    void Update()
    {
        //z축 방향으로 앞으로 이동
        transform.Translate(0, 0, speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Destroy(gameObject);
        }
        
    }
}
