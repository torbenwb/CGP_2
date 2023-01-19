using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public Vector3 position = Vector3.zero;
    public Vector3 positionOffset = Vector3.zero;
    public float moveSpeed = 10f;
    public int health;
    public int damage;
    TextMeshPro textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        ToggleTarget(false);
        SetHealth(health);
    }

    public void Act(){
        FindObjectOfType<Player>().TakeDamage(damage);
    }

    void SetHealth(int newAmount){
        health = newAmount;
        textMeshPro.text = newAmount.ToString();
    }

    public void ChangeHealth(int change){
        if (change < 0) CameraShake.Shake();
        SetHealth(health + change);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, position + positionOffset, moveSpeed * Time.deltaTime);
    }

    public void ToggleTarget(bool isTarget){
        transform.localScale = isTarget ? (Vector2.one * 1.2f) : Vector2.one;
    }
}
