using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BossEnteringController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossPosition;
    [SerializeField] private GameObject lights;

    [SerializeField] private Light2D globalLight;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bossMusic;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBoss()
    {
        Instantiate(boss, bossPosition.transform.position, Quaternion.identity);
        audioSource.clip = bossMusic;
        audioSource.Play();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerChar"))
        {
            dialogueBox.SetActive(true);
            Invoke("CreateBoss", 9f);
            Invoke("TurnOnLights", 3f);
        }
    }

    public void TurnOnLights()
    {
        lights.SetActive(true);
        globalLight.intensity = 0.7f;
    }
}
