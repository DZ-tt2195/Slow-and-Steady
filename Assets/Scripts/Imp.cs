using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    public override void InitialStats()
    {
        this.gameObject.SetActive(true);
        this.health = 1;
        this.speed = Random.Range(200f, 250f);
    }
}
