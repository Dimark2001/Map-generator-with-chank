using UnityEngine;

public class Chunk : MonoBehaviour
{
    private Vector2 _chunkPos;

    public void SetChunkPos(Vector2 pos)
    {
        _chunkPos = pos;
        transform.position = new Vector3(_chunkPos.x, 0, _chunkPos.y) * 10;
    }

    public void CreateChunkEnvironment()
    {
        
    }

    public void ResetChunk()
    {

    }
}
