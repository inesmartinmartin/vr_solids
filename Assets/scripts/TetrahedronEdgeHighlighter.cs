using UnityEngine;

public class TetrahedronEdgeHighlighter : EdgeHighlighter
{
    private void Awake()
    {
        edgePairs = new int[,]
        {
            // fill in your pairs here based on debug step
            // format: {indexA, indexB},
            {0, 1}, {0, 2}, {0, 3}, {1, 2}, {2, 3}, {3, 1}
        };
    }
}