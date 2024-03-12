using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataHandler : MonoBehaviour
{
    private GameObject furniture;
    [SerializeField] private ButtonManager buttonPrefab;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private List<Item> items;
    [SerializeField] private string label;

    private int currentID = 0;

    private static DataHandler instance;
    public static DataHandler Instance 
    {
        get 
        { 
            if(instance == null)
            {
                instance = FindObjectOfType<DataHandler>();
            }
            return instance;
        } 
    }

    private async void Start()
    {
        //LoadItems();
        await Get(label);
        CreateButtons();
    }

   /* void LoadItems()
    {
        var itemsObject = Resources.LoadAll("Items", typeof(Item));
        foreach(Item item in itemsObject)
        {
            items.Add(item as Item);
        }
    }*/

    void CreateButtons()
    {
        foreach(var item in items)
        {
            ButtonManager b = Instantiate(buttonPrefab, buttonContainer.transform);
            b.ItemID = currentID;
            b.ButtonTexture = item.itemSprite;
            currentID++;
        }
    }

    public void SetFurniture(int id)
    {
        furniture = items[id].itemPrefab;
    }

    public GameObject GetFurniture()
    {
        return furniture;
    }

    public async Task Get(string label)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        foreach (var location in locations)
        {
            var obj = await Addressables.LoadAssetAsync<Item>(location).Task;
            items.Add(obj);
        }
    }
}
