using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour
{
    public int offsetX = 2;              //the offset so that we don't get any werid errors

    // these are used for checking if we need to instantitate stuff
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    
    public bool reverseScale = false;  // used if the object is not tilable


    private float spriteWidth = 0f;     // the width of our elemet
    private Camera cam;
    private Transform myTransform;


    // is called before start(). Great for references
    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderere = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderere.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //does it still need buddies ? if not do nothing
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            // calculate the cameras estende (half the width) of what the camera can see in world coordinates
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //calculate the x position where the camera can see the edge of the sprite (element)
            float edgeVisblePostionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisblePostionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            //checking if we can see the edge of the element and the calling MakeNewBuddy if we can
            if (cam.transform.position.x >= edgeVisblePostionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if(cam.transform.position.x <= edgeVisblePostionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }
    // a function that creates a buddy on the side required
    void MakeNewBuddy(int rightOrLeft)
    {
        // calculating the new position for our buddy
        Vector3 newPostion = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // instantiate our new buddy and storing him in a variable
        Transform newBuddy = (Transform)Instantiate(myTransform, newPostion, myTransform.rotation);

        //if not tilable let's reverse the x size of our object to get rid of ugly seams
        if (reverseScale)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }
        //
        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
