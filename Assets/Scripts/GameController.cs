using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pause_game_menu;

    public GameObject in_game_menu;

    public GameObject end_game_menu;

    public GameObject option_menu;
    public TMP_Text winner_text;
    public int boardSize = 15; // The size of the Gomoku board.
    public int[][] boardState; // The state of the Gomoku board, where 0 is empty, 1 is black, and 2 is white.
    public int currentPlayer; // The current player, where 1 is black and 2 is white.
    public GameObject white_stone_prefab;

    public GameObject black_stone_prefab;

    public GameObject window_button;

    public GameObject fullscreen_button;


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
        Debug.Log("Screen Mode: " + Screen.fullScreenMode);
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            fullscreen_button.SetActive(true);
            window_button.SetActive(false);
        }
        else
        {
            fullscreen_button.SetActive(false);
            window_button.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (option_menu.activeSelf)
            {
                option_menu.SetActive(false);
                in_game_menu.SetActive(true);
            }
            else
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }

        }

    }

    public void Restart()
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
        Resume();

    }
    // Makes a move on the board.
    public void MakeMove(int y, int x)
    {
        if (GameIsPaused == false)
        {
            if (boardState[y][x] == 0)
            {
                boardState[y][x] = currentPlayer;

                currentPlayer = currentPlayer == 1 ? 2 : 1;

                print("Made a move at Y:" + y + ", X:" + x);
                PlaceStone(y, x);

                if (CheckWinner() == 1)
                {
                    End("Black Won");
                }
                if (CheckWinner() == 2)
                {
                    End("White Won");
                }
            }
            else
            {
                print("Invalid Move");
            };
        }


    }
    public void PlaceStone(int y, int x)
    {
        if (GameIsPaused == false)
        {
            x -= 7;
            y -= 7;
            GameObject stones = GameObject.Find("Stones");
            if (currentPlayer % 2 != 0)
            {
                GameObject stone = Instantiate(white_stone_prefab);

                stone.transform.position = new Vector3(x * 1, 1.15f, y * 1);
                stone.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180.0f, 180.0f), 0));
                stone.transform.parent = stones.transform;

                stone.name = $"Stone ({y},{x})";
            }
            else
            {
                GameObject stone = Instantiate(black_stone_prefab);

                stone.transform.position = new Vector3(x * 1, 1.15f, y * 1);
                stone.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180.0f, 180.0f), 0));
                stone.transform.parent = stones.transform;

                stone.name = $"Stone ({y},{x})";
            }
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
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int player = boardState[i][j];

                if ((player != 0 && (i <= 10 && j <= 10) && player == boardState[i + 1][j + 1] && player == boardState[i + 2][j + 2] && player == boardState[i + 3][j + 3] && player == boardState[i + 4][j + 4]) ||
                    ((player != 0) && (i <= 10 && j >= 4) && player == boardState[i + 1][j - 1] && player == boardState[i + 2][j - 2] && player == boardState[i + 3][j - 3] && player == boardState[i + 4][j - 4]))
                {
                    return player;
                }
            }
        }

        return 0;
    }

    public void Resume()
    {
        in_game_menu.SetActive(true);
        pause_game_menu.SetActive(false);
        end_game_menu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        in_game_menu.SetActive(false);
        pause_game_menu.SetActive(true);
        end_game_menu.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void End(string input_winner_string)
    {

        winner_text.SetText(input_winner_string);
        in_game_menu.SetActive(false);
        pause_game_menu.SetActive(false);
        end_game_menu.SetActive(true);
        GameIsPaused = true;
    }
    public void Menu()
    {
        Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void ScreenMode(string input_screen_mode)
    {
        Debug.Log("Screen Mode: " + input_screen_mode);

        if (input_screen_mode == "window")
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;

        }
        if (input_screen_mode == "full")
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;


        }

    }
}
