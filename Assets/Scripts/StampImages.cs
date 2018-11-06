using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StampImages : MonoBehaviour {

    [SerializeField]
    private RawImage _RawImage;
    [SerializeField]
    private int _ImageCopies = 8;
    [SerializeField]
    private float _RefreshRate = 0.1f;
    [SerializeField]
    private Material _DynamicUserMaterial;

    private List<RawImage> _RawImages = new List<RawImage>();
    private int _MyIndex = 0;
    private Texture2D placeholderTexture;
    private List<Texture2D> _PlaceholderTextures = new List<Texture2D>();

    void Awake()
    {
        RectTransform curRect = _RawImage.GetComponent<RectTransform>();
        for(int i=0; i < _ImageCopies; i++)
        {
            /*            GameObject curGameObject = new GameObject();
                        RawImage ri = curGameObject.AddComponent<RawImage>();
                        ri.rectTransform = curRect;
                        curGameObject.transform.SetParent(this.gameObject.transform);
                        _RawImages.Add(ri);*/

            RawImage curImage = Instantiate(_RawImage, this.gameObject.transform) as RawImage;
            curImage.material = _DynamicUserMaterial;
            _RawImages.Add(curImage);
            Color curColor = curImage.color;
            curColor.a = 1.0f;
            curImage.color = curColor;

            ForegroundToRawImage curForeground = curImage.GetComponent<ForegroundToRawImage>();
            PortraitBackground curBackground = curImage.GetComponent<PortraitBackground>();
            RandomColorUser curColorUser = curImage.GetComponent<RandomColorUser>();

            if (curForeground != null)
                Destroy(curForeground);

            if (curBackground != null)
                Destroy(curBackground);

            if (curColorUser != null)
                Destroy(curColorUser);

            Texture2D curTexture = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
            _PlaceholderTextures.Add(curTexture);

            curImage.texture = curTexture;
        }

        

        InvokeRepeating("SetImage", 0.0f, _RefreshRate);
    }

    private int _CurIndex = 0;

    void SetImage()
    {

        if (_RawImage.texture != null && _RawImage != null)
        {
            if (_CurIndex >= _ImageCopies)
                _CurIndex = 0;

            //get the current rawImage from our array
            RawImage curImage = _RawImages[_CurIndex];
            placeholderTexture = _PlaceholderTextures[_CurIndex];

            if (_CurIndex == 0)
            {
                Graphics.CopyTexture(_RawImage.texture, placeholderTexture);
            }
            else
            {
                RawImage oldImage = _RawImages[(_CurIndex - 1)];
                Graphics.CopyTexture(oldImage.texture, placeholderTexture);
            }
            //curImage.texture = placeholderTexture;

            _CurIndex++;
        }
    }
}
