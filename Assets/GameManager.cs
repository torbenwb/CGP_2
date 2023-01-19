using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    static int turnCount = 0;

    public static int TurnCount{ get => turnCount;}

    private static TurnState turnState = TurnState.Wait;
    public static UnityEvent<TurnState> OnTurnStateUpdate = new UnityEvent<TurnState>();

    public static TurnState GetTurnState() => turnState;

    public enum TurnState
    {
        Wait,
        Player,
        Enemy
    }

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        StartPlayerTurn();
    }

    static void SetTurnState(float time, TurnState nextState){
        if (nextState == turnState) return;
        instance.StartCoroutine(instance.TurnStateTransition(time, nextState));
    }

    IEnumerator TurnStateTransition(float time, TurnState nextState){
        turnState = TurnState.Wait;
        OnTurnStateUpdate.Invoke(turnState);
        yield return new WaitForSeconds(time);
        turnState = nextState;
        OnTurnStateUpdate.Invoke(turnState);
    }

    public static void StartPlayerTurn(){
        turnCount++;
        SetTurnState(0.5f, TurnState.Player);
    }

    public static void EndPlayerTurn(){
        if (turnState == TurnState.Player) StartEnemyTurn();
    }

    public static void StartEnemyTurn(){
        SetTurnState(0.5f, TurnState.Enemy);
    }

    public static void EndEnemyTurn(){
        if (turnState == TurnState.Enemy) StartPlayerTurn();
    }
}
