using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class BlackboardView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<BlackboardView, UxmlTraits> { }

    Blackboard bBoard;
    ListView keyListView;
    EnumField keykind;
    TextField keyName;
    Button AddButton;

    public BlackboardView()
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BlackboardEditor.uss");
        styleSheets.Add(styleSheet);
    }
    public void BindElement()
    {
        keyListView = this.Q<ListView>();
        keykind = this.Q<EnumField>();
        keyName = this.Q<TextField>();
        AddButton = this.Q<Button>();
        if (AddButton == null)
        {
            Debug.Log("씨발");
        }

        AddButton.clicked += OnAddButtonClicked;
    }
    private void OnAddButtonClicked()
    {
        Debug.Log("clicked");
        BT_Key.KeyType type = (BT_Key.KeyType)keykind.value;
        CreateKey(name, type);

        Debug.Log(bBoard.bb_keys.Count);
    }
    public void Populateboard(Blackboard board)
    {
        Debug.Log("Pop");
        bBoard.bb_keys.Keys.ToList().ForEach((key) =>
        {
            object obj = bBoard.bb_keys[key];
            if(obj is bool)
            {
                CreateKeyView(key, BT_Key.KeyType.E_bool, obj);
            }
            else if (obj is GameObject)
            {
                CreateKeyView(key, BT_Key.KeyType.E_gameobject, obj);
            }
            else if (obj is Vector2)
            {
                CreateKeyView(key, BT_Key.KeyType.E_vector2, obj);
            }
            else if(obj is int)
            {
                CreateKeyView(key, BT_Key.KeyType.E_int, obj);
            }
            else if(obj is float)
            {
                CreateKeyView(key, BT_Key.KeyType.E_float, obj);
            }
        });
        BindElement();
    }
    public void CreateKeyView(string name, BT_Key.KeyType type, object val = null)
    {
        switch (type)
        {
            case BT_Key.KeyType.E_bool:
                {
                    Toggle toggleField = new Toggle(name);
                    keyListView.Add(toggleField);
                }
                break;
            case BT_Key.KeyType.E_int:
                {
                    IntegerField intField = new IntegerField(name);
                    keyListView.Add(intField);
                }
                break;
            case BT_Key.KeyType.E_float:
                {
                    FloatField floatField = new FloatField(name);
                    keyListView.Add(floatField);
                }
                break;
            case BT_Key.KeyType.E_vector2:
                {
                    Vector2Field vector2Field = new Vector2Field(name);
                    keyListView.Add(vector2Field);
                }
                break;
            case BT_Key.KeyType.E_gameobject:
                {
                    ObjectField objField = new ObjectField(name);
                    keyListView.Add(objField);
                }
                break;
        }

    }
    public void CreateKey(string name, BT_Key.KeyType type, object val = null)
    {
        bBoard.bb_keys.Add(name, val);
        CreateKeyView(name, type, val);
    }
}
