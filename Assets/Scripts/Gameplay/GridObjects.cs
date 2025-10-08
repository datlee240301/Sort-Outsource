using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridObjects : MonoBehaviour
{
    [SerializeField] private float spacing = 3f; // Khoảng cách giữa các object
    [SerializeField] private float moveDuration = 0.25f; // Thời gian move bằng DOTween
    [SerializeField] private int maxPerRow = 5; // Tối đa 5 object trên 1 hàng
    [SerializeField] private float verticalSpacing = 8f; // Khoảng cách giữa các hàng nếu có nhiều hàng

    private void Start()
    {
        // ArrangeObjects();
    }

    private void OnTransformChildrenChanged()
    {
        ArrangeObjects(); // Tự động sắp xếp khi thêm/xóa object con
    }

    private void ArrangeObjects()
    {
        int count = transform.childCount;
        if (count == 0) return;

        int totalRows = Mathf.CeilToInt((float)count / maxPerRow);

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);

            int row = i / maxPerRow;
            int col = i % maxPerRow;

            // Số phần tử trong hàng hiện tại
            int inRowCount = Mathf.Min(maxPerRow, count - row * maxPerRow);

            // Tính tổng chiều ngang của hàng để canh giữa
            float totalWidth = (inRowCount - 1) * spacing;
            float xPos = -totalWidth / 2f + col * spacing;

            // Tính tổng chiều cao để canh giữa dọc
            float totalHeight = (totalRows - 1) * verticalSpacing;
            float yPos = totalHeight / 2f - row * verticalSpacing;

            Vector3 targetPos = new Vector3(xPos, yPos, 0f);

            child.DOKill();
            child.DOLocalMove(targetPos, moveDuration).SetEase(Ease.OutQuad);
        }
    }
}
