﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DNA", menuName = "DNA")]
public class DNA : ScriptableObject
{
    [SerializeField] public float mutationPercentage { get; private set; }
    
    [SerializeField] public float speed { get; private set; }
    [SerializeField] public float sense { get; private set; }
    [SerializeField] public float senseFrequency { get; private set; }
    
    [SerializeField] public float percEnergyToReproduce { get; private set; }
    [SerializeField] public float minEnergyToReproduce { get; private set; }

    public Color color
    {
        get
        {
            int steps = 6;
            float hue = speed/1.75f;

            const float saturation = 0.85f;
            const float valueBrightness = 1f;

            Color rgb = Color.HSVToRGB(hue, saturation, valueBrightness, true);
            rgb.a = 1f;
            
            return rgb;
        }
    }

    public float energyCostPerSecond
    {
        get
        {
            if (_energyCostPerSecond < 0)
                _energyCostPerSecond = CalculateEnergyCostPerSecond();
            return _energyCostPerSecond;
        }
    }

    private float CalculateEnergyCostPerSecond()
    {
        float e = speed * 1f + (sense * 1f / senseFrequency);
        return e / 3;
    }

    private float _energyCostPerSecond = -1;

    public void SetValues(float mutationPercentage = 0.2f, float speed = 1f, float sense= 3f, float senseFrequency= 3f, float percEnergyToReproduce = 0.5f, float minEnergyToReproduce = 100f)
    {
        this.mutationPercentage = mutationPercentage;
        this.speed = speed;
        this.sense = sense;
        this.senseFrequency = senseFrequency;
        this.percEnergyToReproduce = percEnergyToReproduce;
        this.minEnergyToReproduce = minEnergyToReproduce;
    }

    public  DNA GetMutation()
    {
        EasyRandom rnd = new EasyRandom();
        
        float mutationMutationPercentage = rnd.GetRandomFloat(1-mutationPercentage, 1+mutationPercentage)*mutationPercentage;
        float mutatedSpeed = rnd.GetRandomFloat(1-mutationPercentage, 1+mutationPercentage)*speed;
        float mutatedSense = rnd.GetRandomFloat(1-mutationPercentage, 1+mutationPercentage)*sense;
        float mutatedSenseFrequency = rnd.GetRandomFloat(1-mutationPercentage, 1+mutationPercentage)*senseFrequency;
        float mutatedPercEnergyToReproduce = rnd.GetRandomFloat(1-mutationPercentage, 1+mutationPercentage)*percEnergyToReproduce;
        float mutatedMinEnergyToReproduce = rnd.GetRandomFloat(1-mutationPercentage, 1+mutationPercentage)*minEnergyToReproduce;
        
        DNA newDna = ScriptableObject.CreateInstance<DNA>();
        newDna.SetValues(mutationMutationPercentage, mutatedSpeed, mutatedSense, mutatedSenseFrequency, mutatedPercEnergyToReproduce, mutatedMinEnergyToReproduce);
        return newDna;
    }


    public override string ToString()
    {
        string str = "";
        str += "mutationPercentage = " + mutationPercentage + "\n";
        str += "speed = " + speed + "\n";
        str += "sense = " + sense + "\n";
        str += "percEnergyToReproduce = " + percEnergyToReproduce + "\n";
        str += "minEnergyToReproduce = " + minEnergyToReproduce + "\n";
        return str;
    }
}
