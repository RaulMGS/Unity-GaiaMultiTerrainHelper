using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GaiaMultiTerrainHelper : MonoBehaviour
{

    [SerializeField]
    private int centerTerrain;

    [SerializeField]
    private Gaia.GaiaSessionManager gaiaSessionManager;

    [SerializeField]
    private GameObject gaiaStampsRoot;
    
    [SerializeField, HideInInspector]
    private int activeTerrain;

    [SerializeField]
    private List<Terrain> terrains = new List<Terrain>();

    // [SerializeField]
    // private List<Gaia.Stamper> terrainStampers = new List<Gaia.Stamper>();
    
    public void Refresh()
    {
        terrains.Clear();
        //terrainStampers.Clear();

        foreach (Terrain terrain in gameObject.GetComponentsInChildren<Terrain>())
        {
            terrains.Add(terrain);
            //terrainStampers.Add(terrain.GetComponentInChildren<Gaia.Stamper>());
        }
    }
    public void SetGaiaActive(int terrainIdx)
    {
        activeTerrain = terrainIdx;
        terrains.ForEach(x => x.gameObject.SetActive(false));
        terrains[terrainIdx].gameObject.SetActive(true);

        // size of terrain (all terrains must have same size)
        int sizeX = (int)terrains[terrainIdx].terrainData.size.x;
        int sizeY = (int)terrains[terrainIdx].terrainData.size.z;
        terrains[terrainIdx].transform.position = new Vector3(- sizeX / 2, 0, - sizeY / 2);

        // set base stamp position
        terrains[terrainIdx].GetComponentInChildren<Gaia.Stamper>().transform.localPosition = new Vector3(sizeX / 2, 0, sizeX / 2);

        // offset stamps 
        // matrix index of terrain
        int cx = int.Parse(terrains[centerTerrain].name.Split('_')[1]);
        int cy = int.Parse(terrains[centerTerrain].name.Split('_')[2]);

        // get matrix positions
        int tx = int.Parse(terrains[terrainIdx].name.Split('_')[1]);
        int ty = int.Parse(terrains[terrainIdx].name.Split('_')[2]);

        // get matrix position relative to center
        int distX = tx - cx; // -size/2 to center terrain at 0
        int distY = cy - ty; // -size/2 to center terrain at 0
        
        gaiaStampsRoot.transform.position = new Vector3(-distY * sizeY, 0, -distX * sizeX);
    }
    public void SetGaiaMatrix()
    {
        // set active
        terrains.ForEach(x => x.gameObject.SetActive(true));

        // set stamp root
        gaiaStampsRoot.transform.position = new Vector3(0, 0, 0);

        // matrix index of center terrain
        int cx = int.Parse(terrains[centerTerrain].name.Split('_')[1]);
        int cy = int.Parse(terrains[centerTerrain].name.Split('_')[2]);

        // size of center terrain (all terrains must have same size)
        int sizeX = (int)terrains[centerTerrain].terrainData.size.x;
        int sizeY = (int)terrains[centerTerrain].terrainData.size.z;

        for (int i = 0; i < terrains.Count; i++)
        {
            // get matrix positions
            int x = int.Parse(terrains[i].name.Split('_')[1]);
            int y = int.Parse(terrains[i].name.Split('_')[2]);

            // get matrix position relative to center
            int distX = x - cx; // -size/2 to center terrain at 0
            int distY = cy - y; // -size/2 to center terrain at 0

            // set position
            terrains[i].transform.position = new Vector3(distY * sizeY - (sizeY / 2), 0, distX * sizeX - (sizeX / 2));

            // set base stamp position
            terrains[i].GetComponentInChildren<Gaia.Stamper>().transform.localPosition = new Vector3(sizeX / 2, 0, sizeX / 2);
        }
    }
    public void FlattenAll()
    {
        foreach (Terrain terrain in terrains)
        {
            int terrainX = terrain.terrainData.heightmapWidth;
            int terrainY = terrain.terrainData.heightmapHeight;
            var heights = terrain.terrainData.GetHeights(0, 0, terrainX, terrainY);

            for (int x = 0; x < terrainX; x++)
            {
                for (int y = 0; y < terrainY; y++)
                {
                    heights[x, y] = 0;
                }
            }
            terrain.terrainData.SetHeights(0, 0, heights);
        }
    }
    public void PlaySession()
    {
        gaiaSessionManager.PlaySession();
    }
}
