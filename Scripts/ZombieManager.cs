
using System.Collections.Generic;
using UnityEngine;


public class ZombieManager : MonoSingleton<ZombieManager>
{
    public List<ZombieBrain> waveZombies = new();
    public List<ZombieBrain> normalZombies = new();
   
    public int totalWaveZombie;
    [SerializeField]int deadWaveZombieCount;
    public Material ZombieFreezeMat;
    private void Start()
    {
        SetPassiveWaveZombies();
        GameManager.Instance.StartingBattle += StartWaveZombieAttack;
        GameManager.Instance.FinishingGame += DestroyNormalZombies;
        GameManager.Instance.GameOver += DestroyAllWaveZombies;
        GameManager.Instance.GameOver += DestroyNormalZombies ;
        totalWaveZombie =waveZombies.Count;
    }
    public void StartWaveZombieAttack()
    {
      
        for (int i = 0; i < waveZombies.Count; i++)
        {
            waveZombies[i].waveZombieCanAttack = true;
            waveZombies[i].GetComponent<BoxCollider>().enabled = true;
            Vector3 randomPos = waveZombies[i].transform.position + new Vector3(0, 0, -30f);
            waveZombies[i].SetCustomDestination(randomPos);
           
        }
    }
    private void SetPassiveWaveZombies()
    {
        for (int i = 0; i < waveZombies.Count; i++)
        {
            waveZombies[i].waveZombieCanAttack = false;
            waveZombies[i].IsWaveZombie = true;
            waveZombies[i].GetComponent<BoxCollider>().enabled = false;
            
        }
    }
    public void OnWaveZombieDead()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.CompareState(GameStates.Fighting))
            {
                deadWaveZombieCount++;
                OnAllWaveZombiesDead();
            }
        }
    }
    private void OnAllWaveZombiesDead()
    {
        if(deadWaveZombieCount == totalWaveZombie)
        {
           
            GameManager.Instance.OnGameOver();
        }
    }
    public void AddZombieToTheList(ZombieBrain zombie)
    {
        if (zombie.IsWaveZombie)
        {
           if(!waveZombies.Contains(zombie)) { waveZombies.Add(zombie);}
        }
        else
        {
            normalZombies.Add(zombie);
        }
    }
    private void DestroyNormalZombies()
    {
        for (int i = 0; i < normalZombies.Count; i++)
        {
            if (normalZombies[i]!=null)
            {
                if (normalZombies[i].gameObject.activeSelf)
                {
                    
                    normalZombies[i].Health = 0f;
                    normalZombies[i].OnZombieDeath();
                    

                }
            }
            
        }
    }
    private void DestroyAllWaveZombies()
    {
        for (int i = 0; i < waveZombies.Count; i++)
        {
            if (waveZombies[i] != null)
            {
                if (waveZombies[i].gameObject.activeSelf)
                {
                    
                   waveZombies[i].Health = 0f;
                   waveZombies[i].OnZombieDeath();
                   

                }
            }

        }
    }
}
