using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager gm;
    public static bool newGame = true; // newGame became false if autosave have been made

    public level1Concierge levelConcierge;


    private void Awake()
    {
        if(gm != null && gm != this)
        {
            Destroy(this); 
        }
        else 
        {
            DontDestroyOnLoad(this);
            gm = this;
        }

        levelConcierge = GetComponentInChildren<level1Concierge>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gm != null && gm != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            gm = this;
        }

        levelConcierge = GetComponentInChildren<level1Concierge>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
