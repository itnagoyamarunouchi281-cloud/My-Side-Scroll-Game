using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MyItem : MonoBehaviour
{

    const int Num = (int)ItemData.Type.MAX_ITEM;

    int[] Storage = new int[Num];

    bool fadestart = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<Storage.Length;++i)
        {
            Storage[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StaticItem.IsUpdate = false;
        // ��
        //if (Storage[0] == 2)
        //{
        //    if (!fadestart)
        //    {
        //        fadestart = true;
        //        FadeManager.Instance.LoadScene("Result", 2.0f);
        //        Debug.Log("a");
        //        //fadestart = false;
        //    }
            
        //}
       
    }

    public void AddItem(ItemData.Type type)
    {
        Storage[(int)type] += 1;
        Debug.Log(Storage[(int)type]);
    }

    public int[] GetStorage()
    {
        return Storage;
    }
}
