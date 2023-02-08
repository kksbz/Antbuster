using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class AntControl : MonoBehaviour
{
    private Image antHpImage = default;
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
    private bool isDead = false;
    private float antHp = default;
    private float antMaxHp = 10f;
    private float antHpGuageAmount = default;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        antHp = antMaxHp;
        gameObject.GetComponentMust<Rigidbody2D>();
        objRect = gameObject.GetComponentMust<RectTransform>();
        rootObj = GFunc.GetRootObj("GameObjs");
        cakeObj = rootObj.FindChildObj("Cake").GetComponentMust<RectTransform>();
        poolObj = rootObj.FindChildObj("Pool").GetComponentMust<RectTransform>();
        antHpImage = gameObject.FindChildObj("HpBar").GetComponentMust<Image>();
    }

    private void OnEnable()
    {
        isDead = false;
        isGetCake = false;
        antHp = antMaxHp;
    } //OnEnable

    //개미 이동하는 함수
    private void MoveAnt()
    {
        ShowAntHp();
        if (isGetCake == true && isDead == true)
        {
            if (GameManager.Instance.cakeList[cakeNum].activeInHierarchy)
            {
                StartCoroutine(CakeCallBack());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (isGetCake == false && isDead == true)
        {
            gameObject.SetActive(false);
        }

        if (isGetCake == true && isDead == false)
        {
            GameManager.Instance.cakeList[cakeNum].GetComponentMust<Transform>().position =
            this.gameObject.GetComponentMust<Transform>().position;
        }


        if (isChangeMove == false && isDead == false)
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
        else if (isChangeMove == true &&  isDead == false)
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

    //개미가 케이크에 닿았을 때 케이크한조각의 인덱스주소 가져오고 활성화시키는 함수
    private void GetCakePiece()
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
                    // Debug.Log($"케이크넘버:{cakeNum}");
                    isGetCake = true;

                    cakePiece_.SetActive(true);
                    break;
                }
            }
        }
    } //GetCakePiece

    //cakeList 길이가 줄어들때 케이크조각 이름 재설정하는 함수
    private void cakeListReName(List<GameObject> cakeList_)
    {
        for (int i = 0; i < cakeList_.Count; i++)
        {
            if (i == 0)
            {
                cakeList_[i].name = GData.END_CONDITION_NAME;
            }
            else
            {
                cakeList_[i].name = $"cake{i}";
            }
        }
    } //cakeListReName

    //개미죽음 함수
    private void Die()
    {
        isDead = true;
    } //Die

    //개미가 케이크조각을 갖고 있을 때 죽을 경우 함수
    IEnumerator CakeCallBack()
    {
        GameManager.Instance.cakeList[cakeNum].GetComponentMust<RectTransform>().anchoredPosition =
            Vector2.MoveTowards(GameManager.Instance.cakeList[cakeNum].GetComponentMust<RectTransform>().anchoredPosition, cakeObj.position, 500f * Time.deltaTime);
        yield return new WaitForSeconds(3f);
        GameManager.Instance.cakeList[cakeNum].SetActive(false);
    } //CakeCallBack

    //antHp이미지 출력하는 함수
    private void ShowAntHp()
    {
        antHpGuageAmount = antHp / (float)antMaxHp;
        antHpImage.fillAmount = antHpGuageAmount;
    } //ShowAntHp

    private void OnTriggerEnter2D(Collider2D obj_)
    {
        if (obj_.tag.Equals("Bullet"))
        {
            antHp -= 2f;
            if (antHp <= 0f)
            {
                Die();
            }
        }

        if (obj_.tag.Equals("Cake") && isGetCake == false)
        {
            GetCakePiece();
        }

        //여기서 문제발생 케이크리스트에서
        if (obj_.tag.Equals("AntPool") && isGetCake == true)
        {
            // Debug.Log($"케이크가지고 풀이 도착? {isCakeInPool}");
            GameManager.Instance.cakeList[cakeNum].SetActive(false);
            GameManager.Instance.cakeList[cakeNum].name = "OutCake";
            GameManager.Instance.cakeList.RemoveAt(cakeNum);
            // Debug.Log($"지운 케잌조각 인덱스: {cakeNum}");
            cakeListReName(GameManager.Instance.cakeList);
            // Debug.Log($"케이크리스트 길이: {GameManager.Instance.cakeList.Count}");
            gameObject.SetActive(false);
        }

    } //OnTriggerEnter2D
}
