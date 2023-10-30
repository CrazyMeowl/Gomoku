using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public int boardSize = 15; // The size of the Gomoku board.
    public int[][] boardState; // The state of the Gomoku board, where 0 is empty, 1 is black, and 2 is white.
    public int currentPlayer; // The current player, where 1 is black and 2 is white.
    public GameObject white_stone_prefab;
    public GameObject black_stone_prefab;

    void Start()
    {
        boardState = new int[boardSize][];
        for (int i = 0; i < boardSize; i++)
        {
            boardState[i] = new int[boardSize];
        }
        currentPlayer = 1;

        GameObject stones = new GameObject("Stones");

        stones.transform.position = Vector3.zero;
    }
    void Restart()
    {
        // Reset Player
        currentPlayer = 1;

        // Remove the stones
        GameObject stones = GameObject.Find("Stones");
        for (int i = stones.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(stones.transform.GetChild(i).gameObject);
        }

        //Reset the board data
        boardState = new int[boardSize][];
        for (int i = 0; i < boardSize; i++)
        {
            boardState[i] = new int[boardSize];
        }

    }
    // Makes a move on the board.
    public void MakeMove(int y, int x)
    {

        if (boardState[y][x] == 0)
        {
            boardState[y][x] = currentPlayer;
            currentPlayer = currentPlayer == 1 ? 2 : 1;
            print("Made a move at Y:" + y + ", X:" + x);
            PlaceStone(y, x);
            print(CheckWinner());
            if (CheckWinner() == 1 || CheckWinner() == 2)
            {
                Restart();
            }
        }
        else
        {
            print("Invalid Move");
        };

    }
    public void PlaceStone(int y, int x)
    {
        x -= 7;
        y -= 7;
        GameObject stones = GameObject.Find("Stones");
        if (currentPlayer % 2 != 0)
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
    }

    // Checks if a player has won.
    public int CheckWinner()
    {
        // Check for horizontal wins.
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize - 4; j++)
            {
                int player = boardState[i][j];
                if (player != 0 && player == boardState[i][j + 1] && player == boardState[i][j + 2] && player == boardState[i][j + 3] && player == boardState[i][j + 4])
                {
                    return player;
                }
            }
        }

        // Check for vertical wins.
        for (int i = 0; i < boardSize - 4; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int player = boardState[i][j];
                if (player != 0 && player == boardState[i + 1][j] && player == boardState[i + 2][j] && player == boardState[i + 3][j] && player == boardState[i + 4][j])
                {
                    return player;
                }
            }
        }

        // Check for diagonal wins.
        for (int i = 0; i < boardSize - 4; i++)
        {
            for (int j = 0; j < boardSize - 4; j++)
            {
                int player = boardState[i][j];
                if (player != 0 && player == boardState[i + 1][j + 1] && player == boardState[i + 2][j + 2] && player == boardState[i + 3][j + 3] && player == boardState[i + 4][j + 4])
                {
                    return player;
                }
            }
        }

        return 0;
    }
}
