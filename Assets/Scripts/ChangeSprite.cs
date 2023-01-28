using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Sprite spriteConserto;
    public SpriteRenderer rendererSprite;

    public void consertar() {
        rendererSprite.sprite = spriteConserto;
    }

}
