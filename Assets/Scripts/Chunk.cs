using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector2Int ChunkPos { get; private set; }

    public void SetChunkPos(Vector2Int pos)
    {
        ChunkPos = pos;
        transform.position = new Vector3(ChunkPos.x, 0, ChunkPos.y) * 10;
    }

    public void CreateChunkEnvironment()
    {
        
    }

    public void ResetChunk()
    {

    }
}
