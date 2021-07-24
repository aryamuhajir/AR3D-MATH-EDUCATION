using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Restart1 : VuforiaMonoBehaviour
{

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
