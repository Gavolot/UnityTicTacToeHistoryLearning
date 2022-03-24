using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public List<GridSpace> allGridSpacesList;
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

    public int steps = 0;

    public int boardSizeY = 10;
    public int boardSizeX = 10;


    private const int _winCount = 5;

    private void Awake()
    {
        Random.seed = System.Environment.TickCount;
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
        allGridSpacesList = new List<GridSpace>();
        var posXStart = startPointForGridSpaces.transform.position.x;
        var posX = posXStart;
        var posY = startPointForGridSpaces.transform.position.y;


        var sizeY = boardSizeY;
        var sizeX = boardSizeX;

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
                allGridSpacesList.Add(obj.GetComponent<GridSpace>());
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
        SetInteractibleAllNoEmptyButtons(true);
        SetPlayer1Text.color = Color.red;
        aiSide = Player2Side;
    }

    public void SetInteractibleAllNoEmptyButtons(bool interact)
    {
        for (var i = 0; i < buttonsList.Length; i++) {

            if (textsList[i].text == "")
            {
                buttonsList[i].interactable = interact;
            }
        }
    }

    public void SetOPlayerSide()
    {
        playerSide = Player2Side;
        SetPlayer1Button.interactable = false;
        SetPlayer2Button.interactable = false;
        SetInteractibleAllNoEmptyButtons(true);
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

    public Color GetAIColor()
    {
        return (aiSide == Player1Side) ? Color.red : Color.blue;
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
        allGridSpacesList.Clear();
    }

    #region AI_Logic
    List<GridSpace> lineHorizontalCheck = new List<GridSpace>();
    List<GridSpace> allTargets = new List<GridSpace>();

    private void AI_Click_And_End(GridSpace button)
    {
        button.ClickButton(aiSide, GetAIColor());
        SetInteractibleAllNoEmptyButtons(true);
    }

    private void TryAddTarget(List<GridSpace> list, params GridSpace[] buttons)
    {
        for(int i = 0; i < buttons.Length; i++) {
            var obj = buttons[i];
            if (obj != null)
            {
                list.Add(obj);
            }
        }
    }

    private GridSpace ChooseTarget(List<GridSpace> list)
    {
        GridSpace res = null;
        var rnd = UnityEngine.Mathf.FloorToInt(Random.Range(0, list.Count));

        res = list[rnd];

        return res;
    }

    private bool AI_ChooseTargetOnBoardAndClick()
    {
        bool isClicked = false;
        while (!isClicked)
        {
            var obj = ChooseTarget(allGridSpacesList);
            if (obj.IsEmpty())
            {
                isClicked = true;
                AI_Click_And_End(obj);
                return isClicked;
            }
        }
        return isClicked;
    }
    public void AI_Turn()
    {
        SetInteractibleAllNoEmptyButtons(false);
        lineHorizontalCheck.Clear();
        allTargets.Clear();
        string targetPlayerSideAnalis = "";
        List<GridSpace> targetPlayerSideListSpacesAnalis = null;

        
        if (aiSide == Player1Side)
        {
            targetPlayerSideAnalis = Player2Side;
            targetPlayerSideListSpacesAnalis = gridSpacesPlayer2InGame;
        }
        else if (aiSide == Player2Side)
        {
            targetPlayerSideAnalis = Player1Side;
            targetPlayerSideListSpacesAnalis = gridSpacesPlayer1InGame;
        }

        bool okPlayer = false;
        bool okAI = false;
        //������ ������� ���� ��, �� ����� ������� ������� � �������� ������� �� �����
        //���� ������� � ��������� ������� ����� ������
        if(steps == 0)
        {
            //AI_ChooseTargetOnBoardAndClick();
            
            var rnd = Random.Range(1, 100);
            if(rnd <= 50)
            {
                AI_ChooseTargetOnBoardAndClick();
            }
            else
            {
                okPlayer = CheckLine(
                targetPlayerSideListSpacesAnalis,
                targetPlayerSideAnalis,
                LineCheck.Horizontal,
                1,
                lineHorizontalCheck);

                if (okPlayer)
                {
                    GridSpace left = null;
                    GridSpace right = null;
                    GridSpace up = null;
                    GridSpace down = null;

                    GridSpace downLeft = null;
                    GridSpace upLeft = null;
                    GridSpace downRight = null;
                    GridSpace upRight = null;


                    left = lineHorizontalCheck[0].leftNeighbour;
                    up = lineHorizontalCheck[0].upNeighbour;
                    down = lineHorizontalCheck[0].downNeighbour;
                    right = lineHorizontalCheck[lineHorizontalCheck.Count - 1].rightNeighbour;

                    downLeft = lineHorizontalCheck[0].downLeftNeighbour;
                    upLeft = lineHorizontalCheck[0].upLeftNeighbour;
                    downRight = lineHorizontalCheck[0].downRightNeighbour;
                    upRight = lineHorizontalCheck[0].upRightNeighbour;
                    TryAddTarget(allTargets, left, right, up, down, downLeft, upLeft, downRight, upRight);
                    AI_Click_And_End(ChooseTarget(allTargets));
                }
            }
            
        }
    }
    #endregion


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
        for (int i = 0; i < (int)LineCheck.Size; i++)
        {
            LineCheck check = (LineCheck)i;
            player1Win = CheckLine(gridSpacesPlayer1InGame, Player1Side, check, _winCount);
            if (player1Win) break;
        }


        if (!player1Win)
        {
            for (int i = 0; i < (int)LineCheck.Size; i++)
            {
                LineCheck check = (LineCheck)i;
                player2Win = CheckLine(gridSpacesPlayer2InGame, Player2Side, check, _winCount);
                if (player2Win) break;
            }
        }
        

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

        steps++;

        if (isGameOver)
        {
            SetInteractibleAllNoEmptyButtons(false);
            GameOverPanel.SetActive(true);
        }
    }

    public enum LineCheck
    {
        Horizontal,
        Vertical,
        DiagonalUp,
        DiagonalDown,
        Size
    }


    private GridSpace GetNeighbour(GridSpace obj, LineCheck line)
    {
        GridSpace TT = null;
        if (line == LineCheck.Horizontal)
        {
            TT = obj.rightNeighbour;
        }
        else
        if (line == LineCheck.Vertical)
        {
            TT = obj.upNeighbour;
        }
        else
        if (line == LineCheck.DiagonalDown)
        {
            TT = obj.downLeftNeighbour;
        }
        else
        if (line == LineCheck.DiagonalUp)
        {
            TT = obj.downRightNeighbour;
        }
        return TT;
    }

    #region CheckLines
    public bool CheckLine(List<GridSpace> playerSpacesList, string PLAYER_SIDE, LineCheck line, int count, List<GridSpace> returnList = null)
    {
        GridSpace TT = null;
        int __I = 0;
        foreach (var obj in playerSpacesList)
        {
            if (obj.buttonText.text == PLAYER_SIDE)
            {
                __I++;
                if (returnList != null)
                {
                    returnList.Add(obj);
                    if(count <= 1)
                    {
                        return true;
                    }
                }
                TT = GetNeighbour(obj, line);
                var target = TT;

                if (target != null)
                {
                    for (var i = 0; i < count; i++)
                    {
                        if (target != null)
                        {
                            if (target.buttonText.text == PLAYER_SIDE)
                            {
                                //--
                                if (returnList != null)
                                {
                                    returnList.Add(target);
                                }
                                //--
                                __I++;
                                if (__I == count)
                                {
                                    return true;
                                }
                                TT = GetNeighbour(target, line);
                                target = TT;
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
