using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DamageNumbers : MonoBehaviour
    {
        private GameObject myGO;
        private GameObject childGO;
        private Text textComponent;

        void Start()
        {
            // create game object and child object
            myGO = new GameObject();
            myGO.name = "DamageNumbersCanvas";
            myGO.transform.parent = gameObject.transform;

            childGO = new GameObject();
            childGO.name = "DamageNumbers";

            // set the child object as a child of the parent
            childGO.transform.parent = myGO.transform;

            // add a canvas to the parent
            myGO.AddComponent<Canvas>();
            // add a recttransform to the child
            childGO.AddComponent<RectTransform>();

            // make a reference to the parent canvas and use the ref to set its properties
            Canvas myCanvas = myGO.GetComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            myCanvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            // set text on gui layer
            myCanvas.sortingLayerName = "GUI";
            myCanvas.sortingOrder = 1;

            // add a text component to the child
            childGO.AddComponent<Text>();
            // make a reference to the child rect transform and set its values
            RectTransform childRectTransform = childGO.GetComponent<RectTransform>();
            RectTransform parentRectTransform = myGO.GetComponent<RectTransform>();

            //  Left=position.x Right=sizeDelta.x PosY=position.y PosZ=position.z Height=sizeDelta.y
            // set child anchors for resizing behaviour
            childRectTransform.anchoredPosition3D = new Vector3(200f, -250f, 0f);
            childRectTransform.sizeDelta = new Vector2(0f, 0f);
            childRectTransform.anchorMin = new Vector2(0f, 0f);
            childRectTransform.anchorMax = new Vector2(1f, 1f);

            // set text font type and material at runtime from font stored in Resources folder
            textComponent = childGO.GetComponent<Text>();
            
            textComponent.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            textComponent.text = "hello world";
        }

        public void DrawDamage(int damage)
        {
            Debug.Log(damage);
            
            // Set rand location?
            textComponent.text = damage.ToString();
        }
    }
}


