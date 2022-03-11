using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private GameController gameController;

    public void SetSpace()
    {
        button.interactable = false;
        buttonText.text = gameController.GetPlayerSide();
        buttonText.color = gameController.GetPlayerColor();
        gameController.EndTurn();
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }
}
