using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float scenarioRadius;
    
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning($"More than one GameManager has been created. {instance.gameObject.name} and {gameObject.name}", gameObject);
        else
            instance = this;
    }

    public Vector3 GetRandomPointInScenario(float minDistanceToCenter = 0)
    {
        Vector2 randomPoint = RandomPoint(transform.position.ToVector2WithoutY(), scenarioRadius, minDistanceToCenter);
        return randomPoint.ToVector3NewY(0f);
    }
    
    private Vector2 RandomPoint(Vector2 origin, float maxRadius, float minRadius = 0)
    {
        var randomDistance = Random.Range(minRadius, maxRadius);
        return origin + Random.insideUnitCircle * randomDistance;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scenarioRadius);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            Time.timeScale += 0.25f;
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Time.timeScale -= 0.25f;
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Time.timeScale = 1;
    }
}
