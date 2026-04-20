using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // これが必要

public class SelectSlider : MonoBehaviour
{
    public GameObject obj_Setting;
    public Slider mySlider;
    public KeyCode key_F;
    public KeyCode key_C;
    void Update()
    {
        if(Input.GetKeyDown(key_F))
        {
            // F押下時にSliderを選択状態にする
            EventSystem.current.SetSelectedGameObject(mySlider.gameObject);
        }

        if(Input.GetKeyDown(key_C))
        {
            obj_Setting.SetActive(!obj_Setting.activeSelf);
        }
    }
}