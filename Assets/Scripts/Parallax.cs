using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    Transform cam; // main camera
    Vector3 camStartPos;
    float distance;  // distance between camera start poistion and its current position

    GameObject[] backgrounds;
    Material[] mats;
    float[] backSpeed;

    float farthestBack;

    [Range(0f, 5f)]
    public float parallaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;


        int backCount = transform.childCount;
        mats = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mats[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculator(backCount);

    }

    void BackSpeedCalculator(int backCount)
    {

        for (int i = 0; i < backCount; i++)
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack) {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }
        for (int i = 0; i < backCount; i++) {

            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }


    private void LateUpdate()
    {

        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);

        for (int i = 0; i < backgrounds.Length; i++) {

            float speed = backSpeed[i] * parallaxSpeed;
            mats[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);

        }
        
    }
}
