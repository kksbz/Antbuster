using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntControl : MonoBehaviour
{
    private GameObject rootObj = default;
    private RectTransform objRect = default;
    private RectTransform cakeObj = default;
    private Vector3 randomPos = default;
    private float antSpeed = 100f;
    private float timeLate = 3f;
    private float timeCheck = 0f;
    private bool isChangeMove = false;
    // Start is called before the first frame update
    void Start()
    {
        objRect = gameObject.GetComponentMust<RectTransform>();
        rootObj = GFunc.GetRootObj("GameObjs");
        cakeObj = rootObj.FindChildObj("Cake").GetComponentMust<RectTransform>();
    }

    //개미 이동하는 함수
    private void MoveAnt()
    {
        // Debug.Log($"무브쳌:{isChangeMove}");
        if (isChangeMove == false)
        {
            objRect.anchoredPosition = Vector2.MoveTowards(objRect.anchoredPosition, cakeObj.anchoredPosition, antSpeed * Time.deltaTime);
        }
        else if (isChangeMove == true)
        {
            objRect.anchoredPosition = Vector2.MoveTowards(objRect.anchoredPosition, randomPos, antSpeed * Time.deltaTime);
        }
    } //MoveAnt

    //랜덤확률로 개미의 이동좌표 변경하는 함수
    private void GetRandomPos()
    {
        timeCheck += Time.deltaTime;
        if (timeLate <= timeCheck)
        {
            timeCheck = 0f;
            float randomNum = Random.RandomRange(0, 10);
            // Debug.Log(randomNum);
            if (randomNum < 4)
            {
                isChangeMove = false;
            }
            else
            {
                float randomX = Random.RandomRange(-630, 600);
                float randomY = Random.RandomRange(-250, 465);
                randomPos = new Vector3(randomX, randomY, 0f);
                isChangeMove = true;
            }
        }
    } //GetRandomPos

    // Update is called once per frame
    void Update()
    {
        GetRandomPos();
        MoveAnt();
    } //Update
}
