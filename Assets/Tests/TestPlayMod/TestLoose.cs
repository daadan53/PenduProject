using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestLoose : InputTestFixture
{
    Mouse mouse;
    private GameObject[] buttons;
    private WordsMana wordsMana;
    private Background background;
    private GameObject panelGameOver;
    private GameObject restartButton;

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

            buttons = GameObject.FindGameObjectsWithTag("ButtonBckgrd");

            wordsMana = WordsMana.Instance;
            background = Background.Instance;

            restartButton = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "ButtonRestart1");

            // Rechercher l'objet WinPanel même s'il est désactivé
            panelGameOver = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "GameOverPanel");
            Assert.IsNotNull(panelGameOver, "Le panel game over n'a pas été trouvé.");

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

        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>(); // On récupère la camera 
        Vector3 screenPos = camera.WorldToScreenPoint(uiElement.transform.position); // On recup la position de l'élément vis à vis de la caméra
        Set(mouse.position, screenPos); //On positionne la souris sur l'élément choisis
        Click(mouse.leftButton); // On clique avec notre souris precedemment créer
    }

    [UnityTest]
    public IEnumerator TestWinWithEnumeratorPasses()
    {
        Assert.IsNotNull(wordsMana, "WordsMana n'a pas été initialisé.");
        Assert.IsNotEmpty(buttons, "Aucun bouton n'a été trouvé.");

        string word = wordsMana.wordChoosen;
        int nbrBckgrd = background.backgroundArray.Length;
        int limit = 0;

        yield return new WaitForSeconds(1f);

        foreach (GameObject button in buttons)
        {
            string buttonText = button.GetComponentInChildren<TextMeshProUGUI>().text.ToUpper();

            if (!word.ToUpper().Contains(buttonText) && limit < nbrBckgrd - 1)
            {
                Sprite bckgrd = background.backgroundArray[limit];
                ClickUI(button);
                
                yield return new WaitForSeconds(0.2f);

                limit++;

                Assert.AreNotEqual(bckgrd, background.backgroundArray[limit]);
                
                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(1f);
        Assert.IsTrue(panelGameOver.activeSelf, "Le panel de défaite devrait etre activé");

        yield return new WaitForSeconds(0.5f);
        ClickUI(restartButton);

        yield return new WaitForSeconds(1f);
        //LE, jeu a bien redémarré

        //Scène a été rechargée ?
        Assert.AreEqual(SceneManager.GetActiveScene().name, "SampleScene", "La scène devrait être la même après le redémarrage.");
    }
}
