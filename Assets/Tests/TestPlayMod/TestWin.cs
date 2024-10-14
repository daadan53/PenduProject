using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using System;

public class TestWin : InputTestFixture
{
    Mouse mouse; // Pour créer une souris
    private GameObject panelWin;
    private WordsMana wordsMana;
    private EventMana eventMana;
    private OnClickMana onClickMana;
    private int wordSize;
    private GameObject[] buttons;

    public override void Setup()
    {
        base.Setup();
        SceneManager.sceneLoaded += OnSceneLoaded;  // Abonner l'événement

        SceneManager.LoadScene("Scenes/SampleScene", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // S'assurer que la scène correcte est chargée
        if (scene.name == "SampleScene")
        {
            mouse = InputSystem.AddDevice<Mouse>();

            // Créer un gameObject pour simuler le panel de victoire
            panelWin = new GameObject("WinPanel");
            panelWin.AddComponent<Canvas>();
            panelWin.SetActive(false); // Le panel est initialement caché

            // Créer le panelButton qui contiendra les boutons
            var panelButton = new GameObject("PanelButton");
            RectTransform panelButtonRectTransform = panelButton.AddComponent<RectTransform>();
            panelButtonRectTransform.SetParent(Background.Instance.panelBackground.transform, false); // Mettre panelButton sous panelWin
            panelButtonRectTransform.sizeDelta = new Vector2(800, 200); // Ajuster la taille du panel si nécessaire
            panelButtonRectTransform.anchoredPosition = Vector2.zero; // Centrer le panelButton dans panelWin

            // Simuler l'instance de WordsMana
            var wordsManaObject = new GameObject("WordsMana");
            wordsMana = wordsManaObject.AddComponent<WordsMana>();
            wordsMana.wordChoosen = "test";
            wordsMana.nbrClones = wordsMana.wordChoosen.Length;
            wordSize = wordsMana.wordChoosen.Length;
            wordsMana.textElements = new GameObject[wordSize];

            for (int i = 0; i < wordSize; i++)
            {
                GameObject textObj = new GameObject("TextElement" + i);
                textObj.AddComponent<TextMeshProUGUI>();
                wordsMana.textElements[i] = textObj;
                textObj.SetActive(false); // Les éléments textuels sont cachés au départ
            }

            // Simuler OnClickMana
            var onClickManaObject = new GameObject("OnClickMana");
            onClickMana = onClickManaObject.AddComponent<OnClickMana>();

            // Simuler EventMana
            var eventManaObject = new GameObject("EventMana");
            eventMana = eventManaObject.AddComponent<EventMana>();
            eventMana.panelWin = panelWin; // Assigner le panelWin
            eventMana.nbrLettre = 0; // Initialiser le nombre de lettres

            // Initialiser les boutons
            buttons = new GameObject[wordSize];
            string word = wordsMana.wordChoosen.ToUpper();

            for (int i = 0; i < wordSize; i++)
            {
                buttons[i] = new GameObject("Button" + word[i]);
                var button = buttons[i].AddComponent<Button>();
                TextMeshProUGUI textComponent = buttons[i].AddComponent<TextMeshProUGUI>();
                textComponent.text = word[i].ToString();

                button.onClick.AddListener(() => onClickMana.ChangeBackground(button));

                // Configurer le RectTransform pour centrer le bouton
                RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();
                rectTransform.SetParent(panelButton.transform, false); // Assurez-vous que le bouton est un enfant de panelButton
                rectTransform.sizeDelta = new Vector2(100, 50); // Taille du bouton

                // Ajuster la position des boutons
                float buttonSpacing = 20f; // Espacement entre les boutons
                float totalWidth = (wordSize * rectTransform.sizeDelta.x) + ((wordSize - 1) * buttonSpacing); // Largeur totale
                rectTransform.anchoredPosition = new Vector2((totalWidth / 2) - (i * (rectTransform.sizeDelta.x + buttonSpacing)), 0); // Ajuster la position

                buttons[i].SetActive(true); // Rendre le bouton actif
            }

            SceneManager.sceneLoaded -= OnSceneLoaded; // Désabonner une fois la scène chargée
        }
    }

    // Fonction pour récup un UI élement et sa position et simuler le click dessus 
    public void ClickUI(GameObject uiElement)
    {
        if (uiElement == null)
        {
            Assert.Fail("L'élément UI est null");
            return;
        }

        // Ca ca ne marche que psq la cam et le canvas ont la meme resolution, ca ne marcherai pas par ex avec un 
        // petit canva se trouvant en haut à gauche de l'écran.
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>(); // On récupère la camera 
        Vector3 screenPos = camera.WorldToScreenPoint(uiElement.transform.position); // On recup la position de l'élément vis à vis de la caméra
        Set(mouse.position, screenPos); //On positionne la souris sur l'élément choisis
        Click(mouse.leftButton); // On clique avec notre souris precedemment créer
    }

    [UnityTest]
    public IEnumerator TestWinWithEnumeratorPasses()
    {
        yield return new WaitForSeconds(1f);

        foreach(char letter in wordsMana.wordChoosen)
        {
            Assert.IsNotNull(wordsMana.wordChoosen);

            //LA on recherche le boutton qui contient la lettre actuelle
            for(int i = 0; i < wordSize; i++)
            {
                if (buttons[i] == null)
                {
                    Assert.Fail($"Le bouton à l'index {i} est null.");
                    yield break;
                }

                TextMeshProUGUI textComponent = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                Assert.IsNotNull(textComponent, "Aucun text trouver sur ce boutton");

                if(letter.ToString().ToUpper() == textComponent.text.ToUpper())
                {
                    ClickUI(buttons[i]);
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        yield return new WaitForSeconds(1f); // Attendre que la condition de victoire soit vérifiée

        // Vérifier que le panneau de victoire est activé
        Assert.IsTrue(panelWin.activeSelf, "Le panneau de victoire devrait être activé après avoir cliqué sur tous les bons boutons.");

        // Vérifier la condition de victoire (le panneau de victoire doit être activé dans EventMana)
        //Assert.IsTrue(eventMana.panelWin.activeSelf, "Le panneau de victoire devrait être activé après avoir cliqué sur tous les bons boutons.");
        //yield return null;

        /*
        // Création des boutons UI pour chaque lettre
        GameObject[] buttons = new GameObject[wordSize];
        string word = wordsMana.wordChoosen.ToUpper();

        for (int i = 0; i < wordSize; i++)
        {
            buttons[i] = new GameObject("Button" + word[i]);
            buttons[i].AddComponent<Button>();

            TextMeshProUGUI textComponent = buttons[i].AddComponent<TextMeshProUGUI>();
            textComponent.text = word[i].ToString(); // Assigne la lettre du bouton

            // Ajouter le listener qui lie l'événement à chaque bouton
            Button btn = buttons[i].GetComponent<Button>();
            btn.onClick.AddListener(() => onClickMana.ChangeBackground(btn));
        }

        // Simuler le clic sur chaque bouton correspondant à chaque lettre du mot "TEST"
        for (int i = 0; i < wordSize; i++)
        {
            ClickUI(buttons[i]);  // Simuler un clic sur chaque bouton
            yield return new WaitForSeconds(0.5f); // On attend 0.5 secondes entre chaque clic pour simuler un utilisateur humain
        }

        yield return new WaitForSeconds(1f); // Attendre une seconde pour que la condition de victoire soit traitée

        // Vérifier la condition de victoire (le panneau de victoire doit être activé dans EventMana)
        Assert.IsTrue(eventMana.panelWin.activeSelf, "Le panneau de victoire devrait être activé après avoir cliqué sur tous les bons boutons.");
        */
    }
}
