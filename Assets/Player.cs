using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public CardType cardType;
    public int health;
    public int defense;
    public TextMeshPro healthDisplay;
    public TextMeshPro defenseDisplay;

    private void Start()
    {
        UpdateHealthDisplay();
        UpdateDefenseDisplay();
        GameManager.OnTurnStateUpdate.AddListener(OnTurnStateUpdate);
    }

    void OnTurnStateUpdate(GameManager.TurnState turnState){
        if (turnState == GameManager.TurnState.Player) StartOfTurn();
    }

    void StartOfTurn(){
        if (GameManager.TurnCount == 1) for(int i = 0; i < 5; i++) Hand.DrawCard(cardType);
        else Hand.DrawCard(cardType);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) GameManager.EndPlayerTurn();
    }

    public void TakeDamage(int damage){
        if (defense >= damage){
            defense -= damage;
        }
        else{
            damage -= defense;
            health -= damage;
        }

        UpdateHealthDisplay();
        UpdateDefenseDisplay();
    }

    void UpdateHealthDisplay() => healthDisplay.text = health.ToString();
    void UpdateDefenseDisplay() => defenseDisplay.text = defense.ToString();

    public static bool TryPlayCard(){
        if (!Hand.targetCard || !CharacterManager.targetCharacter) return false;
        
        FindObjectOfType<CharacterManager>().AddDamage(CharacterManager.targetCharacter, 1);
        FindObjectOfType<CharacterManager>().StartPlayerSequence();

        return true;
    }
}
