using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class AntControl : MonoBehaviour
{
    private Animator antAni = default;
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
    public bool isDead = false;
    private float antHp = default;
    private float antMaxHp = 10f;
    private float antHpGuageAmount = default;
    // Start is called before the first frame update
    void Start()
    {
        antAni = gameObject.GetComponentMust<Animator>();
        isDead = false;
        antHp = antMaxHp;
        gameObject.GetComponentMust<Rigidbody2D>();
        objRect = gameObject.GetComponentMust<RectTransform>();
        rootObj = GFunc.GetRootObj("GameObjs");
        cakeObj = rootObj.FindChildObj("Cake").GetComponentMust<RectTransform>();
        poolObj = rootObj.FindChildObj("Pool").GetComponentMust<RectTransform>();
        antHpImage = gameObject.FindChildObj("HpBar").GetComponentMust<Image>();
    }

    //개미가 활성화될 때 bool값 및 Hp 초기화
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
        if (isDead == true)
        {
            return;
        }

        //개미가 케이크조각을 들고있고 죽지않았을 경우 케이크조각의 위치는 개미를 따라다님
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
                Vector2 newVector_1 = new Vector2(cakeObj.anchoredPosition.x, cakeObj.anchoredPosition.y);
                AntRotation(newVector_1);
                objRect.anchoredPosition = Vector2.MoveTowards(objRect.anchoredPosition, cakeObj.anchoredPosition, antSpeed * Time.deltaTime);
            }
            else
            {
                Vector2 newVector_2 = new Vector2(poolObj.anchoredPosition.x, poolObj.anchoredPosition.y);
                AntRotation(newVector_2);
                objRect.anchoredPosition = Vector2.MoveTowards(objRect.anchoredPosition, poolObj.anchoredPosition, antSpeed * Time.deltaTime);
            }
        }
        else if (isChangeMove == true && isDead == false)
        {
            if (randomPos != null || randomPos != default)
            {
                //랜덤방향으로 이동
                AntRotation(randomPos);
                objRect.anchoredPosition = Vector2.MoveTowards(objRect.anchoredPosition, randomPos, antSpeed * Time.deltaTime);
            }
        }
    } //MoveAnt

    //개미가 가는 방향쪽으로 머리를 회전하는 함수
    private void AntRotation(Vector2 taget_)
    {
        float rotateSpeed = 2f;
        float angle = Mathf.Atan2(taget_.y - transform.position.y, taget_.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
    } //AntRotation



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
        Die();
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
                if (cakePiece_.name != GData.END_CONDITION_NAME && cakePiece_.name != GData.OUT_CAKE_NAME)
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

    //개미죽음 함수
    private void Die()
    {
        if (isDead == false)
        {
            return;
        }
        StartCoroutine(CakeCallBack());
    } //Die

    //개미가 죽었을 경우 코루틴
    IEnumerator CakeCallBack()
    {
        if (isGetCake == true)
        {
            //개미가 케이크조각을 갖고있는 상태에서 죽었을 경우
            antAni.SetTrigger("Die");
            //케이크조각의 위치를 케이크의 위치로 보내고 케이크원상복구
            GameManager.Instance.cakeList[cakeNum].GetComponentMust<RectTransform>().anchoredPosition =
                Vector2.MoveTowards(GameManager.Instance.cakeList[cakeNum].GetComponentMust<RectTransform>().anchoredPosition, cakeObj.position, 500f * Time.deltaTime);
            yield return new WaitForSeconds(3f);
            GameManager.Instance.cakeList[cakeNum].SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            antAni.SetTrigger("Die");
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }
    } //CakeCallBack

    //antHp이미지 출력하는 함수
    private void ShowAntHp()
    {
        antHpGuageAmount = antHp / (float)antMaxHp;
        antHpImage.fillAmount = antHpGuageAmount;
    } //ShowAntHp

    private void OnTriggerEnter2D(Collider2D obj_)
    {
        //개미가 총알에 맞았을 때
        if (obj_.tag.Equals("Bullet"))
        {
            antHp -= 2f;
            if (antHp <= 0f)
            {
                isDead = true;
            }
        }

        //개미가 케이크에 도착했을때 케이크를 들고있지 않는 경우
        if (obj_.tag.Equals("Cake") && isGetCake == false)
        {
            GetCakePiece();
        }

        //개미가 케이크조각을 가지고 개미풀에 도착했을 때
        if (obj_.tag.Equals("AntPool") && isGetCake == true)
        {
            // Debug.Log($"케이크가지고 풀이 도착? {isCakeInPool}");
            //도착한 케이크조각의 이름을 바꾸고 false처리
            GameManager.Instance.cakeList[cakeNum].name = GData.OUT_CAKE_NAME;
            GameManager.Instance.cakeList[cakeNum].SetActive(false);

            gameObject.SetActive(false);
        }
    } //OnTriggerEnter2D
}
