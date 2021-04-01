using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinLoss : MonoBehaviour
{
    public GameObject[] humans;
    public float timer;
    public Text timerText;

    // Update is called once per frame
    private void Start()
    {
        timer = 60f;
    }

    void Update()
    {
        CheckWL();
        timer -= 1 * Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("F0"); 
    }

    public void CheckWL()
    {
        humans = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject human in humans)
        {
            if(human.GetComponent<Health>().alive == false)
            {
                Debug.Log("all humans are dead");
                BossWin();
            }
            else
            {
                Debug.Log("some are still alive");
            }
        }

        if (timer <= 0f)
        {
            HumansWin();
        }
    }

    public void BossWin()
    {
        SceneManager.LoadScene("BossWin", LoadSceneMode.Single);
    }

    public void HumansWin()
    {
        SceneManager.LoadScene("HumanWin", LoadSceneMode.Single);
    }
}