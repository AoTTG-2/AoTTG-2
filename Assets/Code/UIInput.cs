using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Input (Basic)")]
public class UIInput : MonoBehaviour
{
    public Color activeColor = Color.white;
    public bool autoCorrect;
    public string caratChar = "|";
    public static UIInput current;
    public GameObject eventReceiver;
    public string functionName = "OnSubmit";
    public bool isPassword;
    public UILabel label;
    public int maxChars;
    private Color mDefaultColor = Color.white;
    private string mDefaultText = string.Empty;
    private bool mDoInit = true;
    private string mLastIME = string.Empty;
    private UIWidget.Pivot mPivot = UIWidget.Pivot.Left;
    private float mPosition;
    private string mText = string.Empty;
    public OnSubmit onSubmit;
    public GameObject selectOnTab;
    public KeyboardType type;
    public bool useLabelTextAtStart;
    public Validator validator;

    private void Append(string input)
    {
        int num = 0;
        int length = input.Length;
        while (num < length)
        {
            char nextChar = input[num];
            switch (nextChar)
            {
                case '\b':
                    if (this.mText.Length > 0)
                    {
                        this.mText = this.mText.Substring(0, this.mText.Length - 1);
                        base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
                    }
                    break;

                case '\r':
                case '\n':
                    if (((UICamera.current.submitKey0 == KeyCode.Return) || (UICamera.current.submitKey1 == KeyCode.Return)) && (!this.label.multiLine || (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                    {
                        current = this;
                        if (this.onSubmit != null)
                        {
                            this.onSubmit(this.mText);
                        }
                        if (this.eventReceiver == null)
                        {
                            this.eventReceiver = base.gameObject;
                        }
                        this.eventReceiver.SendMessage(this.functionName, this.mText, SendMessageOptions.DontRequireReceiver);
                        current = null;
                        this.selected = false;
                        return;
                    }
                    if (this.validator != null)
                    {
                        nextChar = this.validator(this.mText, nextChar);
                    }
                    if (nextChar != '\0')
                    {
                        if ((nextChar != '\n') && (nextChar != '\r'))
                        {
                            this.mText = this.mText + nextChar;
                        }
                        else if (this.label.multiLine)
                        {
                            this.mText = this.mText + "\n";
                        }
                        base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
                    }
                    break;

                default:
                    if (nextChar >= ' ')
                    {
                        if (this.validator != null)
                        {
                            nextChar = this.validator(this.mText, nextChar);
                        }
                        if (nextChar != '\0')
                        {
                            this.mText = this.mText + nextChar;
                            base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    break;
            }
            num++;
        }
        this.UpdateLabel();
    }

    protected void Init()
    {
        this.maxChars = 100;
        this.initMain();
    }

    protected void initMain()
    {
        this.maxChars = 100;
        if (this.mDoInit)
        {
            this.mDoInit = false;
            if (this.label == null)
            {
                this.label = base.GetComponentInChildren<UILabel>();
            }
            if (this.label != null)
            {
                if (this.useLabelTextAtStart)
                {
                    this.mText = this.label.text;
                }
                this.mDefaultText = this.label.text;
                this.mDefaultColor = this.label.color;
                this.label.supportEncoding = false;
                this.label.password = this.isPassword;
                this.mPivot = this.label.pivot;
                this.mPosition = this.label.cachedTransform.localPosition.x;
            }
            else
            {
                base.enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        if (UICamera.IsHighlighted(base.gameObject))
        {
            this.OnSelect(false);
        }
    }

    private void OnEnable()
    {
        if (UICamera.IsHighlighted(base.gameObject))
        {
            this.OnSelect(true);
        }
    }

    private void OnInput(string input)
    {
        if (this.mDoInit)
        {
            this.initMain();
        }
        if ((((this.selected && base.enabled) && NGUITools.GetActive(base.gameObject)) && (Application.platform != RuntimePlatform.Android)) && (Application.platform != RuntimePlatform.IPhonePlayer))
        {
            this.Append(input);
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (this.mDoInit)
        {
            this.initMain();
        }
        if (((this.label != null) && base.enabled) && NGUITools.GetActive(base.gameObject))
        {
            if (isSelected)
            {
                this.mText = (this.useLabelTextAtStart || !(this.label.text == this.mDefaultText)) ? this.label.text : string.Empty;
                this.label.color = this.activeColor;
                if (this.isPassword)
                {
                    this.label.password = true;
                }
                Input.imeCompositionMode = IMECompositionMode.On;
                Transform cachedTransform = this.label.cachedTransform;
                Vector3 pivotOffset = (Vector3) this.label.pivotOffset;
                pivotOffset.y += this.label.relativeSize.y;
                pivotOffset = cachedTransform.TransformPoint(pivotOffset);
                Input.compositionCursorPos = UICamera.currentCamera.WorldToScreenPoint(pivotOffset);
                this.UpdateLabel();
            }
            else
            {
                if (string.IsNullOrEmpty(this.mText))
                {
                    this.label.text = this.mDefaultText;
                    this.label.color = this.mDefaultColor;
                    if (this.isPassword)
                    {
                        this.label.password = false;
                    }
                }
                else
                {
                    this.label.text = this.mText;
                }
                this.label.showLastPasswordChar = false;
                Input.imeCompositionMode = IMECompositionMode.Off;
                this.RestoreLabel();
            }
        }
    }

    private void RestoreLabel()
    {
        if (this.label != null)
        {
            this.label.pivot = this.mPivot;
            Vector3 localPosition = this.label.cachedTransform.localPosition;
            localPosition.x = this.mPosition;
            this.label.cachedTransform.localPosition = localPosition;
        }
    }

    private void Update()
    {
        if (this.selected)
        {
            if ((this.selectOnTab != null) && Input.GetKeyDown(KeyCode.Tab))
            {
                UICamera.selectedObject = this.selectOnTab;
            }
            if (Input.GetKeyDown(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                this.Append(NGUITools.clipboard);
            }
            if (this.mLastIME != Input.compositionString)
            {
                this.mLastIME = Input.compositionString;
                this.UpdateLabel();
            }
        }
    }

    private void UpdateLabel()
    {
        if (this.mDoInit)
        {
            this.initMain();
        }
        if ((this.maxChars > 0) && (this.mText.Length > this.maxChars))
        {
            this.mText = this.mText.Substring(0, this.maxChars);
        }
        if (this.label.font != null)
        {
            string str;
            if (this.isPassword && this.selected)
            {
                str = string.Empty;
                int num = 0;
                int length = this.mText.Length;
                while (num < length)
                {
                    str = str + "*";
                    num++;
                }
                str = str + Input.compositionString + this.caratChar;
            }
            else
            {
                str = !this.selected ? this.mText : (this.mText + Input.compositionString + this.caratChar);
            }
            this.label.supportEncoding = false;
            if (!this.label.shrinkToFit)
            {
                if (this.label.multiLine)
                {
                    str = this.label.font.WrapText(str, ((float) this.label.lineWidth) / this.label.cachedTransform.localScale.x, 0, false, UIFont.SymbolStyle.None);
                }
                else
                {
                    string str2 = this.label.font.GetEndOfLineThatFits(str, ((float) this.label.lineWidth) / this.label.cachedTransform.localScale.x, false, UIFont.SymbolStyle.None);
                    if (str2 != str)
                    {
                        str = str2;
                        Vector3 localPosition = this.label.cachedTransform.localPosition;
                        localPosition.x = this.mPosition + this.label.lineWidth;
                        if (this.mPivot == UIWidget.Pivot.Left)
                        {
                            this.label.pivot = UIWidget.Pivot.Right;
                        }
                        else if (this.mPivot == UIWidget.Pivot.TopLeft)
                        {
                            this.label.pivot = UIWidget.Pivot.TopRight;
                        }
                        else if (this.mPivot == UIWidget.Pivot.BottomLeft)
                        {
                            this.label.pivot = UIWidget.Pivot.BottomRight;
                        }
                        this.label.cachedTransform.localPosition = localPosition;
                    }
                    else
                    {
                        this.RestoreLabel();
                    }
                }
            }
            this.label.text = str;
            this.label.showLastPasswordChar = this.selected;
        }
    }

    public string defaultText
    {
        get
        {
            return this.mDefaultText;
        }
        set
        {
            if (this.label.text == this.mDefaultText)
            {
                this.label.text = value;
            }
            this.mDefaultText = value;
        }
    }

    public bool selected
    {
        get
        {
            return (UICamera.selectedObject == base.gameObject);
        }
        set
        {
            if (!value && (UICamera.selectedObject == base.gameObject))
            {
                UICamera.selectedObject = null;
            }
            else if (value)
            {
                UICamera.selectedObject = base.gameObject;
            }
        }
    }

    public virtual string text
    {
        get
        {
            if (this.mDoInit)
            {
                this.initMain();
            }
            return this.mText;
        }
        set
        {
            if (this.mDoInit)
            {
                this.initMain();
            }
            this.mText = value;
            if (this.label != null)
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = this.mDefaultText;
                }
                this.label.supportEncoding = false;
                this.label.text = !this.selected ? value : (value + this.caratChar);
                this.label.showLastPasswordChar = this.selected;
                this.label.color = (this.selected || (value != this.mDefaultText)) ? this.activeColor : this.mDefaultColor;
            }
        }
    }

    public enum KeyboardType
    {
        Default,
        ASCIICapable,
        NumbersAndPunctuation,
        URL,
        NumberPad,
        PhonePad,
        NamePhonePad,
        EmailAddress
    }

    public delegate void OnSubmit(string inputString);

    public delegate char Validator(string currentText, char nextChar);
}

