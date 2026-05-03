using UnityEngine;
using UnityEngine.InputSystem;

public class IcosahedronNetAnimator : NetAnimator
{
    protected override Vector3 GetNetFacingRotation()
    {
        return new Vector3(-127.38f, 0f, -18f);
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            for (int i = 0; i < faces.Length; i++)
            {
                Debug.Log("Face_" + (i + 1) + " assembled rotation: " + faces[i].transform.localEulerAngles);
            }
        }
        
    }
}