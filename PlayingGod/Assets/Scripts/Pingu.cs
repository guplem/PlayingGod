using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Pingu : MonoBehaviour
{
    [SerializeField] private RectTransform radarEnergyImage;

    private Image energyImage
    {
        get
        {
            if (_energyImage == null)
                _energyImage = radarEnergyImage.GetComponent<Image>();
            return _energyImage;
        }
    }
    private Image _energyImage;

    public DNA dna
    {
        get { return _dna;}
        private set
        {
            if (_dna != null)
            {
                Debug.LogError($"RESSETTING DNA OF PINGU {gameObject.name}", gameObject);
                Debug.Break();
            }
            _dna = value;
            Debug.Log($"Setting pingu's DNA: \n{dna.ToString()}", gameObject);
            
            
            radarEnergyImage.SetWidth(dna.sense*2);
            radarEnergyImage.SetHeight(dna.sense*2);
            navMeshAgent.speed = dna.speed;
            MaterialPropertyBlock block = new MaterialPropertyBlock();

            foreach (MeshRenderer mr in meshRenderersToColor)
            {
                block.SetColor(baseColor, dna.color);
                mr.SetPropertyBlock(block);
                Debug.Log("SETTING COLOR " + dna.color);
            }
            
            InvokeRepeating(nameof(Sense), dna.senseFrequency, dna.senseFrequency);
        } 
    }
    private static readonly int baseColor = Shader.PropertyToID("_Color");

    private DNA _dna;
    private Vector3 home;

    private float energy
    {
        get => _energy;
        set
        {
            _energy = value;
            energyImage.fillAmount = 1 / dna.minEnergyToReproduce * _energy;
        }
    }
    private float _energy;
    private NavMeshAgent navMeshAgent;

    public void Init(DNA dna, Vector3 homePosition, float energyForChild)
    {
        this.dna = dna;
        home = homePosition;
        energy = energyForChild;
    }
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (DestinationReached())
        {
            if (Vector3.Distance(navMeshAgent.destination, home) <= 0.05f)
                Breed();

            if (energy >= dna.minEnergyToReproduce)
            {
                Debug.LogWarning($"Enough energy to reproduce but not reching home to breed. Distance = {Vector3.Distance(navMeshAgent.destination, home)}", gameObject);
                navMeshAgent.destination = home;
            }
            
            navMeshAgent.destination = GameManager.instance.GetRandomPointInScenario(0.1f);
        }

        energy -= dna.energyCostPerSecond*Time.deltaTime;
        
        if (energy <= 0)
            Die();
    }

    private Collider sensedFood = null;
    [SerializeField] private List<MeshRenderer> meshRenderersToColor;

    private void Sense()
    {
        List<Collider> hitColliders = Physics.OverlapSphere(transform.position, dna.sense).ToList();

        if (sensedFood != null)
            if (!hitColliders.Contains(sensedFood))
            {
                sensedFood = null;
                navMeshAgent.destination = GameManager.instance.GetRandomPointInScenario(0.1f);
            }
        
        Collider minDisCol = null;
        float minDis = Single.PositiveInfinity;
        foreach (var hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("Food"))
                continue;
            
            var dis = Vector3.Distance(hitCollider.transform.position, transform.position);
            if (dis < minDis)
            {
                minDis = dis;
                minDisCol = hitCollider;
            }
        }

        if (minDisCol && energy < dna.minEnergyToReproduce)
        {
            navMeshAgent.destination = minDisCol.transform.position;
            Debug.DrawLine(transform.position, navMeshAgent.destination, Color.green, dna.senseFrequency);
        }
    }

    private void Die()
    {
        Debug.Log("Pingu dead");
        Destroy(gameObject);
    }

    private void Breed()
    {
        DNA mutatedDNA = dna.GetMutation();
        float energyForChild = energy * dna.percEnergyToReproduce;
        this.energy -= energyForChild;
        PinguManager.instance.SpawnBreeded(mutatedDNA, this, energyForChild);
    }

    public bool DestinationReached()
    {
        if (navMeshAgent.pathPending) return false;
        if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) return false;
        return !navMeshAgent.hasPath || Math.Abs(navMeshAgent.velocity.sqrMagnitude) < 0.001f;
    }

    private void OnDrawGizmosSelected()
    {
        if (navMeshAgent == null) return;
        
        Gizmos.color = Color.green;
        if (navMeshAgent.destination != default)
            Gizmos.DrawLine(navMeshAgent.destination, navMeshAgent.destination+(Vector3.up*4));
        
        Gizmos.color = Color.blue;
            Gizmos.DrawLine(home, home+(Vector3.up*4));
    }

    public void Feed(float energy)
    {
        this.energy += energy;

        if (this.energy >= dna.minEnergyToReproduce)
        {
            navMeshAgent.destination = home;
            Debug.Log($"Enough energy to reproduce. Setting destination to home: {navMeshAgent.destination} (home: {home})", gameObject);
        }
    }
}
