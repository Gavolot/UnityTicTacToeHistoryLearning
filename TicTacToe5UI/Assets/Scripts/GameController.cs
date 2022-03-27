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

    public enum LineCheck
    {
        Horizontal,
        Vertical,
        DiagonalLeft,
        DiagonalRight,
        Size
    }
    private const int _winCount = 5;
    List<GridSpace> winList = new List<GridSpace>();

    //AI
    List<GridSpace> lineHorizontalCheck = new List<GridSpace>();
    List<GridSpace> lineVerticalCheck = new List<GridSpace>();
    List<GridSpace> lineDiagonalLeftCheck = new List<GridSpace>();
    List<GridSpace> lineDiagonalRightCheck = new List<GridSpace>();

    List<GridSpace> allTargets = new List<GridSpace>();
    //

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
        for (var i = 0; i < textsList.Length; i++)
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
        for (var i = 0; i < buttonsList.Length; i++)
        {

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
    }

    #region AI_Logic


    private void AI_Click_And_End(GridSpace button)
    {
        button.ClickButton(aiSide, GetAIColor());
        SetInteractibleAllNoEmptyButtons(true);
    }

    private void TryAddTarget(List<GridSpace> list, params GridSpace[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var obj = buttons[i];
            if (obj != null)
            {
                if (obj.IsEmpty())
                {
                    list.Add(obj);
                }
            }
        }
    }

    private GridSpace ChooseTarget(List<GridSpace> list)
    {
        GridSpace res = null;
        int rnd = Random.Range(0, list.Count - 1);

        Debug.Log(rnd);
        res = list[rnd];

        return res;
    }

    private bool AI_ChooseTargetOnBoardAndClick()
    {
        bool isClicked = false;
        while (!isClicked)
        {
            var obj = ChooseTarget(allGridSpacesList);
            if (obj.buttonText.text == "")
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
        lineVerticalCheck.Clear();
        lineDiagonalLeftCheck.Clear();
        lineDiagonalRightCheck.Clear();

        allTargets.Clear();
        string targetPlayerSideAnalis = "";
        List<GridSpace> targetPlayerSideListSpacesAnalis = null;
        //====
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
        //====
        bool okMoreAI = !AI_SipleFirstSteps(targetPlayerSideListSpacesAnalis, targetPlayerSideAnalis);
        //====
        //Более сложная версия ИИ, которая уже пытается помешать игроку выиграть
        if (okMoreAI)
        {
            List<GridSpace> targetList = null;
            #region DiagonalLeft
            for (int size = 4; size > 0; size--)
            {
                targetList = lineDiagonalLeftCheck;
                var ok = false;
                targetList.Clear();
                CheckLine(
                targetPlayerSideListSpacesAnalis,
                targetPlayerSideAnalis,
                LineCheck.DiagonalLeft,
                size,
                targetList);
                GridSpace one = null;
                GridSpace two = null;

                try
                {
                    if (!targetList[0].isBlockDiagonalLeft)
                        one = targetList[0].upRightNeighbour;
                }
                catch
                {
                    one = null;
                }

                try
                {
                    if (!targetList[targetList.Count - 1].isBlockDiagonalLeft)
                        two = targetList[targetList.Count - 1].downLeftNeighbour;
                }
                catch
                {
                    two = null;
                }
                if (one != null)
                {
                    if (one.IsEmpty())
                    {
                        AI_Click_And_End(one);
                        ok = true;
                    }
                }

                if (two != null)
                {
                    if (two.IsEmpty())
                    {
                        AI_Click_And_End(two);
                        ok = true;
                    }
                }

                //foreach (var obj in targetList)
                //{
                //obj.button.interactable = true;
                //}


                bool okBlock = false;
                if (one != null && two != null)
                {
                    if (one.buttonText.text == aiSide && two.buttonText.text == aiSide)
                    {
                        okBlock = true;
                    }
                }

                if (okBlock)
                {
                    foreach (var obj in targetList)
                    {
                        obj.isBlockDiagonalLeft = true;
                        obj.button.interactable = true;
                    }
                }

                if (ok)
                {
                    return;
                }

            }
            #endregion
            #region DiagonalRight
            /*for (int size = 4; size > 1; size--)
            {
                lineDiagonal1Check.Clear();
                CheckLine(
                targetPlayerSideListSpacesAnalis,
                targetPlayerSideAnalis,
                LineCheck.DiagonalRight,
                size,
                lineDiagonal1Check);
                GridSpace one = null;
                GridSpace two = null;

                try
                {
                    one = lineDiagonal1Check[0].upLeftNeighbour;
                }
                catch
                {
                    one = null;
                }

                try
                {
                    two = lineDiagonal1Check[lineDiagonal1Check.Count - 1].downRightNeighbour;
                }
                catch
                {
                    one = null;
                }
                if (one != null)
                {
                    if (one.IsEmpty())
                    {
                        AI_Click_And_End(one);
                        return;
                    }
                }

                if (two != null)
                {
                    if (two.IsEmpty())
                    {
                        AI_Click_And_End(two);
                        return;
                    }
                }

                foreach (var obj in lineDiagonal1Check)
                {
                    obj.button.interactable = true;
                }
            }*/
            #endregion
        }



        //while (size > 0)
        //{
        /*
        for (int i = 0; i < (int)LineCheck.Size; i++)
        {
            LineCheck check = (LineCheck)i;
            lineHorizontalCheck.Clear();
            lineVerticalCheck.Clear();
            lineDiagonal1Check.Clear();
            lineDiagonal2Check.Clear();

            List<GridSpace> targetList = null;

            if (check == LineCheck.Horizontal)
            {
                targetList = lineHorizontalCheck;
            }
            else
            if (check == LineCheck.Vertical)
            {
                targetList = lineVerticalCheck;
            }
            else if (check == LineCheck.DiagonalLeft)
            {
                targetList = lineDiagonal1Check;
            }
            else if (check == LineCheck.DiagonalRight)
            {
                targetList = lineDiagonal2Check;
            }

            CheckLine(
            targetPlayerSideListSpacesAnalis,
            targetPlayerSideAnalis,
            check,
            size,
            targetList);
            GridSpace one = null;
            GridSpace two = null;
            try
            {
                switch (check)
                {
                    case LineCheck.Horizontal:
                        one = targetList[0].leftNeighbour;
                        break;
                    case LineCheck.Vertical:
                        one = targetList[0].upNeighbour;
                        break;
                    case LineCheck.DiagonalLeft:
                        one = targetList[0].downLeftNeighbour;
                        break;
                    case LineCheck.DiagonalRight:
                        one = targetList[0].downRightNeighbour;
                        break;
                }

            }
            catch
            {
                one = null;
            }
            try
            {
                switch (check)
                {
                    case LineCheck.Horizontal:
                        two = targetList[targetList.Count - 1].rightNeighbour;
                        break;
                    case LineCheck.Vertical:
                        two = targetList[targetList.Count - 1].downNeighbour;
                        break;
                    case LineCheck.DiagonalLeft:
                        two = targetList[targetList.Count - 1].upRightNeighbour;
                        break;
                    case LineCheck.DiagonalRight:
                        two = targetList[targetList.Count - 1].upLeftNeighbour;
                        break;
                }
            }
            catch
            {
                two = null;
            }
            //----
            if (two != null)
            {
                if (two.IsEmpty())
                {
                    AI_Click_And_End(two);
                    return;
                }
            }

            if (one != null)
            {
                if (one.IsEmpty())
                {
                    AI_Click_And_End(one);
                    return;
                }
            }


        }
        */


        //size--;
        //}
        //}
        //AI_ChooseTargetOnBoardAndClick();

        //}


    }

    //Изыски первого хода ИИ, он может выбрать сходить в случайно позиции на доске
    //либо сходить в случайной позиции возле игрока
    private bool AI_SipleFirstSteps(List<GridSpace> listSpacesAnalis, string playerSideAnalis)
    {

        if (steps <= 1)
        {
            bool okPlayer = false;
            var rnd = Random.Range(1, 100);
            if (rnd <= 50)
            {
                AI_ChooseTargetOnBoardAndClick();
                return true;
            }
            else
            {

                okPlayer = CheckLine(
                listSpacesAnalis,
                playerSideAnalis,
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
                    if (allTargets.Count > 0)
                    {
                        AI_Click_And_End(ChooseTarget(allTargets));
                        return true;
                    }
                    else
                    {
                        AI_ChooseTargetOnBoardAndClick();
                        return true;
                    }
                }
            }

        }
        return false;
    }
    #endregion


    public void EndTurn()
    {

        if (with_ai)
        {
            //if(gameController.playerSide == buttonText.text)
            //{
            AI_Turn();
            //}
        }
        bool player1Win = false;
        bool player2Win = false;
        bool isGameOver = false;


        if (!with_ai)
        {
            ChangePlayerSide();
        }
        //Check 5 line win
        winList.Clear();
        for (int i = 0; i < (int)LineCheck.Size; i++)
        {
            winList.Clear();
            LineCheck check = (LineCheck)i;
            CheckLine(gridSpacesPlayer1InGame, Player1Side, check, _winCount, winList);

            if (winList.Count == _winCount)
            {
                player1Win = true;
            }
            if (player1Win)
            {
                for (int t = 0; t < winList.Count; t++)
                {
                    var obj = winList[t];
                    obj.SetColor(Color.green);
                }
                break;
            }
        }


        if (!player1Win)
        {
            winList.Clear();
            for (int i = 0; i < (int)LineCheck.Size; i++)
            {
                winList.Clear();
                LineCheck check = (LineCheck)i;
                CheckLine(gridSpacesPlayer2InGame, Player2Side, check, _winCount, winList);
                if (winList.Count == _winCount)
                {
                    player2Win = true;
                }
                if (player2Win)
                {
                    for (int t = 0; t < winList.Count; t++)
                    {
                        var obj = winList[t];
                        obj.SetColor(Color.green);
                    }
                    break;
                }
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
            if (gridSpacesPlayer1InGame.Count + gridSpacesPlayer2InGame.Count == allGridSpaces)
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




    private GridSpace GetNeighbour(GridSpace obj, LineCheck line)
    {
        GridSpace TT = null;
        if (line == LineCheck.Horizontal)
        {
            if (obj.rightNeighbour != null)
            {
                if (!obj.isBlockHorizontal)
                {
                    TT = obj.rightNeighbour;
                }
                else
                {
                    TT = null;
                }
            }
        }
        else
        if (line == LineCheck.Vertical)
        {

            if (obj.upNeighbour != null)
            {
                if (!obj.isBlockVertical)
                {
                    TT = obj.upNeighbour;
                }
                else
                {
                    TT = null;
                }
            }
        }
        else
        if (line == LineCheck.DiagonalLeft)
        {
            if (obj.downLeftNeighbour != null)
            {
                if (!obj.isBlockDiagonalLeft)
                {
                    TT = obj.downLeftNeighbour;
                }
                else
                {
                    TT = null;
                }
            }
        }
        else
        if (line == LineCheck.DiagonalRight)
        {
            if (obj.downRightNeighbour != null)
            {
                if (!obj.isBlockDiagonalRight)
                {
                    TT = obj.downRightNeighbour;
                }
                else
                {
                    TT = null;
                }
            }
        }
        return TT;
    }

    private bool CheckBlock(LineCheck line, GridSpace obj)
    {
        if (line == LineCheck.Horizontal)
        {
            if (obj.isBlockHorizontal)
            {
                return true;
            }
        }
        if (line == LineCheck.Vertical)
        {
            if (obj.isBlockVertical)
            {
                return true;
            }
        }
        if (line == LineCheck.DiagonalLeft)
        {
            if (obj.isBlockDiagonalLeft)
            {
                return true;
            }
        }
        if (line == LineCheck.DiagonalRight)
        {
            if (obj.isBlockDiagonalRight)
            {
                return true;
            }
        }
        return false;
    }

    #region CheckLines
    public bool CheckLine(List<GridSpace> playerSpacesList, string PLAYER_SIDE, LineCheck line, int count, List<GridSpace> returnList = null)
    {
        GridSpace TT = null;
        int __I = 0;
        foreach (var obj in playerSpacesList)
        {
            returnList.Clear();
            __I = 0;
            if (obj.buttonText.text == PLAYER_SIDE)
            {
                __I++;

                if (CheckBlock(line, obj)) continue;
                if (returnList != null)
                {
                    returnList.Add(obj);
                    if (count == 1)
                        return true;
                }


                TT = GetNeighbour(obj, line);
                var target = TT;

                if (target != null && count > 1)
                {
                    for (var i = 0; i < count - 1; i++)
                    {
                        if (target != null)
                        {
                            if (target.buttonText.text == PLAYER_SIDE)
                            {
                                //--
                                if (CheckBlock(line, target)) return false;
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
                                return false;
                            }
                            else
                            {
                                returnList.Clear();
                                __I = 0;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    returnList.Clear();
                    __I = 0;
                }
            }
        }
        return false;
    }
    #endregion
}
