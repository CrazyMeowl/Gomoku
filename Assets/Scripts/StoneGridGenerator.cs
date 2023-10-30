using UnityEngine;

public class StoneGridGenerator : MonoBehaviour
{
    public GameObject white_stone_prefab;
    public GameObject black_stone_prefab;
    // public int gridSize = 15;

    private void Start()
    {

        GameObject stones = new GameObject("Stones");

        stones.transform.position = Vector3.zero;

        int a = 0;
        for (int x = -7; x < 8; x++)
        {
            for (int y = -7; y < 8; y++)
            {
                if (a % 2 == 0)
                {
                    GameObject stone = Instantiate(white_stone_prefab);

                    stone.transform.position = new Vector3(x * 1, 1.15f, y * 1);

                    stone.transform.parent = stones.transform;

                    stone.name = $"Stone ({y},{x})";
                }
                else
                {
                    GameObject stone = Instantiate(black_stone_prefab);

                    stone.transform.position = new Vector3(x * 1, 1.15f, y * 1);

                    stone.transform.parent = stones.transform;

                    stone.name = $"Stone ({y},{x})";
                }
                a += 1;
            }
        }
    }
}
