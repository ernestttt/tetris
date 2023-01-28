using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void UpdateScoreText(int completedRows)
    {
        scoreText.text = $"Completed rows: {completedRows}";
    }
}
