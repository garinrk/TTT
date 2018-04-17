using System.Collections;
using System.Collections.Generic;
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

    #endregion



    #region Unity Lifecycle

    // Use this for initialization
    void Start () {
        currentPlayer = Player.PlayerOne;
        boardController.SetTurnText(currentPlayer);
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

    private void EndTurn()
    {
        CheckForWin(currentPlayer);

        if (gameRunning)
        {

            switch (currentPlayer)
            {
                case Player.PlayerOne:
                    currentPlayer = Player.PlayerTwo;
                    break;
                case Player.PlayerTwo:
                    currentPlayer = Player.PlayerOne;
                    break;
            }

            boardController.SetTurnText(currentPlayer);
        }
    
    }


    private void CheckForWin(Player i_player)
    {
        if(WinCheck(i_player))
        {
            boardController.SetWinningText(i_player);
            gameRunning = false;
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

    #endregion
}
