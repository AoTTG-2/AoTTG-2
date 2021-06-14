// Terrain Rotate tool by UnityCoder.com

using UnityEditor;
using UnityEngine;

namespace unitycoder_TerrainRotator
{

    public class TerrainRotator : EditorWindow
    {

        private const string appName = "Terrain Rotator";

        private bool autoRotate = true;
        private float angle = 0; // rotation angle
        private float oldAngle = -1; // previous angle
        private bool isRotating = false; // are we currently rotating

        private float[,] origHeightMap; // original heightmap, unrotated
        private int[][,] origDetailLayer; // original detail layer, unrotated
        private float[,,] origAlphaMap; // original alphamap, unrotated
        private TreeInstance[] origTrees; // original trees, unrotated

        private bool grabOriginal = false; // have we grabbed original data

        [MenuItem("Window/Terrain/" + appName)]
        static void Init()
        {
            TerrainRotator window = (TerrainRotator)EditorWindow.GetWindow(typeof(TerrainRotator));
            window.titleContent = new GUIContent(appName);
            window.minSize = new Vector2(300, 250);
        }



        // GUI
        void OnGUI()
        {
            GUILayout.Label(appName, EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // display error message if nothing or more than 1 object selected
            if (Selection.gameObjects.Length != 1)
            {
                GUI.enabled = false;
                GUILayout.Label("ERROR: You must have 1 terrain selected", EditorStyles.miniLabel);
                EditorGUILayout.Space();
            } else
            {
                if (Selection.gameObjects.Length == 1)
                {
                    if (Selection.gameObjects[0].GetComponent<Terrain>() == null)
                    {
                        GUI.enabled = false;
                        GUILayout.Label("ERROR: You must have 1 terrain selected", EditorStyles.miniLabel);
                        EditorGUILayout.Space();
                    }
                }
            }

            // button: read data
            if (GUILayout.Button("Read selected terrain data", GUILayout.Height(30)))
            {
                // check if its terrain
                if (Selection.gameObjects[0].GetComponent<Terrain>() != null)
                {
                    // grab terrain data (heightmap)
                    Terrain t = Selection.gameObjects[0].GetComponent<Terrain>();
                    origHeightMap = t.terrainData.GetHeights(0, 0, t.terrainData.heightmapResolution, t.terrainData.heightmapResolution);
                    origDetailLayer = new int[t.terrainData.detailPrototypes.Length][,];
                    for (int n = 0; n < t.terrainData.detailPrototypes.Length; n++)
                    {
                        origDetailLayer[n] = t.terrainData.GetDetailLayer(0, 0, t.terrainData.detailWidth, t.terrainData.detailHeight, n);
                    }
                    origAlphaMap = t.terrainData.GetAlphamaps(0, 0, t.terrainData.alphamapWidth, t.terrainData.alphamapHeight);
                    origTrees = t.terrainData.treeInstances;
                    angle = 0;
                    oldAngle = 0;
                    grabOriginal = true;

                } // if terrain
            } // if button

            // disable gui if havent read data or if currently rotating
            if (!grabOriginal || isRotating)
            {
                GUI.enabled = false;
            }

            GUILayout.Space(15);

            // display angle
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Angle:");
            angle = EditorGUILayout.FloatField(Mathf.Clamp(angle, 0, 360), GUILayout.Width(50));
            autoRotate = GUILayout.Toggle(autoRotate, "AutoRotate");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            // rotation angle
            angle = GUILayout.HorizontalSlider((int)angle, 0, 360);

            // rotate button & angle display
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Rotate", GUILayout.Height(30)) || (autoRotate && oldAngle != angle && grabOriginal))
            {
                if (!isRotating)
                {

                    // if terrain
                    if (Selection.gameObjects[0].GetComponent<Terrain>() != null)
                    {
                        TerrainRotate(Selection.gameObjects[0]);
                    }

                } // if not rotating
            } // button pressed


            // get previous angle
            oldAngle = angle;
            GUILayout.EndHorizontal();

            // if autorotate enabled
            if (autoRotate)
            {
                if (oldAngle != angle && grabOriginal)
                {
                    TerrainRotate(Selection.gameObjects[0]);
                }
            }


            GUILayout.Space(10);


            // reset button
            if (GUILayout.Button("Reset rotation", GUILayout.Height(20)))
            {
                if (Selection.gameObjects[0].GetComponent<Terrain>() != null)
                {
                    TerrainReset(Selection.gameObjects[0]);
                } // if terrain
            } // if reset button

            /*
			// TODO: show terrain stats, resolution etc
			if (Selection.gameObjects.Length==1)
			{
				if (Selection.gameObjects[0].GetComponent<Terrain>())
				{

				}
			}else{ // nothing selected
				GUILayout.Label("");	
			}
			*/


        } // ongui






        // *** FUNCTIONS ***

        // rotate terrain
        void TerrainRotate(GameObject go)
        {

            if (origHeightMap == null)
            {
                grabOriginal = false;
                Debug.LogWarning("Cannot rotate terrain, array has been cleared..");
                return;
            }

            isRotating = true;

            Terrain terrain = go.GetComponent<Terrain>();

            int nx, ny;
            float cs, sn;

            // heightmap rotation
            int tw = terrain.terrainData.heightmapResolution;
            int th = terrain.terrainData.heightmapResolution;
            float[,] newHeightMap = new float[tw, th];
            float angleRad = angle * Mathf.Deg2Rad;
            float heightMiddle = (terrain.terrainData.heightmapResolution) / 2.0f; // pivot at middle

            for (int y = 0; y < th; y++)
            {
                for (int x = 0; x < tw; x++)
                {
                    cs = Mathf.Cos(angleRad);
                    sn = Mathf.Sin(angleRad);

                    nx = (int)((x - heightMiddle) * cs - (y - heightMiddle) * sn + heightMiddle);
                    ny = (int)((x - heightMiddle) * sn + (y - heightMiddle) * cs + heightMiddle);

                    if (nx < 0) nx = 0;
                    if (nx > tw - 1) nx = tw - 1;
                    if (ny < 0) ny = 0;
                    if (ny > th - 1) ny = th - 1;

                    newHeightMap[x, y] = origHeightMap[nx, ny];
                } // for x
            } // for y



            // detail layer (grass, meshes)
            int dw = terrain.terrainData.detailWidth;
            int dh = terrain.terrainData.detailHeight;
            float detailMiddle = (terrain.terrainData.detailResolution) / 2.0f; // pivot at middle
            int numDetails = terrain.terrainData.detailPrototypes.Length;
            int[][,] newDetailLayer = new int[numDetails][,];

            // build new layer arrays
            for (int n = 0; n < numDetails; n++)
            {
                newDetailLayer[n] = new int[dw, dh];
            }

            for (int z = 0; z < numDetails; z++)
            {
                for (int y = 0; y < dh; y++)
                {
                    for (int x = 0; x < dw; x++)
                    {
                        cs = Mathf.Cos(angleRad);
                        sn = Mathf.Sin(angleRad);

                        nx = (int)((x - detailMiddle) * cs - (y - detailMiddle) * sn + detailMiddle);
                        ny = (int)((x - detailMiddle) * sn + (y - detailMiddle) * cs + detailMiddle);


                        if (nx < 0) nx = 0;
                        if (nx > dw - 1) nx = dw - 1;
                        if (ny < 0) ny = 0;
                        if (ny > dh - 1) ny = dh - 1;

                        newDetailLayer[z][x, y] = origDetailLayer[z][nx, ny];
                    } // for x
                } // for y
            } // for z


            // alpha layer (texture splatmap) rotation
            dw = terrain.terrainData.alphamapWidth;
            dh = terrain.terrainData.alphamapHeight;
            int dz = terrain.terrainData.alphamapLayers;
            float alphaMiddle = (terrain.terrainData.alphamapResolution) / 2.0f; // pivot at middle
            float[,,] newAlphaMap = new float[dw, dh, dz];

            for (int z = 0; z < dz; z++)
            {
                for (int y = 0; y < dh; y++)
                {
                    for (int x = 0; x < dw; x++)
                    {
                        cs = Mathf.Cos(angleRad);
                        sn = Mathf.Sin(angleRad);

                        nx = (int)((x - alphaMiddle) * cs - (y - alphaMiddle) * sn + alphaMiddle);
                        ny = (int)((x - alphaMiddle) * sn + (y - alphaMiddle) * cs + alphaMiddle);

                        if (nx < 0) nx = 0;
                        if (nx > dw - 1) nx = dw - 1;
                        if (ny < 0) ny = 0;
                        if (ny > dh - 1) ny = dh - 1;

                        newAlphaMap[x, y, z] = origAlphaMap[nx, ny, z];
                    } // for x
                } // for y
            } // for z



            // trees rotation, one by one..
            // TODO: use list instead, then can remove trees outside the terrain
            int treeCount = terrain.terrainData.treeInstances.Length;
            TreeInstance[] newTrees = new TreeInstance[treeCount];
            Vector3 newTreePos = Vector3.zero;
            float tx, tz;

            for (int n = 0; n < treeCount; n++)
            {

                cs = Mathf.Cos(angleRad);
                sn = Mathf.Sin(angleRad);

                tx = origTrees[n].position.x - 0.5f;
                tz = origTrees[n].position.z - 0.5f;

                newTrees[n] = origTrees[n];

                newTreePos.x = (cs * tx) - (sn * tz) + 0.5f;
                newTreePos.y = origTrees[n].position.y;
                newTreePos.z = (cs * tz) + (sn * tx) + 0.5f;

                newTrees[n].position = newTreePos;
            } // for treeCount

            // this is too slow in unity..
            //Undo.RecordObject(terrain.terrainData,"Rotate terrain ("+angle+")");

            // Apply new data to terrain
            terrain.terrainData.SetHeights(0, 0, newHeightMap);
            terrain.terrainData.SetAlphamaps(0, 0, newAlphaMap);
            terrain.terrainData.treeInstances = newTrees;
            for (int n = 0; n < terrain.terrainData.detailPrototypes.Length; n++)
            {
                terrain.terrainData.SetDetailLayer(0, 0, n, newDetailLayer[n]);
            }

            // we are done..
            isRotating = false;

        } //TerrainRotate




        // restore terrain data (from previous grab)
        void TerrainReset(GameObject go)
        {
            angle = 0;
            oldAngle = angle;
            Terrain terrain = go.GetComponent<Terrain>();

            if (origHeightMap == null)
            {
                grabOriginal = false;
                Debug.LogWarning("Cannot reset terrain, array has been cleared..");
                return;
            }

            terrain.terrainData.SetHeights(0, 0, origHeightMap);
            terrain.terrainData.SetAlphamaps(0, 0, origAlphaMap);
            for (int n = 0; n < terrain.terrainData.detailPrototypes.Length; n++)
            {
                terrain.terrainData.SetDetailLayer(0, 0, n, origDetailLayer[n]);
            }
            terrain.terrainData.treeInstances = origTrees;
        }

        // update editor window 
        void OnInspectorUpdate()
        {
            Repaint();
        }

    } //class

} // namespace

