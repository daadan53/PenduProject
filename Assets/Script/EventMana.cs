using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EventMana : MonoBehaviour
{
    private WordsMana wordsMana;
    private Background background;

    private int currentIndex = 0;
    public int nbrLettre = 0;

    public GameObject panelWin;
    [SerializeField] private GameObject[] changBckgrdButton;
    [SerializeField] private GameObject panelGameOver;


    // Start is called before the first frame update
    void Start()
    {
        wordsMana = WordsMana.Instance;
        background = Background.Instance;

        // S'abonner à l'événement
        if (wordsMana != null)
        {
            wordsMana.OnLetterChecked.AddListener(OnLetterChecked);
        }
    }

     private void OnLetterChecked(bool isCorrect, string textButton)
    {
        if (isCorrect && currentIndex < background.backgroundArray.Length && background.backgroundArray[currentIndex] != null)
        {
            Debug.Log("La lettre est correcte !");
            // Afficher les traits associés
            int textCount = Mathf.Min(wordsMana.nbrClones, wordsMana.textElements.Length);

            for (int i = 0; i < textCount; i++)
            {
                TextMeshProUGUI textComponent = wordsMana.textElements[i].GetComponent<TextMeshProUGUI>();

                if (textComponent.text.ToUpper() == textButton.ToUpper())
                {
                    wordsMana.textElements[i].SetActive(true);
                    nbrLettre++;
                }
            }

            // Vérifier si le mot complet est trouvé
            if (nbrLettre == wordsMana.wordChoosen.Length)
            {
                panelWin.SetActive(true);
            }
        }
        else
        {
            // Lettre pas dans le mot, changer l'arrière-plan
            currentIndex++;
            if (currentIndex < background.backgroundArray.Length - 1)
            {
                background.backgroundImage.sprite = background.backgroundArray[currentIndex];
            }
            else
            {
                background.backgroundImage.sprite = background.backgroundArray[currentIndex];
                panelGameOver.SetActive(true);
                panelGameOver.GetComponentInChildren<Text>().text = "Le mot était : " + wordsMana.wordChoosen.ToUpper();
            }
        }
    }

    private void OnDestroy()
    {
        if (wordsMana != null)
        {
            wordsMana.OnLetterChecked.RemoveListener(OnLetterChecked);
        }
    }
}
