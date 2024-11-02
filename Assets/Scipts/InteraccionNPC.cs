using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteraccionNPC : MonoBehaviour
{
    public Transform Jugador; // Referencia al transform del jugador
    public float distanciaInteraccion = 1.5f; // Distancia de interacción para activar
    public float velocidadRotacion = 100f; // Velocidad de rotación durante la interacción
    public float distanciaDetrasX = -1f; // Distancia fija detrás del jugador en el eje X
    private bool estaInteraccionando = false; // Controla el estado de interacción
    private bool seguirDetrasDelJugador = false; // Controla si el NPC debe estar detrás del jugador
    private bool interaccionRealizada = false; // Controla si la interacción ya se realizó
    private float tiempoInteraccion = 0f; // Temporizador para la interacción

    private MovimientoJugador movimientoJugador; // Referencia al script del jugador

    private void Start()
    {
        // Obtiene el componente MovimientoJugador del jugador
        movimientoJugador = Jugador.GetComponent<MovimientoJugador>();
    }

    private void Update()
    {
        // Verificar la distancia entre el jugador y el NPC
        float distancia = Vector2.Distance(transform.position, Jugador.position);
        
        // Activar la interacción si está dentro de la distancia y no se ha realizado
        if (distancia <= distanciaInteraccion && !estaInteraccionando && !interaccionRealizada && !Input.GetKey(KeyCode.RightArrow))
        {
            estaInteraccionando = true; // Inicia la interacción
            interaccionRealizada = true; // Marca la interacción como realizada
            movimientoJugador.DeshabilitarMovimiento(3f); // Deshabilita el movimiento del jugador por 3 segundos
            tiempoInteraccion = 0f; // Reinicia el temporizador de interacción
        }

        // Si está en interacción, rota el NPC y controla el tiempo
        if (estaInteraccionando)
        {
            RotarNPC();
            tiempoInteraccion += Time.deltaTime; // Aumenta el tiempo de interacción

            // Termina la interacción después de 3 segundos
            if (tiempoInteraccion >= 3f)
            {
                estaInteraccionando = false; // Termina la interacción
                movimientoJugador.EstablecerInteraccion(false); // Permite que el jugador se mueva de nuevo
                seguirDetrasDelJugador = true; // Activa el seguimiento detrás del jugador
                RestablecerRotacion(); // Restablece la rotación inicial del NPC
                PosicionInstantaneaDetrasDelJugador(); // Coloca instantáneamente al NPC detrás del jugador
            }
        }
        
        // Si el estado de seguimiento está activado, el NPC sigue detrás del jugador
        if (seguirDetrasDelJugador)
        {
            PosicionInstantaneaDetrasDelJugador();
        }

        // Resetear el estado de la interacción cuando el jugador se aleje
        if (distancia > distanciaInteraccion)
        {
            interaccionRealizada = false; // Permitir que la interacción ocurra de nuevo
        }
    }

    private void RotarNPC()
    {
        // Rota el NPC hacia la derecha mientras la interacción está activa
        if (estaInteraccionando)
        {
            transform.Rotate(Vector3.forward, velocidadRotacion * Time.deltaTime);
        }
    }

    private void RestablecerRotacion()
    {
        // Restablece la rotación inicial del NPC
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void PosicionInstantaneaDetrasDelJugador()
    {
        // Coloca instantáneamente al NPC a una distancia fija en X detrás del jugador, y al mismo nivel en Y
        transform.position = new Vector3(Jugador.position.x + distanciaDetrasX, Jugador.position.y, transform.position.z);
    }
}









