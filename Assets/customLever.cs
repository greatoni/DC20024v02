using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customLever : MonoBehaviour
{
    public string leverEvent = "DrawbridgeActivated";
    public Sprite leverUp;
    public Sprite leverDown;
    public Collider leverCollider;
    public float distanceToLever = 5.0f;

    public Camera cutsceneCam;
    public Animator BridgeAnim;

    private Camera cam;

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

            if (leverCollider.Raycast(ray, out hit, distanceToLever))
            {
                if (!gameManager.gm.levelConcierge.checkBoolByString("DrawbridgeActivated"))
                {
                    gameManager.gm.levelConcierge.switchBool("DrawbridgeActivated", true);
                    gameObject.GetComponent<SpriteRenderer>().sprite = leverDown;

                    BridgeAnim.SetBool("LeverDown", true);
                }
                else
                {
                    gameManager.gm.levelConcierge.switchBool("DrawbridgeActivated", false);
                    gameObject.GetComponent<SpriteRenderer>().sprite = leverUp;
                    BridgeAnim.SetBool("LeverDown", false);
                }

            }

        }
    }
}
