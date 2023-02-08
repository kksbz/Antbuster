using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagetInAttackRange : MonoBehaviour
{
    public static Vector2 tagetPos = default;

    //Ant가 범위안에 들어왔을때 엔트의 포지션값을 저장
    private void OnTriggerStay2D(Collider2D obj_)
    {
        if(obj_.tag.Equals("Ant"))
        {
            tagetPos = obj_.gameObject.GetComponentMust<Transform>().position;
            // Debug.Log($"레인지안의 타겟 포지션:{tagetPos}");
        }
    }
}
