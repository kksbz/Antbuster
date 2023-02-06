using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntControl : MonoBehaviour
{
    private GameObject rootObj = default;
    private Transform cakeObj = default;
    private Vector3 randomPos = default;
    private float antSpeed = 1f;
    private float timeLate = 3f;
    private float timeCheck = 0f;
    private float moveLate = 2f;
    private float moveCheck = 0f;
    // Start is called before the first frame update
    void Start()
    {
        rootObj = GFunc.GetRootObj("GameObjs");
        cakeObj = rootObj.FindChildObj("Cake").GetComponentMust<Transform>();
        // newVector2 = cakeObj.gameObject.GetComponentMust<RectTransform>().anchoredPosition;
        // newVector2 = cakeObj.transform.position;
        // Debug.Log($"케이크좌표:{cakeObj.anchoredPosition}");
    }

    private void MoveAnt(Vector3 randomPos_)
    {
        moveCheck += Time.deltaTime;
        // gameObject.transform.rotation = Quaternion.LookRotation(newVector2);
        if(moveLate <= moveCheck)
        {
            moveCheck = 0f;
            gameObject.transform.LookAt(cakeObj);
            gameObject.transform.position = Vector2.MoveTowards(transform.position, cakeObj.transform.position, antSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.position = Vector2.MoveTowards(transform.position, randomPos_, antSpeed * Time.deltaTime);
        }

    } //MoveAnt

    private Vector3 GetRandomPos()
    {
        Vector3 randomPos = default;
        timeCheck += Time.deltaTime;
        if (timeLate <= timeCheck)
        {
            timeCheck = 0f;
            float randomX = Random.RandomRange(-630, 600);
            float randomY = Random.RandomRange(-250, 465);
            randomPos = new Vector3(randomX, randomY, 0f);
        }
        Debug.Log(randomPos);
        return randomPos;
    }

    // Update is called once per frame
    void Update()
    {
        randomPos = GetRandomPos();
        MoveAnt(randomPos);
    }
}
