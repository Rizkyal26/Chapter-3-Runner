using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{
    [Header("Templates")]
    public List<TerrainTemplateController> terrainTemplates;
    public float terrainTemplateWidth;

    [Header("Force Early Template")]
    public List<TerrainTemplateController> earlyTerrainTemplates;

    [Header("Generator Area")]
    public Camera gameCamera;
    public float areaStartOffset;
    public float areaEndOffset;

    private List<GameObject> spawnedTerrain;

    private float lastGeneratedPositionX;
    private float lastRemovedPositionX;

    private Vector3 areaStarPosition;
    private const float debugLineHeight = 10.0f;

    private Dictionary<string, List<GameObject>> pool;

    private void Start()
    {
        pool = new Dictionary<string, List<GameObject>>();

        spawnedTerrain = new List<GameObject>();

        lastGeneratedPositionX = GetHorizontalPositionStart();
        lastRemovedPositionX = lastRemovedPositionX - terrainTemplateWidth;

        foreach (TerrainTemplateController terrain in earlyTerrainTemplates)
        {
            GenerateTerrain(lastGeneratedPositionX, terrain);
            lastGeneratedPositionX += terrainTemplateWidth;
        }

        while (lastGeneratedPositionX < GetHorizontalEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
    }


    // Update is called once per frame
    private void Update()
    {
        while (lastGeneratedPositionX < GetHorizontalEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
        while (lastRemovedPositionX + terrainTemplateWidth  < GetHorizontalPositionStart())
        {
            RemoveTerrain(lastRemovedPositionX);
            lastRemovedPositionX += terrainTemplateWidth;
        }
    }

    private float GetHorizontalPositionStart()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
    }

    private float GetHorizontalEnd()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;
    }

    private void GenerateTerrain(float posX, TerrainTemplateController forceTerrain = null)
    {
        GameObject newTerrain = null;

        if (forceTerrain == null)
        {
            newTerrain = Generatefrompool(terrainTemplates[Random.Range(0, terrainTemplates.Count)].gameObject, transform);
        }
        else
        {
            newTerrain = Generatefrompool(forceTerrain.gameObject, transform); 
        }

        newTerrain.transform.position = new Vector2(posX, 0f);

        spawnedTerrain.Add(newTerrain);
    }

    private void RemoveTerrain(float posX)
    {
        GameObject terraintoRemove = null;

        foreach (GameObject item in spawnedTerrain)
        {
            if (item.transform.position.x == posX)
            {
                terraintoRemove = item;
                break;
            }
        }

        if (terraintoRemove != null)
        {
            spawnedTerrain.Remove(terraintoRemove);
            returntopool(terraintoRemove);
        }
    }

    private GameObject Generatefrompool(GameObject item, Transform parent)
    {
        if (pool.ContainsKey(item.name))
        {
            if (pool[item.name].Count > 0)
            {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                return newItemFromPool;
            }
        }
        else
        {
            pool.Add(item.name, new List<GameObject>());
        }

        GameObject newItem = Instantiate(item, parent);
        newItem.name = item.name;
        return newItem;
    }

    private void returntopool(GameObject item)
    {
        if (!pool.ContainsKey(item.name));
        {
            Debug.LogError("Invalid Pool Item!");
        }


        pool[item.name].Add(item);
        item.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart();
        areaEndPosition.x = GetHorizontalEnd();

        Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, areaStartPosition + Vector3.down * debugLineHeight /
                2, Color.green);
        Debug.DrawLine(areaEndPosition + Vector3.up * debugLineHeight / 2, areaEndPosition + Vector3.down * debugLineHeight /
        2, Color.green);

    }
}

 