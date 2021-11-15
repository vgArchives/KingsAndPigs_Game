using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas howToPlayCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HowToPlay()
    {
        menuCanvas.gameObject.SetActive(false);
        howToPlayCanvas.gameObject.SetActive(true);
    }

    public void Menu()
    {
        howToPlayCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);       
    }
}
