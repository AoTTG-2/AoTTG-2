using System;
using UnityEngine;

public class AnimatedAlpha : MonoBehaviour
{
    public float alpha = 1f;
    private UIPanel mPanel;
    private UIWidget mWidget;

    private void Awake()
    {
        this.mWidget = base.GetComponent<UIWidget>();
        this.mPanel = base.GetComponent<UIPanel>();
        this.Update();
    }

    private void Update()
    {
        if (this.mWidget != null)
        {
            this.mWidget.alpha = this.alpha;
        }
        if (this.mPanel != null)
        {
            this.mPanel.alpha = this.alpha;
        }
    }
}

