using System.Collections;
using UnityEngine;

public class NetAnimator : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject[] faces;
    public float unfoldDuration = 1.5f;
    public float faceStaggerDelay = 0.05f;

    [Header("Original mesh to hide during net")]
    public Renderer[] solidRenderers;

    private Vector3[] assembledPositions;
    private Quaternion[] assembledRotations;
    private bool isUnfolded = false;
    private bool isAnimating = false;
    private float lastToggleTime = 0f;
    private float toggleCooldown = 0.5f;

    protected virtual Vector3[] GetNetPositions() { return new Vector3[0]; }
    protected virtual Quaternion[] GetNetRotations() { return new Quaternion[0]; }
    protected virtual Vector3 GetNetFacingRotation() { return Vector3.zero; }

    private void Awake()
    {
        assembledPositions = new Vector3[faces.Length];
        assembledRotations = new Quaternion[faces.Length];
        for (int i = 0; i < faces.Length; i++)
        {
            assembledPositions[i] = faces[i].transform.localPosition;
            assembledRotations[i] = faces[i].transform.localRotation;
        }
    }

    public void Toggle()
    {
        if (Time.time - lastToggleTime < toggleCooldown) return;
        if (isAnimating) return;
        lastToggleTime = Time.time;
        if (isUnfolded) StartCoroutine(Fold());
        else StartCoroutine(Unfold());
    }

    private IEnumerator Unfold()
    {
        isAnimating = true;
        isUnfolded = true;

        transform.localRotation = Quaternion.Euler(GetNetFacingRotation());

        foreach (Renderer r in solidRenderers)
            r.enabled = false;

        foreach (GameObject face in faces)
            face.SetActive(true);

        yield return new WaitForSeconds(20f); // debug pause — remove when done

        // face animation will go here later

        isAnimating = false;
    }

    private IEnumerator Fold()
    {
        isAnimating = true;
        isUnfolded = false;

        // fold animation will go here later

        isAnimating = false;
        yield break;
    }

    public bool IsUnfolded => isUnfolded;
    public bool IsAnimating => isAnimating;

    public void FoldImmediate()
    {
        StopAllCoroutines();
        isAnimating = false;
        isUnfolded = false;
        foreach (Renderer r in solidRenderers)
            r.enabled = true;
    }
}