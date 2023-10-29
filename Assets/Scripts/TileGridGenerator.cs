using UnityEngine;

public class TileGridGenerator : MonoBehaviour
{
    public GameObject tile_prefab;

    // public int gridSize = 15;

    private void Start()
    {

        GameObject tiles = new GameObject("Tiles");

        tiles.transform.position = Vector3.zero;

        for (int i = -7; i < 8; i++)
        {
            for (int j = -7; j < 8; j++)
            {

                GameObject tile = Instantiate(tile_prefab);

                tile.transform.position = new Vector3(i * 1, 1.01f, j * 1);

                tile.transform.parent = tiles.transform;

                tile.name = $"{i + 7}_{j + 7}";

            }
        }
    }
}
