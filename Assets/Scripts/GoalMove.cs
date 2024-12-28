using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMove : MonoBehaviour
{

     // Configuração dos limites
    public Vector2 xBounds = new Vector2(-8, 8);
    public Vector2 yBounds = new Vector2(-5, 5);

    // Configuração do movimento browniano
    public float brownianStepRate = 0.5f; // Intensidade do movimento browniano
    public float brownianStepInterval = 0.1f; // Intervalo de atualização para o movimento browniano

    // Configuração do movimento direcionado
    public float directedSpeed = 1.0f; // Velocidade do movimento direcionado
    public float directionChangeInterval = 2.0f; // Tempo entre mudanças de direção

    private Vector3 currentDirection; // Direção atual do movimento direcionado
    private float nextBrownianTime = 0f;
    private float nextDirectionChangeTime = 0f;

    void Start()
    {
        // Inicializar direção aleatória
        ChangeDirection();
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        // Movimento direcionado
        if (Time.time >= nextDirectionChangeTime)
        {
            nextDirectionChangeTime = Time.time + directionChangeInterval;
            ChangeDirection();
        }

        Vector3 directedMovement = currentDirection * directedSpeed * deltaTime;

        // Movimento browniano
        Vector3 brownianMovement = Vector3.zero;
        if (Time.time >= nextBrownianTime)
        {
            nextBrownianTime = Time.time + brownianStepInterval;

            float deltaX = Random.Range(-brownianStepRate, brownianStepRate);
            float deltaY = Random.Range(-brownianStepRate, brownianStepRate);

            brownianMovement = new Vector3(deltaX, deltaY, 0);
        }

        // Combinar movimentos
        Vector3 newPosition = transform.position + directedMovement + brownianMovement;

        // Limitar a posição aos limites da tela
        newPosition.x = Mathf.Clamp(newPosition.x, xBounds.x, xBounds.y);
        newPosition.y = Mathf.Clamp(newPosition.y, yBounds.x, yBounds.y);

        // Aplicar a nova posição
        transform.position = newPosition;
    }

    // Função para alterar a direção aleatoriamente
    void ChangeDirection()
    {
        float angle = Random.Range(0f, 360f); // Ângulo aleatório em graus
        float radian = angle * Mathf.Deg2Rad;
        currentDirection = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0).normalized;
    }
}
