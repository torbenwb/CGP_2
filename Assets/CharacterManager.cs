using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public float width = 1.5f;
    public float padding = 0.5f;
    public static Character targetCharacter;
    public bool getTargetCharacter = false;

    bool sequenceActive = false;
    List<Character> characters = new List<Character>();

    Queue<KeyValuePair<Character, int>> damageQueue = new Queue<KeyValuePair<Character, int>>();
    Queue<Character> deadQueue = new Queue<Character>();

    // Interface
    public List<Character> Characters { get => characters;}

    public void AddDamage(Character character, int damage){
        if (sequenceActive) return;
        if (!character) return;

        damageQueue.Enqueue(new KeyValuePair<Character, int>(character, damage));
    }

    public void StartPlayerSequence(){
        if (!sequenceActive) StartCoroutine(PlayerSequence());
    }

    public void StartCharacterSequence(){
        if (!sequenceActive) StartCoroutine(CharacterSequence());
    }
    //

    private void Start()
    {
        GameManager.OnTurnStateUpdate.AddListener(OnTurnStateUpdate);

        SpawnNewCharacter();
        SpawnNewCharacter();
        SpawnNewCharacter();
    }

    void OnTurnStateUpdate(GameManager.TurnState turnState){
        if (turnState == GameManager.TurnState.Enemy) StartCharacterSequence();
    }

    IEnumerator PlayerSequence(){
        sequenceActive = true;

        while(damageQueue.Count > 0){
            KeyValuePair<Character, int> nextDamage = damageQueue.Dequeue();
            DamageCharacter(nextDamage.Key, nextDamage.Value);
            yield return new WaitForSeconds(0.5f);
        }

        while(deadQueue.Count > 0){
            Character remove = deadQueue.Dequeue();
            RemoveCharacter(remove);
            yield return new WaitForSeconds(0.5f);
        }
        sequenceActive = false;
    }

    IEnumerator CharacterSequence(){
        sequenceActive = true;
        for(int i = 0; i < characters.Count; i++){
            // Each character performs an action
            characters[i].positionOffset = Vector3.down;
            characters[i].Act();
            yield return new WaitForSeconds(0.5f);
            characters[i].positionOffset = Vector3.zero;
        }
        sequenceActive = false;

        GameManager.EndEnemyTurn();
    }

    private void Update()
    {
        if (getTargetCharacter) GetTargetCharacter();
        else targetCharacter = null;

        for(int i = 0; i < characters.Count; i++){
            Vector3 position = HorizontalLayout.GetPositionInLayout(i, characters.Count, width, padding);
            characters[i].position = transform.position + position;
        }

        foreach(Character c in characters) c.ToggleTarget(targetCharacter == c);
    }

    private void OnMouseEnter() => getTargetCharacter = true;
    private void OnMouseExit() => getTargetCharacter = false;

    void GetTargetCharacter(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        float minDistance = 100f;
        targetCharacter = null;
        foreach(Character c in characters){
            float distance = (c.position - mousePosition).magnitude;
            if (distance <= minDistance){
                minDistance = distance;
                targetCharacter = c;
            }
        }
    }

    void SpawnNewCharacter(){
        GameObject newCharacter = Instantiate(characterPrefab);
        newCharacter.name = transform.childCount.ToString();
        newCharacter.transform.parent = transform;
        Character character = newCharacter.GetComponent<Character>();
        characters.Add(character);
    }

    void DamageCharacter(Character character, int damageAmount){
        character.ChangeHealth(-Mathf.Abs(damageAmount));
        if (character.health <= 0) deadQueue.Enqueue(character);
    }

    void RemoveCharacter(Character c){
        Destroy(c.gameObject);
        characters.Remove(c);
    }
}
