using UnityEngine;
using UnityEngine.Pool;

public class WorldCreator : MonoBehaviour
{
    [SerializeField] private Transform poolObjectParent;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Chunk chunkPrefab;
    [SerializeField] private int countChunkByX = 9;
    [SerializeField] private int countChunkByY = 9;
    private ObjectPool<Chunk> _chunkPool;
    
    private void Awake()
    {
        _chunkPool = new ObjectPool<Chunk>(CreatePool, OnGetChunk, OnReleaseChunk, null, true, countChunkByX*countChunkByY);
        
        CreateChunks(Vector2.zero);
    }

    public void CreateChunks(Vector2 chunkWithPlayerPos)
    {
        var x = Mathf.RoundToInt(countChunkByX / 2 + 1);
        var y = Mathf.RoundToInt(countChunkByY / 2 + 1);
        for (var i = -x; i < countChunkByX-x; i++)
        {
            for (var j = -y; j < countChunkByY-y; j++)
            {
                var chunk = _chunkPool.Get();
                chunk.SetChunkPos(new Vector2(i, j));
            }
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
}
