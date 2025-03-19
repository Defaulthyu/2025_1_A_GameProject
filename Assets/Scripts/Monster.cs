using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100; //ü�� �Լ� ����
    public float Timer = 1.0f;      //Ÿ�̸� ����
    public int Attackpoint = 10; //���ݷ� ����
    // ù ������ ������ �ѹ� ����
    void Start()
    {
        Health = 100; // ù ������ ������ ����ɶ� 100 ü�� �߰�
  
    }

    // �Ź� ������ �� ȣ��
    void Update()
    {

        CharacterHealthup();
        ChackDeath();
    }

    void CharacterHealthup()
    {
        Timer -= Time.deltaTime; //�ð��� �� �����Ӹ��� ����

        if (Timer <= 0)     //���� Ÿ�̸� ��ġ�� 0���Ϸ� ������ ���
        {
            Timer = 1.0f; //Ÿ�̸� �ٽ� 1��
            Health += 20; //1�ʸ��� ü�� 20 ����
        }
    }

    public void CharacterHit(int Damage) //Ŀ���� �������� �޴� �Լ��� ���
    {
        Health -= Damage; //���� ���ݷ¿� ���� ü���� ����
    }

    void ChackDeath()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }
}
