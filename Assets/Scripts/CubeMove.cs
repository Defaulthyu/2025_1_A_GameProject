using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, -moveSpeed * Time.deltaTime); //축 마이너스 방향으로 감       
        if (transform.position.z < -20)
        {
            Destroy(gameObject);
        }
        
    }
}
