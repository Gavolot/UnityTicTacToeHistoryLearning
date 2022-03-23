using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private GameController gameController;


    public GridSpace upNeighbour;
    public GridSpace downNeighbour;
    public GridSpace leftNeighbour;
    public GridSpace rightNeighbour;
    public GridSpace upLeftNeighbour;
    public GridSpace upRightNeighbour;
    public GridSpace downLeftNeighbour;
    public GridSpace downRightNeighbour;

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

    public void SetSpace()
    {
        ClickButton(gameController.GetPlayerSide(), gameController.GetPlayerColor());
        //--
        //--
        //--

        if (gameController.with_ai)
        {
            if(gameController.playerSide == buttonText.text)
            {
                gameController.AI_Turn();
                gameController.SetInteractibleAllButtons(false);
            }
        }
        gameController.EndTurn();
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }
}
