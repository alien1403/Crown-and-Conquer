using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWoodManager : MonoBehaviour
{
    [SerializeField]
    public int startWoodAmount;
    private int remainingWood;

    private void Start()
    {
        remainingWood = startWoodAmount;
    }

    public void DecrementWood()
    {
        if(remainingWood > 0)
        {
            remainingWood--;
            if(remainingWood == 0)
            {
                Invoke("ResetWoodAmount", 60f);
            }
        }
    }

    void ResetWoodAmount()
    {
        remainingWood = startWoodAmount;
        Debug.Log("Wood amount reset for " + gameObject.name);
    }

    public int GetRemainingWood()
    {
        return remainingWood;
    }
}
