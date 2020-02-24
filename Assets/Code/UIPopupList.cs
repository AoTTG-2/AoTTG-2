using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Popup List"), ExecuteInEditMode]
public class UIPopupList : MonoBehaviour
{
    private const float animSpeed = 0.15f;
    public UIAtlas atlas;
    public Color backgroundColor = Color.white;
    public string backgroundSprite;
    public static UIPopupList current;
    public GameObject eventReceiver;
    public UIFont font;
    public string functionName = "OnSelectionChange";
    public Color highlightColor = new Color(0.5960785f, 1f, 0.2f, 1f);
    public string highlightSprite;
    public bool isAnimated = true;
    public bool isLocalized;
    public List<string> items = new List<string>();
    private UISprite mBackground;
    private float mBgBorder;
    private GameObject mChild;
    private UISprite mHighlight;
    private UILabel mHighlightedLabel;
    private List<UILabel> mLabelList = new List<UILabel>();
    private UIPanel mPanel;
    [SerializeField, HideInInspector]
    private string mSelectedItem;
    public OnSelectionChange onSelectionChange;
    public Vector2 padding = new Vector3(4f, 4f);
    public Position position;
    public Color textColor = Color.white;
    public UILabel textLabel;
    public float textScale = 1f;

    private void Animate(UIWidget widget, bool placeAbove, float bottom)
    {
        this.AnimateColor(widget);
        this.AnimatePosition(widget, placeAbove, bottom);
    }

    private void AnimateColor(UIWidget widget)
    {
        Color color = widget.color;
        widget.color = new Color(color.r, color.g, color.b, 0f);
        TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
    }

    private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
    {
        Vector3 localPosition = widget.cachedTransform.localPosition;
        Vector3 vector2 = !placeAbove ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z);
        widget.cachedTransform.localPosition = vector2;
        TweenPosition.Begin(widget.gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
    }

    private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
    {
        GameObject gameObject = widget.gameObject;
        Transform cachedTransform = widget.cachedTransform;
        float y = (this.font.size * this.textScale) + (this.mBgBorder * 2f);
        Vector3 localScale = cachedTransform.localScale;
        cachedTransform.localScale = new Vector3(localScale.x, y, localScale.z);
        TweenScale.Begin(gameObject, 0.15f, localScale).method = UITweener.Method.EaseOut;
        if (placeAbove)
        {
            Vector3 localPosition = cachedTransform.localPosition;
            cachedTransform.localPosition = new Vector3(localPosition.x, (localPosition.y - localScale.y) + y, localPosition.z);
            TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
        }
    }

