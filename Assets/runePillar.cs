using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runePillar : MonoBehaviour
{
    private Camera cam;
    public float distanceToPillar = 5f;
    public Collider pillarCollider;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // if left mouse button pressed
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (pillarCollider.Raycast(ray, out hit, distanceToPillar))
            {
                Debug.Log("Refilled all values");
                gameManager.gm.pc.RefillAllValues();
            }

        }
    }
}
