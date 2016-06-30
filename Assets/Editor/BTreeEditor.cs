using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;
using BehaviourTree;
using System.Collections.Generic;
using System.Text;

public class BTreeEditor : EditorWindow 
{
    public BTreeEditor()
    {
        String strRootPath = Application.dataPath + "/Scripts/Common/BTree/";
        m_kFileList = GetAllFiles(strRootPath);
        InitTypeList();
    }
    // 创建编辑器的菜单 
    [MenuItem("FIFA Editor/行为树编辑器")]
    public static void BTTreeEditorMenu()
    {
        EditorWindow.GetWindow(typeof(BTreeEditor));
        
        //UnityEngine.Object.DontDestroyOnLoad(kWindow);
    }

    void Update()
    {
        Repaint();
    }

    // 创建窗口界面里的按钮
    void OnGUI()
    {
        GUILayout.BeginVertical();
        InitButtons();
        GUILayout.EndVertical();
        InitScrollView();
        //DoEvents();
    }

    private void InitButtons()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("新建"))
        {
            CreateBTree();
        }

        if (GUILayout.Button("加载"))
        {
            LoadBTree();
        }

        if (GUILayout.Button("保存"))
        {
            SaveBTree();
        }

        if (GUILayout.Button("另存为"))
        {
            SaveAsBTree();
        }
        GUILayout.EndHorizontal();
    }

    private void InitScrollView()
    {
        m_kScrollPos = GUI.BeginScrollView(new Rect(0, 20, position.width - 240, position.height),m_kScrollPos, new Rect(0, 0, this.maxSize.x, this.maxSize.y));
        RenderBTree();
        GUI.EndScrollView();
    }

    private void CreateBTree()
    {
        m_strFileName = "";
        m_kBTree = new BTree();
    }
    private void LoadBTree()
    {
        m_strFileName = EditorUtility.OpenFilePanel("行为树", m_strPath + "Tables/AI/", "json");
        if (String.IsNullOrEmpty(m_strFileName))
            return;
        m_kBTree = new BTree();
        m_kBTree.EditorLoad(m_strFileName);
    }
    private void SaveBTree()
    {
        if(String.IsNullOrEmpty(m_strFileName))
        {
            SaveAsBTree();
            return;
        }
        if (null == m_kBTree)
            return;
        m_kBTree.Save(m_strFileName);
    }

    private void SaveAsBTree()
    {
        String strFilePath = EditorUtility.SaveFilePanel("行为树", m_strPath + "Tables/AI/","", "json");
        if (String.IsNullOrEmpty(strFilePath))
            return;

        if (String.IsNullOrEmpty(m_strFileName))
            m_strFileName = strFilePath;
        if (null == m_kBTree)
            return;
        m_kBTree.Save(strFilePath);
    }
    
    private void RenderBTree()
    {
        if (null == m_kBTree)
            return;
        int iStartX = 10;
        int iStartY = 10;
        RenderNode(m_kBTree.Root, iStartX, ref iStartY);
    }
    private void RenderNode(BTNode kNode,int x,ref int y)
    {
        if (null == kNode)
            return;

        Event kEvt = Event.current;

        Rect kRect = new Rect(x, y, m_uiNodeWidth*2, m_uiNodeHeight);
        bool bMousePosInRect = kRect.Contains(kEvt.mousePosition);
        if (bMousePosInRect)
        {
            switch(kEvt.type)
            {
                case EventType.MouseDown:
                    if (0 == kEvt.button)
                        m_kSelectedNode = kNode;
                    break;
                case EventType.ContextClick:
                    {
                        GenericMenu kGenericMenu = new GenericMenu();
                        foreach(var kItem in m_kTypeList)
                        {
                            String strMenuName = String.Format("创建节点/{0}/",kItem.Key);
                            List<Type> kTypeList = kItem.Value;
                            foreach (var kType in kTypeList)
                            {
                                kGenericMenu.AddItem(new GUIContent(strMenuName + kType.Name), false, MenuAddCallback, kType);
                            }
                        }
                        kGenericMenu.AddItem(new GUIContent("粘贴节点"), false, MenuPasteCallback, "");
                        kGenericMenu.AddItem(new GUIContent("复制节点"), false, MenuCopyCallback, "");
                        kGenericMenu.AddItem(new GUIContent("删除节点"), false, MenuDelCallback, "");
                        kGenericMenu.ShowAsContext();
                    }
                    break;
            }

        }
        GUIStyle kGUIStyle = new GUIStyle();
        Vector2 kSize = kGUIStyle.CalcSize(new GUIContent(kNode.Name));
        kSize.x += 10;

        if(null != m_kSelectedNode && m_kSelectedNode == kNode)
        {
            // 高亮显示选中的节点
            Texture2D kTex = new Texture2D(1, 1);
            kTex.SetPixel(0, 0, Color.blue);
            kTex.Apply();
            GUI.DrawTexture(new Rect(x, y, kSize.x + m_uiNodeWidth * 2, m_uiNodeHeight), kTex);
        }

        GUILayout.BeginHorizontal();
        EditorGUI.LabelField(new Rect(x, y, kSize.x, m_uiNodeHeight), kNode.Name);
        kNode.DisplayName = EditorGUI.TextField(new Rect(x + kSize.x, y, m_uiNodeWidth * 2, m_uiNodeHeight), kNode.DisplayName);
        GUILayout.EndHorizontal();
        Handles.color = Color.red;
        Vector3 kPos1 = new Vector3(x + (int)m_uiNodeWidth / 8, y + m_uiNodeHeight, 0);
        for(int iIdx = 0;iIdx < kNode.Children.Count;iIdx++)
        {
            //有孩子节点才画线
            BTNode kChildNode = kNode.Children[iIdx];
            y = y + (int)(m_uiNodeHeight) + 2;
            Vector3 kPos2 = new Vector3(x + (int)m_uiNodeWidth / 8, y + m_uiNodeHeight / 2, 0);
            Vector3 kPos3 = new Vector3(x + (int)m_uiNodeWidth / 4, y + m_uiNodeHeight / 2,0);
            Handles.DrawPolyLine(new Vector3[] { kPos1, kPos2, kPos3 });
            RenderNode(kChildNode, x + (int)m_uiNodeWidth / 4, ref y);

        }
    }

    public void MenuAddCallback(object kArg)
    {
        if (null == m_kSelectedNode)
            return;
        Type kType = kArg as Type;
        BTNode kNode = Activator.CreateInstance(kType) as BTNode;
        m_kSelectedNode.AddChild(kNode);
        Repaint();
    }

    public void MenuDelCallback(object kArg)
    {
        if (null == m_kSelectedNode)
            return;
        BTNode kParent = m_kSelectedNode.Parent;
        if (null == kParent)
            return;
        kParent.RemoveChild(m_kSelectedNode);
        m_kSelectedNode = null;
        m_kCopiedNode = null;
        Repaint();
    }

    public void MenuCopyCallback(object kArg)
    {
        if (null == m_kSelectedNode)
            return;

        m_kCopiedNode = m_kSelectedNode;
    }

    public void MenuPasteCallback(object kArg)
    {
        if (null == m_kCopiedNode || m_kSelectedNode == m_kCopiedNode)
            return;

        m_kSelectedNode.AddChild(m_kCopiedNode);
        Repaint();
    }
    

    private void InitTypeList()
    {
        // Composite
        List<Type> kTypeList = new List<Type>();
        Type kType = typeof(BTComposite);
        m_kTypeList.Add("组合节点", kTypeList);
        InitTypeClass(kType, kTypeList);
        kType = typeof(BTConditional);
        kTypeList = new List<Type>();
        m_kTypeList.Add("条件节点", kTypeList);
        InitTypeClass(kType, kTypeList);
        kType = typeof(BTAction);
        kTypeList = new List<Type>();
        m_kTypeList.Add("行为节点", kTypeList);
        InitTypeClass(kType, kTypeList);
    }

    private void InitTypeClass(Type kClassType,List<Type> kTypeList)
    {
        foreach (String fileName in m_kFileList)
        {
            String strClass = fileName.Split('.')[0];
            Type kType = ClassFactory.Instance.GetType("BehaviourTree." + strClass);
            if (null == kType)
                continue;
            if(kType.IsSubclassOf(kClassType))
            {
                kTypeList.Add(kType);
            }
        }
    }

    private List<String> GetAllFiles(String strPath)
    {
        List<String> kFileList = new List<String>();
        DirectoryInfo kDirInfo = new DirectoryInfo(strPath);
        foreach (FileInfo kFile in kDirInfo.GetFiles("*.cs"))
        {
            kFileList.Add(kFile.Name);
        }

        String[] kDirs = Directory.GetDirectories(strPath);
        for(int iIdx = 0;iIdx < kDirs.Length;iIdx++)
        {
            String strDir = kDirs[iIdx];
            kFileList.AddRange(GetAllFiles(strDir));
        }

        return kFileList;
    }

    private Dictionary<String, List<Type>> m_kTypeList = new Dictionary<String, List<Type>>();
    private String m_strFileName;
    private String m_strPath = Application.dataPath + "/Resources/";
    private BTree m_kBTree = null;
    private UInt32 m_uiNodeHeight = 20;
    private UInt32 m_uiNodeWidth = 100;
    private List<String> m_kFileList = new List<String>();
    private BTNode m_kSelectedNode = null;
    private BTNode m_kCopiedNode = null;
    private Vector2 m_kScrollPos = new Vector2(0, 0);
}
