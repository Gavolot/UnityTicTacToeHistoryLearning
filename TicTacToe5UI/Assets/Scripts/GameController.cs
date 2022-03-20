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

    public string playerSide = "";

    public bool with_ai = false;
    public string aiSide = "";

    public string Player1Side = "X";
    public string Player2Side = "O";


    public GameObject GameOverPanel;
    public Text GameOverText;
    public GameObject Panel;
    public Button SetPlayer1Button;
    public Button SetPlayer2Button;
    public Text SetPlayer1Text;
    public Text SetPlayer2Text;

    public List<GridSpace> gridSpacesPlayer1InGame;
    public List<GridSpace> gridSpacesPlayer2InGame;
    public int allGridSpaces = 0;


    private const int _winCount = 5;

    private void Awake()
    {
        SetPlayer1Text.text = Player1Side;
        SetPlayer2Text.text = Player2Side;

        gridSpacesPlayer1InGame = new List<GridSpace>();
        gridSpacesPlayer2InGame = new List<GridSpace>();
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
            //obj.transform.parent = GridLinesContainer.transform;
            obj.transform.SetParent(GridLinesContainer.transform);
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
            obj.transform.SetParent(GridLinesContainer.transform);
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
                allGridSpaces++;
                var obj = GameObject.Instantiate(GridSpacePrefub);
                obj.transform.SetParent(GridSpacesContainer.transform);
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

        for (int Y = 0; Y < sizeY; Y++)
        {
            for (int X = 0; X < sizeX; X++)
            {
                var obj = gridSpaces[Y, X];
                try
                {
                    obj.upNeighbour = gridSpaces[Y - 1, X];
                }
                catch
                {
                    //
                }
                //==
                try
                {
                    obj.downNeighbour = gridSpaces[Y + 1, X];
                }
                catch
                {
                    //
                }
                //==
                try
                {
                    obj.leftNeighbour = gridSpaces[Y, X - 1];
                }
                catch
                {
                    //
                }
                //==
                try
                {
                    obj.rightNeighbour = gridSpaces[Y, X + 1];
                }
                catch
                {
                    //
                }
                //==
                try
                {
                    obj.upLeftNeighbour = gridSpaces[Y - 1, X - 1];
                }
                catch
                {
                    //
                }
                //==
                try
                {
                    obj.upRightNeighbour = gridSpaces[Y - 1, X + 1];
                }
                catch
                {
                    //
                }
                //==
                try
                {
                    obj.downLeftNeighbour = gridSpaces[Y + 1, X - 1];
                }
                catch
                {
                    //
                }
                //==
                try
                {
                    obj.downRightNeighbour = gridSpaces[Y + 1, X + 1];
                }
                catch
                {
                    //
                }
            }
        }
    }

    void ChangePlayerSide()
    {
        playerSide = (playerSide == Player1Side) ? Player2Side : Player1Side;

        if (playerSide == Player1Side)
        {
            SetPlayer1Text.color = Color.red;
            SetPlayer2Text.color = Color.black;
        }
        else
        if (playerSide == Player2Side)
        {
            SetPlayer1Text.color = Color.black;
            SetPlayer2Text.color = Color.blue;
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

        SetPlayer2Text.color = Color.black;
        SetPlayer1Text.color = Color.black;

        GameOverPanel.SetActive(false);
    }

    public void SetXPlayerSide()
    {
        playerSide = Player1Side;
        SetPlayer1Button.interactable = false;
        SetPlayer2Button.interactable = false;
        SetInteractibleAllButtons(true);
        SetPlayer1Text.color = Color.red;
        aiSide = Player2Side;
    }

    public void SetInteractibleAllButtons(bool interact)
    {
        for (var i = 0; i < buttonsList.Length; i++)
        {
            buttonsList[i].interactable = interact;
        }
    }

    public void SetOPlayerSide()
    {
        playerSide = Player2Side;
        SetPlayer1Button.interactable = false;
        SetPlayer2Button.interactable = false;
        SetInteractibleAllButtons(true);
        SetPlayer2Text.color = Color.blue;
        aiSide = Player1Side;
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public Color GetPlayerColor()
    {
        return (playerSide == Player1Side) ? Color.red : Color.blue;
    }

    public void RestartGame()
    {
        for (var i = 0; i < textsList.Length; i++)
        {
            var obj = textsList[i];
            obj.text = "";
        }
        GameOverPanel.SetActive(false);
        SetPlayer1Button.interactable = true;
        SetPlayer2Button.interactable = true;
        SetPlayer1Text.color = Color.black;
        SetPlayer2Text.color = Color.black;

        gridSpacesPlayer1InGame.Clear();
        gridSpacesPlayer2InGame.Clear();
    }

    public void AI_Turn()
    {
        //gameController.SetInteractibleAllButtons(true);
    }


    public void EndTurn()
    {
        bool player1Win = false;
        bool player2Win = false;
        bool isGameOver = false;

        
        if (!with_ai)
        {
            ChangePlayerSide();
        }
        //Check 5 line win
        /*
        if (!player1Win)
        {
            player1Win = CheckRightLineWin(gridSpacesPlayer1InGame, Player1Side);
        }
        if (!player1Win)
        {
            player1Win = CheckLeftLineWin(gridSpacesPlayer1InGame, Player1Side);
        }

        if (!player1Win)
        {
            if (!player2Win)
            {
                player2Win = CheckRightLineWin(gridSpacesPlayer2InGame, Player2Side);
            }
            if (!player2Win)
            {
                player2Win = CheckLeftLineWin(gridSpacesPlayer2InGame, Player2Side);
            }
        }
        */
        player1Win = CheckHorizontalLineWin(gridSpacesPlayer1InGame, Player1Side);
        player2Win = CheckHorizontalLineWin(gridSpacesPlayer2InGame, Player2Side);

        //===
        if (player1Win)
        {
            Debug.Log("1 WIN!");
            GameOverText.text = Player1Side + " Win!";
        }
        else if (player2Win)
        {
            GameOverText.text = Player2Side + " Win!";
        }
        //===
        if (player1Win || player2Win)
        {
            isGameOver = true;
        }
        else
        {
            //Check Draw win condition
            bool ok = false;
            if(gridSpacesPlayer1InGame.Count + gridSpacesPlayer2InGame.Count == allGridSpaces)
            {
                ok = true;
            }
            if (ok)
            {
                isGameOver = true;
                GameOverText.text = "Draw";
            }
        }
        //===


        if (isGameOver)
        {
            SetInteractibleAllButtons(false);
            GameOverPanel.SetActive(true);
        }
    }

    #region CheckLines
    public bool CheckHorizontalLineWin(List<GridSpace> playerSpacesList, string PLAYER_SIDE)
    {
        int __I = 0;
        foreach (var obj in playerSpacesList)
        {
            if (obj.buttonText.text == PLAYER_SIDE)
            {
                __I++;
                var target = obj.rightNeighbour;
                if (target != null)
                {
                    for (var i = 0; i < _winCount; i++)
                    {
                        if (target != null)
                        {
                            if (target.buttonText.text == PLAYER_SIDE)
                            {
                                __I++;
                                if (__I == _winCount)
                                {
                                    return true;
                                }
                                target = target.rightNeighbour;
                            }
                            else
                            {
                                __I = 0;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    __I = 0;
                }
            }
        }

        return false;
    }
    #endregion
}
