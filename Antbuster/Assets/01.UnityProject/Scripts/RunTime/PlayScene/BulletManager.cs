using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public List<GameObject> bulletList = default;
    private static BulletManager instance = null;
    public static BulletManager Instance
    {
        get
        {
            if(instance == null || instance == default)
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
        bulletList = new List<GameObject>();
    }

    
}
