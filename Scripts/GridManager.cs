using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class GridManager : MonoSingleton<GridManager>
{
    public float rowStartPosition=-12.2f;
    public float columnStartPosition=96.1f;

    public int width, height;
    public float gridHeight;
    public float cellSize;
    
    public List<NewStack> allStacks=new();
    public List<Vector3> gridPositions=new();
    [SerializeField] AnimationCurve curve;
    public event Action OnAllPlantsArranged;

    
    public Tile tilePrefab;
    public List<Tile> tileList=new();
    private void Start()
    {
        DOTween.SetTweensCapacity(500, 50);
        allStacks = NewStackManager.Instance.stacks;
        GameManager.Instance.FinishingGame += StartMerge;
        CustomGrid grid = new(width, height, cellSize, gridHeight, new Vector3(rowStartPosition, 0, columnStartPosition));
        gridPositions = grid.GetGridPositions();
        CreateTiles();
   
    }
    private void CreateTiles()
    {
        GameObject tileParent = new GameObject("TileParent");
        for (int i = 0; i < gridPositions.Count; i++)
        {
            Vector3 targetPos = new(gridPositions[i].x + (cellSize / 2f), 0.7f, gridPositions[i].z + (cellSize / 2f));
            Tile temp= Instantiate(tilePrefab, targetPos,Quaternion.identity);
            
            temp.transform.parent=tileParent.transform;
            tileList.Add(temp);
            
        }
    }

    private IEnumerator ArrangePlants()
    {
       
        float stoppingDistance = 0.1f;
        for (int i = 0; i < allStacks.Count; i++)
        {
            float timer = 0f;
            float duration = 0.3f;

            NewStack currentStack = allStacks[allStacks.Count-i-1];
            currentStack.canMove = false;
           
            Vector3 plantStartPos = currentStack.transform.position;
            Vector3 targetPos = new(gridPositions[i].x+(cellSize /2),currentStack.transform.position.y+0.2f, gridPositions[i].z+(cellSize / 2));
            currentStack.gridPosition = targetPos;
            tileList[i].TileStack = currentStack;
            tileList[i].TileStack.currentTile = tileList[i];
            while (Vector3.Distance(currentStack.transform.position,targetPos)>=stoppingDistance)
            {

                var delta = timer / duration;
                //var easing = curve.Evaluate(delta);
                float time = Easing.InOutCubic(delta);
                currentStack.transform.position = Vector3.Lerp(plantStartPos, targetPos,time);
                timer += Time.deltaTime;
                yield return null;
            }
          
        }
        OnAllPlantsArranged?.Invoke();


    }
   
    public void StartMerge()
    {
        StartCoroutine(ArrangePlants());

    }
}
public class CustomGrid
{
    int width;
    int height;
    float cellSize;
    int[,] gridArray;
    float gridHeight;
    Vector3 originPosition;
     List<Vector3> gridPositions = new();
    public CustomGrid(int width,int height,float cellSize,float gridHeight,Vector3 originPosition)
    {
       this.width = width;
       this.height = height;
       this.cellSize = cellSize;
        this.gridHeight = gridHeight;
        this.originPosition = originPosition;   
        gridArray=new int[this.width,this.height];
        DrawGrid();

       
    }
    void DrawGrid()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1),Color.white,100f);
                Debug.DrawLine(GetWorldPosition(x, y) , GetWorldPosition(x+1, y), Color.white, 100f);
                gridPositions.Add(GetWorldPosition(x,y));
                
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width,height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

    }
    Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x*cellSize, gridHeight, y*cellSize)+originPosition;
    }
    public List<Vector3> GetGridPositions()
    {
        return gridPositions;
    }
    

    
}
