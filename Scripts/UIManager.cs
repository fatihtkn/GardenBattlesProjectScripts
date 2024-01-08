using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]GameObject startBattleButton;
    [SerializeField]Button restartButton;
    [SerializeField]GameObject GameOverPanel;

    private void Start()
    {
        GridManager.Instance.OnAllPlantsArranged += SetUIElements;
        GameManager.Instance.GameOver += SetActiveGameOverPanel;
    }
    public void StartBattle()//button
    {
        GameManager.Instance.OnBattleStart();
        startBattleButton.SetActive(false);

    }
    private void SetUIElements()
    {
        startBattleButton.SetActive(true);
    }
    public void SetActiveGameOverPanel()
    {
        if(GameOverPanel!=null) GameOverPanel.SetActive(true);

    }
    public void RestartTheGame()
    {
        SceneManager.LoadScene(0);
    }
}
