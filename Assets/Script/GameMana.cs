using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMana : MonoBehaviour
{
    [SerializeField] private Sprite[] backgroundArray;
    [SerializeField] private GameObject panelBackground;
    [SerializeField] private Image backgroundImage;
    private int currentIndex = 0;
    [SerializeField] private GameObject[] changBckgrdButton; //Récup tous les bouttons

    void Start()
    {
        backgroundArray = new Sprite[10];
        panelBackground = GameObject.Find("Background");
        backgroundImage = panelBackground.GetComponent<Image>();
        changBckgrdButton = GameObject.FindGameObjectsWithTag("ButtonBckgrd");

        if (panelBackground == null)
        {
            Debug.LogError("Pannel non trouvé");
            return;
        }

        TakeBackground();
        TakeButton();

    }

    void Update()
    {
        
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

    //On récup les bouttons un par un et on leur met l'event changebckgrd
    public void TakeButton()
    {
        foreach(GameObject button in changBckgrdButton)
        {
            Button btn = button.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => ChangeBackground(btn)); // Ajoute un listener qui désactivera le bouton cliqué
            }
            else
            {
                Debug.LogError("Le GameObject " + button.name + " n'a pas de composant Button !");
            }
        }
    }

    //Methode à l'appuie sur le bouton
    public void ChangeBackground(Button clickedButton)
    {
        currentIndex++;

        //Cond de défaite
        if(currentIndex >= backgroundArray.Length)
        {
            Application.Quit();
            Debug.Log("Cest finit");
        }

        if(backgroundArray[currentIndex] != null)
        {
            backgroundImage.sprite = backgroundArray[currentIndex];
        }
        else
        {
            Debug.LogError("Sprite à l'index " + currentIndex + " est null !");
        }
        
        clickedButton.interactable = false;
    }
}
