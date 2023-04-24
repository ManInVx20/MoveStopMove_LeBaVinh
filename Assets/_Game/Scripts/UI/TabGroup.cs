using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private List<TabButton> tabButtonList;
    [SerializeField]
    private List<ShopSkinPage> skinPageLists;

    private TabButton selectedTabButton;

    private void OnEnable()
    {
        if (tabButtonList != null && tabButtonList.Count > 0)
        {
            OnTabSelected(tabButtonList[0]);
        }
    }

    public void OnTabEnter(TabButton tabButton)
    {
        //ResetTabs();
    }

    public void OnTabExit(TabButton tabButton)
    {
        //ResetTabs();
    }

    public void OnTabSelected(TabButton tabButton)
    {
        selectedTabButton = tabButton;

        ResetTabs();

        tabButton.ActiveTab();

        UpdatePages();
    }

    private void ResetTabs()
    {
        for (int i = 0; i < tabButtonList.Count; i++)
        {
            if (selectedTabButton != null && tabButtonList[i] == selectedTabButton)
            {
                continue;
            }

            tabButtonList[i].DeactiveTab();
        }
    }

    private void UpdatePages()
    {
        int selectedTabIndex = tabButtonList.IndexOf(selectedTabButton);

        for (int i = 0; i < skinPageLists.Count; i++)
        {
            if (i == selectedTabIndex)
            {
                skinPageLists[i].Show();
            }
            else
            {
                skinPageLists[i].Hide();
            }
        }
    }
}
