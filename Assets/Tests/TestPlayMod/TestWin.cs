using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using TMPro;
using System.Linq;

public class TestWin : InputTestFixture
{
    Mouse mouse;
    private GameObject[] buttons;
    private WordsMana wordsMana;
    private GameObject panelWin;
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

            restartButton = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "ButtonRestart2");

            // Rechercher l'objet WinPanel même s'il est désactivé
            panelWin = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "WinPanel");
            Assert.IsNotNull(panelWin, "Le panel win n'a pas été trouvé.");

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

        yield return new WaitForSeconds(1f);

        foreach (GameObject button in buttons)
        {
            string buttonText = button.GetComponentInChildren<TextMeshProUGUI>().text.ToUpper();

            if (word.ToUpper().Contains(buttonText))
            {
                ClickUI(button);
                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(2f);
        Assert.IsTrue(panelWin.activeSelf, "Le panel de victoire devrait etre activé");

        yield return new WaitForSeconds(0.5f);
        ClickUI(restartButton);

        yield return new WaitForSeconds(1f);
        //LE, jeu a bien redémarré

        //Scène a été rechargée ?
        Assert.AreEqual(SceneManager.GetActiveScene().name, "SampleScene", "La scène devrait être la même après le redémarrage.");
    }
}
