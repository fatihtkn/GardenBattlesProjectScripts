using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 cellCenterPos;
    public MeshRenderer rend;
    [SerializeField] private NewStack tileStack;
    public NewStack TileStack { get { return tileStack; } set { tileStack = value;  } }
    public TileHoverEffect hoverEffect;
    private void Start()
    {
        cellCenterPos = transform.position;
        rend = GetComponent<MeshRenderer>();
        TileManager.Instance.AddTile(this);
        hoverEffect = GetComponentInChildren<TileHoverEffect>();
        
    }
    
   

    public void SetTileStackOnBought(NewStack newStack)
    {
        tileStack= newStack;
        newStack.transform.position= cellCenterPos+new Vector3(0f,1f,0f);
        NewStackManager.Instance.AddStack(newStack);
        newStack.BuyStack(this);
        newStack.Bloom();
       
    }
}
