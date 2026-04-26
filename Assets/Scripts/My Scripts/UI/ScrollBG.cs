using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBG : MonoBehaviour
{
    private const float speed = 1f;

    Image image;

    Player player;

    private float latepos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Actor").GetComponent<Player>();
        image = GetComponent<Image>();
        image.material.mainTextureOffset =
            new Vector2(0, 0);
        latepos = player.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (player.transform.position.x != latepos)
            {
                image.material.mainTextureOffset +=
                    new Vector2(
                        Time.deltaTime * 0.01f, 0);
                latepos = player.transform.position.x;
            }
        }
    }
}
