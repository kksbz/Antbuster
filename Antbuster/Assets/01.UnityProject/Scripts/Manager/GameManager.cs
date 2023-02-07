using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }
}
