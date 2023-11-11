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
        if (probability == 1)
            return list.Last().gameObject;
        int Left = 0, Right = list.Count, Mid;
        while(Left < Right - 1)
        {
            Mid = (Left + Right) / 2;
            if (probability < list[Mid].probability)
                Right = Mid;
            else
            {
                if (probability > list[Mid].probability)
                    Left = Mid;
                else
                    return list[Mid].gameObject;
            }
        }
        return list[Left].gameObject;
    }

    public int CompareTo(GameObjectProbability other)
    {
        return this.probability.CompareTo(other.probability);
    }
}
