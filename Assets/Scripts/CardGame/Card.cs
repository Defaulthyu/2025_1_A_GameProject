using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardValue; //카드의 값  
    public Sprite cardImage; //카드의 이미지
    public TextMeshPro cardText; //카드의 텍스트

    public void InitCard(int value, Sprite image)
    {
        cardValue = value; //카드의 값
        cardImage = image; //카드의 이미지
        
        GetComponent<SpriteRenderer>().sprite = image; //카드의 이미지 설정

        if(cardText != null)
        {
            cardText.text = cardValue.ToString(); //카드의 텍스트 설정
        }
    }
        
 }
