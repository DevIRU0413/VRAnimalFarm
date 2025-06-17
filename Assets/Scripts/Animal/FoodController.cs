using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float SaveHunger { get; private set; }

    [field: SerializeField] public float EatDuration { get; private set; }
    [SerializeField] private float _detectionRadius;
    
    public void ConsumeFood()
    {
        Destroy(gameObject);
    }
}
