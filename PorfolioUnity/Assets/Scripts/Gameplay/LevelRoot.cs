using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using ZLinq;
using Random = UnityEngine.Random;

public class LevelRoot : MonoBehaviour
{
    [SerializeField] private LevelGeneratorStorage levelGeneratorStorage;
    
    private Player playerInstance;
    private UniTask generationTask;
    
    private void Start()
    {
        playerInstance = Game.Resolver.Resolve<Player>();
        GenerateLevel();
    }

    [ButtonMethod]
    private void GenerateLevel()
    {
        generationTask = GenerateLevelAsync();
    }
    
    private async UniTask GenerateLevelAsync()
    {
        var tilesOnLevel = levelGeneratorStorage.tilesOnTheLevel.GetValueRoundToInt();
        var lenght = levelGeneratorStorage.tileSize;
        var rawPoints = new Vector3[tilesOnLevel];

        rawPoints[0] = Vector3.zero;

        for (var i = 1; i < tilesOnLevel; ++i)
        {
            var direction = Helper.DirectionsVectors[Random.Range(0,  Helper.DirectionsVectors.Length)] * lenght;
            var position = rawPoints[i - 1] + direction;

            if (rawPoints.AsValueEnumerable().Contains(position))
            {
                --i;
                continue;
            }

            rawPoints[i] = position;
        }
        
        await UniTask.Yield();
        
        foreach (var point in rawPoints)
        {
            SpawnTile(point, rawPoints);
        }
    }
    
    private void SpawnTile(Vector3 position, Vector3[] path)
    {
        var result = Direction.None;

        foreach (var point in path)
        {
            if (point == position)
                continue;
            
            var offset = (point - position).normalized;
            
            if (offset == Vector3.forward)    result |= Direction.Forward;
            else if (offset == Vector3.back)  result |= Direction.Back;
            else if (offset == Vector3.left)  result |= Direction.Left;
            else if (offset == Vector3.right) result |= Direction.Right;
        }
        
        var tile = levelGeneratorStorage.GetTile(result);
        var go = Instantiate(tile.tileGo);
        
        go.transform.position = position;
    }
    
    public void BackToHub()
    {
        Game.SetState<HubState>();
    }
}

