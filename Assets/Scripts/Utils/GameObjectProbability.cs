using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameObjectProbability : IComparable<GameObjectProbability>
{
    public GameObject gameObject;
    public float probability;
    public static void NormalizeProbabilities(ref List<GameObjectProbability> list)
    {
        float sum = list.Sum(x => x.probability);
        if (list.Count > 1)
            list[0].probability /= sum;
        for(int i = 1; i < list.Count; i++)
        {
            list[i].probability /= sum;
            list[i].probability = list[i - 1].probability + list[i].probability;
        }
        list.Sort();
    }
    public static GameObject GetCorrespondentGameObject(ref List<GameObjectProbability> list, float probability)
    {
        int index = list.BinarySearch(new GameObjectProbability { probability = probability });
        if (index >= 0)
            return list[index].gameObject;
        int insertionPoint = -(~index);
        if (insertionPoint > 0 && insertionPoint < list.Count)
            return list[insertionPoint - 1].gameObject;
        return list.Last().gameObject;
    }

    public int CompareTo(GameObjectProbability other)
    {
        return this.probability.CompareTo(other.probability);
    }
}
