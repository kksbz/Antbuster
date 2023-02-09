using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = default;
    public List<GameObject> cakeList = default;
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null || instance == default)
            {
                return null;
            }
            return instance;
        }
    } //Instance프로퍼티

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitGame();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    } //Awake

    public void InitGame()
    {
        cakeList = new List<GameObject>();
        isGameOver = false;
    } //InitGame

    //게임오버 조건 체크하는 함수
    private void GameOverCheck()
    {
        int outCakeNum = 8;
        foreach (GameObject cakePiece_ in cakeList)
        {
            if (cakePiece_.name.Equals(GData.OUT_CAKE_NAME))
            {
                outCakeNum -= 1;
            }
        }

        if (outCakeNum == 0)
        {
            isGameOver = true;
        }
    } //GameOverCheck

    private void ShowGameOver()
    {
        GameOverCheck();
        if(isGameOver == true)
        {
            GameObject gameOverObj = GFunc.GetRootObj("UiObj");
            gameOverObj = gameOverObj.FindChildObj(GData.GAME_OVER_UI_NAME);
            gameOverObj.SetActive(true);
            if(Input.GetKeyDown(KeyCode.R))
            {
                InitGame();
                GFunc.LoadScene(GData.TITLE_SCENE_NAME);
            }
        }
    } //ShowGameOver

    void Update()
    {
        ShowGameOver();
    }
}
