using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class EdenArea : Area
{
[Tooltip("The agent inside the area")]
public AdamAgent adamAgent;

[Tooltip("The eve inside the area")]
public GameObject eve;

[Tooltip("The TextMeshPro text that shows the cumulative reward of the agent")]
public TextMeshPro cumulativeRewardText;

[Tooltip("Prefab of an Apple")]
public GameObject applePrefab;

private EdenAcademy edenAcademy;
private List<GameObject> appleList;

    public override void ResetArea()
    {
        RemoveAllApple();
        PlaceAdam();
        PlaceEve();
        SpawnApple(4);
    }
    public void RemoveSpecificApple(GameObject apple)
    {
      appleList.Remove(apple);
      Destroy(apple);
    }

    public int AppleRemaining
    {
        get{return appleList.Count;}
    }

    private void RemoveAllApple()
    {
        if(appleList != null)
        {
            for(int i=0;i<appleList.Count;i++)
            {
                if(appleList[i]!=null)
                {
                    Destroy(appleList[i]);
                }
            }
        }
        appleList=new List<GameObject>();
    } 
    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if (maxRadius > minRadius)
        {
            // Pick a random radius
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        }

        if (maxAngle > minAngle)
        {
            // Pick a random angle
            angle = UnityEngine.Random.Range(minAngle, maxAngle);
        }

        // Center position + forward vector rotated around the Y axis by "angle" degrees, multiplies by "radius"
        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
    }

    private void PlaceAdam()
    {
        Rigidbody rigidbody=adamAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        adamAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        adamAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }    
    
    private void PlaceEve()
    {
        Rigidbody rigidbody = eve.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        eve.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        eve.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void SpawnApple(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn and place the fish
            GameObject apple = Instantiate<GameObject>(applePrefab.gameObject);
            apple.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * .5f;
            apple.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            // Set the fish's parent to this area's transform
            apple.transform.SetParent(transform);

            // Keep track of the fish
            appleList.Add(apple);
        }
    }

    private void Start()
    {
        edenAcademy = FindObjectOfType<EdenAcademy>();
        ResetArea();
    }

    private void Update()
    {
        // Update the cumulative reward text
        cumulativeRewardText.text = adamAgent.GetCumulativeReward().ToString("0.00");
    }   
}
