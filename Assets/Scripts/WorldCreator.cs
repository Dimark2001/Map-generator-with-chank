using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class WorldCreator : MonoBehaviour
{
    [SerializeField] private Transform poolObjectParent;
    [SerializeField] private Chunk chunkPrefab;
    [SerializeField] private int chunkWidth = 9;
    [SerializeField] private int seed = 0;
    private ObjectPool<Chunk> _chunkPool;

    private void Awake()
    {
        SetSeed();
        _chunkPool = new ObjectPool<Chunk>(CreatePool, OnGetChunk, OnReleaseChunk);
        CreateChunks(Vector2Int.zero);
    }

    [Button()]
    private void SetSeed()
    {
        var newSeed = seed;
        if (newSeed == 0)
            newSeed = (int)DateTime.Now.Ticks;
        
        Random.InitState(newSeed);
    }

    public void CreateChunks(Vector2Int chunkWithPlayerPos)
    {
        var chunkPoses = GetNeededPoses(chunkWithPlayerPos);
        foreach (var chunkPos in chunkPoses)
        {
            var chunk = _chunkPool.Get();
            chunk.SetChunkPos(chunkPos);
        }
    }

    private Chunk CreatePool()
    {
        return Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity, poolObjectParent);
    }

    private void OnGetChunk(Chunk chunk)
    {
        chunk.CreateChunkEnvironment();
        chunk.transform.SetParent(transform);
    }
    
    private void OnReleaseChunk(Chunk chunk)
    {
        chunk.ResetChunk();
        chunk.transform.SetParent(poolObjectParent);
    }

    private List<Vector2Int> GetNeededPoses(Vector2Int pos)
    {
        var closeTiles = new List<Vector2Int>();
        var rad = chunkWidth/2;
        var minX = pos.x - rad;
        var minY = pos.y - rad;
        var maxX = pos.x + rad;
        var maxY = pos.y + rad;

        for (var x = minX; x <= maxX; x++)
        {
            for (var y = minY; y <= maxY; y++)
            {
                var newPos = new Vector2Int(x, y);
                if(!closeTiles.Contains(newPos))
                    closeTiles.Add(newPos);
            }
        }
        
        return closeTiles;
    }
}
