using UnityEngine;

public class TileGridGenerator : MonoBehaviour
{
    public GameObject tile_prefab;

    // public int gridSize = 15;

    private void Start()
    {

        GameObject tiles = new GameObject("Tiles");

        tiles.transform.position = Vector3.zero;

        for (int x = -7; x < 8; x++)
        {
            for (int y = -7; y < 8; y++)
            {

                GameObject tile = Instantiate(tile_prefab);

                tile.transform.position = new Vector3(x * 1, 1.01f, y * 1);

                tile.transform.parent = tiles.transform;

                tile.name = $"{y + 7}_{x + 7}";

            }
        }
    }
}
