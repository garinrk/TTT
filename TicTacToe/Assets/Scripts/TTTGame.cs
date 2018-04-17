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

    private bool turnOver = false;

    #endregion



    #region Unity Lifecycle

    // Use this for initialization
    void Start () {
        currentPlayer = Player.PlayerOne;
        turnOver = false;
        boardController.SetTurnText(currentPlayer);
	}

    private void Update()
    {
        if(turnOver)
        {
            EndTurn();
        }
    }


    #endregion


    #region Private Interface

    private void ResetGame()
    {

    }

    private Player WinCheck(Player toCheck)
    {
        Player result = Player.NONE;

        return result;

    }

    private void EndTurn()
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

        turnOver = false;
    
    }

    #endregion

    #region Public Interface
    
    public void SetButtonState(int i_buttonIndex)
    {
        string moveToSet = "";
        ButtonController toControl = boardController.board[i_buttonIndex];

        if (!toControl.isOccupied)
        {
            toControl.SetOccupation(currentPlayer);
            turnOver = true;
        }
        else
        {
            Debug.Log("Square " + i_buttonIndex + " is occupied, invalid move");
        }
        
    }

    #endregion
}
