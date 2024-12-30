using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Threading;
using UnityEditor.Experimental.GraphView;
public class MoveToGoalScript : Agent
{
    [SerializeField] private SpriteRenderer renderInstance;
    public override void OnEpisodeBegin()
    {
        //Muda a cor do agente usando o enum AgentColor
        switch ((AgentColor)Random.Range(0, 7))
        {
            case AgentColor.Red:
                renderInstance.color = Color.red;
                break;
            case AgentColor.Blue:
                renderInstance.color = Color.blue;
                break;
            case AgentColor.Green:
                renderInstance.color = Color.green;
                break;
            case AgentColor.Yellow:
                renderInstance.color = Color.yellow;
                break;
            case AgentColor.Purple:
                renderInstance.color = new Color(0.5f, 0, 0.5f);
                break;
            case AgentColor.Cyan:
                renderInstance.color = Color.cyan;
                break;
            case AgentColor.White:
                renderInstance.color = Color.white;
                break;
        }

        transform.position = new Vector3(Random.Range(-4f, 4f),Random.Range(-4f, 4f),0);
    }
    [SerializeField] private Transform targetTransform;
    public override void OnActionReceived(ActionBuffers actions)
    {
        float movex = actions.ContinuousActions[0];
        float movey = actions.ContinuousActions[1];
        float moveSpeed = 4f;
        transform.position += new Vector3(movex, movey, 0) * Time.deltaTime * moveSpeed;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("goal")) {
            SetReward(1f);
            EndEpisode();
        }
    }
    public void OnEpisodeEnd() {
        //Calcual a distancia entre o agente e o alvo
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        //Penaliza o agente com base na distancia
        SetReward(-distanceToTarget);
    }
    public void Update()
    {
        //Verifica se a o agente saiu do quadrado feito por
        //(-5,-5) e (5,5)
        if (transform.position.x < -8 || transform.position.x > 8 || transform.position.y < -5 || transform.position.y > 5)
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
        continuousActionsOut[1] = Input.GetAxisRaw("Vertical");
    }
}
public enum AgentColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Cyan,
    White,
}