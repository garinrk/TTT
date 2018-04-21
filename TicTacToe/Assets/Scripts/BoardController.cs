using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour {

    #region Unity Serialized Fields

    [SerializeField] private ButtonController[] cells;
    [SerializeField] private Text currentTurnLabel;
    [SerializeField] private Text humanScoreLabel;
    [SerializeField] private Text robotScoreLabel;
    

    #endregion

    #region Public Fields

    public ButtonController[] board
    {
        get
        {
            return cells;
        }
        
    }

    #endregion    

    #region Public Interface

    public Player GetButtonState(int i_index)
    {
        ButtonController toCheck = cells[i_index];

        return toCheck.occupation;
    }

    public void ResetBoard()
    {
        foreach (ButtonController b in cells)
        {
            b.Reset();
        }
    }

    public void SetTurnText(Player i_currentPlayer)
    {
        if(i_currentPlayer == Player.Human)
        {
            currentTurnLabel.text = Strings.InterfaceText.HUMAN_TURN;
            currentTurnLabel.color = Color.green;
        }
        else if(i_currentPlayer == Player.Robot)
        {
            currentTurnLabel.text = Strings.InterfaceText.ROBOT_TURN;
            currentTurnLabel.color = Color.red;
        }
        else if(i_currentPlayer == Player.NONE)
        {
            currentTurnLabel.text = Strings.InterfaceText.DRAW;
            currentTurnLabel.color = Color.white;
        }
    }

    public void SetWinningText(Player winner)
    {
        if (winner == Player.Human)
        {
            currentTurnLabel.text = Strings.InterfaceText.HUMAN_WIN;
            currentTurnLabel.color = Color.green;
        }
        else
        {
            currentTurnLabel.text = Strings.InterfaceText.ROBOT_WIN;
            currentTurnLabel.color = Color.red;
        }
    }

    public void SetScoreText(Player i_ToSet, int i_score)
    {
        if (i_ToSet == Player.Human)
        {
            humanScoreLabel.text = i_score.ToString();
        }
        else
        {
            robotScoreLabel.text = i_score.ToString();
        }
    }
    
    #endregion
}
