using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand instance;
    public static Card targetCard;

    public List<Card> cards;
    public GameObject cardPrefab;
    
    public Vector3 drawPileLocation;
    public float width = 1f;
    public float padding = 0.5f;
    public float targetPadding;
    bool mouseOver = false;
    
    // Idle - do nothing
    // Select - find target card
    // Play - try to play target card
    public enum State{Idle, Select, Play}
    public State state;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseEnter() => mouseOver = true;
    void OnMouseExit() => mouseOver = false;
    

    // Update is called once per frame
    void Update()
    {
        if (cards.Count == 0) return;

        UpdateCardPositions();

        switch (state){
            case State.Idle:
                IdleState();
                break;
            case State.Select:
                SelectState();
                break;
            case State.Play:
                PlayState();
                break;
        }

    }

    void IdleState(){
        targetCard = null;
        if (mouseOver) state = State.Select;
    }

    void SelectState(){
        if (!mouseOver) state = State.Idle;
        else if (Input.GetMouseButtonDown(0)){
            if (GameManager.GetTurnState() != GameManager.TurnState.Player) return;
            state = State.Play;
        }
        else GetTargetCard();
    }

    void PlayState(){
        if (Input.GetMouseButtonUp(0)){
            if (Player.TryPlayCard()) DiscardCard(targetCard);
            state = State.Idle;
        }
        else{
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            targetCard.positionOffset = mousePosition - targetCard.position;
        }
    }

    void UpdateCardPositions(){
        
        for(int i = 0; i < cards.Count; i++) cards[i].position = transform.position + HorizontalLayout.GetPositionInLayout(i, cards.Count, width, padding);
        foreach(Card c in cards) c.positionOffset = Vector3.zero;

        if (targetCard){
            int targetIndex = cards.IndexOf(targetCard);

            if (targetIndex >= 1) cards[targetIndex - 1].positionOffset = Vector3.left * targetPadding;
            if (targetIndex < cards.Count - 1) cards[targetIndex + 1].positionOffset = Vector3.right * targetPadding;
            cards[targetIndex].positionOffset = Vector3.up;
        }
    }

    void SelectCard(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        GetTargetCard();
    }

    public static void DrawCard(CardType cardType = null){
        GameObject newCard = Instantiate(instance.cardPrefab, instance.drawPileLocation, Quaternion.identity);
        newCard.transform.parent = instance.transform;
        Card card = newCard.GetComponent<Card>();
        card.SetCardType(cardType);
        instance.cards.Add(card);
    }

    void DiscardCard(Card card){
        cards.Remove(card);
        Destroy(card.gameObject);
    }

    void GetTargetCard(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        float minDistance = 100f;
        targetCard = null;

        for(int i = 0; i < cards.Count; i++){
            float distance = (cards[i].transform.position - mousePosition).magnitude;
            if (distance <= minDistance){
                minDistance = distance;
                targetCard = cards[i];
            }
        }
    }
}
