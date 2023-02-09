using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagetInAttackRange : MonoBehaviour
{
    public static Vector2 tagetPos = default;
    public static bool isTagetInAttackRange = false;

    //Ant가 범위안에 들어왔을때 엔트의 포지션값을 저장
    private void OnTriggerStay2D(Collider2D obj_)
    {
        if (obj_.tag.Equals("Ant"))
        {
            if (obj_.GetComponent<AntControl>().isDead == false)
            {
                isTagetInAttackRange = true;
                tagetPos = obj_.gameObject.GetComponentMust<Transform>().position;
                // Debug.Log($"레인지안의 타겟 포지션:{tagetPos}");
            }
            else
            {
                isTagetInAttackRange = false;
            }
        }
    } //OnTriggerStay2D

    private void OnTriggerExit2D(Collider2D obj_)
    {
        //타겟이 레인지 범위 밖으로 나갔을 때
        if (obj_.tag.Equals("Ant"))
        {
            isTagetInAttackRange = false;
            tagetPos = default;
        }
    } //OnTriggerExit2D
}
