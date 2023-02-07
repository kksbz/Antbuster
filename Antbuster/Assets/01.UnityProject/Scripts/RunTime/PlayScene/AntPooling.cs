using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntPooling : MonoBehaviour
{
    private GameObject antPrefab = default;
    private List<GameObject> antList = default;
    private float timeLate = 0.5f;
    private float timeCheck = 0f;
    // Start is called before the first frame update
    void Start()
    {
        antList = new List<GameObject>();
        antPrefab = Resources.Load("Prefabs/Ant") as GameObject;
        antList = AntPoolSetup();
    }

    //개미풀 생성하는 함수
    private List<GameObject> AntPoolSetup()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject ant_ = Instantiate(antPrefab);
            ant_.SetActive(false);
            ant_.transform.parent = this.transform;
            antList.Add(ant_);
        }
        return antList;
    } //AntPoolSetup

    //개미의 로컬스케일과 좌표 설정하는 함수
    private void SetupAntListScale(List<GameObject> antList_)
    {
        timeCheck += Time.deltaTime;
        if (timeLate <= timeCheck)
        {
            timeCheck = 0f;
            foreach (GameObject ant_ in antList_)
            {

                if (!ant_.activeInHierarchy)
                {
                    ant_.transform.localScale = Vector3.one;
                    ant_.GetComponentMust<RectTransform>().anchoredPosition = new Vector3(-575f, 430f, 0f);
                    ant_.SetActive(true);
                    return;
                }
            }
        }
    } //SetupAntListScale

    // Update is called once per frame
    void Update()
    {
        SetupAntListScale(antList);
    }
}
