using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AdamAgent : Agent
{
[Tooltip("How fast the agent moves forward")]
public float moveSpeed=5f;

[Tooltip("How fast does the agent turn")]
public float turnSpeed=180f;

[Tooltip("Prefab of heart that appears when eve gets an apple")]
public GameObject heartPrefab;

public GameObject applePrefab;

private EdenArea edenArea;
private EdenAcademy edenAcademy;
new private Rigidbody rigidbody;
private GameObject eveBody;

private bool isFull;//if eve is full
private float catchRadius=0f;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        edenArea=GetComponentInParent<EdenArea>();
        edenAcademy=FindObjectOfType<EdenAcademy>();
        eveBody=edenArea.eve;
        rigidbody=GetComponent<Rigidbody>();
    }
    
    public override void AgentAction(float[] vectorAction) 
    {
      
    // Convert the first action to forward movement
    float forwardAmount = vectorAction[0];

    // Convert the second action to turning left or right
    float turnAmount = 0f;
    if (vectorAction[1] == 1f)
    {
        turnAmount = -1f;
    }
    else if (vectorAction[1] == 2f)
    {
        turnAmount = 1f;
    }

    // Apply movement
    rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
    transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

    // Apply a tiny negative reward every step to encourage action
    AddReward(-1f / agentParameters.maxStep);  
    }
    public override float[] Heuristic()
    {
        float forwardAction = 0f;
        float turnAction = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            forwardAction = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // turn left
            turnAction = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // turn right
            turnAction = 2f;
        }

        // Put the actions into an array and return
        return new float[] { forwardAction, turnAction };
    }

    public override void AgentReset()
    {
       isFull=false;
       edenArea.ResetArea();
       catchRadius=edenAcademy.CatchRadius;
    }

    public override void CollectObservations()
    {
        AddVectorObs(isFull);
        AddVectorObs(Vector3.Distance(eveBody.transform.position,transform.position));
        AddVectorObs((eveBody.transform.position-transform.position).normalized);
        AddVectorObs(transform.forward);
    }
    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position,eveBody.transform.position)<catchRadius)
        {
            DropApple();
        }
    }

    private void OnCollisionEnter(Collision collision)
{
    if (collision.transform.CompareTag("apple"))
    {
        // Try to eat the fish
        EatApple(collision.gameObject);
    }
    else if (collision.transform.CompareTag("eve"))
    {
        // Try to feed the baby
        DropApple();
    }
}
    private void EatApple(GameObject apple)
    {
        if (isFull) return; // Can't eat another apple while full
        isFull = true;

        edenArea.RemoveSpecificApple(apple);

        AddReward(1f);
    }

    private void DropApple()
    {
        if (!isFull) return; // Nothing to drop
        isFull = false;

        
        GameObject appleDrop = Instantiate<GameObject>(applePrefab);
        appleDrop.transform.parent = transform.parent;
        appleDrop.transform.position = eveBody.transform.position;
        Destroy(appleDrop, 4f);

    
        GameObject heart = Instantiate<GameObject>(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.position = eveBody.transform.position + Vector3.up;
        Destroy(heart, 4f);

        AddReward(1f);

        if (edenArea.AppleRemaining <= 0)
        {
            Done();
        }
    }
}
