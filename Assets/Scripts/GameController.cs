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

    public static bool GameIsThinking = false;

    public GameObject pause_game_menu;

    public GameObject in_game_menu;

    public GameObject end_game_menu;

    public GameObject option_menu;
    public TMP_Text winner_text;
    public int boardSize; // The size of the Gomoku board.
    public int[,] boardState; // The state of the Gomoku board, where 0 is empty, 1 is black, and 2 is white.
    public int currentPlayer; // The current player, where 1 is black and 2 is white.

    public GameObject tile_prefab;
    public GameObject white_stone_prefab;

    public GameObject black_stone_prefab;


    public GameObject window_button;

    public GameObject fullscreen_button;

    public int moveCounter = 0;

    public int maxDepth = 5;

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

        // Create the board
        boardState = new int[boardSize, boardSize];

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
        moveCounter = 0;

        // Remove the stones
        GameObject stones = GameObject.Find("Stones");
        for (int i = stones.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(stones.transform.GetChild(i).gameObject);
        }

        //Reset the board data
        boardState = new int[boardSize, boardSize];
        Resume();

    }
    // Makes a move on the board.
    public int MakeMove(int player_y, int player_x)
    {
        int win_state = 0;
        int draw_state = 0;
        // if the game is paused
        if (GameIsPaused == false || GameIsThinking == true)
        {
            // if the postion of the move is valid
            if (boardState[player_y, player_x] == 0)
            {
                boardState[player_y, player_x] = currentPlayer;
                PlaceStone(player_y, player_x);
                moveCounter++;
                win_state = CheckWinner(player_y, player_x);
                draw_state = CheckDraw();
                // Stop the game if the game is done or draw
                if (win_state == 1 || draw_state == 1)
                {
                    return 1;
                }
                // print("Made a move at Y:" + y + ", X:" + x);

                // switch the player
                currentPlayer = currentPlayer == 1 ? 2 : 1;

                // Make a bot movement
                int bot_y;
                int bot_x;

                GetBotMove(player_y, player_x, out bot_y, out bot_x);
                boardState[bot_y, bot_x] = currentPlayer;
                PlaceStone(bot_y, bot_x);
                moveCounter++;
                win_state = CheckWinner(bot_y, bot_x);
                draw_state = CheckDraw();
                if (win_state == 1 || draw_state == 1)
                {
                    return 1;
                }
                // switch the player again ...
                currentPlayer = currentPlayer == 1 ? 2 : 1;


            }
            else
            {
                print("Invalid Move");
                return 0;
            };
        }
        return 0;
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

    // Start bot think
    public void GetBotMove(int move_y, int move_x, out int bot_y, out int bot_x)
    {

        // UnityEngine.Random.Range(0, boardSize)
        bot_y = UnityEngine.Random.Range(0, boardSize);
        bot_x = UnityEngine.Random.Range(0, boardSize);

        print($"Player Y {move_y}, Player X: {move_x} ");
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

        string line_string = "";
        int max_score = -9999;
        int max_y = 0;
        int max_x = 0;
        for (int y_counter = lower_y_limit; y_counter <= upper_y_limit; y_counter++)
        {
            for (int x_counter = lower_x_limit; x_counter <= upper_x_limit; x_counter++)
            {
                line_string += boardState[y_counter, x_counter].ToString() + " ";
                // if empty slot then calculate the point of the move
                if (boardState[y_counter, x_counter] == 0)
                {
                    int move_score = CalculateMove(y_counter, x_counter);
                    if (move_score > max_score)
                    {
                        max_score = move_score;
                        max_y = y_counter;
                        max_x = x_counter;
                    }
                }
            }
            line_string += "\n";
        }
        print(line_string);
        print("Max score: " + max_score);
        print($"Y: {max_y}, X: {max_x}");
        bot_y = max_y;
        bot_x = max_x;
    }

    // Player play as black
    // 
    // Calculate the point of the move
    public int CalculateMove(int move_y, int move_x)
    {
        // Defend calculation
        int[,] temp_boardstate_1 = boardState.Clone() as int[,];
        temp_boardstate_1[move_y, move_x] = 1;
        string[] check_lines = GetLines(temp_boardstate_1, move_y, move_x,3);
        int total_score = -20;
        // loop through the lines in check_lines
        foreach (string check_line in check_lines)
        {
            int line_score = 0;
            if (check_line.Contains("11111"))
            {
                if (check_line.Contains("2111112"))
                {
                    line_score = Math.Max(line_score, 0);

                }
                else
                {
                    line_score = Math.Max(line_score, 99999);

                }
            }
            if (check_line.Contains("1111"))
            {
                if (check_line.Contains("211112"))
                {
                    line_score = Math.Max(line_score, 0);

                }
                else
                {
                    line_score = Math.Max(line_score, 9999);

                }

            }
            if (check_line.Contains("111"))
            {
                if (check_line.Contains("21112"))
                {
                    line_score = Math.Max(line_score, 0);

                }
                else
                {
                    line_score = Math.Max(line_score, 999);

                }
            }
            if (check_line.Contains("11"))
            {
                line_score = Math.Max(line_score, 99);

            }
            if (check_line.Contains("1"))
            {
                line_score = Math.Max(line_score, 9);
            }

            total_score += line_score;
        }

        // Attack score calculation
        int[,] temp_boardstate_2 = boardState.Clone() as int[,];
        temp_boardstate_2[move_y, move_x] = 2;
        check_lines = GetLines(temp_boardstate_2, move_y, move_x, 3);

        // loop through the lines in check_lines
        foreach (string check_line in check_lines)
        {
            int line_score = 0;
            if (check_line.Contains("22222"))
            {
                if (check_line.Contains("1222221"))
                {
                    line_score = Math.Max(line_score, 0);

                }
                else
                {
                    line_score = Math.Max(line_score, 99999);

                }
            }
            if (check_line.Contains("2222"))
            {
                if (check_line.Contains("122221"))
                {
                    line_score = Math.Max(line_score, 0);

                }
                else
                {
                    line_score = Math.Max(line_score, 9999);

                }

            }
            if (check_line.Contains("222"))
            {
                if (check_line.Contains("12221"))
                {
                    line_score = Math.Max(line_score, 0);

                }
                else
                {
                    line_score = Math.Max(line_score, 999);

                }
            }
            if (check_line.Contains("22"))
            {
                line_score = Math.Max(line_score, 99);

            }
            if (check_line.Contains("2"))
            {
                line_score = Math.Max(line_score, 9);
            }

            total_score += line_score;
        }
        
        return total_score;
    }
    // Check draw 
    public int CheckDraw()
    {
        if (moveCounter != boardSize * boardSize - 1)
        {
            return 0;
        }
        else
        {
            End("Draw");
            return 1;
        }
    }

    // Get horizontal, vertical, diagonal lines of the point
    public string[] GetLines(int[,] check_board, int move_y, int move_x, int check_range)
    {

        // Set out the limit of the area for checking winner
        int lower_x_limit = 0;
        int upper_x_limit = boardSize - 1;
        int lower_y_limit = 0;
        int upper_y_limit = boardSize - 1;

        if (move_x >= 5 + check_range)
        {
            lower_x_limit = move_x - ( 5+ check_range);
        }

        if (move_x <= boardSize - (6 + check_range))
        {
            upper_x_limit = move_x + ( 5 + check_range);
        }

        if (move_y >= 5 + check_range)
        {
            lower_y_limit = move_y - (5 + check_range);
        }
        if (move_y < boardSize - (6 + check_range))
        {
            upper_y_limit = move_y + (5 + check_range);
        }
        int diagonal_1_limit = upper_y_limit - lower_y_limit;
        string horizontal = "";
        string vertical = "";
        string diagonal_1 = "";
        string diagonal_2 = "";
        // Get the horizontal line.
        for (int j = lower_x_limit; j <= upper_x_limit; j++)
        {
            horizontal += check_board[move_y, j].ToString();
        }
        // Get the vertical line.
        for (int i = lower_y_limit; i <= upper_y_limit; i++)
        {
            vertical += check_board[i, move_x].ToString();
        }

        // Get the diagonal lines.
        for (int offset = -5; offset < diagonal_1_limit; offset++)
        {
            try
            {
                // Get the main diagonal line
                diagonal_1 += check_board[move_y + offset, move_x + offset].ToString();
            }
            catch (Exception e)
            {
                string error = e.Message + "Handled";
                // print(e.Message);
            }
            try
            {
                // Get the other diagonal line
                diagonal_2 += check_board[move_y - offset, move_x + offset].ToString();
            }
            catch (Exception e)
            {
                string error = e.Message + "Handled";
                // print(e.Message);
            }


        }

        string[] check_lines = new string[] { horizontal, vertical, diagonal_1, diagonal_2 };


        return check_lines;
    }
    // Checks if a player has won.
    public int CheckWinner(int move_y, int move_x)
    {
        string[] check_lines = GetLines(boardState, move_y, move_x,0);
        int otherPlayer = currentPlayer == 1 ? 2 : 1;
        string winString = $"{currentPlayer}{currentPlayer}{currentPlayer}{currentPlayer}{currentPlayer}";
        string blockedWinString = $"{otherPlayer}{currentPlayer}{currentPlayer}{currentPlayer}{currentPlayer}{currentPlayer}{otherPlayer}";
        string[] playerList = new string[] { "None", "Black", "White" };
        // loop through the lines in check_lines
        foreach (string check_line in check_lines)
        {
            // Check if the have 5 in a row
            if (check_line.Contains(winString))
            {
                // Check if the winning line is blocked
                if (!check_line.Contains(blockedWinString))
                {
                    End($"{playerList[currentPlayer]} Won");
                    return currentPlayer;
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


