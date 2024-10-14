using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tiret : MonoBehaviour
{
    private WordsMana wordsMana;
    public int nbrClones;
    public GameObject[] textElements;
    [SerializeField] private float spacing = 120f;

    private void Awake() 
    {
        wordsMana = WordsMana.Instance;
    }

    void Start()
    {
        DupplicateTiret(wordsMana.wordChoosen);
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
            textComponent.text = word[i].ToString().ToUpper();
            
            textElements[i].SetActive(false);
            
        }
    }
}
