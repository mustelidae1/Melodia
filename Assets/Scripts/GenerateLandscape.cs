using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLandscape : MonoBehaviour {

    public int numTrees = 10;
    public int numFlowers = 200;
    public int numGrass = 5000;
    public int numStones = 50; 

    float terrainWidth; 

    public GameObject tree;
    public List<GameObject> flowers; 
    public GameObject grass;
    public GameObject stone; 

    public Terrain terrain;
    public GameObject terrainBound;

    public bool displayTrees = true;
    public bool displayFlowers = true;
    public bool displayGrass = true;
    public bool displayStones = true;

    private int numFlowersSpawned = 0; 

    public static GenerateLandscape s; 

    private void Start()
    {
        s = this; 

        terrainWidth = terrainBound.transform.position.x;
        if (displayTrees)
        {
            for (int i = 0; i < numTrees; i++)
            {
                float xPos = Random.Range(0, terrainWidth);
                float zPos = Random.Range(0, terrainWidth);
                Vector3 treePos = new Vector3(xPos, 0, zPos);
                float treeHeight = terrain.SampleHeight(treePos);
                treePos.y = treeHeight - 1;
                GameObject newTree = Instantiate(tree);
                newTree.transform.Rotate(0, Random.Range(0, 360), 0);
                newTree.transform.position = treePos;
            }
        }
     
     
        if (displayFlowers)
        {
            for (int i = 0; i < numFlowers; i++)
            {
                float xPos = Random.Range(200, 300);
                float zPos = Random.Range(70, 220);
                Vector3 flowerPos = new Vector3(xPos, 0, zPos);
                float flowerHeight = terrain.SampleHeight(flowerPos);
                flowerPos.y = flowerHeight;
                GameObject newTree = Instantiate(flowers[Random.Range(0, flowers.Count)]);
                newTree.transform.position = flowerPos;
            }
        }
        
        if (displayGrass)
        {
            for (int i = 0; i < numGrass; i++)
            {
                float xPos = Random.Range(200, 400);
                float zPos = Random.Range(70, 220);
                Vector3 grassPos = new Vector3(xPos, 0, zPos);
                float grassHeight = terrain.SampleHeight(grassPos);
                grassPos.y = grassHeight;
                GameObject newGrass = Instantiate(grass);
                newGrass.transform.localScale = new Vector3(10, 8, 10);
                newGrass.transform.Rotate(0, Random.Range(0, 360), 0);
                newGrass.transform.position = grassPos;

                int clusterNum = Random.Range(0, 10);
                // Maybe make this recursive 
                for (int j = 0; j < clusterNum; j++)
                {
                    float xOffset = Random.Range(-2f, 2f);
                    float zOffset = Random.Range(-2f, 2f);

                    GameObject newGrass1 = Instantiate(grass);
                    //newGrass.transform.eulerAngles = new Vector3(0, Random.Range(0, 180), 90);
                    newGrass1.transform.Rotate(0, Random.Range(0, 360), 0);
                    newGrass1.transform.localScale = new Vector3(8, 7, 8);

                    Vector3 newGrassPos = new Vector3(grassPos.x + xOffset, grassPos.y, grassPos.z + zOffset);
                    newGrassPos.y = terrain.SampleHeight(newGrassPos);
                    newGrass1.transform.position = newGrassPos;

                    numGrass--;
                }

            }
        }
       
        if (displayStones)
        {
            for (int i = 0; i < numStones; i++)
            {
                float xPos = Random.Range(200, 400);
                float zPos = Random.Range(70, 220);
                Vector3 stonePos = new Vector3(xPos, 0, zPos);
                float stoneHeight = terrain.SampleHeight(stonePos);
                stonePos.y = stoneHeight - 0.1f;
                GameObject newStone = Instantiate(stone);
                newStone.transform.position = stonePos;
            }
        }
       
    }

    // TODO: Possibly also create a random flower somewhere else to fill in the landscape more??? 
    public void createFlower(float xPos, float zPos, int note)
    {
        if (numFlowersSpawned <= numFlowers)
        {
            Vector3 flowerPos = new Vector3(xPos, 0, zPos);
            float flowerHeight = terrain.SampleHeight(flowerPos);
            flowerPos.y = flowerHeight;
            //GameObject newFlower = Instantiate(flowers[Random.Range(0, flowers.Count)]);
            GameObject newFlower = Instantiate(flowers[note]);
            newFlower.SetActive(true);
            newFlower.transform.position = flowerPos;
            Animator fAnimator = newFlower.GetComponent<Animator>();
            fAnimator.SetTrigger("flowerGrow");
            numFlowersSpawned++;
        }
    }

    private void addObjects(int num, float heightOffset, float rangeStart, float rangeEnd, GameObject item)
    {
      
    }

}
