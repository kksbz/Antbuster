using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{
    public void StartGame()
    {
        GFunc.LoadScene(GData.PLAY_SCENE_NAME);
    }
}
