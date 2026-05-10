using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestItemStruct : MonoBehaviour
{
    [System.Serializable]
    public struct Item
    {
        public GameObject itemPrefab;
        public ItemData.Type itemType;
        public float weight;

        public Item(ItemData.Type itemType, float weight, GameObject prefab)
        {

            this.itemType = itemType;
            this.weight = weight;
            itemPrefab = prefab;
        }
    }

    public Transform spawnRoot;
    public int itemNum;
    public List<Item> itemList = new List<Item>();

    private string spreadsheetUrl = "https://docs.google.com/spreadsheets/d/1pqPGuv8jBHC14NyoYFvFbXLcP9vKUc4RgYPn84pnew8/export?format=csv";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            itemNum++;

            if (itemNum >= itemList.Count)
            {
                itemNum = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(LoadItems());
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            SpawnItem();
        }
    }

    IEnumerator LoadItems()
    {
        UnityWebRequest request = UnityWebRequest.Get(spreadsheetUrl);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }

        string csv = request.downloadHandler.text;
        string[] lines = csv.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                Debug.Log("空行");
                continue;
            }

            string[] data = lines[i].Split(',');

            if (data.Length < 5)
            {
                Debug.Log("列不足");
                continue;
            }
            
            Debug.Log($"データ: {data[0]}, {data[1]})");
        }
    }

    public void SpawnItem()
    {
        if (itemList.Count == 0)
        {
            Debug.LogWarning("アイテムリストが空です。");
            return;
        }

        Vector3 offset = spawnRoot.position;

        Item item = itemList[itemNum];
        Instantiate(item.itemPrefab, transform.position + offset, Quaternion.identity, spawnRoot);
    }
}
