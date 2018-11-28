using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pocketsphinx;

public enum Scene { MENU, GAME}
public class SpeechRecognition : MonoBehaviour
{

    AudioSource aud;
    
    private SphinxDecoder _decoder;
    private string grammar;
    private string _mic;

    private float transition = 0.0f;
    private float animationDuration = 4.0f;

    private bool _listening;
    private bool _processing;

    public Scene scene;

    public void Awake()
    {
        if (scene == Scene.MENU) { grammar = "menu.gram"; }
        else { grammar = "gameplay.gram"; }
        var config = new SphinxConfig();
        config.Hmm = Application.dataPath + "/StreamingAssets/Models/en-us/en-us";
        config.Dict = Application.dataPath + "/StreamingAssets/Models/en-us/cmudict-en-us.dict";
        config.Jsgf = Application.dataPath + "/StreamingAssets/Grammar/" + grammar;
        config.Logfn = Application.dataPath + "/StreamingAssets/sphinxlog.txt";

        _decoder = new SphinxDecoder(config);

        _listening = true;
        _processing = false;

        aud = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (scene.Equals(Scene.MENU))
        {
            if (_listening)
                StartUtterance();
            if (_processing)
                EndUtterance();
        }
        else
        {
            if (transition > 1.0f)
            {
                if (_listening)
                    StartUtterance();
                if (_processing)
                    EndUtterance();
            }
            else
            {
                transition += Time.deltaTime / animationDuration;
            }
        }
    }

    void StartUtterance()
    {
        _listening = false;
        _decoder.StartUtterance(System.Guid.NewGuid().ToString());

        _mic = Microphone.devices[0];

        Debug.Log("Start Listening");
        //ScoreManager.Instance.ShowMic(_mic);
        StartCoroutine(DetectUtterance());
    }

    void EndUtterance()
    {
        _processing = false;
        Debug.Log("Stop Listening");
        StopAllCoroutines();
        Microphone.End(_mic);

        _decoder.EndUtterance();

        float score;
        string uttid;
        string hypothesis = _decoder.GetHypothesis(out score, out uttid);

        Debug.Log("Hyp: " + hypothesis);

        if (hypothesis != null)
        {
            if (scene == Scene.MENU)
            {
                MenuController.Instance.SpeechHyp(hypothesis);
            }
            else
            {
                PlayerController.Instance.SpeechHyp(hypothesis);
            }
        }

        StartCoroutine(Wait());
    }

    IEnumerator DetectUtterance()
    {
        aud.clip = Microphone.Start(_mic, true, 3, 16000);

        int readHead = 0;
        int writeHead;
        int nFloatsToGet;
        float[] buffer;

        while (true)
        {
            writeHead = Microphone.GetPosition(_mic);

            nFloatsToGet = (aud.clip.samples + writeHead - readHead) % aud.clip.samples;

            if (nFloatsToGet < 1024)
            {
                yield return null;
                continue;
            }

            buffer = new float[nFloatsToGet];
            aud.clip.GetData(buffer, readHead);

            readHead = (readHead + nFloatsToGet) % aud.clip.samples;

            _decoder.ProcessRaw(buffer, 0, nFloatsToGet);

            float score;
            string uttid;
            string hypothesis = _decoder.GetHypothesis(out score, out uttid);

            if (hypothesis != null)
            {
                //Debug.Log(hypothesis);
                _listening = false;
                _processing = true;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        _listening = true;
    }
}
