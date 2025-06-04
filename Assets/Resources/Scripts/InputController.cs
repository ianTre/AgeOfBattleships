using Assets.Resources.Scripts;

using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update
    AOBLogger logger;
    void Start()
    {
        logger = new AOBLogger();
        logger.Log("Creating Logger");
    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse being clicked");
        }
    }
}
