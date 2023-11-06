using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private Transform environmentParent;
    [SerializeField] private List<Transform> environmentPoses;
    [SerializeField] private List<GameObject> environmentObjects;
    
    public Vector2Int ChunkPos { get; private set; }
    private float _randomValue;
    
    /// <summary>
    /// Устанавливает основные параметры чанка
    /// </summary>
    public void Init(Vector2Int pos, float randomValue)
    {
        _randomValue = randomValue;
        ChunkPos = pos;
        transform.position = new Vector3(ChunkPos.x, 0, ChunkPos.y) * 10;
        ShowChunk();
    }
    
    private void ShowChunk()
    {
        gameObject.SetActive(true);
        transform.DOScale(new Vector3(1, 1, 1), 0.2f);
        GenerateChunkEnvironment();
    } // Функция появления
    
    /// <summary>
    /// Генерирует наполнение чанка
    /// </summary>
    private void GenerateChunkEnvironment()
    {
        for (var i = 0; i < environmentPoses.Count; i++)
        {
            var environmentPose = environmentPoses[i];
            var x = ChunkPos.x + 0.1f;
            var y = ChunkPos.y + 0.1f;
            var value = _randomValue * 10 * (x * y) * (y / x + 1) * i;
            value = Mathf.Abs(value);
            for (var j = 0; j < 100; j++)
            {
                if (value < 8)
                    break;
                value /= 10;
            }
            
            environmentObjects[Mathf.RoundToInt(value)].transform.position = environmentPose.position;
            switch (value)
            {
                case <= 1:
                    break;
                case <= 2:
                    environmentObjects[i].SetActive(true);
                    break;
                case <= 3:
                    break;
                case <= 4:
                    environmentObjects[i].SetActive(true);
                    break;
                case <= 5:
                    break;
                case <= 6:
                    break;
                case <= 7:
                    environmentObjects[i].SetActive(true);
                    break;
                case <= 8:
                    break;
                case <= 9:
                    environmentObjects[i].SetActive(true);
                    break;
            }
        }

        if (environmentObjects.FindAll(o => o.activeSelf).Count < 3)
        {
            wall.SetActive(true);
            wall.transform.rotation = Quaternion.Euler(new Vector3(0, _randomValue * 100 * ChunkPos.x * ChunkPos.y, 0));
        }
    }
    /// <summary>
    /// Убирает сгенерированное наполнение
    /// </summary>
    public void ResetChunk()
    {
        gameObject.SetActive(false);
        wall.SetActive(false);
        foreach (var environmentObject in environmentObjects)
        {
            environmentObject.SetActive(false);
        }
    }
}
