using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent 
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float moveX = vectorAction[0];
        float moveY = vectorAction[1];

        transform.position += new Vector3(moveX, moveY, 0f).normalized * moveSpeed * Time.deltaTime;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void Heuristic(float[] actionsOut)
    {
        float[] continousActions = actionsOut;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
        continousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MonsterEnergy"))
        {
            SetReward(1f);
            EndEpisode();
        }

        if (other.CompareTag("Water"))
        {
            //SetReward(-1f);
            EndEpisode();
        }
    }

}
