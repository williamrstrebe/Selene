using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeQuestAtHome : MonoBehaviour
{

    public static List<string> objetivos = preencheObjetivos();
    // Start is called before the first frame update

    public static int currentQuest = 0;




    private static List<string> preencheObjetivos()
    {
        List<string> lista = new List<string>();


        lista.Add("Explore! Saia procurar um pouco de madeira."); //GOTO First Level
        lista.Add("Clique nos buracos para realizar o conserto."); // Fix house
        lista.Add("Vá atrás de mais madeira."); //GOTO Second Level
        lista.Add("Clique nas janelas para consertá-las!!"); // Fix house
        lista.Add("Por enquanto, sua base está segura! (Fim de jogo)"); // END


        return lista;

    }

}
