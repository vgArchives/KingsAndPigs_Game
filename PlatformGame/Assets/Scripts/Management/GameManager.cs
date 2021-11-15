using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static int MaxLp = 3;
    private static int PlayerLifePoints = MaxLp;
    [SerializeField] private static int gameScorePoints = 0;
    [SerializeField] private Text scoreText;

    [SerializeField] private GameObject[] hearts;

    [SerializeField] private GameObject endDoor;

    [SerializeField] private Animator startPlayer;
    [SerializeField] private Animator startDoor;
    [SerializeField] private AudioClip doorOpening;

    // Start is called before the first frame update
    void Start()
    {
        CheckLife();
        scoreText.text = gameScorePoints.ToString();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void CheckLife() //Atualizando os corações no canvas de acordo com a vida atual do player
    {
        for (int i = 1; i <= hearts.Length; i++)
        {
            if (i <= PlayerLifePoints) hearts[i - 1].SetActive(true);
            else hearts[i - 1].SetActive(false);
        }
    }

    public int GetPlayerLifePoints()
    {
        return PlayerLifePoints;
    }

    public void SetPlayerLifePoints(int lifePoints)
    {
        PlayerLifePoints = lifePoints;
    }

    public void GainScore(int score)
    {
        gameScorePoints += score;
        scoreText.text = gameScorePoints.ToString();
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void StartNewGame()
    {
        PlayerLifePoints = MaxLp;
        ChangeScene("Scene1");
    }

    public void StartGameMenu()
    {
        startDoor.SetTrigger("Open");
        startPlayer.SetTrigger("Start");        
        Invoke("StartNewGame", 3f);
        AudioSource.PlayClipAtPoint(doorOpening, transform.position);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
