using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TTTGame : MonoBehaviour {

    #region Unity Serialized Fields

    [SerializeField] private BoardController boardController;

    #endregion

    #region Public Fields

    public Player currentPlayer = Player.NONE;

    #endregion

    #region Private Fields

    private bool gameRunning = true;
    private static int currentChoice = 0;
    private static Player[] currentBoard = null;

    #endregion



    #region Unity Lifecycle

    // Use this for initialization
    void Start () {
        currentPlayer = Player.Human;
        boardController.SetTurnText(currentPlayer);
	}

    private void Update()
    {

    }

    #endregion


    #region Private Interface

    private void ResetGame()
    {

    }

    private bool WinCheck(Player i_player)
    {
        ButtonController[] boardCells = boardController.board;

        if((boardCells[0].occupation == i_player && boardCells[1].occupation == i_player && boardCells[2].occupation == i_player) ||
            (boardCells[3].occupation == i_player && boardCells[4].occupation == i_player && boardCells[5].occupation == i_player) ||
            (boardCells[6].occupation == i_player && boardCells[7].occupation == i_player && boardCells[8].occupation == i_player) ||
            (boardCells[0].occupation == i_player && boardCells[3].occupation == i_player && boardCells[6].occupation == i_player) ||
            (boardCells[1].occupation == i_player && boardCells[4].occupation == i_player && boardCells[7].occupation == i_player) ||
            (boardCells[2].occupation == i_player && boardCells[5].occupation == i_player && boardCells[8].occupation == i_player) ||
            (boardCells[0].occupation == i_player && boardCells[4].occupation == i_player && boardCells[8].occupation == i_player) ||
            (boardCells[2].occupation == i_player && boardCells[4].occupation == i_player && boardCells[6].occupation == i_player)
            )
        {
            return true;
        }
        else
        {
            return false;
        }

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

    private int GetScore(Player[] i_board, Player i_player)
    {

        if (WinCheckWithBoard(i_board, i_player)) return 10; //you won
        else if (WinCheckWithBoard(i_board, GetOpponent(i_player))) return -10; //they won
        else return 0; //draw
    }

    private Player GetOpponent(Player you)
    {
        if (you == Player.Human) return Player.Robot;
        else return Player.Human;
    }


    private void EndTurn()
    {
        //CheckForWin(currentPlayer);

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
                Robot();
            }
        }
    
    }

    private void Robot()
    {
        currentBoard = GetBoard();
        MiniMax(MakeCopy(currentBoard), Player.Robot);
        Debug.Log("Robot chose : " + currentChoice);
    }

    private Player[] MakeCopy(Player[] i_toCopy)
    {
        Player[] copy = new Player[i_toCopy.Length];
        i_toCopy.CopyTo(copy, 0);
        return copy;

    }
    private void CheckForWin(Player i_player)
    {
        if(WinCheck(i_player))
        {
            boardController.SetWinningText(i_player);
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

    private int MiniMax(Player[] i_board, Player i_player)
    {
        //Player[] theBoard = GetBoard();

        //check for terminal state
        int score = GetScore(i_board, i_player);

        if (score != 0)
            return score;
        else if (CheckForGameEnd(i_board))
            return 0;

        List<int> scores = new List<int>();
        List<int> moves = new List<int>();

        //run through possible moves
        for(int i = 0; i < 9; i++)
        {
            //empty space
            if(i_board[i] == Player.NONE)
            {
                scores.Add(MiniMax(MakeMoveAndReturnCopy(i_board, i_player, i), GetOpponent(i_player)));
                moves.Add(i);
            }

        }

        if(i_player == Player.Robot)
        {
            int maxScoreIndex = scores.IndexOf(scores.Max());
            int choice = moves[maxScoreIndex];
            return scores.Max();
        }
        else
        {
            int minScoreIndex = scores.IndexOf(scores.Min());
            int choice = moves[minScoreIndex];
            return scores.Min();
        }

    }

    #endregion

    #region Public Interface
    
    public void MakeMove(int i_buttonIndex)
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

    public Player[] GetBoard()
    {
        ButtonController[] theBoard = boardController.board;

        Player[] result = new Player[9];
        for(int i = 0; i < 9; i++)
        {
            result[i] = theBoard[i].occupation;
        }

        return result;
    }

    #endregion
}
