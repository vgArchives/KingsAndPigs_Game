using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BossEndingController : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;


    // Start is called before the first frame update
    void Start()
    {
        globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnGlobalLight()
    {
        globalLight.intensity = 1f;
    }
}
