using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    // Start is called before the first frame update
    private int[,] fsm;
    private int currentState;
    
    public FSM(int cantEstados, int cantEventos, int initState) {
        fsm = new int[cantEstados, cantEventos];
        for (int i = 0; i < cantEstados; i++) {
            for (int j = 0; j < cantEventos; j++) {
                fsm[i, j] = -1;
            }
        }
        currentState = initState;
    }
    public void SetRelations(int EstadoOrigen, int EstadoDestino, int Evento) {
        fsm[EstadoOrigen, Evento] = EstadoDestino;
    }
    public void SendEvent(int Evento) {
        if (fsm[currentState, Evento] != -1) {
            currentState = fsm[currentState, Evento];
        }
    }
    public int GetCurrentState() {
        return currentState;
    }
}
