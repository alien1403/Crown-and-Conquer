using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkProperties", menuName = "New Properties/Chunk")]
public class ChunkPropertiesScriptableObject : ScriptableObject
{
    public enum ChunkType
    {
        Forest,
        Base,
    }
    [SerializeField]
    ChunkType type;
    public ChunkType Type { get => type; private set => type = value; }
}