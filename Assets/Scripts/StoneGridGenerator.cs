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
        for (int i = -7; i < 8; i++)
        {
            for (int j = -7; j < 8; j++)
            {
                if (a % 2 == 0)
                {
                    GameObject stone = Instantiate(white_stone_prefab);

                    stone.transform.position = new Vector3(i * 1, 1.15f, j * 1);

                    stone.transform.parent = stones.transform;

                    stone.name = $"Stone ({i},{j})";
                }
                else
                {
                    GameObject stone = Instantiate(black_stone_prefab);

                    stone.transform.position = new Vector3(i * 1, 1.15f, j * 1);

                    stone.transform.parent = stones.transform;

                    stone.name = $"Stone ({i},{j})";
                }
                a += 1;
            }
        }
    }
}
