using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SwayingFloor : MonoBehaviour
{
    [SerializeField]
    private float minSwayLimit = 0.01f;
    
    [SerializeField]
    private float maxSwayLimit = 0.1f;

    [SerializeField]
    private float swayPerSecond = 0.01f;

    private float _limit;
    private int _swayDirection = -1;

    private void Start()
    {
        GenerateRandomSwayLimit();
    }

    private void Update()
    {
        // TODO: Slide characters around
        if (_swayDirection * transform.rotation.z < _limit)
        {
            SwayToSide();
        }
        else
        {
            // TODO: Pause between direction switches
            _swayDirection *= -1;
            GenerateRandomSwayLimit();
        }
    }

    private void GenerateRandomSwayLimit() => _limit = Random.Range(minSwayLimit, maxSwayLimit);

    private void SwayToSide()
    {
        transform.Rotate(0, 0, swayPerSecond * _swayDirection * Time.deltaTime);
    }
}