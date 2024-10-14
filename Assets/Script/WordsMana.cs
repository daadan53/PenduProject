//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WordsMana : MonoBehaviour
{
    public static WordsMana Instance;

    public UnityEvent<bool, string> OnLetterChecked;

    [SerializeField] private List<string> wordsList = new List<string>();
    private int rdmWordIndex;
    public string wordChoosen;
    [SerializeField] private float spacing = 120f;
    public GameObject[] textElements;
    public int nbrClones;

    private void Awake() 
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }   
        Instance = this;

        LoadWords();

        //Initialise l'event
        if (OnLetterChecked == null)
        {
            OnLetterChecked = new UnityEvent<bool, string>();
        }
    }

    void Start()
    {
        rdmWordIndex = Random.Range(0, wordsList.Count);
        wordChoosen = wordsList[rdmWordIndex];

        DupplicateTiret(wordChoosen);
    }

    //Recup les mots du fichier txt
    void LoadWords()
    {
        // Chargement du fichier depuis le dossier Resources
        TextAsset wordFile = Resources.Load<TextAsset>("Words/WordsList");
        
        if (wordFile != null)
        {
            // Séparer les mots par ligne
            string[] words = wordFile.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                wordsList.Add(word.ToLower()); // Add et converti en minuscule
            }
        }
        else
        {
            Debug.LogError("Le fichier 'WordsList' n'a pas été trouvé dans le dossier Resources.");
        }
    }

    void DupplicateTiret(string word)
    {
        //Recup le nombred de lettre dans le mot
        nbrClones = word.Length; 

        //Duppliquer les traits 
        GameObject trait = GameObject.FindGameObjectWithTag("Trait");
        
        //Traits
        if (trait != null)
        {
            RectTransform origineTraitPos = trait.GetComponent<RectTransform>();

            for (int i = 1; i < nbrClones; i++)
            {
                // Instancier le nouveau trait
                GameObject newTrait = Instantiate(trait);

                // Recup la pos du new trait
                RectTransform newRectTransform = newTrait.GetComponent<RectTransform>();

                //En fonction de l'original
                newRectTransform.SetParent(origineTraitPos.parent);


                //New position
                newRectTransform.anchoredPosition = new Vector2(origineTraitPos.anchoredPosition.x + (i * spacing), 
                origineTraitPos.anchoredPosition.y);
            }
        }
        else
        {
            Debug.LogError("Aucun GameObject avec le tag 'Trait' trouvé.");
        }

        //Mot dans champs de text//
        //Récup les champs de texte
        textElements = GameObject.FindGameObjectsWithTag("txtChoosen");

        //On s'assure de la longueur dépasse pas le nbr de txtChoosen dispo
        int textCount = Mathf.Min(nbrClones, textElements.Length);

        for (int i = 0; i < textCount; i++)
        {
            // Récupérer le composant Text de chaque élément
            TextMeshProUGUI textComponent = textElements[i].GetComponent<TextMeshProUGUI>();

            // Assigner la lettre correspondante
            textComponent.color = Color.red;
            textComponent.text = word[i].ToString().ToUpper();
            
            textElements[i].SetActive(false);
            
        }
    }

    private void OnDestroy() 
    {
        Instance = null;
    }
}
