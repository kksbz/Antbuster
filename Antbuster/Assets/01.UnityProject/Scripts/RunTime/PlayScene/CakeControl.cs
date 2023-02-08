using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CakeControl : MonoBehaviour
{
    private Image cakeImage = default;
    public Sprite noCake = default;
    public Sprite cakeFull = default;
    public Sprite cake7 = default;
    public Sprite cake6 = default;
    public Sprite cake5 = default;
    public Sprite cake4 = default;
    public Sprite cake3 = default;
    public Sprite cake2 = default;
    public Sprite cake1 = default;
    private List<Sprite> cakePieces = default;
    // private List<GameObject> cakeList = default;
    private GameObject cakePrefab = default;
    // Start is called before the first frame update
    void Start()
    {
        cakeImage = gameObject.GetComponent<Image>();
        cakePieces = new List<Sprite>();
        cakePieces.Add(noCake);
        cakePieces.Add(cake1);
        cakePieces.Add(cake2);
        cakePieces.Add(cake3);
        cakePieces.Add(cake4);
        cakePieces.Add(cake5);
        cakePieces.Add(cake6);
        cakePieces.Add(cake7);
        cakePieces.Add(cakeFull);
        cakePrefab = Resources.Load("Prefabs/CakePiece") as GameObject;
        GameManager.Instance.cakeList = SetupCake();
        GameManager.Instance.cakeList = SetupCakeListScale(GameManager.Instance.cakeList);
    }

    //케이크 이미지 리스트 생성 함수
    private void SetupCakeImage(List<GameObject> cakeList_)
    {
        int cakeNum = 0;
        foreach (GameObject cakePiece_ in cakeList_)
        {
            if (!cakePiece_.activeInHierarchy)
            {
                //개미가 케이크를 개미집까지 옮긴경우의 이미지출력을 위한 예외처리
                if (cakePiece_.name != GData.OUT_CAKE_NAME)
                {
                    cakeNum += 1;
                }
            }
        }
        cakeImage.sprite = cakePieces[cakeNum - 1];
    } //SetupCakeImage

    //케이크 생성 함수
    private List<GameObject> SetupCake()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject cakePiece_ = Instantiate(cakePrefab);
            cakePiece_.SetActive(false);
            cakePiece_.transform.parent = this.transform;
            if (i == 0)
            {
                //List의 첫번째에는 게임승패를 판별하기 위한 이름을 저장
                cakePiece_.name = GData.END_CONDITION_NAME;
            }
            else
            {
                cakePiece_.name = $"cake{i}";
            }
            GameManager.Instance.cakeList.Add(cakePiece_);
        }
        return GameManager.Instance.cakeList;
    } //SetupCake

    //케이크조각 스케일 정하는 함수
    private List<GameObject> SetupCakeListScale(List<GameObject> cakeList_)
    {
        foreach (GameObject cakePiece_ in cakeList_)
        {
            cakePiece_.transform.localScale = Vector3.one;
        }
        return cakeList_;
    } //SetupCakeListScale

    // Update is called once per frame
    void Update()
    {
        SetupCakeImage(GameManager.Instance.cakeList);
    } //Update
}
