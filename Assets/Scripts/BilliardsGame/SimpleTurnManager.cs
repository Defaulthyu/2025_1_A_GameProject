using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{

    public static bool canPlay = true; // �÷��� ���� ����
    public static bool anyBallMoveing = false; // ���� �����̰� �ִ��� ����

    void CheckAllBalls()
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoveing = false; // �ʱ�ȭ

        foreach(SimpleBallController ball in allBalls)
        {
            if (ball.IsMoving()) // ���� �����̰� �ִٸ�
            {
                anyBallMoveing = true; // ���� �����̰� �ִٰ� ����
                break; // �� �̻� Ȯ���� �ʿ� ����
            }
        }
    }

    public static void OnBallHit()
    {
        canPlay = false; // �÷��� �Ұ��� ���·� ����
        anyBallMoveing = true; // ���� �����̰� �ִٰ� ����
        Debug.Log("�� ����! ���� ���� �� ���� ��ٸ�����");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls(); // ��� ���� ���¸� Ȯ��
        
        if(!anyBallMoveing && !canPlay) // ��� ���� ���߸� �ٽ� ĥ �� �ְ� ��
        {
            canPlay = true; // �÷��� ���� ���·� ����
            Debug.Log("�� ����! �ٽ� ĥ �� �ֽ��ϴ�");
        }
    }

}
