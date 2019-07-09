// ----------------------------------------------------------------------------
// The MIT License
// CropScale https://github.com/mopsicus/unity-crop-scale-texture
// Copyright (c) 2019 Mopsicus <mail@mopsicus.ru>
// ----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour {

    [SerializeField]
    private Image SourceImage;

    [SerializeField]
    private Image ResultImage;

    [SerializeField]
    private InputField CropWidthInput;

    [SerializeField]
    private InputField CropHeightInput;

    [SerializeField]
    private GameObject CustomOptions;

    [SerializeField]
    private InputField CropOffsetXInput;

    [SerializeField]
    private InputField CropOffsetYInput;

    [SerializeField]
    private Dropdown CropOptionsList;

    [SerializeField]
    private InputField ScaleWidthInput;

    [SerializeField]
    private InputField ScaleHeightInput;

    private int _selectedCropOption;

    public void Scale () {
        Texture2D source = Resources.Load<Sprite> ("image").texture;
        Texture2D tmp = new Texture2D (1, 1);
        tmp.LoadImage (source.EncodeToJPG ());
        int width = int.Parse (ScaleWidthInput.text);
        int height = int.Parse (ScaleHeightInput.text);
        tmp = CropScale.ScaleTexture (tmp, width, height);
        ResultImage.sprite = Sprite.Create (tmp, new Rect (0f, 0f, tmp.width, tmp.height), new Vector2 (0.5f, 0.5f));
    }

    public void Crop () {
        Texture2D source = Resources.Load<Sprite> ("image").texture;
        Texture2D tmp = new Texture2D (1, 1);
        tmp.LoadImage (source.EncodeToJPG ());
        CropOptions options = (CropOptions) _selectedCropOption;
        int width = int.Parse (CropWidthInput.text);
        int height = int.Parse (CropHeightInput.text);
        int x = int.Parse (CropOffsetXInput.text);
        int y = int.Parse (CropOffsetYInput.text);
        tmp = CropScale.CropTexture (tmp, new Vector2 (width, height), options, x, y);
        ResultImage.sprite = Sprite.Create (tmp, new Rect (0f, 0f, tmp.width, tmp.height), new Vector2 (0.5f, 0.5f));
    }

    public void OnCropOptionsChanged (int index) {
        _selectedCropOption = index;
        CustomOptions.SetActive (index == 5);
    }
}