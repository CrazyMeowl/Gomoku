using UnityEngine;

public class StoneLogo : MonoBehaviour
{
    public GameObject white_stone_prefab;
    public GameObject black_stone_prefab;
    // public int gridSize = 15;
    public int boardSize = 15;
    public int[,] gameLogo;


    private void Start()
    {
        int[,] gameLogo = new int[15, 15]
        {
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 2, 2, 1 },
            { 1, 1, 1, 2, 2, 1, 1, 1, 2, 2, 1, 1, 2, 2, 2 },//center
            { 2, 1, 1, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 0, 2, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 0 },
            { 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0 },
            { 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0 },
            { 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0 }
        };

        GameObject stones = new GameObject("Stones");

        stones.transform.position = Vector3.zero;

        for (int x = -7; x < 8; x++)
        {
            for (int y = -7; y < 8; y++)
            {
                if (gameLogo[y + 7, x + 7] == 1)
                {
                    GameObject stone = Instantiate(white_stone_prefab);
                    stone.transform.position = new Vector3(x * 1, 1.15f, y * 1);
                    stone.transform.parent = stones.transform;
                    stone.name = $"Stone ({y},{x})";
                }
                if (gameLogo[y + 7, x + 7] == 2)
                {
                    GameObject stone = Instantiate(black_stone_prefab);
                    stone.transform.position = new Vector3(x * 1, 1.15f, y * 1);
                    stone.transform.parent = stones.transform;
                    stone.name = $"Stone ({y},{x})";
                }
                // a += 1;
            }
        }
    }
}
