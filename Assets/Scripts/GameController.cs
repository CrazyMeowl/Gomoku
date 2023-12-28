using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
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
    public int boardSize; // The size of the Gomoku board.
    public int[][] boardState; // The state of the Gomoku board, where 0 is empty, 1 is black, and 2 is white.
    public int currentPlayer; // The current player, where 1 is black and 2 is white.

    public GameObject tile_prefab;
    public GameObject white_stone_prefab;

    public GameObject black_stone_prefab;


    public GameObject window_button;

    public GameObject fullscreen_button;

    public int moveCounter = 0;

    void Start()
    {
        // get the board size from the settings
        boardSize = (int)PlayerPrefs.GetInt("BoardSize");

        // Make the boardsize odd
        if (boardSize % 2 == 0)
        {
            boardSize += 1;
        }
        print("Board Size:" + boardSize.ToString());
        GenerateTileGrid();

        // Create the board.
        boardState = new int[boardSize][];
        for (int i = 0; i < boardSize; i++)
        {
            boardState[i] = new int[boardSize];
        }
        currentPlayer = 1;
        // Create the game object to hold the stones
        GameObject stones = new GameObject("Stones");
        stones.transform.position = Vector3.zero;

        // Check for the state of the screen mode and change the state of the buttons accordingly.
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
        // Check if there is no more move to do and call the game to draw.
        if (moveCounter == boardSize * boardSize)
        {
            End("Draw");
        }
        // Check for Escape Key Event.
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

    // Create the tile to detect mouse movement / slot selection.
    void GenerateTileGrid()
    {
        GameObject tiles = new GameObject("Tiles");

        tiles.transform.position = Vector3.zero;
        int offset = boardSize / 2;

        for (int x = -offset; x <= offset; x++)
        {
            for (int y = -offset; y <= offset; y++)
            {

                GameObject tile = Instantiate(tile_prefab);

                tile.transform.position = new Vector3(x * 1, 1.01f, y * 1);

                tile.transform.parent = tiles.transform;


                string tile_prefix = "";
                if (x == 0 && y == 0)
                {
                    tile_prefix = "C";
                }
                else
                {
                    if (y == -offset)
                    {
                        tile_prefix += "B";
                    }
                    if (y == offset)
                    {
                        tile_prefix += "T";
                    }
                    if (x == -offset)
                    {
                        tile_prefix += "L";
                    }
                    if (x == offset)
                    {
                        tile_prefix += "R";
                    }

                }

                tile.name = $"{tile_prefix}.{y + offset}_{x + offset}";

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
        // if the game is paused
        if (GameIsPaused == false)
        {
            // if the postion of the move is valid
            if (boardState[y][x] == 0)
            {
                boardState[y][x] = currentPlayer;



                // print("Made a move at Y:" + y + ", X:" + x);
                PlaceStone(y, x);
                CheckWinnerNew(y, x);

                // switch the player
                currentPlayer = currentPlayer == 1 ? 2 : 1;
                // Old Check Winner logic
                // if (CheckWinnerOld() == 1)
                // {
                //     End("Black Won");
                // }
                // if (CheckWinnerOld() == 2)
                // {
                //     End("White Won");
                // }

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
            int offset = boardSize / 2;
            x -= offset;
            y -= offset;
            GameObject stones = GameObject.Find("Stones");
            if (currentPlayer % 2 == 0)
            {
                GameObject stone = Instantiate(white_stone_prefab);

                stone.transform.position = new Vector3(x * 1, 1.15f, y * 1);
                stone.transform.rotation = Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-180.0f, 180.0f), 90));
                stone.transform.parent = stones.transform;

                stone.name = $"Stone ({y},{x})";
            }
            else
            {
                GameObject stone = Instantiate(black_stone_prefab);

                stone.transform.position = new Vector3(x * 1, 1.15f, y * 1);
                stone.transform.rotation = Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-180.0f, 180.0f), 90));
                stone.transform.parent = stones.transform;

                stone.name = $"Stone ({y},{x})";
            }

        }

    }

    // Checks if a player has won.
    public int CheckWinnerOld()
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

                if ((player != 0 && (i <= boardSize - 5 && j <= boardSize - 5) && player == boardState[i + 1][j + 1] && player == boardState[i + 2][j + 2] && player == boardState[i + 3][j + 3] && player == boardState[i + 4][j + 4]) ||
                    ((player != 0) && (i <= boardSize - 5 && j >= 4) && player == boardState[i + 1][j - 1] && player == boardState[i + 2][j - 2] && player == boardState[i + 3][j - 3] && player == boardState[i + 4][j - 4]))
                {
                    return player;
                }
            }
        }

        return 0;
    }

    // Checks if a player has won.
    public int CheckWinnerNew(int move_y, int move_x)
    {

        // Set out the limit of the area for checking winner
        int lower_x_limit = 0;
        int upper_x_limit = boardSize - 1;
        int lower_y_limit = 0;
        int upper_y_limit = boardSize - 1;

        if (move_x > 4)
        {
            lower_x_limit = move_x - 5;
        }

        if (move_x < boardSize - 6)
        {
            upper_x_limit = move_x + 5;
        }

        if (move_y > 4)
        {

            lower_y_limit = move_y - 5;
        }
        if (move_y < boardSize - 6)
        {
            upper_y_limit = move_y + 5;
        }
        int diagonal_1_limit = upper_y_limit - lower_y_limit;
        string horizontal = "";
        string vertical = "";
        string diagonal_1 = "";
        string diagonal_2 = "";
        // Get the horizontal line.
        for (int j = lower_x_limit; j < upper_x_limit + 1; j++)
        {
            horizontal += boardState[move_y][j].ToString();
        }
        // Get the vertical line.
        for (int i = lower_y_limit; i < upper_y_limit + 1; i++)
        {
            vertical += boardState[i][move_x].ToString();
        }

        // Get the diagonal lines.
        for (int offset = -5; offset < diagonal_1_limit; offset++)
        {
            try
            {
                // Get the main diagonal line
                diagonal_1 += boardState[move_y + offset][move_x + offset].ToString();
            }
            catch (Exception e)
            {
                string error = e.Message + "Handled";
                // print(e.Message);
            }
            try
            {
                // Get the other diagonal line
                diagonal_2 += boardState[move_y - offset][move_x + offset].ToString();
            }
            catch (Exception e)
            {
                string error = e.Message + "Handled";
                // print(e.Message);
            }


        }


        // F O R  D E B U G  O N L Y 
        // print("BoardSize: " + boardSize);
        // print("Current Player" + currentPlayer.ToString());
        // print("Move Y:" + move_y + ", Move X:" + move_x);
        // print("Lower X: " + lower_x_limit + ", Upper X: " + upper_x_limit);
        // print("Lower Y: " + lower_y_limit + ", Upper Y: " + upper_y_limit);
        // print("Horizontal: " + horizontal);
        // print("Vertical: " + vertical);
        // print("Diagonal_1: " + diagonal_1);
        // print("Diagonal_2: " + diagonal_2);

        string[] check_lines = new string[] { horizontal, vertical, diagonal_1, diagonal_2 };
        // print();

        int otherPlayer = currentPlayer == 1 ? 2 : 1;
        string winString = $"{currentPlayer}{currentPlayer}{currentPlayer}{currentPlayer}{currentPlayer}";
        string blockedWinString = $"{otherPlayer}{currentPlayer}{currentPlayer}{currentPlayer}{currentPlayer}{currentPlayer}{otherPlayer}";
        string[] playerList = new string[] { "None", "Black", "White" };

        foreach (string check_line in check_lines)
        {
            // code block to be executed
            if (check_line.Contains(winString))
            {
                if (!check_line.Contains(blockedWinString))
                {
                    End($"{playerList[currentPlayer]} Won");
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
