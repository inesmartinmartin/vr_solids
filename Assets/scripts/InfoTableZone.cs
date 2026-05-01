using UnityEngine;
using System.Collections;

public class InfoTableZone : MonoBehaviour
{
    public Transform floatPoint;
    public float rotationSpeed = 30f;
    public SolidIdentity CurrentSolid => currentSolid;

    [Header("UI")]
    public InfoTableUI tableUI;


    private SolidIdentity currentSolid;
    private Rigidbody currentRigidbody;
    private bool isFrozen = false;
    private bool isInsideZone = false;


    private void OnTriggerEnter(Collider other)
    {
        SolidIdentity identity = other.GetComponent<SolidIdentity>();
        if (identity != null && currentSolid == null)
        {
            currentSolid = identity;
            currentRigidbody = other.GetComponent<Rigidbody>();
            isInsideZone = true;
            // Don't freeze yet — player is still holding it
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SolidIdentity identity = other.GetComponent<SolidIdentity>();
        if (identity != null && identity == currentSolid && !isFrozen)
        {
            // Solid left the zone while player was still holding it
            // so just unregister it, don't touch physics
            currentSolid = null;
            currentRigidbody = null;
            isInsideZone = false;
        }
    }

    public void OnSolidReleased(GameObject solidObj)
    {
        if (currentSolid != null
            && currentSolid.gameObject == solidObj
            && isInsideZone)
        {
            FreezeSolid();
        }
        else
        {
            // Released outside the zone — make sure physics is fully restored
            // regardless of what XR Grab Interactable thinks the previous state was
            Rigidbody rb = solidObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            // Clean up table state in case this was our solid
            if (currentSolid != null && currentSolid.gameObject == solidObj)
            {
                currentSolid = null;
                currentRigidbody = null;
                isFrozen = false;
                isInsideZone = false;
                tableUI.ShowIdle();
            }
        }
    }

    // Wired to Select Entered — player just grabbed something
    public void OnSolidGrabbed(GameObject solidObj)
    {
        // Only act if the grabbed object is the one WE froze
        if (isFrozen
            && currentSolid != null
            && currentSolid.gameObject == solidObj)
        {
            if (currentRigidbody != null)
            {
                currentRigidbody.isKinematic = false;
                currentRigidbody.useGravity = true;
            }
            currentSolid = null;
            currentRigidbody = null;
            isFrozen = false;
            isInsideZone = false;
            tableUI.ShowIdle();
        }
        // If the grabbed object is not our frozen solid, do nothing
    }

    private void FreezeSolid()
    {
        if (currentRigidbody != null)
        {
            currentRigidbody.linearVelocity = Vector3.zero;
            currentRigidbody.angularVelocity = Vector3.zero;
            currentRigidbody.useGravity = false;
            currentRigidbody.isKinematic = true;
        }
        if (currentSolid != null)
        {
            isFrozen = true; // set early so Update() rotation doesn't fight the coroutine
            StartCoroutine(FloatToPoint(currentSolid.transform, floatPoint.position, 0.4f));
            tableUI.ShowSolid(currentSolid.data);
        }
    }

    private IEnumerator FloatToPoint(Transform solidTransform, Vector3 target, float duration)
    {
        Vector3 startPos = solidTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (solidTransform == null) yield break; // guard if grabbed mid-flight
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Smooth ease-out curve
            t = 1f - Mathf.Pow(1f - t, 3f);

            solidTransform.position = Vector3.Lerp(startPos, target, t);
            yield return null;
        }

        if (solidTransform != null)
            solidTransform.position = target;
    }

    private void Update()
    {
        if (currentSolid != null && isFrozen)
        {
            currentSolid.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}