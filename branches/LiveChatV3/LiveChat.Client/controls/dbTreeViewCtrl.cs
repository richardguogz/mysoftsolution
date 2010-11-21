// This is the C# implementation of Data Binding TreeView Control converted and modified 
// from VB .NET by LZF of www.codeproject.com. The original code and article, 
// writen by Duncan Mackenzie of MSDN, can be found at
// "http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwinforms/html/custcntrlsamp3.asp"
//
// Following lines are the copyright from the original code

/* 
 * Copyright (C) 2002 Microsoft Corporation
 * All rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER
 * EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF
 * MERCHANTIBILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 * 
 * Date: May 2002
 * Author: Duncan Mackenzie
 * 
 * Requires the release version of .NET Framework
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using LiveChat.Client;

namespace ControlLib
{
    /// <summary>
    /// Summary description for MyTreeViewCtrl.
    /// </summary>
    public class dbTreeViewCtrl : System.Windows.Forms.TreeView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public dbTreeViewCtrl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitComponent call

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            // 
            // MyTreeViewCtrl
            // 
            components = new Container();

        }
        #endregion

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        private bool m_autoBuild = true;

        public bool AutoBuildTree
        {
            get
            {
                return this.m_autoBuild;
            }
            set
            {
                this.m_autoBuild = value;
            }
        }

        #region Data Binding
        private CurrencyManager m_currencyManager = null;
        private String m_ValueMember;
        private String m_DisplayMember;
        private object m_oDataSource;

        [Category("Data")]
        public object DataSource
        {
            get
            {
                return m_oDataSource;
            }
            set
            {
                if (value == null)
                {
                    this.m_currencyManager = null;
                    this.Nodes.Clear();
                }
                else
                {
                    if (!(value is IList || m_oDataSource is IListSource))
                        throw (new System.Exception("Invalid DataSource"));
                    else
                    {
                        if (value is IListSource)
                        {
                            IListSource myListSource = (IListSource)value;
                            if (myListSource.ContainsListCollection == true)
                                throw (new System.Exception("Invalid DataSource"));
                        }
                        this.m_oDataSource = value;
                        this.m_currencyManager = (CurrencyManager)this.BindingContext[value];
                        if (this.AutoBuildTree)
                            BuildTree();
                    }
                }
            }
        } // end of DataSource property

        [Category("Data")]
        public string ValueMember
        {
            get
            {
                return this.m_ValueMember;
            }
            set
            {
                this.m_ValueMember = value;
            }
        }

        [Category("Data")]
        public string DisplayMember
        {
            get
            {
                return this.m_DisplayMember;
            }
            set
            {
                this.m_DisplayMember = value;
            }
        }

        public object GetValue(int index)
        {
            IList innerList = this.m_currencyManager.List;
            if (innerList != null)
            {
                if ((this.ValueMember != "") && (index >= 0 && 0 < innerList.Count))
                {
                    PropertyDescriptor pdValueMember;
                    pdValueMember = this.m_currencyManager.GetItemProperties()[this.ValueMember];
                    return pdValueMember.GetValue(innerList[index]);
                }
            }
            return null;
        }

        public object GetDisplay(int index)
        {
            IList innerList = this.m_currencyManager.List;
            if (innerList != null)
            {
                if ((this.DisplayMember != "") && (index >= 0 && 0 < innerList.Count))
                {
                    PropertyDescriptor pdDisplayMember;
                    pdDisplayMember = this.m_currencyManager.GetItemProperties()[this.ValueMember];
                    return pdDisplayMember.GetValue(innerList[index]);
                }
            }
            return null;
        }

        #endregion

        #region Building the Tree

        private ArrayList treeGroups = new ArrayList();

        public void BuildTree()
        {
            this.Nodes.Clear();
            if ((this.m_currencyManager != null) && (this.m_currencyManager.List != null))
            {
                IList innerList = this.m_currencyManager.List;
                TreeNodeCollection currNode = this.Nodes;
                int currGroupIndex = 0;
                int currListIndex = 0;


                if (this.treeGroups.Count > currGroupIndex)
                {
                    Group currGroup = (Group)treeGroups[currGroupIndex];
                    dbTreeNode myFirstNode = null;
                    PropertyDescriptor pdGroupBy;
                    PropertyDescriptor pdValue;
                    PropertyDescriptor pdDisplay;

                    pdGroupBy = this.m_currencyManager.GetItemProperties()[currGroup.GroupBy];
                    pdValue = this.m_currencyManager.GetItemProperties()[currGroup.ValueMember];
                    pdDisplay = this.m_currencyManager.GetItemProperties()[currGroup.DisplayMember];

                    string currGroupBy = null;
                    if (innerList.Count > currListIndex)
                    {
                        object currObject;
                        while (currListIndex < innerList.Count)
                        {
                            currObject = innerList[currListIndex];
                            if (pdGroupBy.GetValue(currObject).ToString() != currGroupBy)
                            {
                                currGroupBy = pdGroupBy.GetValue(currObject).ToString();

                                myFirstNode = new dbTreeNode(currGroup.Name,
                                    pdDisplay.GetValue(currObject).ToString(),
                                    currObject,
                                    pdValue.GetValue(innerList[currListIndex]),
                                    currGroup.ImageIndex,
                                    currGroup.SelectedImageIndex,
                                    currListIndex);

                                currNode.Add((TreeNode)myFirstNode);
                            }
                            else
                                AddNodes(currGroupIndex, ref currListIndex, myFirstNode.Nodes, currGroup.GroupBy);
                        } // end while
                    } // end if
                } // end if
                else
                {
                    while (currListIndex < innerList.Count)
                    {
                        AddNodes(currGroupIndex, ref currListIndex, this.Nodes, "");
                    }
                } // end else

                if (this.Nodes.Count > 0)
                    this.SelectedNode = this.Nodes[0];

            } // end if
        }

        private void AddNodes(int currGroupIndex,
                              ref int currentListIndex,
                              TreeNodeCollection currNodes,
                              String prevGroupByField)
        {
            IList innerList = this.m_currencyManager.List;
            System.ComponentModel.PropertyDescriptor pdPrevGroupBy = null;
            string prevGroupByValue = null; ;
            Group currGroup;

            if (prevGroupByField != "")
                pdPrevGroupBy = this.m_currencyManager.GetItemProperties()[prevGroupByField];

            currGroupIndex += 1;

            if (treeGroups.Count > currGroupIndex)
            {
                currGroup = (Group)treeGroups[currGroupIndex];
                PropertyDescriptor pdGroupBy = null;
                PropertyDescriptor pdValue = null;
                PropertyDescriptor pdDisplay = null;

                pdGroupBy = this.m_currencyManager.GetItemProperties()[currGroup.GroupBy];
                pdValue = this.m_currencyManager.GetItemProperties()[currGroup.ValueMember];
                pdDisplay = this.m_currencyManager.GetItemProperties()[currGroup.DisplayMember];

                string currGroupBy = null;

                if (innerList.Count > currentListIndex)
                {
                    if (pdPrevGroupBy != null)
                        prevGroupByValue = pdPrevGroupBy.GetValue(innerList[currentListIndex]).ToString();

                    dbTreeNode myFirstNode = null;
                    object currObject = null;

                    while ((currentListIndex < innerList.Count) &&
                        (pdPrevGroupBy != null) &&
                        (pdPrevGroupBy.GetValue(innerList[currentListIndex]).ToString() == prevGroupByValue))
                    {
                        currObject = innerList[currentListIndex];
                        if (pdGroupBy.GetValue(currObject).ToString() != currGroupBy)
                        {
                            currGroupBy = pdGroupBy.GetValue(currObject).ToString();

                            myFirstNode = new dbTreeNode(currGroup.Name,
                                pdDisplay.GetValue(currObject).ToString(),
                                currObject,
                                pdValue.GetValue(innerList[currentListIndex]),
                                currGroup.ImageIndex,
                                currGroup.SelectedImageIndex,
                                currentListIndex);

                            currNodes.Add((TreeNode)myFirstNode);
                        }
                        else
                            AddNodes(currGroupIndex, ref currentListIndex, myFirstNode.Nodes, currGroup.GroupBy);
                    }
                }
            }
            else
            {
                dbTreeNode myNewLeafNode;
                object currObject = this.m_currencyManager.List[currentListIndex];

                if ((this.DisplayMember != null) && (this.ValueMember != null) &&
                     (this.DisplayMember != "") && (this.ValueMember != ""))
                {
                    PropertyDescriptor pdDisplayloc =
                        this.m_currencyManager.GetItemProperties()[this.DisplayMember];
                    PropertyDescriptor pdValueloc =
                        this.m_currencyManager.GetItemProperties()[this.ValueMember];

                    myNewLeafNode = new dbTreeNode(this.Tag == null ? "" : this.Tag.ToString(),
                        pdDisplayloc.GetValue(currObject).ToString(),
                        currObject,
                        pdValueloc.GetValue(currObject),
                        currentListIndex);
                }
                else
                    myNewLeafNode = new dbTreeNode("", currentListIndex.ToString(),
                        currObject,
                        currObject,
                        this.ImageIndex, this.SelectedImageIndex,
                        currentListIndex);

                currNodes.Add((TreeNode)myNewLeafNode);
                currentListIndex += 1;
            }
        }
        #endregion

        #region Groups

        public void RemoveGroup(Group group)
        {
            if (treeGroups.Contains(group))
            {
                treeGroups.Remove(group);
                if (this.AutoBuildTree)
                    BuildTree();
            }
        }

        public void RemoveGroup(string groupName)
        {
            foreach (Group group in this.treeGroups)
            {
                if (group.Name == groupName)
                {
                    RemoveGroup(group);
                    return;
                }
            }
        }

        public void RemoveAllGroups()
        {
            this.treeGroups.Clear();
            if (this.AutoBuildTree)
                this.BuildTree();
        }

        public void AddGroup(Group group)
        {
            try
            {
                treeGroups.Add(group);
                if (this.AutoBuildTree)
                    this.BuildTree();
            }
            catch (NotSupportedException e)
            {
                ClientUtils.ShowError(e);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public void AddGroup(String name, String groupBy, String displayMember,
            String valueMember, int imageIndex, int selectedImageIndex)
        {
            Group myNewGroup = new Group(name, groupBy, displayMember, valueMember,
                imageIndex, selectedImageIndex);
            this.AddGroup(myNewGroup);
        }

        public Group[] GetGroups()
        {
            return ((Group[])treeGroups.ToArray(Type.GetType("Group")));
        }

        #endregion

        public void SetLeafData(String name, String displayMember, String valueMember,
            int imageIndex, int selectedImageIndex)
        {
            this.Tag = name;
            this.DisplayMember = displayMember;
            this.ValueMember = valueMember;
            this.ImageIndex = imageIndex;
            this.SelectedImageIndex = selectedImageIndex;
        }

        public bool IsLeafNode(TreeNode node)
        {
            return (node.Nodes.Count == 0);
        }

        #region Keeping Everything In Sync

        public TreeNode FindNodeByValue(object value)
        {
            return FindNodeByValue(value, this.Nodes);
        }

        public TreeNode FindNodeByValue(object Value, TreeNodeCollection nodesToSearch)
        {
            int i = 0;
            TreeNode currNode;
            dbTreeNode leafNode;

            while (i < nodesToSearch.Count)
            {
                currNode = nodesToSearch[i];
                i++;
                if (currNode.LastNode == null)
                {
                    leafNode = (dbTreeNode)currNode;
                    if (leafNode.Value.ToString() == Value.ToString())
                        return currNode;
                }
                else
                {
                    currNode = FindNodeByValue(Value, currNode.Nodes);
                    if (currNode != null)
                        return currNode;
                }
            }

            return null;
        }

        private TreeNode FindNodeByPosition(int posIndex)
        {
            return FindNodeByPosition(posIndex, this.Nodes);
        }

        private TreeNode FindNodeByPosition(int posIndex, TreeNodeCollection nodesToSearch)
        {
            int i = 0;
            TreeNode currNode;
            dbTreeNode leafNode;

            while (i < nodesToSearch.Count)
            {
                currNode = nodesToSearch[i];
                i++;
                if (currNode.Nodes.Count == 0)
                {
                    leafNode = (dbTreeNode)currNode;
                    if (leafNode.Position == posIndex)
                    {
                        return currNode;
                    }
                    else
                    {
                        currNode = FindNodeByPosition(posIndex, currNode.Nodes);
                        if (currNode != null)
                            return currNode;
                    }
                }
            }
            return null;
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            dbTreeNode leafNode = (dbTreeNode)e.Node;

            if (leafNode != null)
            {
                if (this.m_currencyManager.Position != leafNode.Position)
                    this.m_currencyManager.Position = leafNode.Position;
            }
            // TODO:  Add MyTreeViewCtrl.OnAfterSelect implementation
            base.OnAfterSelect(e);
        }

        #endregion

    }

    public class Group
    {
        private String groupName;
        private String groupByMember;
        private String groupByDisplayMember;
        private String groupByValueMember;

        private int groupImageIndex;
        private int groupSelectedImageIndex;

        public Group(String name, String groupBy, String displayMember,
            String valueMember, int imageIndex, int selectedImageIndex)
        {
            this.ImageIndex = imageIndex;
            this.Name = name;
            this.GroupBy = groupBy;
            this.DisplayMember = displayMember;
            this.ValueMember = valueMember;
            this.SelectedImageIndex = selectedImageIndex;
        }

        public Group(String name, String groupBy, String displayMember,
            String valueMember, int imageIndex) :
            this(name, groupBy, displayMember, valueMember, imageIndex, imageIndex)
        {
        }

        public Group(String name, String groupBy) :
            this(name, groupBy, groupBy, groupBy, -1, -1)
        {
        }

        public int SelectedImageIndex
        {
            get
            {
                return groupSelectedImageIndex;
            }
            set
            {
                groupSelectedImageIndex = value;
            }
        }
        public int ImageIndex
        {
            get
            {
                return groupImageIndex;
            }
            set
            {
                groupImageIndex = value;
            }
        }

        public String Name
        {
            get
            {
                return groupName;
            }
            set
            {
                groupName = value;
            }
        }

        public String GroupBy
        {
            get
            {
                return groupByMember;
            }
            set
            {
                groupByMember = value;
            }
        }

        public String DisplayMember
        {
            get
            {
                return groupByDisplayMember;
            }
            set
            {
                groupByDisplayMember = value;
            }
        }

        public String ValueMember
        {
            get
            {
                return groupByValueMember;
            }
            set
            {
                groupByValueMember = value;
            }
        }
    }

    public class dbTreeNode : TreeNode
    {
        private String m_groupName;
        private object m_value;
        private object m_item;
        private int m_position;

        public dbTreeNode()
        {
        }

        public dbTreeNode(String groupName, String text, object item, object value,
            int imageIndex, int selectedImgIndex, int position)
        {
            this.GroupName = groupName;
            this.Text = text;
            this.Item = item;
            this.Value = value;
            this.ImageIndex = imageIndex;
            this.SelectedImageIndex = selectedImgIndex;
            this.m_position = position;
        }

        public dbTreeNode(String groupName, String text, object item, object value, int position)
        {
            this.GroupName = groupName;
            this.Text = text;
            this.Item = item;
            this.Value = value;
            this.m_position = position;
        }

        public String GroupName
        {
            get
            {
                return m_groupName;
            }
            set
            {
                this.m_groupName = value;
            }
        }

        public object Item
        {
            get
            {
                return m_item;
            }
            set
            {
                m_item = value;
            }
        }

        public object Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }

        public int Position
        {
            get
            {
                return m_position;
            }
        }
    }
}
