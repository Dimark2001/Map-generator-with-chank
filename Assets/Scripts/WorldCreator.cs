using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class WorldCreator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform poolObjectParent;
    [SerializeField] private Chunk chunkPrefab;
    [Tooltip("Радиус генерации чанков")]
    [Range(1, 9)]
    [SerializeField] private int chunkWidth;
    [Tooltip("Установив значение отличное от нуля, можно получить предсказуемую генерацию")]
    [SerializeField] private int seed = 0;
    private ObjectPool<Chunk> _chunkPool;
    private List<Chunk> _createdChunks;
    private Vector2Int _currentChunkPos;
    private float _randomValue;
    
    private void Awake()
    {
        _currentChunkPos = Vector2Int.zero;
        _chunkPool ??= new ObjectPool<Chunk>(CreateChunk, OnGetChunk, OnReleaseChunk);
        _createdChunks = new List<Chunk>(); 
        
        SetSeed();
        GenerateChunks();
    }

    /// <summary>
    /// Устанавливает стартовое значение сида
    /// </summary>
    private void SetSeed()
    {
        var newSeed = seed;
        if (newSeed == 0)
            newSeed = (int)DateTime.Now.Ticks;
        seed = newSeed;
        Random.InitState(newSeed);
        _randomValue = Random.value;
    }

    /// <summary>
    /// Получает чанки из пула и инизиализизирует их
    /// </summary>
    private void GenerateChunks()
    {
        var chunkPoses = GetNeededPoses();
        foreach (var chunkPos in chunkPoses)
        {
            if(_createdChunks.Any(chunk => chunk.ChunkPos == chunkPos))
                continue;
            var chunk = _chunkPool.Get();
            chunk.Init(chunkPos, _randomValue);
        }
    }

    private Chunk CreateChunk()
    {
        return Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity, poolObjectParent);
    }

    private void OnGetChunk(Chunk chunk)
    {
        chunk.transform.SetParent(transform);
        _createdChunks.Add(chunk);
    }
    
    private void OnReleaseChunk(Chunk chunk)
    {
        chunk.ResetChunk();
        chunk.transform.SetParent(poolObjectParent);
        _createdChunks.Remove(chunk);
    }
    
    /// <summary>
    /// Возвращает список позиций в радиусе, в которых необходимо сгенерировать чанки
    /// </summary>
    private List<Vector2Int> GetNeededPoses()
    {
        var closeTiles = new List<Vector2Int>();
        var rad = chunkWidth;
        var minX = _currentChunkPos.x - rad;
        var minY = _currentChunkPos.y - rad;
        var maxX = _currentChunkPos.x + rad;
        var maxY = _currentChunkPos.y + rad;

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
    
    /// <summary>
    /// Возвращает ближайший к игроку чанк
    /// </summary>
    private Vector2Int GetChunkWithPlayerPosition()
    {
        var pos = new Vector2Int();
        var minDistance = 1000f;
        foreach (var chunk in _createdChunks)
        {
            var distance = Vector3.Distance(player.position, chunk.transform.position);
            if (!(minDistance > distance)) 
                continue;
            minDistance = distance;
            pos = chunk.ChunkPos;
        }
    
        return pos;
    }
    
    /// <summary>
    /// Удаляет чанки, расположенные за пределами радиуса генерации
    /// </summary>
    private void ReleaseChunksOutsideTheRad()
    {
        var chunks = new List<Chunk>(_createdChunks);
        foreach (var pos in GetNeededPoses())
        {
            chunks.Remove(chunks.Find(chunk => chunk.ChunkPos == pos));
        }

        for (var i = chunks.Count - 1; i >= 0; i--)
        {
            var chunk = chunks[i];
            _chunkPool.Release(chunk);
        }
    }
    
    private void Update()
    {
        var currentChunkPos = GetChunkWithPlayerPosition();
        if (_currentChunkPos != currentChunkPos)
        {
            _currentChunkPos = currentChunkPos;
            GenerateChunks();
            ReleaseChunksOutsideTheRad();
        }
    }
}
