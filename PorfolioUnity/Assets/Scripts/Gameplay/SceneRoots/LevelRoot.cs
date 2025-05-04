using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZLinq;
using Random = UnityEngine.Random;

public class LevelRoot : MonoBehaviour
{
    [SerializeField] private LevelGeneratorStorage levelGeneratorStorage;
    [SerializeField] private Transform tilesContainer;
    
    private Player playerInstance;
    private Enemy_Skeleton skeletonInstance;
    private UniTask generationTask;
    private UniTask spawnTask;
    
    private void Start()
    {
        playerInstance = Game.Resolver.Resolve<Player>();
        generationTask = GenerateLevelAsync();
        spawnTask = Spawn();
    }

    private UniTask Spawn()
    {
        // Game.Resolver.Resolve<Enemy_Skeleton>().transform.position = MathHelpers.GaussianPointInCircle(3F, 5F).GetX0Z();
        // Game.Resolver.Resolve<Enemy_Skeleton>().transform.position = MathHelpers.GaussianPointInCircle(3F, 10F).GetX0Z();
        // Game.Resolver.Resolve<Enemy_Skeleton>().transform.position = MathHelpers.GaussianPointInCircle(3F, 7.5F).GetX0Z();
        //
        // for (var i = 0; i < 50; i++)
        // {
        //     Game.Resolver.Resolve<Enemy_Skeleton>().transform.position = playerInstance.Position + MathHelpers.GaussianPointInCircle(3F, 7.5F).GetX0Z();
        //     await UniTask.Delay(TimeSpan.FromSeconds(0.2F));
        // }
        return UniTask.CompletedTask;
    }
  
    private async UniTask GenerateLevelAsync()
    {
        tilesContainer.DestroyAllChildren();
        
        await UniTask.DelayFrame(5);
        
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
        var go = Game.Resolver.Instantiate(tile.tileGo, position, Quaternion.identity, tilesContainer);
    }
    
    public void BackToHub()
    {
        Game.SetState<HubState>();
    }
}

