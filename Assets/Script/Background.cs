using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public static Background Instance;

    public Sprite[] backgroundArray;
    public Image backgroundImage;
    public GameObject panelBackground;

    private void Awake() 
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        backgroundArray = new Sprite[10];
        backgroundImage = panelBackground.GetComponent<Image>();
        //panelBackground = GameObject.Find("Background");

        if (panelBackground == null)
        {
            Debug.LogError("PannelBack non trouvé");
            return;
        }

        TakeBackground();
    }

    // Methode qui récup les asset du pendu
    public void TakeBackground()
    {
        for (int i = 0; i < backgroundArray.Length; i++)
        {
            //On récup les sprites dans le dossier ressources/Background
            backgroundArray[i] = Resources.Load<Sprite>("Background/pendu_" + i);
            
            // Vérification si le sprite a bien été chargé
            if (backgroundArray[i] == null)
            {
                Debug.LogError("Le sprite 'pendu_" + i + "' n'a pas été trouvé !");
            }
        }

        //Afficher au start le premier background par def
        backgroundImage.sprite = backgroundArray[0];
    } 

    private void OnDestroy() 
    {
        Instance = null;
    }
}
