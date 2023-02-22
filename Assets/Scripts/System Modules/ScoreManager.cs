using System.Collections;
using TonyLearning.ShootingGame.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TonyLearning.ShootingGame.System_Modules
{
    public class ScoreManager : PersistentSingleton<ScoreManager>
    {
        private int score;
        private int currenScore;
        private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);
        public void ResetScore()
        {
            score = 0;
            currenScore = 0;
            ScoreDisplay.UpdateText(score);
        }

        public void AddScore(int scorePoint)
        {
            currenScore += scorePoint;
            StartCoroutine(nameof(AddSoreCoroutine));
        }

        IEnumerator AddSoreCoroutine()
        {
            ScoreDisplay.ScaleText(scoreTextScale);
            while (score < currenScore)
            {
                score += 1;
                ScoreDisplay.UpdateText(score);
                yield return null;
            }
            ScoreDisplay.ScaleText(Vector3.one);
        }

    }
}
