using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    private bool doJumpInput;
    private bool doInteractInput;


    #region Properties

    public float getHotizontalInput
    {
        get { return horizontalInput; }
        private set { horizontalInput = value; }
    }

    public float getInteractInput
    {
        get { return verticalInput; }
        private set { verticalInput = value; }
    }

    public bool getDoJumpInput
    {
        get { return doJumpInput; }
        private set { doJumpInput = value; }
    }

    public bool getDoInteractInput
    {
        get { return doInteractInput; }
        private set { doInteractInput = value; }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
