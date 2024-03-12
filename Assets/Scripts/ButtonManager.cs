using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonManager : MonoBehaviour
{
    private Button btn;
    public GameObject furniture;

    [SerializeField] private RawImage buttonImage;
    private int _itemID;
    private Sprite _buttonTexture;

    public int ItemID { set => _itemID = value; }
    public Sprite ButtonTexture 
    { 
        set 
        { 
            _buttonTexture = value; 
            buttonImage.texture = _buttonTexture.texture; 
        } 
    }


    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectObject);
    }

    private void Update()
    {
        if(UIManager.Instance.OnEntered(gameObject))
        {
            transform.DOScale(Vector3.one * 2, 0.3f);
        }
        else
        {
            transform.DOScale(Vector3.one, 0.3f);  
        }
    }

    private void SelectObject()
    {
        DataHandler.Instance.SetFurniture(_itemID);
    }
}
