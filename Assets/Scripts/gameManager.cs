using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public PlayerScript player;
    public EnemyScript enemy;
    public int minimumDist, maximumDist;
    public float deathTimer = 1;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindFirstObjectByType<PlayerScript>(FindObjectsInactive.Include);
        enemy = FindFirstObjectByType<EnemyScript>(FindObjectsInactive.Include);

        //StartLevel();
    }

    public void StartLevel()
    {
        player.transform.position = Vector3.zero;

        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        int x = UnityEngine.Random.Range(-maximumDist, maximumDist);
        int z = UnityEngine.Random.Range(-maximumDist, maximumDist);

        if(MathF.Abs(x) < minimumDist)
        {
            x = (int)(minimumDist * Mathf.Sign(x));
        }
        if (MathF.Abs(z) < minimumDist)
        {
            z = (int)(minimumDist * Mathf.Sign(z));
        }

        enemy.transform.position = Vector3.zero + new Vector3(x, 1.5f, z);
    }

    public void PlayerDead()
    {
        StartCoroutine(RestartDelay());
    }


    public IEnumerator RestartDelay()
    {
        yield return new WaitForSeconds(deathTimer);

        SceneManager.LoadScene("SampleScene");
    }


    public void EnemyDead()
    {
        Debug.Log("you win");
    }
}
