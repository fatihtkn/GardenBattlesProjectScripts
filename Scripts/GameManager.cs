using System;


public class GameManager : MonoSingleton<GameManager>
{
    public event Action FinishingGame;
    public event Action StartingBattle;
    public event Action GameOver;
    public GameStates currentState;
    private void Start()
    {
       SetGameState(GameStates.Running);
        GameOver += () => currentState = GameStates.GameOver;
    }
    public void OnGameFinished()
    {
      
        FinishingGame?.Invoke();  
    }
    public void OnGameOver()
    {
        GameOver?.Invoke();
    }
    public void OnBattleStart()
    {
        SetGameState(GameStates.Fighting);
        StartingBattle?.Invoke();
      
    }
    public void SetGameState(GameStates state)
    {
        currentState = state;
    }
    public GameStates GetCurrentState()
    {
        return currentState;
    }
    public bool CompareState(GameStates desiredState)
    {
        if (desiredState == currentState)
        {
            return true;
        }
        return false;
    }
}
public enum GameStates
{
    Running,
    Merging,
    Fighting,
    GameOver
}
