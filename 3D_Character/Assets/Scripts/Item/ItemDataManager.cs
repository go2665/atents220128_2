using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas = null;
    public ItemData this[int i]     // 인덱서 : 프로퍼티의 배열 버전
    {
        get     // get만 만들어서 읽기 전용으로 하는 것이 목적
        {
            return itemDatas[i];
        }
    }
    public ItemData this[ItemIDCode code]     // 인덱서 : 프로퍼티의 배열 버전
    {
        get     // get만 만들어서 읽기 전용으로 하는 것이 목적
        {
            return itemDatas[(int)code];
        }
    }
    public int Length
    {
        get
        {
            return itemDatas.Length;
        }
    }

    const string ITEM_DATA_FOLDER_NAME = "ItemData";    // Assets폴더에 ItemData 파일이 있는 폴더 이름
    const string ITEM_DATA_RESOURCES_FOLDER_NAME = "Resources/ItemData";

    public void Initialize()
    {
        //string itemDataFullPath = $"{Application.dataPath}/{ITEM_DATA_RESOURCES_FOLDER_NAME}";    // "Assets/Resources/ItemData"로 합치기
        //DirectoryInfo info = new DirectoryInfo(itemDataFullPath);   // ItemData가 있는 전체 경로를 이용해 해당 폴더의 정보 가져오기
        //FileInfo[] files = info.GetFiles("*.asset");                // 확장자가 asset인 파일정보를 가져오기
        //itemDatas = new ItemData[files.Length];
        //for ( int i=0;i<files.Length;i++)            
        //{
        //    string loadFileName = $"{ITEM_DATA_FOLDER_NAME}/{Path.GetFileNameWithoutExtension(files[i].Name)}";
        //    itemDatas[i] = Resources.Load<ItemData>(loadFileName);
        //}

        ItemData[] temp = Resources.FindObjectsOfTypeAll<ItemData>();
        itemDatas = new ItemData[temp.Length];
        foreach (ItemData item in temp)
        {
            itemDatas[item.id] = item;  // 원래 id의 목적 중 하나가 배열에서의 index와 매칭 시키는 것
        }
    }
}
