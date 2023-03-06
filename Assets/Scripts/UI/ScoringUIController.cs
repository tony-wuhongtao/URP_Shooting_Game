using System;
using System.Collections;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace TonyLearning.ShootingGame.UI
{
    public class ScoringUIController : MonoBehaviour
    {
        [Header("==== BACKGROUND ====")]
        [SerializeField] Image background;
        [SerializeField] Sprite[] backgroundImages;
        [SerializeField] private float changeBKInterval = 2f;

        [Header("==== SCORING SCREEN ====")]
        [SerializeField] private Canvas scoringScreenCanvas;
        [SerializeField] private Text playerScoreText;
        [SerializeField] private Button buttonMainMenu;
        [SerializeField] Transform highScoreLeaderboardContainer;
        
        [Header("==== HIGH SCORE SCREEN ====")]
        [SerializeField] Canvas newHighScoreScreenCanvas;
        [SerializeField] Button buttonCancel;
        [SerializeField] Button buttonSubmit;
        [SerializeField] InputField playerNameInputField;
        
        void ShowRandomBackground()
        {
            background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
        }

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(nameof(ChangeBackgroundImageCoroutine));
            
            // ShowScoringScreen();
            if (ScoreManager.Instance.HasNewHighScore)
            {
                ShowNewHighScoreScreen();
            }
            else 
            {
                ShowScoringScreen();
            }
            
            
            ButtonPressedBehaviour.buttonFuntionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);
            ButtonPressedBehaviour.buttonFuntionTable.Add(buttonSubmit.gameObject.name, OnButtonSubmitClicked);
            ButtonPressedBehaviour.buttonFuntionTable.Add(buttonCancel.gameObject.name, HideNewHighScoreScreen);
            GameManager.GameState = GameState.Scoring;
        }

        private void OnDisable()
        {
            StopCoroutine(nameof(ChangeBackgroundImageCoroutine));
            ButtonPressedBehaviour.buttonFuntionTable.Clear();
        }

        IEnumerator ChangeBackgroundImageCoroutine()
        {
            while (true)
            {
                ShowRandomBackground();
                yield return new WaitForSeconds(changeBKInterval);
            }
            
        }

        void ShowNewHighScoreScreen()
        {
            newHighScoreScreenCanvas.enabled = true;
            UIInput.Instance.SelectUI(buttonCancel);
        }
    
        void HideNewHighScoreScreen()
        {
            newHighScoreScreenCanvas.enabled = false;
            ScoreManager.Instance.SavePlayerScoreData();
            ShowRandomBackground();
            ShowScoringScreen();
        }
        
        void ShowScoringScreen()
        {
            scoringScreenCanvas.enabled = true;
            playerScoreText.text = ScoreManager.Instance.Score.ToString();
            UIInput.Instance.SelectUI(buttonMainMenu);
            
            UpdateHighScoreLeaderboard();
        }

        void UpdateHighScoreLeaderboard()
        {
           var playerScoreList =  ScoreManager.Instance.LoadPlayerScoreData().list;
           for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++)
           {
               var child = highScoreLeaderboardContainer.GetChild(i);

               child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
               child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
               child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
           }
        }
        

        void OnButtonMainMenuClicked()
        {
            scoringScreenCanvas.enabled = false;
            SceneLoader.Instance.LoadMainMainScene();
        }
        
        void OnButtonSubmitClicked()
        {
            if (!string.IsNullOrEmpty(playerNameInputField.text))
            {
                ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
            }

            HideNewHighScoreScreen();
        }

    }
}
