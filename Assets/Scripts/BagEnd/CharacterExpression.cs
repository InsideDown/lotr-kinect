using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterExpression : MonoBehaviour
{

    public Material FaceMaterial;
    public List<Texture> FaceTextures = new List<Texture>();
    public int WaitTime = 2;
    public int MinWaitTime = 2;


    void Awake()
    {
        StartCoroutine(SetRandomMaterial());
    }

    IEnumerator SetRandomMaterial()
    {
        int ranItem = UnityEngine.Random.Range(0, FaceTextures.Count);
        FaceMaterial.mainTexture = FaceTextures[ranItem];
        int ranTimer = UnityEngine.Random.Range(WaitTime, (WaitTime + MinWaitTime));
        yield return new WaitForSeconds(ranTimer);
        StartCoroutine(SetRandomMaterial());
    }
}
