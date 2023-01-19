using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public CardType cardType;
    public Vector3 position;
    public Vector3 positionOffset;
    public float moveSpeed = 10f;

    public TextMeshPro cardCost;
    public TextMeshPro cardName;

    public void SetCardType(CardType cardType){
        cardName.text = cardType.cardName;
        cardCost.text = cardType.manaCost.ToString();
    }

    public CardType GetCardType() => cardType;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, position + positionOffset, moveSpeed * Time.deltaTime);
    }
}
