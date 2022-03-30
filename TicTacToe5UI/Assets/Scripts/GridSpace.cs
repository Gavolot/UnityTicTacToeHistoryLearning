using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private GameController gameController;


    public GridSpace upNeighbour = null;
    public GridSpace downNeighbour = null;
    public GridSpace leftNeighbour = null;
    public GridSpace rightNeighbour = null;
    public GridSpace upLeftNeighbour = null;
    public GridSpace upRightNeighbour = null;
    public GridSpace downLeftNeighbour = null;
    public GridSpace downRightNeighbour = null;


    public bool DiagonalUpBlock = false;

    public void ClickButton(string Side, Color afterPressedColor)
    {
        button.interactable = false;
        buttonText.text = Side;
        buttonText.color = afterPressedColor;

        //--
        //--
        //--

        if (buttonText.text == gameController.Player1Side)
        {
            gameController.gridSpacesPlayer1InGame.Add(this);
        }
        if (buttonText.text == gameController.Player2Side)
        {
            gameController.gridSpacesPlayer2InGame.Add(this);
        }
    }

    public void SetColor(Color color)
    {
        buttonText.color = color;
    }

    public bool IsEmpty()
    {
        return buttonText.text == "" ? true : false;
    }

    public void SetSpace()
    {
        ClickButton(gameController.GetPlayerSide(), gameController.GetPlayerColor());
        //--
        //--
        //--


        gameController.EndTurn();
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }
}
