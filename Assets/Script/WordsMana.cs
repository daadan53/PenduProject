//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsMana : MonoBehaviour
{
    public static WordsMana Instance;
    [SerializeField] private List<string> wordsList = new List<string>();
    private int rdmWordIndex;
    public string wordChoosen;
    [SerializeField] private float spacing = 120f;

    private void Awake() 
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }   
        Instance = this;

        LoadWords();
    }

    void Start()
    {
        rdmWordIndex = Random.Range(0, wordsList.Count);
        wordChoosen = wordsList[rdmWordIndex];

        DupplicateTiret(wordChoosen);
    }


    void Update()
    {
        
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
        int nbrClones = word.Length; 

        //Duppliquer les traits 
        GameObject trait = GameObject.FindGameObjectWithTag("Trait");

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
    }

    private void OnDestroy() 
    {
        Instance = null;
    }
}
