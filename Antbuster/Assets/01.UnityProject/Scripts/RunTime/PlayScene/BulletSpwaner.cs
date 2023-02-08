using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpwaner : MonoBehaviour
{
    private Transform tagetTransform = default;
    private float timeLate = 1f;
    private float timeCheck = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    //bullet 쏘는 함수
    private void ShootBullet(List<GameObject> BulletList_)
    {
        //{포탑머리가 레인지범위에 들어온 개미를 향하게 하는 식
        Vector2 tagetPos = TagetInAttackRange.tagetPos;
        // if(tagetPos == null || tagetPos == default)
        // {
        //     return;
        // }
        float angle = Mathf.Atan2(tagetPos.y - transform.position.y, tagetPos.x - transform.position.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        //}포탑머리가 레인지범위에 들어온 개미를 향하게 하는 식

        timeCheck += Time.deltaTime;
        if (timeLate <= timeCheck)
        {
            timeCheck = 0f;
            GameObject bullet = BulletManager.Instance.GetBullet(BulletList_);
            if(bullet == null || bullet == default)
            {
                return;
            }
            bullet.transform.position = this.transform.position;
            bullet.transform.rotation = this.transform.rotation;
            bullet.SetActive(true);
        }
    } //ShootBullet

    // Update is called once per frame
    void Update()
    {
        ShootBullet(BulletManager.Instance.bulletList);
    }
}
