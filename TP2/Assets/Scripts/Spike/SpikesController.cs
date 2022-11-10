using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikesController : MonoBehaviour
{
    private bool HitSpikes = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!HitSpikes)
        {
            StartCoroutine(RespawnAfterTime(1));
            GameObject.FindWithTag("Player").SetActive(false);
        }
        HitSpikes = true;
    }

    IEnumerator RespawnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
