using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private GameController gameController;


    public Text upNeighbour;
    public Text downNeighbour;
    public Text leftNeighbour;
    public Text rightNeighbour;
    public Text upLeftNeighbour;
    public Text upRightNeighbour;
    public Text downLeftNeighbour;
    public Text downRightNeighbour;

    public void SetSpace()
    {
        button.interactable = false;
        buttonText.text = gameController.GetPlayerSide();
        buttonText.color = gameController.GetPlayerColor();



        if (buttonText.text == "X")
        {
            gameController.gridSpacesPlayer1InGame.Add(this);
        }
        if (buttonText.text == "O")
        {
            gameController.gridSpacesPlayer2InGame.Add(this);
        }
        gameController.EndTurn();




    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }
}
