using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMiddle : MonoBehaviour
{
    public LogicScript Logic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Logic.AddScore();
    }
}
