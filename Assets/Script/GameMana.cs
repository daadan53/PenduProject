using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMana : MonoBehaviour
{
    //public static GameMana instance;

    [SerializeField] private Sprite[] backgroundArray;
    [SerializeField] private GameObject panelBackground;
    [SerializeField] private Image backgroundImage;
    private int currentIndex = 0;
    [SerializeField] private GameObject[] changBckgrdButton; //Récup tous les bouttons

    /*private void Awake() 
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }   
        instance = this;
    }*/

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
                // Add l'index du bouton avec son text dans un dico ?
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
        //Cond de défaite
        if(currentIndex >= backgroundArray.Length)
        {
            Application.Quit();
            Debug.Log("Cest finit");
            return;
        }

        // Vérifier si clickedButton est nul
        if (clickedButton == null)
        {
            Debug.LogError("Le bouton cliqué est nul !");
            return;
        }

        //On est dans la liste de background
        if(currentIndex < backgroundArray.Length && backgroundArray[currentIndex] != null)
        {
            TextMeshProUGUI textComponent = clickedButton.GetComponentInChildren<TextMeshProUGUI>();

            //C'est ici que je vais vérifié si la lettre est dans le mot
            if(textComponent != null )
            {
                string textButton = textComponent.text.ToLower(); // Récupérer le texte du premier txtmshpro trouver
                //Debug.Log(textButton);

                //Lettre dans le mot
                if(WordsMana.Instance.wordChoosen.Contains(textButton))
                {
                    Debug.Log("Bien joué " + textButton + " est dans ce mot");
                }
                //Lettre pas dans le mot
                else 
                {
                    currentIndex++;
                    backgroundImage.sprite = backgroundArray[currentIndex];
                }
            } 
            else
            {
                Debug.LogError("Aucun composant TextMeshPro trouvé dans l'enfant du bouton !");
            }
        }
        else
        {
            Debug.LogError("Sprite à l'index " + currentIndex + " est null !");
        }
        
        clickedButton.interactable = false;
    }

    /*private void OnDestroy() 
    {
        instance = null;
    }*/
}
