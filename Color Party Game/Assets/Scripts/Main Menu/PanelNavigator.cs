using UnityEngine;

// Main Menu Panel Navigator
// Contains Behavior Similar to a Book (Composed of Pages)
public class PanelNavigator : MonoBehaviour
{
    [SerializeField] private Transform pageHolder;                    // Page Container
    private PageData[] pages;                                         // Page Data Array

    // Private Variables
    private int currentPage;                                          // Current Page Number Indicator

    // Page Data Struct
    public struct PageData
    {
        public int PageNumber;
        public GameObject PageObject;
    }

    void OnEnable()
    {
        // Reset to First Page
        currentPage = 0;
        ActivatePage(currentPage);
    }

    void Awake()
    {
        // Initialize Page Holder
        pages = new PageData[pageHolder.childCount];

        for (int i = 0; i < pageHolder.childCount; i++)
        {
            pages[i].PageNumber = i;
            pages[i].PageObject = pageHolder.GetChild(i).gameObject;
        }
    }

    #region UI Button Functions
    /// <summary>
    /// Return Button
    /// </summary>
    public void OnReturnButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("main-menu-panel");
    }

    /// <summary>
    /// Previous Button
    /// </summary>
    public void OnPreviousButtonClicked()
    {
        currentPage--;

        if (currentPage < 0)
        {
            currentPage = 0;
        }

        ActivatePage(currentPage);
    }

    /// <summary>
    /// Next Button
    /// </summary>
    public void OnNextButtonClicked()
    {
        currentPage++;

        if (currentPage >= pages.Length)
        {
            currentPage = pages.Length - 1;
        }

        ActivatePage(currentPage);
    }
    #endregion

    /// <summary>
    /// Activate Page Index
    /// </summary>
    /// <param name="pageNumber"></param>
    public void ActivatePage(int pageNumber)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].PageObject.SetActive(pages[i].PageNumber == pageNumber);
        }
    }
}
