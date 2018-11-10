using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Sauce3D
{
    public class SauceCreditsTool : EditorWindow
    {
        #region Variables
        //GUI variables.
        private Texture2D logo;
        private GUISkin skin;
        private const string root = "Assets/ImportedFromAssetStore/3DSauce/3DSTools/Editor/";
        private const string logoPath = root + "Skin/Images/logo.png";
        private const string skinPath = root + "Skin/skin.guiskin";
        private const string darkBackground = root + "Skin/Images/BackgroundPro.png";
        public Vector2 scrollPosition;

        private const string entryField = root + "Skin/Images/EntryField.png";
        private const string entryFieldFocus = root + "Skin/Images/EntryFieldFocus.png";
        private const string dropdownPopup = root + "Skin/Images/DropdownPopup.png";
        private const string dropdownPopupFocus = root + "Skin/Images/DropdownPopupFocus.png";
        private const string logoIcon = root + "Skin/Images/favicon.png";
        private const string dividerImage = root + "Skin/Images/boxDivider.png";
        private const string buttonPress = root + "Skin/Images/buttonPressed.png";
        private const string buttonRelease = root + "Skin/Images/buttonReleased.png";
        private const string starsPath = root + "Skin/Images/stars.png";
        public Font fontHeader;

        //Object variables.
        public TextAsset textFileCredits;
        public GameObject scrollViewContent;

        //Tool variables.
        public string[] optionsItemType = new string[] { "Title", "Name", "Info" };
        public int indexItemType;
        public string[] optionsColorItem = new string[] { "Color Titles", "Color Names", "Color Info" };
        public int indexColorItem;
        private string itemName = null;
        private string urlAddress = "http://3dsauce.com/";
        private string urlButtonText;
        private string tempItemName;
        private string tempURL;
        private string tempButtonText;
        private Sprite imageElement;
        private Color itemColor = new Color(0.5f, 0.5f, 0.5f, 1);
        private Font fontElement;
        #endregion

        void Awake()
        {
            logo = AssetDatabase.LoadAssetAtPath(logoPath, typeof(Texture2D)) as Texture2D;
            skin = AssetDatabase.LoadAssetAtPath(skinPath, typeof(GUISkin)) as GUISkin;

            //Gets the header font.
            foreach (string guid in AssetDatabase.FindAssets("Ubuntu-B"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                fontHeader = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Font)) as Font;
            }
        }
        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/3D Sauce Credits Tool")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            SauceCreditsTool window = (SauceCreditsTool)EditorWindow.GetWindow(typeof(SauceCreditsTool));
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            window.title = "3D Sauce";
            window.minSize = new Vector2(220, 250);
            window.Show();
#else
            //New Unity 5.3+ code for icon beside title.
            Texture icon = AssetDatabase.LoadAssetAtPath(logoIcon, typeof(Texture2D)) as Texture2D;
            GUIContent titleContent = new GUIContent("3DSauce", icon);
            window.titleContent = titleContent;
            window.minSize = new Vector2(220, 250);
            window.Show();
#endif
        }

        void OnGUI()
        {
            #region Enter Pressed
            //Listen for Enter press from keyboard during item name entry.
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                if (GUI.GetNameOfFocusedControl() == "namingField")
                {
                    if (scrollViewContent != null)
                    {
                        if (tempItemName != null)
                        {
                            tempItemName = null;
                            Repaint();
                            EditorGUI.FocusTextInControl("namingField");
                            PlaceCreditItem();
                        }
                    }
                    else
                    {
                        //EditorGUI.FocusTextInControl("contentField");
                        Debug.Log("Must assign the scroll view content before placing elements.");
                    }
                }
                else if (GUI.GetNameOfFocusedControl() == "urlField" || GUI.GetNameOfFocusedControl() == "buttonTextField")
                {
                    if (scrollViewContent != null)
                    {
                        if (tempURL == "")
                            tempURL = null;
                        if (tempButtonText == "")
                            tempButtonText = null;

                        if (tempURL != null && tempButtonText != null)
                        {
                            tempButtonText = null;
                            tempURL = null;
                            GUI.FocusControl("Clear");
                            Repaint();
                            PlaceURLButton();
                        }
                        else if (tempURL != null && tempButtonText == null)
                        {
                            EditorGUI.FocusTextInControl("buttonTextField");
                            Repaint();
                            Debug.Log("Must fill in Button text field.");
                        }
                        else if (tempURL == null && tempButtonText != null)
                        {
                            EditorGUI.FocusTextInControl("urlField");
                            Repaint();
                            Debug.Log("Must fill in URL text field.");
                        }
                        else
                        {
                            Debug.Log("Must fill in URL and Button text fields.");
                        }
                    }
                    else
                    {
                        //EditorGUI.FocusTextInControl("contentField");
                        Debug.Log("Must assign the scroll view content before placing elements.");
                    }
                }
            }
            #endregion

            //Begin area and then begin the scrollview.
            GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

            //Box style for entire tool area.
            GUIStyle boxStyle = new GUIStyle();
            //boxStyle.normal.background = (Texture2D)Resources.Load("Textures/default_box_bgPro");
            boxStyle.normal.background = AssetDatabase.LoadAssetAtPath(darkBackground, typeof(Texture2D)) as Texture2D;
            GUILayout.BeginVertical(boxStyle);

            //Define style variable for header labels.
            GUIStyle labelStyleHeader = new GUIStyle();
            //Define style variable for tiny labels.
            GUIStyle labelStyle = new GUIStyle();
            //Define style variable for buttons.
            GUIStyle buttonStyle = new GUIStyle();
            //Define style variable for section dividers.
            GUIStyle dividerStyle = new GUIStyle();

            //Define style variable for text fields.
            GUIStyle textFieldStyle = new GUIStyle();
            textFieldStyle.normal.background = AssetDatabase.LoadAssetAtPath(entryField, typeof(Texture2D)) as Texture2D;
            textFieldStyle.normal.textColor = new Color(195 / 255F, 195 / 255F, 195 / 255F, 1);
            textFieldStyle.margin = new RectOffset(4, 4, 2, 2);
            textFieldStyle.padding = new RectOffset(3, 3, 1, 2);
            textFieldStyle.border = new RectOffset(3, 3, 3, 3);
            textFieldStyle.focused.background = AssetDatabase.LoadAssetAtPath(entryFieldFocus, typeof(Texture2D)) as Texture2D;
            textFieldStyle.focused.textColor = new Color(195 / 255F, 195 / 255F, 195 / 255F, 1);

            //Define style variable for Popups (dropdowns)
            GUIStyle popupStyle = new GUIStyle();
            popupStyle.normal.background = AssetDatabase.LoadAssetAtPath(dropdownPopup, typeof(Texture2D)) as Texture2D;
            popupStyle.normal.textColor = new Color(195 / 255F, 195 / 255F, 195 / 255F, 1);
            popupStyle.margin = new RectOffset(4, 4, 2, 2);
            popupStyle.padding = new RectOffset(4, 3, 1, 2);
            popupStyle.border = new RectOffset(3, 12, 3, 3);
            popupStyle.focused.background = AssetDatabase.LoadAssetAtPath(dropdownPopupFocus, typeof(Texture2D)) as Texture2D;
            popupStyle.focused.textColor = new Color(195 / 255F, 195 / 255F, 195 / 255F, 1);

            labelStyle.wordWrap = true;

            //Define style for header labels.
            labelStyleHeader.fontSize = 14;
            labelStyleHeader.normal.textColor = new Color(195 / 255F, 173 / 255F, 100 / 255F, 1);
            labelStyleHeader.font = fontHeader;
            labelStyleHeader.margin.left = 4;
            labelStyleHeader.margin.right = 4;
            labelStyleHeader.margin.top = 6;
            labelStyleHeader.margin.bottom = 6;
            labelStyleHeader.padding.left = 2;
            labelStyleHeader.padding.right = 2;
            labelStyleHeader.padding.top = 1;
            labelStyleHeader.padding.bottom = 2;
            labelStyleHeader.alignment = TextAnchor.MiddleLeft;

            //Define style for tiny labels.
            labelStyle.fontSize = 10;
            labelStyle.normal.textColor = new Color(195 / 255F, 195 / 255F, 195 / 255F, 1);
            labelStyle.contentOffset = new Vector2(6, 10);

            //Define style for buttons.
            buttonStyle.normal.background = AssetDatabase.LoadAssetAtPath(buttonRelease, typeof(Texture2D)) as Texture2D;
            buttonStyle.normal.textColor = new Color(195 / 255F, 195 / 255F, 195 / 255F, 1);
            buttonStyle.active.background = AssetDatabase.LoadAssetAtPath(buttonPress, typeof(Texture2D)) as Texture2D;
            buttonStyle.active.textColor = new Color(184 / 255F, 221 / 255F, 172 / 255F, 1);
            buttonStyle.margin.left = 4;
            buttonStyle.margin.right = 4;
            buttonStyle.margin.top = 8;
            buttonStyle.font = fontHeader;
            buttonStyle.fontSize = 14;
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            //Define style for dividers.
            dividerStyle.normal.background = AssetDatabase.LoadAssetAtPath(dividerImage, typeof(Texture2D)) as Texture2D;

            if (!Application.HasProLicense()) GUI.backgroundColor = new Color(0.32f, 0.32f, 0.32f);
            if (GUILayout.Button(logo))
            {
                GUI.FocusControl("Clear");
                //This will occur when the 3D Sauce benner is pressed...
                Debug.Log("Visit 3DSauce.com!");
                //Application.OpenURL("http://3dsauce.com/");
            }
            GUI.backgroundColor = Color.white;

            if (skin != null) GUI.skin = skin;

            #region BoxSeperators
            //All box seperators are layed out ahead of time below. This allows them to stretch with the horizontal width.
            GUILayout.BeginArea(new Rect(4, 56, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 172, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 322, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 408, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 548, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 640, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 750, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 852, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 964, position.width - 8, 100));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            GUILayout.EndArea();
            #endregion

            //Set all labels font size.
            GUI.skin.label.fontSize = 14;

            #region Edit Credits Tools
            //Credit controls.
            GUILayout.BeginArea(new Rect(4, 74, 195, 200));
            GUILayout.BeginVertical();

            //Content components setup.
            GUILayout.Label("ScrollView Content:");
            GameObject tempHas = scrollViewContent;
            GUI.SetNextControlName("contentField");
            if (!Application.HasProLicense()) GUI.backgroundColor = new Color(0.753f, 0.753f, 0.753f);
            scrollViewContent = EditorGUILayout.ObjectField(scrollViewContent, typeof(GameObject), true) as GameObject;
            GUI.backgroundColor = Color.white;
            if (scrollViewContent != tempHas)
            {
                if (scrollViewContent != null)
                {
                    VerticalLayoutGroup componentA = scrollViewContent.GetComponent<VerticalLayoutGroup>();
                    ContentSizeFitter componentB = scrollViewContent.GetComponent<ContentSizeFitter>();
                    if (componentA == null || componentB == null)
                    {
                        if (componentA == null)
                        {
                            VerticalLayoutGroup vlg = scrollViewContent.AddComponent<VerticalLayoutGroup>();
                            vlg.spacing = 4;
                            vlg.childAlignment = TextAnchor.UpperCenter;
                            vlg.childForceExpandHeight = false;
                            vlg.childForceExpandWidth = false;
                        }
                        if (componentB == null)
                        {
                            ContentSizeFitter csf = scrollViewContent.AddComponent<ContentSizeFitter>();
                            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                        }
                    }
                }
            }

            //Add auto scroll script.
            if (GUILayout.Button("Attach Auto-Scroll", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                if (scrollViewContent != null)
                {
                    CreditScroller componentAutoCredits = scrollViewContent.GetComponent<CreditScroller>();
                    if (componentAutoCredits == null)
                    {
                        componentAutoCredits = scrollViewContent.AddComponent<CreditScroller>();
                        componentAutoCredits.uguiContent = scrollViewContent;
                    }

                    //Automagically adds gameobjects to script if they are found.
                    GameObject canvasRoot = scrollViewContent.transform.root.gameObject;
                    Transform[] children = canvasRoot.GetComponentsInChildren<Transform>();
                    foreach (var child in children)
                    {
                        if (child.name == "UICam")
                            componentAutoCredits.uguiCamera = child.gameObject.GetComponent<Camera>();
                        if (child.name == "ScrollView_CreditsPro")
                            componentAutoCredits.uguiScrollView = child.gameObject;
                        if (child.name == "ScrollRect")
                            componentAutoCredits.uguiScrollRect = child.gameObject;
                        if (child.name == "ScrollbarVertical")
                            componentAutoCredits.uguiScrollbar = child.gameObject;
                    }
                }
                else
                {
                    EditorGUI.FocusTextInControl("contentField");
                    Debug.Log("Must assign the scroll view content before attaching script.");
                }
            }

            GUILayout.Space(28);
            GUILayout.Label("Text Element:");

            //Item type dropdown.
            indexItemType = EditorGUILayout.Popup(indexItemType, optionsItemType, popupStyle);

            GUILayout.Label("Field Name:");

            //Item name text field.
            tempItemName = itemName;
            GUI.SetNextControlName("namingField");
            itemName = EditorGUILayout.TextField(itemName, textFieldStyle);

            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 284, 195, 200));
            GUILayout.BeginVertical();

            //Place item button.
            if (GUILayout.Button("Insert Item", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                if (tempItemName != null)
                {
                    tempItemName = null;
                    GUI.FocusControl("Clear");
                    PlaceCreditItem();
                }
                else
                {
                    Debug.Log("Must fill in name field.");
                }
            }

            GUILayout.Space(10);

            //Place line break button.
            if (GUILayout.Button("Line Break", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                GUI.FocusControl("Clear");
                PlaceBlankItem();
            }

            //Place Divider button.
            if (GUILayout.Button("Divider", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                GUI.FocusControl("Clear");
                PlaceDividerItem();
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(4, 414, 195, 200));
            GUILayout.BeginVertical();

            GUILayout.Label("Internet Address:");

            //URL text field.
            tempURL = urlAddress;
            GUI.SetNextControlName("urlField");
            urlAddress = EditorGUILayout.TextField(urlAddress, textFieldStyle);

            GUILayout.Label("Button Text:");

            //Button text field.
            tempButtonText = urlButtonText;
            GUI.SetNextControlName("buttonTextField");
            urlButtonText = EditorGUILayout.TextField(urlButtonText, textFieldStyle);

            //Place URL Options.
            if (GUILayout.Button("URL Button", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                if (scrollViewContent != null)
                {
                    if (tempURL == "")
                        tempURL = null;
                    if (tempButtonText == "")
                        tempButtonText = null;

                    if (tempURL != null && tempButtonText != null)
                    {
                        tempButtonText = null;
                        tempURL = null;
                        GUI.FocusControl("Clear");
                        Repaint();
                        PlaceURLButton();
                    }
                    else if (tempURL != null && tempButtonText == null)
                    {
                        EditorGUI.FocusTextInControl("buttonTextField");
                        Repaint();
                        Debug.Log("Must fill in Button text field.");
                    }
                    else if (tempURL == null && tempButtonText != null)
                    {
                        EditorGUI.FocusTextInControl("urlField");
                        Repaint();
                        Debug.Log("Must fill in URL text field.");
                    }
                    else
                    {
                        Debug.Log("Must fill in URL and Button text fields.");
                    }
                }
                else
                {
                    EditorGUI.FocusTextInControl("contentField");
                    Debug.Log("Must assign the scroll view content before placing elements.");
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();

            //Insert image controls.
            GUILayout.BeginArea(new Rect(4, 554, 195, 200));
            GUILayout.BeginVertical();

            GUILayout.Label("Source Image:");
            GUI.SetNextControlName("spriteField");
            if (!Application.HasProLicense()) GUI.backgroundColor = new Color(0.753f, 0.753f, 0.753f);
            imageElement = EditorGUILayout.ObjectField(imageElement, typeof(Sprite), true) as Sprite;
            GUI.backgroundColor = Color.white;

            if (GUILayout.Button("Insert Image", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                if (imageElement != null)
                {
                    GUI.FocusControl("Clear");
                    PlaceImage();
                }
                else
                {
                    EditorGUI.FocusTextInControl("spriteField");
                    Debug.Log("Must define sprite first.");
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();

            //Color customization.
            GUILayout.BeginArea(new Rect(4, 646, 195, 200));
            GUILayout.BeginVertical();

            GUILayout.Label("Mass Color:");
            indexColorItem = EditorGUILayout.Popup(indexColorItem, optionsColorItem, popupStyle);
            itemColor = EditorGUILayout.ColorField(itemColor);
            if (GUILayout.Button("Colorize", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                ColorizeItems();
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();

            //Font switching.
            GUILayout.BeginArea(new Rect(4, 756, 195, 200));
            GUILayout.BeginVertical();

            GUILayout.Label("Font:");
            GUI.SetNextControlName("fontField");
            if (!Application.HasProLicense()) GUI.backgroundColor = new Color(0.753f, 0.753f, 0.753f);
            fontElement = EditorGUILayout.ObjectField(fontElement, typeof(Font), true) as Font;
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Change Font", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                if (fontElement != null)
                {
                    GUI.FocusControl("Clear");
                    ChangeFontAll();
                }
                else
                {
                    EditorGUI.FocusTextInControl("fontField");
                    Debug.Log("Must choose desired font.");
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();

            #endregion

            #region Text File Loader
            //Load text file interface.
            GUILayout.BeginArea(new Rect(4, 868, 195, 100));
            GUILayout.BeginVertical();

            GUILayout.Label("Import Text File:");
            GUI.SetNextControlName("textFileField");
            if (!Application.HasProLicense()) GUI.backgroundColor = new Color(0.753f, 0.753f, 0.753f);
            textFileCredits = EditorGUILayout.ObjectField(textFileCredits, typeof(TextAsset), true) as TextAsset;
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Import", buttonStyle, GUILayout.MinWidth(55), GUILayout.MinHeight(30)))
            {
                GUI.FocusControl("Clear");
                ProcessTextFile();
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
            #endregion

            //Seperates the previous element from the next. Also helps with scrollbar appearance.
            GUILayout.Space(920);

            //End scrollview and then end area.
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void ProcessTextFile()
        {
            if (textFileCredits == null)
            {
                EditorGUI.FocusTextInControl("textFileField");
                Debug.Log("You haven't chosen a text file.");
            }
            if (scrollViewContent == null)
            {
                EditorGUI.FocusTextInControl("contentField");
                Debug.Log("You haven't specidifed the scroll view content object.");
            }
            else if (textFileCredits != null && scrollViewContent != null)
            {
                var textArray = textFileCredits.text.Split('\n');
                foreach (string credit in textArray)
                {
                    if (credit.Contains("<t>"))
                    {
                        GameObject creditItemTitle = Instantiate(Resources.Load("Prefabs/Elements/Credits_Title", typeof(GameObject))) as GameObject;
                        Undo.RegisterCreatedObjectUndo(creditItemTitle, "Created Credits");
                        creditItemTitle.transform.SetParent(scrollViewContent.transform);
                        creditItemTitle.transform.localScale = new Vector3(1, 1, 1);
                        Vector3 localPosTitle = creditItemTitle.transform.localPosition;
                        localPosTitle.z = 0;
                        creditItemTitle.transform.localPosition = localPosTitle;
                        itemName = credit.Replace("<t>", "");
                        creditItemTitle.name = "<T> " + itemName;
                        Text textComponentTitle = creditItemTitle.GetComponent<Text>();
                        textComponentTitle.text = itemName;
                    }
                    else if (credit.Contains("<n>"))
                    {
                        GameObject creditItemName = Instantiate(Resources.Load("Prefabs/Elements/Credits_Name", typeof(GameObject))) as GameObject;
                        Undo.RegisterCreatedObjectUndo(creditItemName, "Created Credits");
                        creditItemName.transform.SetParent(scrollViewContent.transform);
                        creditItemName.transform.localScale = new Vector3(1, 1, 1);
                        Vector3 localPosName = creditItemName.transform.localPosition;
                        localPosName.z = 0;
                        creditItemName.transform.localPosition = localPosName;
                        itemName = credit.Replace("<n>", "");
                        creditItemName.name = "<N> " + itemName;
                        Text textComponentName = creditItemName.GetComponent<Text>();
                        textComponentName.text = itemName;
                    }
                    else if (credit.Contains("<br>"))
                    {
                        GameObject creditItemSpacer = Instantiate(Resources.Load("Prefabs/Elements/Credits_Spacer", typeof(GameObject))) as GameObject;
                        Undo.RegisterCreatedObjectUndo(creditItemSpacer, "Created Credits");
                        creditItemSpacer.transform.SetParent(scrollViewContent.transform);
                        creditItemSpacer.transform.localScale = new Vector3(1, 1, 1);
                        Vector3 localPosSpacer = creditItemSpacer.transform.localPosition;
                        localPosSpacer.z = 0;
                        creditItemSpacer.transform.localPosition = localPosSpacer;
                        creditItemSpacer.name = "<br>";
                    }
                    else if (credit.Contains("<i>"))
                    {
                        GameObject creditItemDescription = Instantiate(Resources.Load("Prefabs/Elements/Credits_Information", typeof(GameObject))) as GameObject;
                        Undo.RegisterCreatedObjectUndo(creditItemDescription, "Created Credits");
                        creditItemDescription.transform.SetParent(scrollViewContent.transform);
                        creditItemDescription.transform.localScale = new Vector3(1, 1, 1);
                        Vector3 localPosDescription = creditItemDescription.transform.localPosition;
                        localPosDescription.z = 0;
                        creditItemDescription.transform.localPosition = localPosDescription;
                        itemName = credit.Replace("<i>", "");
                        Text textComponentDescription = creditItemDescription.GetComponent<Text>();
                        textComponentDescription.text = itemName;
                        if (itemName.Length >= 10)
                            itemName = itemName.Substring(0, 10) + "...";
                        creditItemDescription.name = "<I> " + itemName;
                    }
                    else if (credit.Contains("<divider>"))
                    {
                        GameObject creditItemSeparator = Instantiate(Resources.Load("Prefabs/Elements/Credits_Divider", typeof(GameObject))) as GameObject;
                        Undo.RegisterCreatedObjectUndo(creditItemSeparator, "Created Credits");
                        creditItemSeparator.transform.SetParent(scrollViewContent.transform);
                        creditItemSeparator.transform.localScale = new Vector3(1, 1, 1);
                        Vector3 localPosSeparator = creditItemSeparator.transform.localPosition;
                        localPosSeparator.z = 0;
                        creditItemSeparator.transform.localPosition = localPosSeparator;
                        creditItemSeparator.name = "<divider>";
                    }
                }
                itemName = null;
            }
        }
        private void PlaceCreditItem()
        {
            if (scrollViewContent != null)
            {
                //Places a menu prefab item depending on selection type.
                switch (indexItemType)
                {
                    case 0:
                        GameObject creditItemTitle = Instantiate(Resources.Load("Prefabs/Elements/Credits_Title", typeof(GameObject))) as GameObject;
                        Undo.RegisterCreatedObjectUndo(creditItemTitle, "Created Credits");
                        creditItemTitle.transform.SetParent(scrollViewContent.transform);
                        creditItemTitle.transform.localScale = new Vector3(1, 1, 1);
                        Vector3 localPosTitle = creditItemTitle.transform.localPosition;
                        localPosTitle.z = 0;
                        creditItemTitle.transform.localPosition = localPosTitle;
                        creditItemTitle.name = "<T> " + itemName;
                        Text textComponentTitle = creditItemTitle.GetComponent<Text>();
                        textComponentTitle.text = itemName;
                        Selection.activeGameObject = creditItemTitle;
                        itemName = null;
                        break;
                    case 1:
                        GameObject creditItemName = Instantiate(Resources.Load("Prefabs/Elements/Credits_Name", typeof(GameObject))) as GameObject;
                        Undo.RegisterCreatedObjectUndo(creditItemName, "Created Credits");
                        creditItemName.transform.SetParent(scrollViewContent.transform);
                        creditItemName.transform.localScale = new Vector3(1, 1, 1);
                        Vector3 localPosName = creditItemName.transform.localPosition;
                        localPosName.z = 0;
                        creditItemName.transform.localPosition = localPosName;
                        creditItemName.name = "<N> " + itemName;
                        Text textComponentName = creditItemName.GetComponent<Text>();
                        textComponentName.text = itemName;
                        Selection.activeGameObject = creditItemName;
                        itemName = null;
                        break;
                    case 2:
                        GameObject creditItemDescription = Instantiate(Resources.Load("Prefabs/Elements/Credits_Information", typeof(GameObject))) as GameObject;
                        Undo.RegisterCreatedObjectUndo(creditItemDescription, "Created Credits");
                        creditItemDescription.transform.SetParent(scrollViewContent.transform);
                        creditItemDescription.transform.localScale = new Vector3(1, 1, 1);
                        Vector3 localPosDescription = creditItemDescription.transform.localPosition;
                        localPosDescription.z = 0;
                        creditItemDescription.transform.localPosition = localPosDescription;
                        Text textComponentDescription = creditItemDescription.GetComponent<Text>();
                        textComponentDescription.text = itemName;
                        if (itemName.Length >= 10)
                            itemName = itemName.Substring(0, 10) + "...";
                        creditItemDescription.name = "<I> " + itemName;
                        Selection.activeGameObject = creditItemDescription;
                        itemName = null;
                        break;
                }
            }
            else
            {
                EditorGUI.FocusTextInControl("contentField");
                Debug.Log("Must assign the scroll view content before placing elements.");
            }
        }
        private void PlaceBlankItem()
        {
            if (scrollViewContent != null)
            {
                GameObject creditItemSpacer = Instantiate(Resources.Load("Prefabs/Elements/Credits_Spacer", typeof(GameObject))) as GameObject;
                Undo.RegisterCreatedObjectUndo(creditItemSpacer, "Created Credits");
                creditItemSpacer.transform.SetParent(scrollViewContent.transform);
                creditItemSpacer.transform.localScale = new Vector3(1, 1, 1);
                Vector3 localPosSpacer = creditItemSpacer.transform.localPosition;
                localPosSpacer.z = 0;
                creditItemSpacer.transform.localPosition = localPosSpacer;
                creditItemSpacer.name = "<br>";
                Selection.activeGameObject = creditItemSpacer;
            }
            else
            {
                EditorGUI.FocusTextInControl("contentField");
                Debug.Log("Must assign the scroll view content before placing elements.");
            }
        }

        private void PlaceDividerItem()
        {
            if (scrollViewContent != null)
            {
                GameObject creditItemSeparator = Instantiate(Resources.Load("Prefabs/Elements/Credits_Divider", typeof(GameObject))) as GameObject;
                Undo.RegisterCreatedObjectUndo(creditItemSeparator, "Created Credits");
                creditItemSeparator.transform.SetParent(scrollViewContent.transform);
                creditItemSeparator.transform.localScale = new Vector3(1, 1, 1);
                Vector3 localPosSeparator = creditItemSeparator.transform.localPosition;
                localPosSeparator.z = 0;
                creditItemSeparator.transform.localPosition = localPosSeparator;
                creditItemSeparator.name = "<divider>";
                Selection.activeGameObject = creditItemSeparator;
            }
            else
            {
                EditorGUI.FocusTextInControl("contentField");
                Debug.Log("Must assign the scroll view content before placing elements.");
            }
        }

        private void PlaceURLButton()
        {
            GameObject urlButton = Instantiate(Resources.Load("Prefabs/Elements/Button_URL", typeof(GameObject))) as GameObject;
            Undo.RegisterCreatedObjectUndo(urlButton, "Created Credits");
            urlButton.transform.SetParent(scrollViewContent.transform);
            urlButton.transform.localScale = new Vector3(1, 1, 1);
            Vector3 localButtonSpacer = urlButton.transform.localPosition;
            localButtonSpacer.z = 0;
            urlButton.transform.localPosition = localButtonSpacer;
            urlButton.name = "<URL> " + urlAddress;

            //Sets button text.
            Text textCompUrlButton = urlButton.GetComponentInChildren<Text>();
            textCompUrlButton.text = urlButtonText;

            //Sets URL in script component.
            Sauce3D.ButtonOpenURL OpenURLScript = urlButton.GetComponent<ButtonOpenURL>();
            OpenURLScript.url = urlAddress;

            Selection.activeGameObject = urlButton;

            urlAddress = null;
            urlButtonText = null;
        }

        private void PlaceImage()
        {
            if (scrollViewContent != null)
            {
                GameObject creditsImage = Instantiate(Resources.Load("Prefabs/Elements/Credits_Image", typeof(GameObject))) as GameObject;
                Undo.RegisterCreatedObjectUndo(creditsImage, "Created Credits");
                creditsImage.transform.SetParent(scrollViewContent.transform);
                creditsImage.transform.localScale = new Vector3(1, 1, 1);
                Vector3 localImageSpacer = creditsImage.transform.localPosition;
                localImageSpacer.z = 0;
                creditsImage.transform.localPosition = localImageSpacer;
                creditsImage.name = "<IMG> " + imageElement.name;
                Image imgComponent = creditsImage.GetComponent<Image>();
                imgComponent.sprite = imageElement;
                Selection.activeGameObject = creditsImage;
            }
            else
            {
                EditorGUI.FocusTextInControl("contentField");
                Debug.Log("Must assign the scroll view content before placing elements.");
            }
        }

        private void ColorizeItems()
        {
            if (scrollViewContent != null)
            {
                //Gets list of all children in the scroll view content.
                var creditsList = new List<GameObject>();
                foreach (Transform child in scrollViewContent.transform)
                {
                    creditsList.Add(child.gameObject);
                }

                //Colors credit items depending on selection type.
                switch (indexColorItem)
                {
                    case 0:
                        foreach (GameObject element in creditsList)
                        {
                            var goName = element.name;
                            if (goName.Contains("<T>"))
                            {
                                Text textComponent = element.GetComponent<Text>();
                                textComponent.color = itemColor;
                            }
                        }
                        break;
                    case 1:
                        foreach (GameObject element in creditsList)
                        {
                            var goName = element.name;
                            if (goName.Contains("<N>"))
                            {
                                Text textComponent = element.GetComponent<Text>();
                                textComponent.color = itemColor;
                            }
                        }
                        break;
                    case 2:
                        foreach (GameObject element in creditsList)
                        {
                            var goName = element.name;
                            if (goName.Contains("<I>"))
                            {
                                Text textComponent = element.GetComponent<Text>();
                                textComponent.color = itemColor;
                            }
                        }
                        break;
                }
                //This simply refreshes the scene and colors by creating and destroying an empty game object.
                var go = new GameObject();
                DestroyImmediate(go);
            }
            else
            {
                EditorGUI.FocusTextInControl("contentField");
                Debug.Log("Must assign the scroll view content before colorizing elements.");
            }
        }

        private void ChangeFontAll()
        {
            if (scrollViewContent != null)
            {
                //Gets list of all children in the scroll view content.
                foreach (Transform child in scrollViewContent.transform)
                {
                    if (child.GetComponent<Text>() != null)
                    {
                        Text textComponent = child.GetComponent<Text>();
                        textComponent.font = fontElement;
                    }
                }
                //This simply refreshes the scene and colors by creating and destroying an empty game object.
                var go = new GameObject();
                DestroyImmediate(go);
            }
            else
            {
                EditorGUI.FocusTextInControl("contentField");
                Debug.Log("Must assign the scroll view content before changing font.");
            }
        }
    }
}