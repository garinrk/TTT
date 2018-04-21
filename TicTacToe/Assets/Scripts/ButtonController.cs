using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonController : MonoBehaviour {

    #region Private Fields

    private Text buttonText;

    #endregion

    #region Public Fields

    public bool isOccupied
    {
        get
        {
            return buttonText.text.Length != 0;
        }
    }
    public Player occupation = Player.NONE;

    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        SetChildTextReference();       
    }

    #endregion

    #region Private Interface

    private void SetChildTextReference()
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        buttonText = children[1].GetComponent<Text>();
    }

    #endregion

    #region Public Interface

    public void Reset()
    {
        buttonText.text = "";
        occupation = Player.NONE;
    }

    public void SetOccupation(Player currentPlayer)
    {
        switch (currentPlayer)
        {
            case Player.Human:
                buttonText.text = "X";
                occupation = currentPlayer;
                break;
            case Player.Robot:
                buttonText.text = "O";
                occupation = currentPlayer;
                break;
            default:
                buttonText.text = "NONE";
                break;
        }

    }

    #endregion
}
