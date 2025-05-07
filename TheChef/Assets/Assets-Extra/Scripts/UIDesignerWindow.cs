using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UIDesignerWindow : EditorWindow
{
    private enum PaletteType { Latte, Frappé, Macchiato, Mocha }

    private class CatColor
    {
        public string name;
        public Color color;
        public CatColor(string name, string hex)
        {
            this.name = name;
            ColorUtility.TryParseHtmlString(hex, out color);
        }
    }

    private PaletteType selectedPalette = PaletteType.Latte;
    private List<GameObject> selectedObjects = new();
    private Dictionary<System.Type, bool> componentToggles = new()
    {
        { typeof(Graphic), true },
        { typeof(TMP_Text), true },
        { typeof(SpriteRenderer), true },
        { typeof(Renderer), false }
    };

    [MenuItem("Tools/UI Designer")]
    public static void ShowWindow()
    {
        UIDesignerWindow window = GetWindow<UIDesignerWindow>("UI Designer");
        window.autoRepaintOnSceneChange = true;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("🎨 Catppuccin UI Designer", EditorStyles.boldLabel);
        selectedPalette = (PaletteType)EditorGUILayout.EnumPopup("Palette", selectedPalette);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("🧠 Selected Objects", EditorStyles.boldLabel);

        selectedObjects = Selection.gameObjects.ToList();
        if (selectedObjects.Count == 0)
        {
            EditorGUILayout.HelpBox("Select at least one GameObject in the Scene or Hierarchy.", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField($"Selected: {selectedObjects.Count} GameObjects");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("🎯 Apply To Components", EditorStyles.boldLabel);

        bool anyValid = false;
        foreach (var key in componentToggles.Keys.ToList())
        {
            bool existsInSelection = selectedObjects.Any(go => go.GetComponent(key) != null);
            if (existsInSelection)
            {
                anyValid = true;
                componentToggles[key] = EditorGUILayout.ToggleLeft(key.Name, componentToggles[key]);
            }
        }

        if (!anyValid)
        {
            EditorGUILayout.HelpBox("None of the selected objects contain supported colorable components.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("🎨 Palette Colors", EditorStyles.boldLabel);

        var palette = GetPalette(selectedPalette);
        int columns = 4;
        int i = 0;

        while (i < palette.Count)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < columns && i < palette.Count; j++, i++)
            {
                var catColor = palette[i];
                GUI.backgroundColor = catColor.color;

                if (GUILayout.Button(catColor.name, GUILayout.Height(35)))
                {
                    ApplyColorToSelected(catColor.color);
                }

                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("🔄 Clear Selection"))
        {
            Selection.objects = new Object[0];
            selectedObjects.Clear();
        }
    }

    private void ApplyColorToSelected(Color color)
    {
        foreach (var go in selectedObjects)
        {
            if (componentToggles[typeof(Graphic)])
            {
                var graphic = go.GetComponent<Graphic>();
                if (graphic)
                {
                    Undo.RecordObject(graphic, "Apply Catppuccin Color");
                    graphic.color = color;
                    EditorUtility.SetDirty(graphic);
                }
            }

            if (componentToggles[typeof(TMP_Text)])
            {
                var tmp = go.GetComponent<TMP_Text>();
                if (tmp)
                {
                    Undo.RecordObject(tmp, "Apply Catppuccin Color");
                    tmp.color = color;
                    EditorUtility.SetDirty(tmp);
                }
            }

            if (componentToggles[typeof(SpriteRenderer)])
            {
                var sprite = go.GetComponent<SpriteRenderer>();
                if (sprite)
                {
                    Undo.RecordObject(sprite, "Apply Catppuccin Color");
                    sprite.color = color;
                    EditorUtility.SetDirty(sprite);
                }
            }

            if (componentToggles[typeof(Renderer)])
            {
                var renderer = go.GetComponent<Renderer>();
                if (renderer && renderer.sharedMaterial != null)
                {
                    Undo.RecordObject(renderer.sharedMaterial, "Apply Catppuccin Color");
                    renderer.sharedMaterial.color = color;
                    EditorUtility.SetDirty(renderer.sharedMaterial);
                }
            }
        }

        Debug.Log($"✅ Applied color to {selectedObjects.Count} object(s).");
    }

    private List<CatColor> GetPalette(PaletteType type)
{
    Dictionary<string, string> hexes = type switch
    {
        PaletteType.Latte => new Dictionary<string, string>
        {
            // Base UI
            ["Base"] = "#eff1f5", ["Mantle"] = "#e6e9ef", ["Crust"] = "#dce0e8",
            ["Surface0"] = "#ccd0da", ["Surface1"] = "#bcc0cc", ["Surface2"] = "#acb0be",
            ["Text"] = "#4c4f69", ["Subtext1"] = "#5c5f77", ["Subtext0"] = "#6c6f85",
            ["Overlay2"] = "#7c7f93", ["Overlay1"] = "#8c8fa1", ["Overlay0"] = "#9ca0b0",
            // Accents
            ["Rosewater"] = "#dc8a78", ["Flamingo"] = "#dd7878", ["Pink"] = "#ea76cb",
            ["Mauve"] = "#8839ef", ["Red"] = "#d20f39", ["Maroon"] = "#e64553",
            ["Peach"] = "#fe640b", ["Yellow"] = "#df8e1d", ["Green"] = "#40a02b",
            ["Teal"] = "#179299", ["Sky"] = "#04a5e5", ["Sapphire"] = "#209fb5",
            ["Blue"] = "#1e66f5", ["Lavender"] = "#7287fd"
        },
        PaletteType.Frappé => new Dictionary<string, string>
        {
            ["Base"] = "#303446", ["Mantle"] = "#292c3c", ["Crust"] = "#232634",
            ["Surface0"] = "#414559", ["Surface1"] = "#51576d", ["Surface2"] = "#626880",
            ["Text"] = "#c6d0f5", ["Subtext1"] = "#b5bfe2", ["Subtext0"] = "#a5adce",
            ["Overlay2"] = "#949cbb", ["Overlay1"] = "#838ba7", ["Overlay0"] = "#737994",
            ["Rosewater"] = "#f2d5cf", ["Flamingo"] = "#eebebe", ["Pink"] = "#f4b8e4",
            ["Mauve"] = "#ca9ee6", ["Red"] = "#e78284", ["Maroon"] = "#ea999c",
            ["Peach"] = "#ef9f76", ["Yellow"] = "#e5c890", ["Green"] = "#a6d189",
            ["Teal"] = "#81c8be", ["Sky"] = "#99d1db", ["Sapphire"] = "#85c1dc",
            ["Blue"] = "#8caaee", ["Lavender"] = "#babbf1"
        },
        PaletteType.Macchiato => new Dictionary<string, string>
        {
            ["Base"] = "#24273a", ["Mantle"] = "#1e2030", ["Crust"] = "#181926",
            ["Surface0"] = "#363a4f", ["Surface1"] = "#494d64", ["Surface2"] = "#5b6078",
            ["Text"] = "#cad3f5", ["Subtext1"] = "#b8c0e0", ["Subtext0"] = "#a5adcb",
            ["Overlay2"] = "#939ab7", ["Overlay1"] = "#8087a2", ["Overlay0"] = "#6e738d",
            ["Rosewater"] = "#f4dbd6", ["Flamingo"] = "#f0c6c6", ["Pink"] = "#f5bde6",
            ["Mauve"] = "#c6a0f6", ["Red"] = "#ed8796", ["Maroon"] = "#ee99a0",
            ["Peach"] = "#f5a97f", ["Yellow"] = "#eed49f", ["Green"] = "#a6da95",
            ["Teal"] = "#8bd5ca", ["Sky"] = "#91d7e3", ["Sapphire"] = "#7dc4e4",
            ["Blue"] = "#8aadf4", ["Lavender"] = "#b7bdf8"
        },
        PaletteType.Mocha => new Dictionary<string, string>
        {
            ["Base"] = "#1e1e2e", ["Mantle"] = "#181825", ["Crust"] = "#11111b",
            ["Surface0"] = "#313244", ["Surface1"] = "#45475a", ["Surface2"] = "#585b70",
            ["Text"] = "#cdd6f4", ["Subtext1"] = "#bac2de", ["Subtext0"] = "#a6adc8",
            ["Overlay2"] = "#9399b2", ["Overlay1"] = "#7f849c", ["Overlay0"] = "#6c7086",
            ["Rosewater"] = "#f5e0dc", ["Flamingo"] = "#f2cdcd", ["Pink"] = "#f5c2e7",
            ["Mauve"] = "#cba6f7", ["Red"] = "#f38ba8", ["Maroon"] = "#eba0ac",
            ["Peach"] = "#fab387", ["Yellow"] = "#f9e2af", ["Green"] = "#a6e3a1",
            ["Teal"] = "#94e2d5", ["Sky"] = "#89dceb", ["Sapphire"] = "#74c7ec",
            ["Blue"] = "#89b4fa", ["Lavender"] = "#b4befe"
        },
        _ => new Dictionary<string, string>()
    };

    List<CatColor> list = new List<CatColor>();
    foreach (var pair in hexes)
        list.Add(new CatColor(pair.Key, pair.Value));
    return list;
}
}
