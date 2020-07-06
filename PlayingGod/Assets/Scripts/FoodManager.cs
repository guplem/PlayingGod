using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodManager : MonoBehaviour
{
    public static FoodManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning($"More than one GameManager has been created. {instance.gameObject.name} and {gameObject.name}", gameObject);
        else
            instance = this;
    }
    
    [SerializeField] private GameObject food;
    [SerializeField] private int maxFoodQtty;
    [SerializeField] private float foodDelayRespawn;
    private Pool foodPool;
    
    private void Start()
    {
        foodPool = new Pool(food, maxFoodQtty, false);

        for (int f = 0; f < maxFoodQtty; f++)
            InstantSpawn();
    }

    private void InstantSpawn()
    {
        foodPool.Spawn(GameManager.instance.GetRandomPointInScenario(), Quaternion.identity, Vector3.one);
    }

    public void Collected(Food food)
    {
        foodPool.Disable(this.food.gameObject);
        StartCoroutine(nameof(InstantSpawn));
    }
    
    IEnumerator DelayedSpawn(Food food) 
    {
        yield return new WaitForSeconds(foodDelayRespawn);
        InstantSpawn();
    }


}