    private void Highlight(UILabel lbl, bool instant)
    {
        if (this.mHighlight != null)
        {
            TweenPosition component = lbl.GetComponent<TweenPosition>();
            if ((component == null) || !component.enabled)
            {
                this.mHighlightedLabel = lbl;
                UIAtlas.Sprite atlasSprite = this.mHighlight.GetAtlasSprite();
                if (atlasSprite != null)
                {
                    float num = atlasSprite.inner.xMin - atlasSprite.outer.xMin;
                    float y = atlasSprite.inner.yMin - atlasSprite.outer.yMin;
                    Vector3 pos = lbl.cachedTransform.localPosition + new Vector3(-num, y, 1f);
                    if (!instant && this.isAnimated)
                    {
                        TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, pos).method = UITweener.Method.EaseOut;
                    }
                    else
                    {
                        this.mHighlight.cachedTransform.localPosition = pos;
                    }
                }
            }
        }
    }

    private void OnClick()
    {
        if (((this.mChild == null) && (this.atlas != null)) && ((this.font != null) && (this.items.Count > 0)))
        {
            this.mLabelList.Clear();
            this.handleEvents = true;
            if (this.mPanel == null)
            {
                this.mPanel = UIPanel.Find(base.transform, true);
            }
            Transform child = base.transform;
            Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(child.parent, child);
            this.mChild = new GameObject("Drop-down List");
            this.mChild.layer = base.gameObject.layer;
            Transform transform = this.mChild.transform;
            transform.parent = child.parent;
            transform.localPosition = bounds.min;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
            this.mBackground.pivot = UIWidget.Pivot.TopLeft;
            this.mBackground.depth = NGUITools.CalculateNextDepth(this.mPanel.gameObject);
            this.mBackground.color = this.backgroundColor;
            Vector4 border = this.mBackground.border;
            this.mBgBorder = border.y;
            this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
            this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
            this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
            this.mHighlight.color = this.highlightColor;
            UIAtlas.Sprite atlasSprite = this.mHighlight.GetAtlasSprite();
            if (atlasSprite != null)
            {
                float num = atlasSprite.inner.yMin - atlasSprite.outer.yMin;
                float num2 = (this.font.size * this.font.pixelSize) * this.textScale;
                float a = 0f;
                float y = -this.padding.y;
                List<UILabel> list = new List<UILabel>();
                int num5 = 0;
                int count = this.items.Count;
                while (num5 < count)
                {
                    string key = this.items[num5];
                    UILabel item = NGUITools.AddWidget<UILabel>(this.mChild);
                    item.pivot = UIWidget.Pivot.TopLeft;
                    item.font = this.font;
                    item.text = (!this.isLocalized || (Localization.instance == null)) ? key : Localization.instance.Get(key);
                    item.color = this.textColor;
                    item.cachedTransform.localPosition = new Vector3(border.x + this.padding.x, y, -1f);
                    item.MakePixelPerfect();
                    if (this.textScale != 1f)
                    {
                        Vector3 localScale = item.cachedTransform.localScale;
                        item.cachedTransform.localScale = (Vector3) (localScale * this.textScale);
                    }
                    list.Add(item);
                    y -= num2;
                    y -= this.padding.y;
                    a = Mathf.Max(a, item.relativeSize.x * num2);
                    UIEventListener listener = UIEventListener.Get(item.gameObject);
                    listener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
                    listener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
                    listener.parameter = key;
                    if (this.mSelectedItem == key)
                    {
                        this.Highlight(item, true);
                    }
                    this.mLabelList.Add(item);
                    num5++;
                }
                a = Mathf.Max(a, bounds.size.x - ((border.x + this.padding.x) * 2f));
                Vector3 vector5 = new Vector3((a * 0.5f) / num2, -0.5f, 0f);
                Vector3 vector6 = new Vector3(a / num2, (num2 + this.padding.y) / num2, 1f);
                int num7 = 0;
                int num8 = list.Count;
                while (num7 < num8)
                {
                    UILabel label2 = list[num7];
                    BoxCollider collider = NGUITools.AddWidgetCollider(label2.gameObject);
                    vector5.z = collider.center.z;
                    collider.center = vector5;
                    collider.size = vector6;
                    num7++;
                }
                a += (border.x + this.padding.x) * 2f;
                y -= border.y;
                this.mBackground.cachedTransform.localScale = new Vector3(a, -y + border.y, 1f);
                this.mHighlight.cachedTransform.localScale = new Vector3((a - ((border.x + this.padding.x) * 2f)) + ((atlasSprite.inner.xMin - atlasSprite.outer.xMin) * 2f), num2 + (num * 2f), 1f);
                bool placeAbove = this.position == Position.Above;
                if (this.position == Position.Auto)
                {
                    UICamera camera = UICamera.FindCameraForLayer(base.gameObject.layer);
                    if (camera != null)
                    {
                        placeAbove = camera.cachedCamera.WorldToViewportPoint(child.position).y < 0.5f;
                    }
                }
                if (this.isAnimated)
                {
                    float bottom = y + num2;
                    this.Animate(this.mHighlight, placeAbove, bottom);
                    int num10 = 0;
                    int num11 = list.Count;
                    while (num10 < num11)
                    {
                        this.Animate(list[num10], placeAbove, bottom);
                        num10++;
                    }
                    this.AnimateColor(this.mBackground);
                    this.AnimateScale(this.mBackground, placeAbove, bottom);
                }
                if (placeAbove)
                {
                    transform.localPosition = new Vector3(bounds.min.x, (bounds.max.y - y) - border.y, bounds.min.z);
                }
            }
        }
        else
        {
            this.OnSelect(false);
        }
    }

    private void OnItemHover(GameObject go, bool isOver)
    {
        if (isOver)
        {
            UILabel component = go.GetComponent<UILabel>();
            this.Highlight(component, false);
        }
    }

    private void OnItemPress(GameObject go, bool isPressed)
    {
        if (isPressed)
        {
            this.Select(go.GetComponent<UILabel>(), true);
        }
    }

    private void OnKey(KeyCode key)
    {
        if ((base.enabled && NGUITools.GetActive(base.gameObject)) && this.handleEvents)
        {
            int index = this.mLabelList.IndexOf(this.mHighlightedLabel);
            if (key == KeyCode.UpArrow)
            {
                if (index > 0)
                {
                    this.Select(this.mLabelList[--index], false);
                }
            }
            else if (key == KeyCode.DownArrow)
            {
                if ((index + 1) < this.mLabelList.Count)
                {
                    this.Select(this.mLabelList[++index], false);
                }
            }
            else if (key == KeyCode.Escape)
            {
                this.OnSelect(false);
            }
        }
    }

    private void OnLocalize(Localization loc)
    {
        if (this.isLocalized && (this.textLabel != null))
        {
            this.textLabel.text = loc.Get(this.mSelectedItem);
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (!isSelected && (this.mChild != null))
        {
            this.mLabelList.Clear();
            this.handleEvents = false;
            if (this.isAnimated)
            {
                UIWidget[] componentsInChildren = this.mChild.GetComponentsInChildren<UIWidget>();
                int index = 0;
                int length = componentsInChildren.Length;
                while (index < length)
                {
                    UIWidget widget = componentsInChildren[index];
                    Color color = widget.color;
                    color.a = 0f;
                    TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
                    index++;
                }
                Collider[] colliderArray = this.mChild.GetComponentsInChildren<Collider>();
                int num3 = 0;
                int num4 = colliderArray.Length;
                while (num3 < num4)
                {
                    colliderArray[num3].enabled = false;
                    num3++;
                }
                UnityEngine.Object.Destroy(this.mChild, 0.15f);
            }
            else
            {
                UnityEngine.Object.Destroy(this.mChild);
            }
            this.mBackground = null;
            this.mHighlight = null;
            this.mChild = null;
        }
    }

    private void Select(UILabel lbl, bool instant)
    {
        this.Highlight(lbl, instant);
        UIEventListener component = lbl.gameObject.GetComponent<UIEventListener>();
        this.selection = component.parameter as string;
        UIButtonSound[] components = base.GetComponents<UIButtonSound>();
        int index = 0;
        int length = components.Length;
        while (index < length)
        {
            UIButtonSound sound = components[index];
            if (sound.trigger == UIButtonSound.Trigger.OnClick)
            {
                NGUITools.PlaySound(sound.audioClip, sound.volume, 1f);
            }
            index++;
        }
    }

    private void Start()
    {
        if (this.textLabel != null)
        {
            if (string.IsNullOrEmpty(this.mSelectedItem))
            {
                if (this.items.Count > 0)
                {
                    this.selection = this.items[0];
                }
            }
            else
            {
                string mSelectedItem = this.mSelectedItem;
                this.mSelectedItem = null;
                this.selection = mSelectedItem;
            }
        }
    }

    private bool handleEvents
    {
        get
        {
            UIButtonKeys component = base.GetComponent<UIButtonKeys>();
            return ((component == null) || !component.enabled);
        }
        set
        {
            UIButtonKeys component = base.GetComponent<UIButtonKeys>();
            if (component != null)
            {
                component.enabled = !value;
            }
        }
    }

    public bool isOpen
    {
        get
        {
            return (this.mChild != null);
        }
    }

    public string selection
    {
        get
        {
            return this.mSelectedItem;
        }
        set
        {
            if (this.mSelectedItem != value)
            {
                this.mSelectedItem = value;
                if (this.textLabel != null)
                {
                    this.textLabel.text = !this.isLocalized ? value : Localization.Localize(value);
                }
                current = this;
                if (this.onSelectionChange != null)
                {
                    this.onSelectionChange(this.mSelectedItem);
                }
                if (((this.eventReceiver != null) && !string.IsNullOrEmpty(this.functionName)) && Application.isPlaying)
                {
                    this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
                }
                current = null;
                if (this.textLabel == null)
                {
                    this.mSelectedItem = null;
                }
            }
        }
    }

    public delegate void OnSelectionChange(string item);

    public enum Position
    {
        Auto,
        Above,
        Below
    }
}

