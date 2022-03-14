using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject startPointForHorizontalLine;
    public GameObject startPointForVerticalLine;
    public GameObject startPointForGridSpaces;
    public GameObject GridLineHorizontalPrefub;
    public GameObject GridLineVerticalPrefub;
    public GameObject GridLinesContainer;
    public GameObject GridSpacesContainer;
    public GameObject GridSpacePrefub;

    public Text[] textsList;

    public GridSpace[,] gridSpaces;
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
        MakeVerticalGridLines();
        MakeHorizontalGridLines();
        MakeGridSpaces();
        //SetGameControllerReferenceOnButtons();
    }

    void MakeVerticalGridLines()
    {
        var posXStart = startPointForVerticalLine.transform.position.x;
        var posX = posXStart;
        var posY = startPointForVerticalLine.transform.position.y - 82;


        var posXPlus = 58;
        for (int X = 0; X < 9; X++)
        {
            var obj = GameObject.Instantiate(GridLineVerticalPrefub);
            obj.transform.parent = GridLinesContainer.transform;
            obj.transform.position = new Vector3(posX, posY, 0f);
            posX += posXPlus;
        }
    }

    void MakeHorizontalGridLines()
    {
        var posXStart = startPointForHorizontalLine.transform.position.x + 82;
        var posX = posXStart;
        var posY = startPointForHorizontalLine.transform.position.y;


        var posYMinus = 58;
        for (int Y = 0; Y < 9; Y++)
        {
            var obj = GameObject.Instantiate(GridLineHorizontalPrefub);
            obj.transform.parent = GridLinesContainer.transform;
            obj.transform.position = new Vector3(posX, posY, 0f);
            posY -= posYMinus;
        }
    }

    void MakeGridSpaces()
    {
        var posXStart = startPointForGridSpaces.transform.position.x;
        var posX = posXStart;
        var posY = startPointForGridSpaces.transform.position.y;


        var sizeY = 10;
        var sizeX = 10;

        int I = 0;

        buttonsList = new Button[sizeY * sizeX];
        textsList = new Text[sizeY * sizeX];

        gridSpaces = new GridSpace[sizeY, sizeX];

        var posYMinus = 58;
        for (int Y = 0; Y < sizeY; Y++)
        {
            for (int X = 0; X < sizeX; X++)
            {
                var obj = GameObject.Instantiate(GridSpacePrefub);
                obj.transform.parent = GridSpacesContainer.transform;
                obj.transform.position = new Vector3(posX, posY, 0f);
                obj.transform.localScale = new Vector3(0.5f, 0.5f);

                buttonsList[I] = obj.GetComponent<Button>();
                textsList[I] = obj.GetComponentInChildren<Text>();

                //==
                gridSpaces[Y, X] = obj.GetComponent<GridSpace>();
                gridSpaces[Y, X].SetGameController(this);
                //==

                buttonsList[I].interactable = false;
                I++;
                posX += 58;

                
            }
            posY -= posYMinus;
            posX = posXStart;
        }
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
        //gridSpaces = new GridSpace[textsList.Length];
        for(var i = 0; i < textsList.Length; i++)
        {
            var obj = textsList[i];

            var scr = obj.GetComponentInParent<GridSpace>();
            scr.SetGameController(this);
            //gridSpaces[i] = scr;

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

        //Check 5 line win

        //
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
