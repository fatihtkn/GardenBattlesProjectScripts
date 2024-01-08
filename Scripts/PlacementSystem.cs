using DG.Tweening;
using UnityEngine;

public class PlacementSystem : MonoSingleton<PlacementSystem>
{
    private NewStack grabbedStack;
    private bool canSelect;
    private bool startSelectingPhase;
    public Vector3 offset;
    public bool IsSelectingTile { get; private set; }
    public Vector3 TargetCellCenterPos { get; private set; }
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    private void Start()
    {
        GridManager.Instance.OnAllPlantsArranged += StartSelecting;
        
    }

    private void Update()
    {
        if (gameManager.CompareState(GameStates.Merging))
        {
            HandleMouseInput();
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && canSelect)
        {
            SelectPlant();
        }

        if (Input.GetMouseButton(0))
        {
            MoveGrabbedStack();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseStack();
        }
    }

    private void MoveGrabbedStack()
    {
        if (grabbedStack != null)
        {
            Vector3 targetPos = InputManager.Instance.GetSelectedMapPosition() + offset;
            grabbedStack.transform.position = Vector3.Lerp(grabbedStack.transform.position, targetPos, Time.deltaTime * 16f);
        }
    }

    private void ReleaseStack()
    {
        if (grabbedStack != null)
        {
            IsSelectingTile = false;
            grabbedStack.isThisStackGrabbed = false;
            canSelect = true;

            Tile targetTile = InputManager.Instance.GetTile();

            if (targetTile != null)
            {
                HandleTargetTile(targetTile);
            }
            else
            {
                DropObject(grabbedStack.currentTile.cellCenterPos);
            }

            grabbedStack = null;
        }
    }

    private void HandleTargetTile(Tile targetTile)
    {
        if (MergeManager.Instance.Merge(targetTile.TileStack, grabbedStack))
        {
            // On Merge success
        }
        else if (targetTile.TileStack == null)
        {
            PlaceOnEmptyTile(targetTile);
        }
        else
        {
            ReturnToOriginalPosition(grabbedStack.currentTile.cellCenterPos);
        }
    }

    private void PlaceOnEmptyTile(Tile targetTile)
    {
        TargetCellCenterPos = targetTile.cellCenterPos;
        grabbedStack.currentTile.TileStack = null;
        grabbedStack.currentTile = targetTile;
        targetTile.TileStack = grabbedStack;
        DropObject(TargetCellCenterPos);
    }

    private void ReturnToOriginalPosition(Vector3 originalPosition)
    {
        DropObject(originalPosition);
    }

    private void DropObject(Vector3 targetPos)
    {
        grabbedStack.transform.DOMove(new Vector3(targetPos.x, grabbedStack.transform.position.y, targetPos.z), 0.3f).SetEase(Ease.InOutCubic);
    }

    private void SelectPlant()
    {
        NewStack stack = InputManager.Instance.GetSelectedStack();
        if (stack != null)
        {
            grabbedStack = stack;
            grabbedStack.isThisStackGrabbed = true;
            IsSelectingTile = true;
            canSelect = false;
        }
    }

    private void StartSelecting()
    {
        canSelect = true;
        //startSelectingPhase = true;
        gameManager.SetGameState(GameStates.Merging);
    }
}

