using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Camera)), AddComponentMenu("NGUI/UI/Camera"), ExecuteInEditMode]
public class UICamera : MonoBehaviour
{
    [CompilerGenerated]
    private static Comparison<RaycastHit> f__amScache31;
    public bool allowMultiTouch = true;
    public KeyCode cancelKey0 = KeyCode.Escape;
    public KeyCode cancelKey1 = KeyCode.JoystickButton1;
    public bool clipRaycasts = true;
    public static UICamera current = null;
    public static Camera currentCamera = null;
    public static MouseOrTouch currentTouch = null;
    public static int currentTouchID = -1;
    public bool debug;
    public LayerMask eventReceiverMask = -1;
    public static GameObject fallThrough;
    public static GameObject genericEventHandler;
    public string horizontalAxisName = "Horizontal";
    public static GameObject hoveredObject;
    public static bool inputHasFocus = false;
    public static bool isDragging = false;
    public static RaycastHit lastHit;
    public static Vector2 lastTouchPosition = Vector2.zero;
    private Camera mCam;
    private static MouseOrTouch mController = new MouseOrTouch();
    private static List<Highlighted> mHighlighted = new List<Highlighted>();
    private static GameObject mHover;
    private bool mIsEditor;
    public LayerMask mLayerMask;
    private static List<UICamera> mList = new List<UICamera>();
    private static MouseOrTouch[] mMouse = new MouseOrTouch[] { new MouseOrTouch(), new MouseOrTouch(), new MouseOrTouch() };
    private static float mNextEvent = 0f;
    public float mouseClickThreshold = 10f;
    public float mouseDragThreshold = 4f;
    private static GameObject mSel = null;
    private GameObject mTooltip;
    private float mTooltipTime;
    private static Dictionary<int, MouseOrTouch> mTouches = new Dictionary<int, MouseOrTouch>();
    public static OnCustomInput onCustomInput;
    public float rangeDistance = -1f;
    public string scrollAxisName = "Mouse ScrollWheel";
    public static bool showTooltips = true;
    public bool stickyPress = true;
    public bool stickyTooltip = true;
    public KeyCode submitKey0 = KeyCode.Return;
    public KeyCode submitKey1 = KeyCode.JoystickButton0;
    public float tooltipDelay = 1f;
    public float touchClickThreshold = 40f;
    public float touchDragThreshold = 40f;
    public bool useController = true;
    public bool useKeyboard = true;
    public bool useMouse = true;
    public bool useTouch = true;
    public string verticalAxisName = "Vertical";

