using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text[] textsList;

    public GridSpace[] gridSpaces;
    private Button[] buttonsList;

    public string playerSide = "X";


    public GameObject GameOverPanel;
    public Text GameOverText;
    public GameObject Panel;
    public Button SetXButton;
    public Button SetOButton;
    public Text SetXText;
    public Text SetOText;

    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
    }

    void ChangePlayerSide()
    {
        playerSide = (playerSide == "X") ? "O" : "X";

        if (playerSide == "X")
        {
            SetXText.color = Color.red;
            SetOText.color = Color.black;
        }
        else
        if (playerSide == "O")
        {
            SetXText.color = Color.black;
            SetOText.color = Color.blue;
        }
    }

    void SetGameControllerReferenceOnButtons()
    {
        buttonsList = new Button[textsList.Length];
        gridSpaces = new GridSpace[textsList.Length];
        for(var i = 0; i < textsList.Length; i++)
        {
            var obj = textsList[i];

            var scr = obj.GetComponentInParent<GridSpace>();
            scr.SetGameController(this);
            gridSpaces[i] = scr;

            var but = obj.GetComponentInParent<Button>();
            buttonsList[i] = but;
            but.interactable = false;
        }

        SetOText.color = Color.black;
        SetXText.color = Color.black;

        GameOverPanel.SetActive(false);
    }

    public void SetXPlayerSide()
    {
        playerSide = "X";
        SetXButton.interactable = false;
        SetOButton.interactable = false;
        for (var i = 0; i < textsList.Length; i++)
        {
            var obj = textsList[i];
            buttonsList[i].interactable = true;
        }
        SetXText.color = Color.red;
    }

    public void SetOPlayerSide()
    {
        playerSide = "O";
        SetXButton.interactable = false;
        SetOButton.interactable = false;
        for (var i = 0; i < textsList.Length; i++)
        {
            var obj = textsList[i];
            buttonsList[i].interactable = true;
        }
        SetOText.color = Color.blue;
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public Color GetPlayerColor()
    {
        return (playerSide == "X") ? Color.red : Color.blue;
    }

    void GameOver()
    {
        for (var i = 0; i < textsList.Length; i++)
        {
            var obj = textsList[i];
            buttonsList[i].interactable = false;
        }
    }

    public void RestartGame()
    {
        for (var i = 0; i < textsList.Length; i++)
        {
            var obj = textsList[i];
            obj.text = "";
        }
        GameOverPanel.SetActive(false);
        SetXButton.interactable = true;
        SetOButton.interactable = true;
        SetXText.color = Color.black;
        SetOText.color = Color.black;
    }

    public void EndTurn()
    {
        bool xWin = false;
        bool oWin = false;

        if(textsList[0].text == playerSide && textsList[1].text == playerSide && textsList[2].text == playerSide)
        {
            if(playerSide == "X")
            {
                xWin = true;
            }
            else if(playerSide == "O")
            {
                oWin = true;
            }
            textsList[0].color = Color.green;
            textsList[1].color = Color.green;
            textsList[2].color = Color.green;
        }
        else
        if (textsList[0].text == playerSide && textsList[3].text == playerSide && textsList[6].text == playerSide)
        {
            if (playerSide == "X")
            {
                xWin = true;
            }
            else if (playerSide == "O")
            {
                oWin = true;
            }
            textsList[0].color = Color.green;
            textsList[3].color = Color.green;
            textsList[6].color = Color.green;
        }
        else
        if (textsList[1].text == playerSide && textsList[4].text == playerSide && textsList[7].text == playerSide)
        {
            if (playerSide == "X")
            {
                xWin = true;
            }
            else if (playerSide == "O")
            {
                oWin = true;
            }
            textsList[1].color = Color.green;
            textsList[4].color = Color.green;
            textsList[7].color = Color.green;
        }
        else
        if (textsList[3].text == playerSide && textsList[4].text == playerSide && textsList[5].text == playerSide)
        {
            if (playerSide == "X")
            {
                xWin = true;
            }
            else if (playerSide == "O")
            {
                oWin = true;
            }
            textsList[3].color = Color.green;
            textsList[4].color = Color.green;
            textsList[5].color = Color.green;
        }
        else
        if (textsList[6].text == playerSide && textsList[7].text == playerSide && textsList[8].text == playerSide)
        {
            if (playerSide == "X")
            {
                xWin = true;
            }
            else if (playerSide == "O")
            {
                oWin = true;
            }
            textsList[6].color = Color.green;
            textsList[7].color = Color.green;
            textsList[8].color = Color.green;
        }
        else
        if (textsList[2].text == playerSide && textsList[5].text == playerSide && textsList[8].text == playerSide)
        {
            if (playerSide == "X")
            {
                xWin = true;
            }
            else if (playerSide == "O")
            {
                oWin = true;
            }
            textsList[2].color = Color.green;
            textsList[5].color = Color.green;
            textsList[8].color = Color.green;
        }
        else
        if (textsList[0].text == playerSide && textsList[4].text == playerSide && textsList[8].text == playerSide)
        {
            if (playerSide == "X")
            {
                xWin = true;
            }
            else if (playerSide == "O")
            {
                oWin = true;
            }
            textsList[0].color = Color.green;
            textsList[4].color = Color.green;
            textsList[8].color = Color.green;
        }
        else
        if (textsList[2].text == playerSide && textsList[4].text == playerSide && textsList[6].text == playerSide)
        {
            if (playerSide == "X")
            {
                xWin = true;
            }
            else if (playerSide == "O")
            {
                oWin = true;
            }
            textsList[2].color = Color.green;
            textsList[4].color = Color.green;
            textsList[6].color = Color.green;
        }
        //===
        if (xWin)
        {
            GameOverText.text = "X Win!";
        }
        else if (oWin)
        {
            GameOverText.text = "O Win!";
        }
        //===
        if (xWin || oWin)
        {
            GameOver();
            GameOverPanel.SetActive(true);
        }
        else
        {
            bool ok = true;
            for (var i = 0; i < textsList.Length; i++)
            {
                var obj = textsList[i];
                if(obj.text == "")
                {
                    ok = false;
                    break;
                }
            }
            if (ok)
            {
                GameOverText.text = "Draw";
                GameOverPanel.SetActive(true);
            }
        }
        //===
        ChangePlayerSide();
    }
}
