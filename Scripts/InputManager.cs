using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoSingleton<InputManager>
{
    private Vector3 lastPosition;
    public LayerMask placementLayerMask;
    public LayerMask stackLayerMask;
    public LayerMask tileLayerMask;
    Camera sceneCamera;
   
    private void Start()
    {
        sceneCamera = Camera.main; 
    }
    
   
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
    public NewStack GetSelectedStack()
    {
        Vector3 mousePos = Input.mousePosition;
        NewStack stack = null;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, stackLayerMask))
        {
            stack=hit.collider.GetComponent<NewStack>();
        }
        return stack;
    }

    public Tile GetTile()
    {
        Vector3 mousePos = Input.mousePosition;
        Tile tile = null;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayerMask))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }
    
}
