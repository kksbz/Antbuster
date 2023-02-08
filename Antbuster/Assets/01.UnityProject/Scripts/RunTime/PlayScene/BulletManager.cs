using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public List<GameObject> bulletList = default;
    private GameObject bulletPrefab = default;
    private static BulletManager instance = null;
    public static BulletManager Instance
    {
        get
        {
            if (instance == null || instance == default)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitBulletManager();
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

    public void InitBulletManager()
    {
        bulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        bulletList = new List<GameObject>();
        bulletList = SetupBulletList(bulletPrefab);
        bulletList = SetupBulletScale(bulletList);
    } //InitBulletManager

    //Bullet 생성하는 함수
    private List<GameObject> SetupBulletList(GameObject bulletPrefab_)
    {
        for (int i = 0; i < 1; i++)
        {
            GameObject bullet_ = Instantiate(bulletPrefab_);
            bullet_.SetActive(false);
            bulletList.Add(bullet_);
            bullet_.transform.parent = this.transform;
        }
        return bulletList;
    } //SetupBulletList

    //Bullet Scale Vector3.one으로 초기화하는 함수
    private List<GameObject> SetupBulletScale(List<GameObject> bulletList_)
    {
        for (int i = 0; i < bulletList_.Count; i++)
        {
            bulletList_[i].transform.localScale = Vector3.one;
        }
        return bulletList_;
    } //SetupBulletScale

    //BulletList에서 한개 뽑아서 활성화시키는 함수
    public GameObject GetBullet(List<GameObject> bulletList_)
    {
        GameObject bullet = default;
        foreach (GameObject bullet_ in bulletList_)
        {
            if (!bullet_.activeInHierarchy)
            {
                bullet = bullet_;
            }
        }
        return bullet;
    } //GetBullet

}
