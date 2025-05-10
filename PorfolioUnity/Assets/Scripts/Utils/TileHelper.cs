using MyBox;
using UnityEngine;


public class TileHelper : MonoBehaviour
{
    [SerializeField] private GameObject[] pieces;
    [SerializeField] private int widthCount;
    [SerializeField] private int lengthCount;
    [SerializeField] private int tileSize;
    

    [ButtonMethod]
    private void GenerateTiles()
    {
        var zero = Vector3.zero;
        for (var i = 0; i < widthCount; i++)
        {
            for (var j = 0; j < lengthCount; j++)
            {
                var go = Instantiate(pieces[Random.Range(0, pieces.Length)], transform);
                zero.x = i * tileSize;
                zero.z = j * tileSize;
                go.transform.position = zero;
            }    
        }
    }
}