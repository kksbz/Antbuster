using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class AntControl : MonoBehaviour
{
    private GameObject rootObj = default;
    private RectTransform objRect = default;
    private RectTransform cakeObj = default;
    private RectTransform poolObj = default;
    private Vector3 randomPos = default;
    private float antSpeed = 100f;
    private float timeLate = 3f;
    private float timeCheck = 0f;
    private int cakeNum = default;
    private bool isChangeMove = false;
    private bool isGetCake = false;
    private bool isCakeInPool = false;
    private float antHp = 50f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentMust<Rigidbody2D>();
        objRect = gameObject.GetComponentMust<RectTransform>();
        rootObj = GFunc.GetRootObj("GameObjs");
        cakeObj = rootObj.FindChildObj("Cake").GetComponentMust<RectTransform>();
        poolObj = rootObj.FindChildObj("Pool").GetComponentMust<RectTransform>();
    }

    //개미 이동하는 함수
    private void MoveAnt()
    {
        if (isGetCake == true && isCakeInPool == false)
        {
            GameManager.Instance.cakeList[cakeNum].GetComponentMust<Transform>().position =
            this.gameObject.GetComponentMust<Transform>().position;
        }

        if (isCakeInPool == true)
        {
            StartCoroutine(CakeCallBack());
        }


        if (isChangeMove == false)
        {
            //케이크조각을 들고 있지 않으면 케이크쪽으로 아니면 pool쪽으로 이동
            if (isGetCake == false)
            {
                objRect.anchoredPosition = Vector2.MoveTowards(objRect.anchoredPosition, cakeObj.anchoredPosition, antSpeed * Time.deltaTime);
            }
            else
            {
                objRect.anchoredPosition = Vector2.MoveTowards(objRect.anchoredPosition, poolObj.anchoredPosition, antSpeed * Time.deltaTime);
            }
        }
        else if (isChangeMove == true)
        {
            objRect.anchoredPosition = Vector2.MoveTowards(objRect.anchoredPosition, randomPos, antSpeed * Time.deltaTime);
        }
    } //MoveAnt

    //개미가 pool에 케이크 넣었을때 함수
    IEnumerator CakeCallBack()
    {
        GameManager.Instance.cakeList[cakeNum].GetComponentMust<RectTransform>().anchoredPosition =
            Vector2.MoveTowards(GameManager.Instance.cakeList[cakeNum].GetComponentMust<RectTransform>().anchoredPosition, cakeObj.position, 500f * Time.deltaTime);
        yield return new WaitForSeconds(3f);
        GameManager.Instance.cakeList[cakeNum].SetActive(false);
        isGetCake = false;
        isCakeInPool = false;
    }

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

    //개미가 케이크에 닿았을 때 케이크한조각의 인덱스주소 가져오고 활성화시키는 함수
    private void GetCakePiece()
    {
        if (isGetCake == true)
        {
            foreach (GameObject cakePiece_ in GameManager.Instance.cakeList)
            {
                if (!cakePiece_.activeInHierarchy)
                {
                    if (cakePiece_.name != GData.END_CONDITION_NAME)
                    {
                        string cakeName = cakePiece_.name;
                        cakeName = Regex.Replace(cakeName, @"\D", "");
                        cakeNum = int.Parse(cakeName);
                        Debug.Log($"케이크넘버:{cakeNum}");

                        cakePiece_.SetActive(true);
                        break;
                    }
                }
            }
        }
    } //GetCakePiece

    private void OnTriggerEnter2D(Collider2D obj_)
    {
        if (obj_.tag.Equals("Cake") && isGetCake == false)
        {
            isGetCake = true;
            GetCakePiece();
        }

        if (obj_.tag.Equals("AntPool") && isGetCake == true)
        {
            isCakeInPool = true;
            Debug.Log($"케이크가지고 풀이 도착? {isCakeInPool}");
        }
    } //OnTriggerEnter2D
}
