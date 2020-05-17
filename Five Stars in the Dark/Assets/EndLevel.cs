using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(endLevel());
        }
    }

    IEnumerator endLevel()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
