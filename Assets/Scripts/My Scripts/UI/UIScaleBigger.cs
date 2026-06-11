using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleBigger : MonoBehaviour
{

    public void UISmoller()
    {
        // RectTransformコンポーネントの取得
        RectTransform rectTransform = GetComponent<RectTransform>();

        // UIの幅（水平方向のサイズ）を300に変更する
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);

        // UIの高さ（垂直方向のサイズ）を200に変更する
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
    }

    public void UIBigger()
    {
        // RectTransformコンポーネントの取得
        RectTransform rectTransform = GetComponent<RectTransform>();

        // UIの幅（水平方向のサイズ）を300に変更する
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);

        // UIの高さ（垂直方向のサイズ）を200に変更する
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 76);
    }
}
