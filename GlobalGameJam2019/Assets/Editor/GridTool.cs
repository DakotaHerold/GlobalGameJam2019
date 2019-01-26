using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Jam
{
    public class GridTool : EditorWindow
    {
        private int numRows = 5;
        private int numCols = 5;
        private float xOffset = 1.1f;
        private float yOffset = 1.1f;
        private GameObject tilePrefab;

        private bool[,] tileChecks;

        Vector2 scrollPos;

        private const float MIN_WIDTH = 500f;
        private const float MIN_HEIGHT = 800f;

        private static GridTool window;

        [MenuItem("Tools/GridTool")]
        public static void ShowWindow()
        {
            window = GetWindow<GridTool>("GridTool");
            window.minSize = new Vector2(MIN_WIDTH, MIN_HEIGHT);
        }

        private void OnGUI()
        {
            tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab: ", tilePrefab, typeof(GameObject), true);
            if (tilePrefab == null)
                return;

            EditorGUI.BeginChangeCheck();
            numRows = EditorGUILayout.IntField("Number of Rows: ", numRows);
            numCols = EditorGUILayout.IntField("Number of Columns: ", numCols);

            if (EditorGUI.EndChangeCheck())
            {
                if (numRows < 1 || numCols < 1)
                {
                    //Debug.LogWarning("Cannot create grid, invalid number of rows or columns");
                    return;
                }
                else
                {
                    tileChecks = new bool[numRows, numCols];
                }
            }
            if (numRows < 1 || numCols < 1)
            {
                return;
            }
            if (tileChecks == null)
            {
                tileChecks = new bool[numRows, numCols];
            }

            xOffset = EditorGUILayout.FloatField("Tile X Offset:", xOffset);
            yOffset = EditorGUILayout.FloatField("Tile Y Offset:", yOffset);

            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height((position.height / 2f)));
            EditorGUILayout.BeginHorizontal();
            // Visualize field
            for (int iRow = 0; iRow < numRows; ++iRow)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(16));
                for (int iCol = 0; iCol < numCols; ++iCol)
                {
                    tileChecks[iRow, iCol] = GUILayout.Toggle(tileChecks[iRow, iCol], string.Empty, GUILayout.Width(16));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Select All"))
            {
                for (int iRow = 0; iRow < numRows; ++iRow)
                {
                    ;
                    for (int iCol = 0; iCol < numCols; ++iCol)
                    {
                        tileChecks[iRow, iCol] = true;
                    }
                }
            }
            if (GUILayout.Button("Generate On Selected Object"))
            {
                if (tilePrefab == null || numRows < 1 || numCols < 1)
                    return;
                GenerateOnSelected();
            }

        }

        private void GenerateOnSelected()
        {
            // TODO, Invert tile generation to go row first instead of column first 
            Vector3 startPos = Selection.activeGameObject.transform.position;
            Vector3 currentPos = startPos;
            for (int iRow = 0; iRow < numRows; ++iRow)
            {
                for (int jCol = 0; jCol < numCols; ++jCol)
                {
                    // Spawn Tile 
                    if (tileChecks[iRow, jCol])
                    {
                        GameObject newTile = GameObject.Instantiate(tilePrefab);
                        newTile.transform.position = currentPos;
                        newTile.transform.parent = Selection.activeGameObject.transform;

                        //newTile.GetComponent<Tile>().row = iRow;
                        //newTile.GetComponent<Tile>().col = jCol;
                    }


                    currentPos.y -= yOffset;
                }
                currentPos.y = startPos.y;
                currentPos.x += xOffset;
            }
        }
    }
}
