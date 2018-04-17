using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonController : MonoBehaviour {

    #region Private Fields

    private Text buttonText;

    #endregion

    #region Public Fields

    public Player occupation
    {
        get
        {
            if(buttonText.text.Equals("X"))
            {
                return Player.PlayerOne;
            }
            else if(buttonText.Equals("O"))
            {
                return Player.PlayerTwo;
            }
            else
            {
                return Player.NONE;
            }
        }
    }

    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        buttonText = children[1].GetComponent<Text>();
        
    }

    #endregion

    public void SetOccupation(Player currentPlayer)
    {
        switch (currentPlayer)
        {
            case Player.PlayerOne:
                buttonText.text = "X";
                break;
            case Player.PlayerTwo:
                buttonText.text = "O";
                break;
            default:
                buttonText.text = "NONE";
                break;
        }

    }
}
