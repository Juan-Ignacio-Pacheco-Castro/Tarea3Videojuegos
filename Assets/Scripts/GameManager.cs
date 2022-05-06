using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Text LivesText;
    [SerializeField] public Transform CratesContainer;
    public static GameManager instance = null;

    public int Lives {set; get;}

    void Awake(){
        if(instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    void Start(){
        Lives = 3;
        LivesText.text = "Lives: "+ Lives;
    }

    public void UpdateLives(int lives){
        LivesText.text = "Lives: "+ lives;
        Lives = lives;
    }

    void Update(){
        if(CratesContainer.childCount == 0){
            Debug.Log("Ganó :)");
        }
        if(Lives == 0){
            Debug.Log("Perdió :(");
            ResetGame();
        }
    }

    void ResetGame(){
        /*Para esto, deberá limpiar los bloques restantes en pantalla, 
        recargar los bloques 
        y volver a asignarle tres vidas restantes al jugador para poder reiniciar su partida.*/
        foreach (Transform crate in CratesContainer) {
            Destroy(crate.gameObject);
        }
        //Recargar bloques
        Start();
    }
}
