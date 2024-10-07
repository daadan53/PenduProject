using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMana : MonoBehaviour
{
    [SerializeField] private Sprite[] background;

    void Start()
    {
        background = new Sprite[10];
        TakeBackground();

    }

    void Update()
    {
        
    }

    // Methode qui récup les asset du pendu
    public void TakeBackground()
    {
        for (int i = 0; i < background.Length; i++)
        {
            //On récup les sprites dans le dossier ressources/Background
            background[i] = Resources.Load<Sprite>("Background/pendu_" + i);
            
            // Vérification si le sprite a bien été chargé
            if (background[i] == null)
            {
                Debug.LogError("Le sprite 'pendu_" + i + "' n'a pas été trouvé !");
            }
        }
    } 

    /*public Sprite ShowBackground()
    {
        // Background par défaut
        Sprite showed = background[0];

        return showed;
    }*/
}
