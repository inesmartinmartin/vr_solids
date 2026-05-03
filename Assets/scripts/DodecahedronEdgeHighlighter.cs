using UnityEngine;

public class DodecahedronEdgeEdgeHighlighter : EdgeHighlighter
{
    private void Awake()
    {
        edgePairs = new int[,]
        {
            // fill in your pairs here based on debug step
            // format: {indexA, indexB},
            {0, 1}, {1, 2}, {2, 3}, {3, 4}, {4, 0},
            {0, 5}, {1, 6}, {2, 7}, {3, 8}, {4, 9},
            {5, 11}, {6, 12}, {7, 13}, {8, 14}, {9, 10},
            {6, 11}, {7, 12}, {8, 13}, {9, 14}, {10, 5},
            {19, 13}, {18, 12}, {17, 11}, {16, 10}, {15, 14},
            {19, 18}, {18, 17}, {17, 16}, {16, 15}, {15, 19}
            
        };
    }
}