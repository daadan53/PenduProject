using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickMana : MonoBehaviour
{
    private WordsMana instanceWord;
    [SerializeField] private GameObject[] changBckgrdButton; //Récup tous les bouttons

    void Start()
    {
        changBckgrdButton = GameObject.FindGameObjectsWithTag("ButtonBckgrd");

        instanceWord = WordsMana.Instance;

        TakeButton();

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
        // Vérifier si clickedButton est nul
        if (clickedButton == null)
        {
            Debug.LogError("Le bouton cliqué est nul !");
            return;
        }

        TextMeshProUGUI textComponent = clickedButton.GetComponentInChildren<TextMeshProUGUI>();

        //C'est ici que je vais vérifié si la lettre est dans le mot
        if(textComponent != null )
        {
            string textButton = textComponent.text.ToLower(); // Récupérer le texte du premier txtmshpro trouver

            // Vérifie si la lettre est dans le mot
            bool isLetterInWord = instanceWord.wordChoosen.Contains(textButton);

            // Déclenche l'évent OnLetterChecked avec le résultat
            instanceWord.OnLetterChecked.Invoke(isLetterInWord, textButton);
        }
        
        clickedButton.interactable = false;
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnButtonQuit()
    {
        Application.Quit();
    }
}
