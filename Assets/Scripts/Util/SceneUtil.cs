using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtil
{
    public static void LoadBaseScene()
    {
        SceneManager.LoadScene("Base Scene");
    }

    public static void LoadVillageScene()
    {
        SceneManager.LoadScene("Village");
    }

    public static void LoadFieldsScene()
    {
        SceneManager.LoadScene("Fields 1");
    }
}