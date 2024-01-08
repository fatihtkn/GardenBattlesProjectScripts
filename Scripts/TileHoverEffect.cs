
using UnityEngine;

public class TileHoverEffect : MonoBehaviour
{
    private Tile tile;
    static PlacementSystem placementSys;
    static Color _targetColor;
    private bool isEnter;
    public static bool arePlantsAranged;
    private void Start()
    {
        tile = transform.parent.GetComponent<Tile>();
        placementSys = PlacementSystem.Instance;
        _targetColor=TileManager.Instance.targetColor;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack") && placementSys.IsSelectingTile)
        {
            isEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stack"))
        {
            isEnter = false;
        }
    }

    private void Update()
    {
        if (arePlantsAranged)
        {
            UpdateTileColor();
            CheckMouseClick();
        }
        
    }

    private void UpdateTileColor()
    {
        Color targetColor = isEnter && placementSys.IsSelectingTile ? Color.white : _targetColor;
        tile.rend.material.color = Color.Lerp(tile.rend.material.color, targetColor, Time.deltaTime * GetLerpSpeed());
    }

    private float GetLerpSpeed()
    {
        return isEnter ? 1.8f : 2f;
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isEnter = false;
        }
    }
}

