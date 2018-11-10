using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditScroller : MonoBehaviour
{
    public Camera uguiCamera;
    public GameObject uguiScrollView;
    public GameObject uguiScrollRect;
    public GameObject uguiContent;
    public GameObject uguiScrollbar;
    private Scrollbar scrollbar;
    private Tweener scrollbarTween;
    private float tweenFloat;
    private bool isAutoScrolling = false;
    private bool touchInside;

    private float currentStart;
    private float easeInA;
    private float easeInB;
    private float easeStart;
    private int fingerID;
    private float initialTime;
    private float mathHelperA;
    private float mathHelperB;
    private float percentage;
    private float time;

    private bool letsGo;
    private ScrollRect srComponent;

    [Range(0, 500)]
    public float scrollSpeed = 50;
    public float initialDelay = 0.9f;

    void Awake()
    {
        //Disables multitouch on mobile.
        Input.multiTouchEnabled = false;
    }

    void OnEnable()
    {
        //Reset variables
        letsGo = false;

        //Checks for filled in GameObject fields in inspector.
        if (uguiCamera == null || uguiContent == null || uguiScrollbar == null || uguiScrollRect == null || uguiScrollView == null)
        {
            Debug.Log("Interactive Credits cannot function until you have filled in the required fields on the script component in the inspector.");
            this.enabled = false;
        }
        else
        {
            //Invokes a wait for 0.1 seconds before proceeding to setup. This is a workaround for the content height not being updated before the first frame.
            Invoke("DelayedSetup", 0.1f);
        }

        scrollbar = uguiScrollbar.GetComponent<Scrollbar>();
        scrollbar.enabled = true;
        tweenFloat = 1;
        isAutoScrolling = true;
    }

    void OnDisable()
    {
        //Reset content to starting position.
        Vector3 newPosition = uguiContent.transform.localPosition;
        newPosition.y = 0;
        uguiContent.transform.localPosition = newPosition;

        //Complete and kill tween.
        DOTween.Complete("ScrollPro");
        DOTween.Kill("ScrollPro");
    }

    void Update()
    {
        if (letsGo == true && touchInside == false && (Mathf.Abs(srComponent.velocity.y - 0) <= 0.1))
        {
            letsGo = false;
            PressReleased();
        }

        if (isAutoScrolling == true)
        {
            scrollbar.value = tweenFloat;
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = uguiCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray))
            {
                CancelInvoke("PressReleased");
                CancelInvoke("InitialEaseIn");
                scrollbarTween.Kill();
                isAutoScrolling = false;
                touchInside = true;
            }
            else
            {
                touchInside = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (touchInside == true)
            {

                tweenFloat = scrollbar.value;
                //Invoke("PressReleased", 0.8f);
                letsGo = true;
                touchInside = false;
            }
        }
#endif

#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE

        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Ray ray = uguiCamera.ScreenPointToRay(Input.GetTouch(i).position);

                if (Physics.Raycast(ray))
                {
                    CancelInvoke("PressReleased");
                    CancelInvoke("InitialEaseIn");
                    scrollbarTween.Kill();
                    isAutoScrolling = false;
                    touchInside = true;
                }
                else
                {
                    touchInside = false;
                }
            }
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (touchInside == true)
                {
                    tweenFloat = scrollbar.value;
                    //Invoke("PressReleased", 0.8f);
                    letsGo = true;
                    touchInside = false;
                }
            }
        }
#endif
    }

    void DelayedSetup()
    {
        //Invokes a wait for the remaining 0.9 seconds until the initial autoscroll starts.
        Invoke("InitialEaseIn", initialDelay);

        //Sets up the box collider for raycasting.
        RectTransform rtScrollRect = uguiScrollView.GetComponent<RectTransform>();
        Vector3 boxColliderSize = new Vector3(rtScrollRect.rect.width, rtScrollRect.rect.height, 1);
        if (uguiScrollRect.GetComponent<ScrollRect>() != null)
        {
            if (uguiScrollRect.GetComponent<BoxCollider>() == null)
            {
                BoxCollider bc = uguiScrollRect.AddComponent<BoxCollider>() as BoxCollider;
                bc.size = boxColliderSize;
            }
        }

        //Gets initial values.
        srComponent = uguiScrollRect.GetComponent<ScrollRect>();
        RectTransform rtContent = uguiContent.GetComponent<RectTransform>();
        mathHelperA = rtContent.rect.height - rtScrollRect.rect.height;
        initialTime = mathHelperA / scrollSpeed;
        mathHelperA = mathHelperA / (scrollSpeed/2);
        mathHelperB = mathHelperA - 1;
        easeInA = mathHelperB / mathHelperA;
        easeInB = 1 - easeInA;
        //Moved scrollbar fetching to on enable.
        //scrollbar = uguiScrollbar.GetComponent<Scrollbar>();
        //scrollbar.enabled = true;
        time = initialTime;
        easeStart = 1;
        currentStart = 1;
        percentage = 0;

        //Disables content auto layout components.
        VerticalLayoutGroup componentA = uguiContent.GetComponent<VerticalLayoutGroup>();
        ContentSizeFitter componentB = uguiContent.GetComponent<ContentSizeFitter>();
        if (componentA != null)
            componentA.enabled = false;
        if (componentB != null)
            componentB.enabled = false;
    }

    void InitialEaseIn()
    {
        tweenFloat = currentStart;
        //isAutoScrolling = true;
        scrollbarTween = DOTween.To(() => tweenFloat, x => tweenFloat = x, easeInA, 1).SetEase(Ease.InQuad).OnComplete(InitialScroll).SetId("ScrollPro");
    }

    void InitialScroll()
    {
        tweenFloat = easeInA;
        scrollbarTween = DOTween.To(() => tweenFloat, x => tweenFloat = x, 0, time).SetEase(Ease.Linear).OnComplete(AutoScrollEnd).SetId("ScrollPro");
    }

    void PressReleased()
    {
        currentStart = scrollbar.value;
        time = initialTime;
        percentage = currentStart;
        time = time * percentage;
        easeStart = currentStart - easeInB;
        tweenFloat = currentStart;
        isAutoScrolling = true;
        if (tweenFloat != 0)
        {
            scrollbarTween = DOTween.To(() => tweenFloat, x => tweenFloat = x, easeStart, 1).SetEase(Ease.InQuad).OnComplete(AutoScrolling).SetId("ScrollPro");
        }
    }

    void AutoScrolling()
    {
        scrollbarTween = DOTween.To(() => tweenFloat, x => tweenFloat = x, 0, time).SetEase(Ease.Linear).OnComplete(AutoScrollEnd).SetId("ScrollPro");
    }

    void AutoScrollEnd()
    {
        isAutoScrolling = false;
    }
}