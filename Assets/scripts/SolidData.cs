using UnityEngine;

// This line adds "Solid Data" to your Right-Click menu in Unity
[CreateAssetMenu(fileName = "NewSolidData", menuName = "Project/Solid Data")]
public class SolidData : ScriptableObject {
    public string solidName;
    
    [TextArea(3, 10)] // Makes the text box bigger in the editor
    public string description;

    public int vertexCount;
    public int edgeCount;
    public int faceCount;
    public string eulerFormula = "V - E + F = 2";
}