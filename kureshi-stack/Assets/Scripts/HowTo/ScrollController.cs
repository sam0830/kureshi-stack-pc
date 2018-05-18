using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollController : MonoBehaviour, IEndDragHandler {
	private ScrollRect scrollRect;

	/**
	 * 1ページの幅
	 * @type {float}
	 */
    private float pageWidth;
    /**
     * 前回のページインデックス
     * @type {int}
     */
    private int prevPageIndex = 0;

	private void Awake() {
		scrollRect = GetComponent<ScrollRect>();
		GridLayoutGroup grid = scrollRect.content.GetComponent<GridLayoutGroup>();
        // 1ページの幅を取得.
        pageWidth = grid.cellSize.x + grid.spacing.x;
	}

	private void Start () {

	}

	public void OnEndDrag(PointerEventData eventData) {
		scrollRect.StopMovement();
		// スナップさせるページを決定する.
        // スナップさせるページのインデックスを決定する.
        int pageIndex = Mathf.RoundToInt(scrollRect.content.anchoredPosition.x / pageWidth);
        // ページが変わっていない且つ、素早くドラッグした場合.
        // ドラッグ量の具合は適宜調整してください.
        if (pageIndex == prevPageIndex && Mathf.Abs(eventData.delta.x) >= 5)
        {
            pageIndex += (int)Mathf.Sign(eventData.delta.x);
        }

        // Contentをスクロール位置を決定する.
        // 必ずページにスナップさせるような位置になるところがポイント.
        float destX = pageIndex * pageWidth;
        scrollRect.content.anchoredPosition = new Vector2(destX, scrollRect.content.anchoredPosition.y);

        // 「ページが変わっていない」の判定を行うため、前回スナップされていたページを記憶しておく.
        prevPageIndex = pageIndex;
	}
}
