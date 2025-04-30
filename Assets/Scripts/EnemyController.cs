using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : NPCcontroller
{
    private WeirdMoveType moveType;

    public float spinSpeed =500f; // 回転スピード
    public enum WeirdMoveType
    {
        SpinMove   //拡張可能、いまは回転する敵のみ
    }

    void Start()
    {
        moveType = WeirdMoveType.SpinMove; 
    }

    void Update()
    {
        if (moveType == WeirdMoveType.SpinMove)
        {
            MoveSpin();
        }
    }


        void MoveSpin()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
    }
}
