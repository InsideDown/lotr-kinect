using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;

public class LothlorienKeywords : MonoBehaviour {

    public GameObject Person1DrawingHolderLeft;
    public GameObject Person1DrawingHolderRight;
    public GameObject Person1Prefab;

    public GameObject Person2DrawingHolderLeft;
    public GameObject Person2DrawingHolderRight;
    public GameObject Person2Prefab;

    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

	// Use this for initialization
	void Start () {
        keywordActions.Add("start drawing", StartDrawing);
        keywordActions.Add("erase", Erase);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
	}

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywordActions[args.text].Invoke();
    }


    void StartDrawing()
    {
        Debug.Log("start drawing called");
        Instantiate(Person1Prefab, Person1DrawingHolderLeft.transform);
        Instantiate(Person1Prefab, Person1DrawingHolderRight.transform);
    }

    void DestroyChildren(GameObject parentGameObj)
    {
        if (parentGameObj == null)
            return; 

        foreach(Transform child in parentGameObj.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Erase()
    {
        Debug.Log("erase");
        DestroyChildren(Person1DrawingHolderLeft);
        DestroyChildren(Person1DrawingHolderRight);
    }
	
}
