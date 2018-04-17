using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour {

    #region Unity Serialized Fields

    [SerializeField] private ButtonController[] squares;
    [SerializeField] private Text turnText;

    #endregion

    #region Public Fields

    public ButtonController[] board
    {
        get
        {
            return squares;
        }
        
    }

    #endregion    

    #region Public Interface

    public Player GetButtonState(int i_index)
    {
        ButtonController toCheck = squares[i_index];

        return toCheck.occupation;
    }

    public void SetTurnText(Player i_currentPlayer)
    {
        if(i_currentPlayer == Player.PlayerOne)
        {
            turnText.text = Strings.InterfaceText.PLAYER_ONE_TURN;
        }
        else if(i_currentPlayer == Player.PlayerTwo)
        {
            turnText.text = Strings.InterfaceText.PLAYER_TWO_TURN;
        }
    }

    public void SetWinningText(Player winner)
    {
        if(winner == Player.PlayerOne)
        {
            turnText.text = Strings.InterfaceText.PLAYER_ONE_WIN;
        }
        else if(winner == Player.PlayerTwo)
        {
            turnText.text = Strings.InterfaceText.PLAYER_TWO_WIN;
        }
    }



    #endregion
}
