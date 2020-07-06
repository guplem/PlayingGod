using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinguManager : MonoBehaviour
{
    public static PinguManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning($"More than one PenguinManager has been created. {instance.gameObject.name} and {gameObject.name}", gameObject);
        else
            instance = this;
    }
    
    [SerializeField] private GameObject penguin;
    [SerializeField] private int maxPinguQtty;
    [SerializeField] private float minDistanceFromCenter;
    private Pool pinguPool;
    
    private void Start()
    {
        pinguPool = new Pool(penguin, maxPinguQtty, false);

        for (int f = 0; f < maxPinguQtty; f++)
            InstantSpawn();
    }

    private void InstantSpawn()
    {
        pinguPool.Spawn(GameManager.instance.GetRandomPointInScenario(minDistanceFromCenter), Quaternion.identity, Vector3.one);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromCenter);
    }

}
