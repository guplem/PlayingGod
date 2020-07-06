using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Pingu pingu = GetComponent<Pingu>();
        if (pingu)
            FoodManager.instance.Collected(this, pingu);
    }
}
