using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoSingleton<TileManager>
{
    public List<Tile> tiles=new();

    public Color emptyColor, targetColor;

    private void Start()
    {
        GridManager.Instance.OnAllPlantsArranged += () => TileHoverEffect.arePlantsAranged = true;
    }


    public List<Tile> GetEmptyTiles()
    {
        List<Tile> emptyTiles = new();

        for (int i = 0;i<tiles.Count; i++)
        {
            if (tiles[i].TileStack == null)
            {
                emptyTiles.Add(tiles[i]);
            }
        }
        return emptyTiles;
    }

    public void AddTile( Tile tile)
    {
        tiles.Add(tile);
    }
}
