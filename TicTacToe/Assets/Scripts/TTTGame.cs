using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class TTTGame : MonoBehaviour {

    #region Unity Serialized Fields

    [SerializeField] private BoardController boardController;
    [SerializeField] private float robotChoiceDelay = 1.0f;
    [SerializeField] private int startingDepth = 3;
    [SerializeField] private Dropdown diffDropdown;
    [SerializeField] private Dropdown firstMoveDropdown;
    #endregion

    #region Public Fields

    public Player currentPlayer = Player.Human;

    #endregion

    #region Private Fields

    private bool gameRunning = false;
    private static int currentRobotChoice = 0;
    private static Player[] currentBoardState = null;

    private int humanWins = 0;
    private int robotWins = 0;

    private bool hardMode = true;
    private bool humanFirst = true;
    #endregion



    #region Unity Lifecycle

    // Use this for initialization
    void Start ()
    {

        StartGame();
    }


    #endregion


    #region Private Interface

    private void StartGame()
    {
        gameRunning = true;
        

        switch(humanFirst)
        {
            case true:
                currentPlayer = Player.Human;
                break;
            case false:
                currentPlayer = Player.Robot;
                StartCoroutine(MakeRobotChoiceWithDelay());
                break;
        }

        boardController.SetTurnText(currentPlayer);

    }

    private bool WinCheckWithBoard(Player[] i_board, Player i_player)
    {
        if ((i_board[0] == i_player && i_board[1] == i_player && i_board[2] == i_player) ||
    (i_board[3] == i_player && i_board[4] == i_player && i_board[5] == i_player) ||
    (i_board[6] == i_player && i_board[7] == i_player && i_board[8] == i_player) ||
    (i_board[0] == i_player && i_board[3] == i_player && i_board[6] == i_player) ||
    (i_board[1] == i_player && i_board[4] == i_player && i_board[7] == i_player) ||
    (i_board[2] == i_player && i_board[5] == i_player && i_board[8] == i_player) ||
    (i_board[0] == i_player && i_board[4] == i_player && i_board[8] == i_player) ||
    (i_board[2] == i_player && i_board[4] == i_player && i_board[6] == i_player) )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int GetScore(Player[] i_board, Player i_player, int depth)
    {

        if (WinCheckWithBoard(i_board, i_player)) return 10 + depth; //you won
        else if (WinCheckWithBoard(i_board, GetOpponent(i_player))) return -10 - depth; //they won
        else return 0; //draw
    }

    private Player GetOpponent(Player you)
    {
        if (you == Player.Human) return Player.Robot;
        else return Player.Human;
    }


    private void EndTurn()
    {
        CheckForDraw();
        CheckForWin(currentPlayer);

        if (gameRunning)
        {

            switch (currentPlayer)
            {
                case Player.Human:
                    currentPlayer = Player.Robot;
                    break;
                case Player.Robot:
                    currentPlayer = Player.Human;
                    break;
            }

            boardController.SetTurnText(currentPlayer);

            if(currentPlayer == Player.Robot)
            {
                StartCoroutine(MakeRobotChoiceWithDelay());
            }
        }
    
    }

    private void CheckForDraw()
    {

        if(CheckForGameEnd(GetBoard()))
        {
            gameRunning = false;
            boardController.SetTurnText(Player.NONE);
        }
        
    }

    private Player[] MakeCopy(Player[] i_toCopy)
    {
        Player[] copy = new Player[i_toCopy.Length];
        i_toCopy.CopyTo(copy, 0);
        return copy;

    }
    private void CheckForWin(Player i_player)
    {
        if (WinCheckWithBoard(GetBoard(),i_player))
        {
            boardController.SetWinningText(i_player);
            switch (i_player)
            {
                case Player.Human:
                    humanWins++;
                    boardController.SetScoreText(Player.Human, humanWins);
                    break;
                case Player.Robot:
                    robotWins++;
                    boardController.SetScoreText(Player.Robot, robotWins);
                    break;
            }

            gameRunning = false;
        }

    }


    private bool CheckForGameEnd(Player[] i_board)
    {
        foreach(Player p in i_board)
        {
            //there's an empty spot
            if (p == Player.NONE)
                return false;
        }

        return true;
    }

    private Player[] MakeMoveAndReturnCopy(Player[] i_board, Player currentPlayer, int pos)
    {
        Player[] copy = new Player[i_board.Length];
        i_board.CopyTo(copy, 0);
        copy[pos] = currentPlayer;
        return copy;
    }

    private int MiniMax(Player[] i_board, Player i_player, int i_depth)
    {
       
        Player[] theBoard = MakeCopy(i_board);

        if (i_depth == 0)
        {
            return GetScore(theBoard, Player.Robot, i_depth);
        }

        //check for terminal state
        int score = GetScore(theBoard, Player.Robot, i_depth);

        if (score != 0)
            return score;
        else if (CheckForGameEnd(theBoard))
            return 0;

        List<int> scores = new List<int>();
        List<int> moves = new List<int>();

        //run through possible moves
        for(int i = 0; i < 9; i++)
        {
            //empty space
            if(theBoard[i] == Player.NONE)
            {
                scores.Add(MiniMax(MakeMoveAndReturnCopy(theBoard, i_player, i), GetOpponent(i_player),i_depth - 1));
                moves.Add(i);
            }

        }

        if(i_player == Player.Robot)
        {
            int maxScoreIndex = scores.IndexOf(scores.Max());
            currentRobotChoice = moves[maxScoreIndex];
            return scores.Max();
        }
        else
        {
            int minScoreIndex = scores.IndexOf(scores.Min());
            currentRobotChoice = moves[minScoreIndex];
            return scores.Min();
        }

    }

    private void SetRobotChoice(int i_selection)
    {
        boardController.board[i_selection].SetOccupation(Player.Robot);
    }

    private Player[] GetBoard()
    {
        ButtonController[] theBoard = boardController.board;

        Player[] result = new Player[9];
        for (int i = 0; i < 9; i++)
        {
            result[i] = theBoard[i].occupation;
        }

        return result;
    }

    private void MakeMove(int i_buttonIndex)
    {
        if (gameRunning && currentPlayer == Player.Human)
        {
            ButtonController toControl = boardController.board[i_buttonIndex];

            if (!toControl.isOccupied)
            {
                toControl.SetOccupation(currentPlayer);
                EndTurn();
            }
            else
            {
                Debug.Log("Square " + i_buttonIndex + " is occupied, invalid move");
            }
        }



    }

    IEnumerator MakeRobotChoiceWithDelay()
    {
        yield return new WaitForSecondsRealtime(robotChoiceDelay);

        currentBoardState = GetBoard();
        switch(hardMode)
        {
            case true:
                MiniMax(MakeCopy(currentBoardState), Player.Robot, (int)Difficulty.HARD);
                break;
            case false:
                MiniMax(MakeCopy(currentBoardState), Player.Robot, (int)Difficulty.EASY);
                break;
        }

        SetRobotChoice(currentRobotChoice);
        EndTurn();
    }
    
    private void ResetScores()
    {
        humanWins = 0;
        robotWins = 0;
        boardController.SetScoreText(Player.Robot, robotWins);
        boardController.SetScoreText(Player.Human, humanWins);
    }

    #endregion

    #region Public Interface

    public void ResetGame()
    {
        boardController.ResetBoard();
        StartGame();
    }

    public void OnDifficultyChanged()
    {
        hardMode = (diffDropdown.value == 0) ? true : false;
        ResetGame();
        ResetScores();
    }

    public void OnFirstMoveChanged()
    {
        humanFirst = (firstMoveDropdown.value == 0) ? true : false;
        ResetGame();
        ResetScores();
    }

    #endregion
}
