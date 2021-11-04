using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int previousTeamHighScore = 0; 
    private int currentTeamHighScore = 0; 
    private const string fileName = "/FrostFireHighScore.dat";

    public static ScoreManager scoreManager 
    {
        get 
        {
            if(scoreManager != null)
                return scoreManager;

            return new ScoreManager();
        }
    }
    
    private void Awake()
    {
        LoadScore();
    }

    private void LoadScore()
    {
        if (File.Exists(Application.persistentDataPath + fileName)) 
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + fileName, FileMode.Open, FileAccess.Read);
            FrostFireGameData ff_data = (FrostFireGameData) binaryFormatter.Deserialize(fileStream); 
            fileStream.Close();

            previousTeamHighScore = ff_data.teamHighScore;
        }
    }

    private void SaveScore() 
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate);

        FrostFireGameData ff_data = new FrostFireGameData();

        if (currentTeamHighScore > previousTeamHighScore) 
            ff_data.teamHighScore = currentTeamHighScore;
        
        binaryFormatter.Serialize(fileStream, ff_data);
        fileStream.Close();

        // TODO: If this was called 'OnGameOver' then reset the score
        currentTeamHighScore = 0;
    }

    public void IncreaseTeamHighScore(int amount) => currentTeamHighScore += amount;

    public void DecreaseTeamHighScore(int amount) => currentTeamHighScore -= amount;
}

[Serializable]
class FrostFireGameData 
{
    public int teamHighScore;
} 