    private void Awake()
    {
        this.cachedCamera.eventMask = 0;
        if ((Application.platform != RuntimePlatform.Android) && (Application.platform != RuntimePlatform.IPhonePlayer))
        {
            if ((Application.platform != RuntimePlatform.PS3) && (Application.platform != RuntimePlatform.XBOX360))
            {
                if ((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.OSXEditor))
                {
                    this.mIsEditor = true;
                }
            }
            else
            {
                this.useMouse = false;
                this.useTouch = false;
                this.useKeyboard = false;
                this.useController = true;
            }
        }
        else
        {
            this.useMouse = false;
            this.useTouch = true;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                this.useKeyboard = false;
                this.useController = false;
            }
        }
        mMouse[0].pos.x = Input.mousePosition.x;
        mMouse[0].pos.y = Input.mousePosition.y;
        lastTouchPosition = mMouse[0].pos;
        if (this.eventReceiverMask == -1)
        {
            this.eventReceiverMask = this.cachedCamera.cullingMask;
        }
    }

    private static int CompareFunc(UICamera a, UICamera b)
    {
        if (a.cachedCamera.depth < b.cachedCamera.depth)
        {
            return 1;
        }
        if (a.cachedCamera.depth > b.cachedCamera.depth)
        {
            return -1;
        }
        return 0;
    }

    public static UICamera FindCameraForLayer(int layer)
    {
        int num = ((int) 1) << layer;
        for (int i = 0; i < mList.Count; i++)
        {
            UICamera camera = mList[i];
            Camera cachedCamera = camera.cachedCamera;
            if ((cachedCamera != null) && ((cachedCamera.cullingMask & num) != 0))
            {
                return camera;
            }
        }
        return null;
    }

    private void FixedUpdate()
    {
        if ((this.useMouse && Application.isPlaying) && this.handlesEvents)
        {
            hoveredObject = !Raycast(Input.mousePosition, ref lastHit) ? fallThrough : lastHit.collider.gameObject;
            if (hoveredObject == null)
            {
                hoveredObject = genericEventHandler;
            }
            for (int i = 0; i < 3; i++)
            {
                mMouse[i].current = hoveredObject;
            }
        }
    }

    private static int GetDirection(string axis)
    {
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        if (mNextEvent < realtimeSinceStartup)
        {
            float num2 = Input.GetAxis(axis);
            if (num2 > 0.75f)
            {
                mNextEvent = realtimeSinceStartup + 0.25f;
                return 1;
            }
            if (num2 < -0.75f)
            {
                mNextEvent = realtimeSinceStartup + 0.25f;
                return -1;
            }
        }
        return 0;
    }

    private static int GetDirection(KeyCode up, KeyCode down)
    {
        if (Input.GetKeyDown(up))
        {
            return 1;
        }
        if (Input.GetKeyDown(down))
        {
            return -1;
        }
        return 0;
    }

    private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
    {
        if (Input.GetKeyDown(up0) || Input.GetKeyDown(up1))
        {
            return 1;
        }
        if (!Input.GetKeyDown(down0) && !Input.GetKeyDown(down1))
        {
            return 0;
        }
        return -1;
    }

    public static MouseOrTouch GetTouch(int id)
    {
        MouseOrTouch touch = null;
        if (!mTouches.TryGetValue(id, out touch))
        {
            touch = new MouseOrTouch {
                touchBegan = true
            };
            mTouches.Add(id, touch);
        }
        return touch;
    }

    private static void Highlight(GameObject go, bool highlighted)
    {
        if (go != null)
        {
            int count = mHighlighted.Count;
            while (count > 0)
            {
                Highlighted item = mHighlighted[--count];
                if ((item != null) && (item.go != null))
                {
                    if (item.go != go)
                    {
                        continue;
                    }
                    if (highlighted)
                    {
                        item.counter++;
                    }
                    else if (--item.counter < 1)
                    {
                        mHighlighted.Remove(item);
                        Notify(go, "OnHover", false);
                    }
                    return;
                }
                mHighlighted.RemoveAt(count);
            }
            if (highlighted)
            {
                Highlighted highlighted3 = new Highlighted {
                    go = go,
                    counter = 1
                };
                mHighlighted.Add(highlighted3);
                Notify(go, "OnHover", true);
            }
        }
    }

    public static bool IsHighlighted(GameObject go)
    {
        int count = mHighlighted.Count;
        while (count > 0)
        {
            Highlighted highlighted = mHighlighted[--count];
            if (highlighted.go == go)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsVisible(ref RaycastHit hit)
    {
        UIPanel panel = NGUITools.FindInParents<UIPanel>(hit.collider.gameObject);
        if ((panel != null) && !panel.IsVisible(hit.point))
        {
            return false;
        }
        return true;
    }

    public static void Notify(GameObject go, string funcName, object obj)
    {
        if (go != null)
        {
            go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
            if ((genericEventHandler != null) && (genericEventHandler != go))
            {
                genericEventHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void OnApplicationQuit()
    {
        mHighlighted.Clear();
    }

    private void OnDestroy()
    {
        mList.Remove(this);
    }

    public void ProcessMouse()
    {
        bool flag;
        if (!(flag = this.useMouse && (Time.timeScale < 0.9f)))
        {
            for (int k = 0; k < 3; k++)
            {
                if (Input.GetMouseButton(k) || Input.GetMouseButtonUp(k))
                {
                    flag = true;
                    break;
                }
            }
        }
        mMouse[0].pos = Input.mousePosition;
        mMouse[0].delta = mMouse[0].pos - lastTouchPosition;
        bool flag2 = mMouse[0].pos != lastTouchPosition;
        lastTouchPosition = mMouse[0].pos;
        if (flag)
        {
            hoveredObject = !Raycast(Input.mousePosition, ref lastHit) ? fallThrough : lastHit.collider.gameObject;
            if (hoveredObject == null)
            {
                hoveredObject = genericEventHandler;
            }
            mMouse[0].current = hoveredObject;
        }
        for (int i = 1; i < 3; i++)
        {
            mMouse[i].pos = mMouse[0].pos;
            mMouse[i].delta = mMouse[0].delta;
            mMouse[i].current = mMouse[0].current;
        }
        bool flag3 = false;
        for (int j = 0; j < 3; j++)
        {
            if (Input.GetMouseButton(j))
            {
                flag3 = true;
                break;
            }
        }
        if (flag3)
        {
            this.mTooltipTime = 0f;
        }
        else if ((this.useMouse && flag2) && (!this.stickyTooltip || (mHover != mMouse[0].current)))
        {
            if (this.mTooltipTime != 0f)
            {
                this.mTooltipTime = Time.realtimeSinceStartup + this.tooltipDelay;
            }
            else if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
        }
        if ((this.useMouse && !flag3) && ((mHover != null) && (mHover != mMouse[0].current)))
        {
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            Highlight(mHover, false);
            mHover = null;
        }
        if (this.useMouse)
        {
            for (int m = 0; m < 3; m++)
            {
                bool mouseButtonDown = Input.GetMouseButtonDown(m);
                bool mouseButtonUp = Input.GetMouseButtonUp(m);
                currentTouch = mMouse[m];
                currentTouchID = -1 - m;
                if (mouseButtonDown)
                {
                    currentTouch.pressedCam = currentCamera;
                }
                else if (currentTouch.pressed != null)
                {
                    currentCamera = currentTouch.pressedCam;
                }
                this.ProcessTouch(mouseButtonDown, mouseButtonUp);
            }
            currentTouch = null;
        }
        if ((this.useMouse && !flag3) && (mHover != mMouse[0].current))
        {
            this.mTooltipTime = Time.realtimeSinceStartup + this.tooltipDelay;
            mHover = mMouse[0].current;
            Highlight(mHover, true);
        }
    }

    public void ProcessOthers()
    {
        currentTouchID = -100;
        currentTouch = mController;
        inputHasFocus = (mSel != null) && (mSel.GetComponent<UIInput>() != null);
        bool pressed = ((this.submitKey0 == KeyCode.None) || !Input.GetKeyDown(this.submitKey0)) ? ((this.submitKey1 != KeyCode.None) && Input.GetKeyDown(this.submitKey1)) : true;
        bool unpressed = ((this.submitKey0 == KeyCode.None) || !Input.GetKeyUp(this.submitKey0)) ? ((this.submitKey1 != KeyCode.None) && Input.GetKeyUp(this.submitKey1)) : true;
        if (pressed || unpressed)
        {
            currentTouch.current = mSel;
            this.ProcessTouch(pressed, unpressed);
            currentTouch.current = null;
        }
        int num = 0;
        int num2 = 0;
        if (this.useKeyboard)
        {
            if (inputHasFocus)
            {
                num += GetDirection(KeyCode.UpArrow, KeyCode.DownArrow);
                num2 += GetDirection(KeyCode.RightArrow, KeyCode.LeftArrow);
            }
            else
            {
                num += GetDirection(KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow);
                num2 += GetDirection(KeyCode.D, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow);
            }
        }
        if (this.useController)
        {
            if (!string.IsNullOrEmpty(this.verticalAxisName))
            {
                num += GetDirection(this.verticalAxisName);
            }
            if (!string.IsNullOrEmpty(this.horizontalAxisName))
            {
                num2 += GetDirection(this.horizontalAxisName);
            }
        }
        if (num != 0)
        {
            Notify(mSel, "OnKey", (num <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow);
        }
        if (num2 != 0)
        {
            Notify(mSel, "OnKey", (num2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow);
        }
        if (this.useKeyboard && Input.GetKeyDown(KeyCode.Tab))
        {
            Notify(mSel, "OnKey", KeyCode.Tab);
        }
        if ((this.cancelKey0 != KeyCode.None) && Input.GetKeyDown(this.cancelKey0))
        {
            Notify(mSel, "OnKey", KeyCode.Escape);
        }
        if ((this.cancelKey1 != KeyCode.None) && Input.GetKeyDown(this.cancelKey1))
        {
            Notify(mSel, "OnKey", KeyCode.Escape);
        }
        currentTouch = null;
    }

    public void ProcessTouch(bool pressed, bool unpressed)
    {
        bool flag;
        float num = !(flag = ((currentTouch == mMouse[0]) || (currentTouch == mMouse[1])) || (currentTouch == mMouse[2])) ? this.touchDragThreshold : this.mouseDragThreshold;
        float num2 = !flag ? this.touchClickThreshold : this.mouseClickThreshold;
        if (pressed)
        {
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            currentTouch.pressStarted = true;
            Notify(currentTouch.pressed, "OnPress", false);
            currentTouch.pressed = currentTouch.current;
            currentTouch.dragged = currentTouch.current;
            currentTouch.clickNotification = !flag ? ClickNotification.Always : ClickNotification.BasedOnDelta;
            currentTouch.totalDelta = Vector2.zero;
            currentTouch.dragStarted = false;
            Notify(currentTouch.pressed, "OnPress", true);
            if (currentTouch.pressed != mSel)
            {
                if (this.mTooltip != null)
                {
                    this.ShowTooltip(false);
                }
                selectedObject = null;
            }
        }
        else
        {
            if ((((currentTouch.clickNotification != ClickNotification.None) && !this.stickyPress) && (!unpressed && currentTouch.pressStarted)) && (currentTouch.pressed != hoveredObject))
            {
                isDragging = true;
                Notify(currentTouch.pressed, "OnPress", false);
                currentTouch.pressed = hoveredObject;
                Notify(currentTouch.pressed, "OnPress", true);
                isDragging = false;
            }
            if ((currentTouch.pressed != null) && (currentTouch.delta.magnitude != 0f))
            {
                currentTouch.totalDelta += currentTouch.delta;
                float magnitude = currentTouch.totalDelta.magnitude;
                if (!currentTouch.dragStarted && (num < magnitude))
                {
                    currentTouch.dragStarted = true;
                    currentTouch.delta = currentTouch.totalDelta;
                }
                if (currentTouch.dragStarted)
                {
                    if (this.mTooltip != null)
                    {
                        this.ShowTooltip(false);
                    }
                    isDragging = true;
                    bool flag2 = currentTouch.clickNotification == ClickNotification.None;
                    Notify(currentTouch.dragged, "OnDrag", currentTouch.delta);
                    isDragging = false;
                    if (flag2)
                    {
                        currentTouch.clickNotification = ClickNotification.None;
                    }
                    else if ((currentTouch.clickNotification == ClickNotification.BasedOnDelta) && (num2 < magnitude))
                    {
                        currentTouch.clickNotification = ClickNotification.None;
                    }
                }
            }
        }
        if (unpressed)
        {
            currentTouch.pressStarted = false;
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            if (currentTouch.pressed != null)
            {
                Notify(currentTouch.pressed, "OnPress", false);
                if (this.useMouse && (currentTouch.pressed == mHover))
                {
                    Notify(currentTouch.pressed, "OnHover", true);
                }
                if ((currentTouch.dragged != currentTouch.current) && ((currentTouch.clickNotification == ClickNotification.None) || (currentTouch.totalDelta.magnitude >= num)))
                {
                    Notify(currentTouch.current, "OnDrop", currentTouch.dragged);
                }
                else
                {
                    if (currentTouch.pressed != mSel)
                    {
                        mSel = currentTouch.pressed;
                        Notify(currentTouch.pressed, "OnSelect", true);
                    }
                    else
                    {
                        mSel = currentTouch.pressed;
                    }
                    if (currentTouch.clickNotification != ClickNotification.None)
                    {
                        float realtimeSinceStartup = Time.realtimeSinceStartup;
                        Notify(currentTouch.pressed, "OnClick", null);
                        if ((currentTouch.clickTime + 0.35f) > realtimeSinceStartup)
                        {
                            Notify(currentTouch.pressed, "OnDoubleClick", null);
                        }
                        currentTouch.clickTime = realtimeSinceStartup;
                    }
                }
            }
            currentTouch.dragStarted = false;
            currentTouch.pressed = null;
            currentTouch.dragged = null;
        }
    }

    public void ProcessTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            currentTouchID = !this.allowMultiTouch ? 1 : touch.fingerId;
            currentTouch = GetTouch(currentTouchID);
            bool pressed = (touch.phase == TouchPhase.Began) || currentTouch.touchBegan;
            bool unpressed = (touch.phase == TouchPhase.Canceled) || (touch.phase == TouchPhase.Ended);
            currentTouch.touchBegan = false;
            if (pressed)
            {
                currentTouch.delta = Vector2.zero;
            }
            else
            {
                currentTouch.delta = touch.position - currentTouch.pos;
            }
            currentTouch.pos = touch.position;
            hoveredObject = !Raycast((Vector3) currentTouch.pos, ref lastHit) ? fallThrough : lastHit.collider.gameObject;
            if (hoveredObject == null)
            {
                hoveredObject = genericEventHandler;
            }
            currentTouch.current = hoveredObject;
            lastTouchPosition = currentTouch.pos;
            if (pressed)
            {
                currentTouch.pressedCam = currentCamera;
            }
            else if (currentTouch.pressed != null)
            {
                currentCamera = currentTouch.pressedCam;
            }
            if (touch.tapCount > 1)
            {
                currentTouch.clickTime = Time.realtimeSinceStartup;
            }
            this.ProcessTouch(pressed, unpressed);
            if (unpressed)
            {
                RemoveTouch(currentTouchID);
            }
            currentTouch = null;
            if (!this.allowMultiTouch)
            {
                break;
            }
        }
    }

    public static bool Raycast(Vector3 inPos, ref RaycastHit hit)
    {
        for (int i = 0; i < mList.Count; i++)
        {
            UICamera camera = mList[i];
            if (camera.enabled && NGUITools.GetActive(camera.gameObject))
            {
                currentCamera = camera.cachedCamera;
                Vector3 vector = currentCamera.ScreenToViewportPoint(inPos);
                if (((vector.x >= 0f) && (vector.x <= 1f)) && ((vector.y >= 0f) && (vector.y <= 1f)))
                {
                    Ray ray = currentCamera.ScreenPointToRay(inPos);
                    int layerMask = currentCamera.cullingMask & camera.eventReceiverMask;
                    float distance = (camera.rangeDistance <= 0f) ? (currentCamera.farClipPlane - currentCamera.nearClipPlane) : camera.rangeDistance;
                    if (camera.clipRaycasts)
                    {
                        RaycastHit[] array = Physics.RaycastAll(ray, distance, layerMask);
                        if (array.Length > 1)
                        {
                            if (f__amScache31 == null)
                            {
                                f__amScache31 = (r1, r2) => r1.distance.CompareTo(r2.distance);
                            }
                            Array.Sort<RaycastHit>(array, f__amScache31);
                            int index = 0;
                            int length = array.Length;
                            while (index < length)
                            {
                                if (IsVisible(ref array[index]))
                                {
                                    hit = array[index];
                                    return true;
                                }
                                index++;
                            }
                            continue;
                        }
                        if ((array.Length != 1) || !IsVisible(ref array[0]))
                        {
                            continue;
                        }
                        hit = array[0];
                        return true;
                    }
                    if (Physics.Raycast(ray, out hit, distance, layerMask))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static void RemoveTouch(int id)
    {
        mTouches.Remove(id);
    }

    public void ShowTooltip(bool val)
    {
        this.mTooltipTime = 0f;
        Notify(this.mTooltip, "OnTooltip", val);
        if (!val)
        {
            this.mTooltip = null;
        }
    }

    private void Start()
    {
        mList.Add(this);
        mList.Sort(new Comparison<UICamera>(UICamera.CompareFunc));
    }

    private void Update()
    {
        if (Application.isPlaying && this.handlesEvents)
        {
            current = this;
            if (this.useMouse || (this.useTouch && this.mIsEditor))
            {
                this.ProcessMouse();
            }
            if (this.useTouch)
            {
                this.ProcessTouches();
            }
            if (onCustomInput != null)
            {
                onCustomInput();
            }
            if ((this.useMouse && (mSel != null)) && (((this.cancelKey0 != KeyCode.None) && Input.GetKeyDown(this.cancelKey0)) || ((this.cancelKey1 != KeyCode.None) && Input.GetKeyDown(this.cancelKey1))))
            {
                selectedObject = null;
            }
            if (mSel != null)
            {
                string inputString = Input.inputString;
                if (this.useKeyboard && Input.GetKeyDown(KeyCode.Delete))
                {
                    inputString = inputString + "\b";
                }
                if (inputString.Length > 0)
                {
                    if (!this.stickyTooltip && (this.mTooltip != null))
                    {
                        this.ShowTooltip(false);
                    }
                    Notify(mSel, "OnInput", inputString);
                }
            }
            else
            {
                inputHasFocus = false;
            }
            if (mSel != null)
            {
                this.ProcessOthers();
            }
            if (this.useMouse && (mHover != null))
            {
                float axis = Input.GetAxis(this.scrollAxisName);
                if (axis != 0f)
                {
                    Notify(mHover, "OnScroll", axis);
                }
                if ((showTooltips && (this.mTooltipTime != 0f)) && (((this.mTooltipTime < Time.realtimeSinceStartup) || Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.RightShift)))
                {
                    this.mTooltip = mHover;
                    this.ShowTooltip(true);
                }
            }
            current = null;
        }
    }

    public Camera cachedCamera
    {
        get
        {
            if (this.mCam == null)
            {
                this.mCam = base.GetComponent<Camera>();
            }
            return this.mCam;
        }
    }

    public static int dragCount
    {
        get
        {
            int num = 0;
            for (int i = 0; i < mTouches.Count; i++)
            {
                if (mTouches[i].dragged != null)
                {
                    num++;
                }
            }
            for (int j = 0; j < mMouse.Length; j++)
            {
                if (mMouse[j].dragged != null)
                {
                    num++;
                }
            }
            if (mController.dragged != null)
            {
                num++;
            }
            return num;
        }
    }

    public static UICamera eventHandler
    {
        get
        {
            for (int i = 0; i < mList.Count; i++)
            {
                UICamera camera = mList[i];
                if (((camera != null) && camera.enabled) && NGUITools.GetActive(camera.gameObject))
                {
                    return camera;
                }
            }
            return null;
        }
    }

    private bool handlesEvents
    {
        get
        {
            return (eventHandler == this);
        }
    }

    public static Camera mainCamera
    {
        get
        {
            UICamera eventHandler = UICamera.eventHandler;
            return ((eventHandler == null) ? null : eventHandler.cachedCamera);
        }
    }

    public static GameObject selectedObject
    {
        get
        {
            return mSel;
        }
        set
        {
            if (mSel != value)
            {
                if (mSel != null)
                {
                    UICamera camera = FindCameraForLayer(mSel.layer);
                    if (camera != null)
                    {
                        current = camera;
                        currentCamera = camera.mCam;
                        Notify(mSel, "OnSelect", false);
                        if (camera.useController || camera.useKeyboard)
                        {
                            Highlight(mSel, false);
                        }
                        current = null;
                    }
                }
                mSel = value;
                if (mSel != null)
                {
                    UICamera camera2 = FindCameraForLayer(mSel.layer);
                    if (camera2 != null)
                    {
                        current = camera2;
                        currentCamera = camera2.mCam;
                        if (camera2.useController || camera2.useKeyboard)
                        {
                            Highlight(mSel, true);
                        }
                        Notify(mSel, "OnSelect", true);
                        current = null;
                    }
                }
            }
        }
    }

    public static int touchCount
    {
        get
        {
            int num = 0;
            for (int i = 0; i < mTouches.Count; i++)
            {
                if (mTouches[i].pressed != null)
                {
                    num++;
                }
            }
            for (int j = 0; j < mMouse.Length; j++)
            {
                if (mMouse[j].pressed != null)
                {
                    num++;
                }
            }
            if (mController.pressed != null)
            {
                num++;
            }
            return num;
        }
    }

    public enum ClickNotification
    {
        None,
        Always,
        BasedOnDelta
    }

    private class Highlighted
    {
        public int counter;
        public GameObject go;
    }

    public class MouseOrTouch
    {
        public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;
        public float clickTime;
        public GameObject current;
        public Vector2 delta;
        public GameObject dragged;
        public bool dragStarted;
        public Vector2 pos;
        public GameObject pressed;
        public Camera pressedCam;
        public bool pressStarted;
        public Vector2 totalDelta;
        public bool touchBegan = true;
    }

    public delegate void OnCustomInput();
}

