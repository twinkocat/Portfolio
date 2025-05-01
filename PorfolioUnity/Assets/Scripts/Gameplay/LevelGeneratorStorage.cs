using System;
using UnityEngine;
using ZLinq;

[CreateAssetMenu(fileName = "LevelGeneratorStorage", menuName = "Portfolio/LevelGeneratorStorage")]
public class LevelGeneratorStorage : ScriptableObject
{
    public Tile[] tiles;
    public Range tilesOnTheLevel = new(5, 9);
    public float tileSize = 40F;


    public Tile GetTile(Direction directionTag)
    {
        return tiles.AsValueEnumerable().FirstOrDefault(t => t.directions.HasFlag(directionTag));
    }
}


[Serializable]
public class Tile
{
    public GameObject tileGo;
    public Direction directions;
}

[Flags]
public enum Direction
{
    None = 0,
    Forward = 1 << 0,
    Back = 1 << 1,
    Right = 1 << 2,
    Left = 1 << 3,
}