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
    
    [SerializeField] private GameObject pingu;
    [SerializeField] private int initialPinguQtty;
    [SerializeField] private float minDistanceFromCenter;
    [SerializeField] private float startEnergy;

    private void Start()
    {
        for (int f = 0; f < initialPinguQtty; f++)
        {
            Pingu spawnedPingu = InstantSpawn().GetComponent<Pingu>();
            DNA newDna = ScriptableObject.CreateInstance<DNA>();
            newDna.SetValues();
            spawnedPingu.Init(newDna, GameManager.instance.GetRandomPointInScenario(minDistanceFromCenter), startEnergy);
        }
    }

    public void SpawnBreeded(DNA mutatedDna, Pingu pingu, float energyForChild)
    {
        Pingu spawnedPingu = InstantSpawn(pingu.gameObject.transform.position).GetComponent<Pingu>();
        spawnedPingu.Init(mutatedDna, GameManager.instance.GetRandomPointInScenario(minDistanceFromCenter), energyForChild);
        Debug.Log($"New Pingu spawned: {spawnedPingu.gameObject.name}", spawnedPingu.gameObject);
    }
    
    private GameObject InstantSpawn(Vector3 position)
    {
        return Instantiate(pingu, position, Quaternion.identity);
    }
    
    private GameObject InstantSpawn()
    {
        return InstantSpawn(GameManager.instance.GetRandomPointInScenario(minDistanceFromCenter));
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromCenter);
    }


}
