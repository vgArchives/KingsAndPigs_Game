using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string scene;
    [SerializeField] private AudioClip doorOpening;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        animator.SetTrigger("Open");
        AudioSource.PlayClipAtPoint(doorOpening, transform.position);
    }

    public void GoToScene()
    {
        FindObjectOfType<GameManager>().ChangeScene(scene);
    }

    public bool HaveScene()
    {
        return scene != "";
    }

    public void ActivateDoor()
    {
        gameObject.SetActive(true);
        Debug.Log("Ativei");
    }
}
