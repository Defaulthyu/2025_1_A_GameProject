using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardValue; //ī���� ��  
    public Sprite cardImage; //ī���� �̹���
    public TextMeshPro cardText; //ī���� �ؽ�Ʈ

    public void InitCard(int value, Sprite image)
    {
        cardValue = value; //ī���� ��
        cardImage = image; //ī���� �̹���
        
        GetComponent<SpriteRenderer>().sprite = image; //ī���� �̹��� ����

        if(cardText != null)
        {
            cardText.text = cardValue.ToString(); //ī���� �ؽ�Ʈ ����
        }
    }
        
 }
