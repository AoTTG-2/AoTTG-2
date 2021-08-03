using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        public FitType fitType;
        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        public bool fitX;
        public bool fitY;

        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputHorizontal();
            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;
                float sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }

            if (fitType == FitType.FixedColumns && !fitY)
            {
                rectTransform.sizeDelta = new Vector2(0, (cellSize.y + spacing.y ) * rectChildren.Count + padding.bottom + padding.top);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColumns || fitType == FitType.Uniform)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float) columns);
            }

            if (fitType == FitType.Height || fitType == FitType.FixedRows || fitType == FitType.Uniform)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float) rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = (parentWidth / (float) columns) - ((spacing.x / (float) columns) * (columns - 1)) - (padding.left / (float) columns) - (padding.right / (float) columns);
            float cellHeight = (parentHeight / (float) rows) - ((spacing.y / (float) rows) * (rows - 1)) - (padding.top / (float) rows) - (padding.bottom / (float) rows);

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            for (var i = 0; i < rectChildren.Count; i++)
            {
                var rowCount = i / columns;
                var columnCount = i % columns;

                var item = rectChildren[i];
                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void SetLayoutHorizontal() { }
        public override void SetLayoutVertical() { }
    }
}