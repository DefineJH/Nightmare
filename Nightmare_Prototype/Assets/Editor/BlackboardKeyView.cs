using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
public class BlackboardKeyView : VisualElement
{

    Label val_text;
    Label name_text;
    Button delete_btn;

    public new class UxmlFactory : UxmlFactory<BlackboardKeyView, UxmlTraits> { }



    public void Init()
    {
        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BlackboardKeyView.uxml");
        TemplateContainer ui = uiAsset.CloneTree();

        Add(ui);

        Debug.Log(childCount);
        val_text = this.Query<Label>("type");
        name_text = this.Query<Label>("name");
        delete_btn = this.Query<Button>();

    }
    public BlackboardKeyView()
    {
    }
    public void GenerateKeyView(string val_type, string name)
    {
        Init();

        val_text.text = val_type;
        name_text.text = name;
    }
}
