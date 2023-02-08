using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSetup : MonoBehaviour
{
    private float bulletSpeed = 0.01f;
    private Vector2 initPos = default;
    // Start is called before the first frame update
    void Start()
    {
    }

    //Active가 Ture가 되었을 때 처음 포지션값 저장
    private void OnEnable()
    {
        initPos = transform.position;
    } //OnEnable

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * bulletSpeed);
        bulletActiveFalse();
    }

    private void bulletActiveFalse()
    {
        float distance = Vector2.Distance(this.transform.position, initPos);
        // Debug.Log($"bullet_과 포탑의 거리:{distance}");
        if (distance >= 1.5f)
        {
            gameObject.SetActive(false);
        }
    } //bulletActiveFalse

    private void OnTriggerEnter2D(Collider2D obj_)
    {
        if (obj_.tag.Equals("Ant"))
        {
            gameObject.SetActive(false);
        }
    }
}
