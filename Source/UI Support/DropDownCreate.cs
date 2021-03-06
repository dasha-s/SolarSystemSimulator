﻿using System; // for assert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for GUI elements: Button, Toggle

public class DropDownCreate : MonoBehaviour
{
    // reference to all UI elements in the Canvas
    public Dropdown mCreateMenu = null;
    public TheWorld TheWorld = null;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(mCreateMenu != null);
        Debug.Assert(TheWorld != null);

        // Drop down menu
        /* if we were to add options during runtime
            List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
            list.Add(new Dropdown.OptionData("Cube"));          // index = 0
            list.Add(new Dropdown.OptionData("Sphere"));        // index = 1
            list.Add(new Dropdown.OptionData("Cylinder"));      // index = 2
            mCreateMenu.AddOptions(list);
        */
        mCreateMenu.onValueChanged.AddListener(UserSelection);
    }

    String[] kLoadType = { "", "AtomRocket", "Alien" };
    void UserSelection(int index)
    {
        if (index == 0)
            return;

        mCreateMenu.value = 0; // always show the menu function: Object to create

        // inform the world of user's action
        TheWorld.ProcessUserSelection(kLoadType[index]);
    }
}
