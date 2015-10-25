using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;

namespace NiceHashMiner
{
    public partial class Form4 : Form
    {
        public class FieldLink
        {
            public FieldInfo F;
            public object obj;
            public int ArrayIndex;

            public FieldLink(FieldInfo fi, object o) { F = fi; obj = o; ArrayIndex = -1; }
            public FieldLink(FieldInfo fi, object o, int index) { F = fi; obj = o; ArrayIndex = index; }

            public Type GetElementType()
            {
                if (F.FieldType.IsArray) return F.FieldType.GetElementType();
                else return F.FieldType;
            }
        }


        public Form4()
        {
            InitializeComponent();

            LoadConfigData();
        }


        private void LoadConfigData()
        {
            TreeNode tn = treeView1.Nodes.Add(Config.ConfigData.GetType().Name);
            LoadNode(tn, Config.ConfigData);
            tn.Expand();
        }


        private void LoadNode(TreeNode tn, object n)
        {
            if (n == null) return;

            Type SC = typeof(SubConfigClass);
            Type T = n.GetType();
            FieldInfo[] Fields = T.GetFields();
            foreach (FieldInfo F in Fields)
            {
                if (F.IsStatic) continue;

                TreeNode tn2 = new TreeNode();
                tn2.Tag = new FieldLink(F, n);
                tn.Nodes.Add(tn2);
                Type ElType = F.FieldType;

                if (ElType.IsArray)
                {
                    // handle array
                    tn2.Text = F.Name;
                    ElType = ElType.GetElementType();
                    Array Value = (Array)F.GetValue(n);

                    if (SC.IsAssignableFrom(ElType))
                    {
                        // handle array of custom classes
                        int i = 0;
                        foreach (object ArrayItem in Value)
                        {
                            TreeNode tn3 = new TreeNode("[" + (i++).ToString() + "]");
                            tn2.Nodes.Add(tn3);
                            LoadNode(tn3, ArrayItem);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Value.Length; i++)
                        {
                            TreeNode tn3 = new TreeNode();
                            tn3.Tag = new FieldLink(F, n, i);
                            UpdateNode(tn3);
                            tn2.Nodes.Add(tn3);
                        }
                    }
                }
                else
                {
                    UpdateNode(tn2);
                }
            }
        }


        private object GetValue(FieldLink fl)
        {
            if (fl.ArrayIndex < 0)
            {
                return fl.F.GetValue(fl.obj);
            }
            else
            {
                Array ArrayValue = (Array)fl.F.GetValue(fl.obj);
                return ArrayValue.GetValue(fl.ArrayIndex);
            }
        }


        private void SetValue(FieldLink fl, object val)
        {
            if (fl.ArrayIndex < 0)
            {
                fl.F.SetValue(fl.obj, val);
            }
            else
            {
                Array ArrayValue = (Array)fl.F.GetValue(fl.obj);
                ArrayValue.SetValue(val, fl.ArrayIndex);
                fl.F.SetValue(fl.obj, ArrayValue);
            }
        }


        private void UpdateNode(TreeNode tn)
        {
            FieldLink fl = tn.Tag as FieldLink;
            object Value = GetValue(fl);
            string Name = "";

            if (fl.ArrayIndex < 0) Name = fl.F.Name;
            else Name = "[" + fl.ArrayIndex.ToString() + "]";

            if (Value == null) tn.Text = Name + ": null";
            else if (Value.GetType() == typeof(Boolean)) tn.Text = Name + ": " + Value.ToString();
            else if (Value.GetType() == typeof(double)) tn.Text = Name + ": \"" + ((double)Value).ToString("F8", CultureInfo.InvariantCulture) + "\"";
            else tn.Text = Name + ": \"" + Value.ToString() + "\"";
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox1.Text = "";
            textBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            if (e.Node.Tag != null)
            {
                FieldLink fl = e.Node.Tag as FieldLink;
                if (fl.F.FieldType.IsArray && fl.ArrayIndex < 0) return;

                Type T = fl.GetElementType();
                if (T == typeof(String) || T == typeof(int) || T == typeof(double))
                {
                    textBox1.Enabled = true;
                    button1.Enabled = true;
                    button2.Enabled = true;

                    object Val = GetValue(fl);
                    if (Val != null)
                    {
                        if (fl.F.FieldType == typeof(double))
                            textBox1.Text = ((double)Val).ToString("F8", CultureInfo.InvariantCulture);
                        else
                            textBox1.Text = Val.ToString();
                    }
                }
                else
                {
                    textBox1.Text = "";
                    textBox1.Enabled = false;

                    if (fl.F.FieldType == typeof(Boolean))
                    {
                        button3.Enabled = true;
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FieldLink fl = treeView1.SelectedNode.Tag as FieldLink;
            Type T = fl.GetElementType();
            object Value = null;

            try
            {
                if (T == typeof(String))
                {
                    Value = textBox1.Text;
                }
                else if (T == typeof(int))
                {
                    Value = int.Parse(textBox1.Text);
                }
                else if (T == typeof(double))
                {
                    Value = double.Parse(textBox1.Text, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SetValue(fl, Value);
            UpdateNode(treeView1.SelectedNode);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            FieldLink fl = treeView1.SelectedNode.Tag as FieldLink;
            Type T = fl.GetElementType();
            SetValue(fl, null);
            UpdateNode(treeView1.SelectedNode);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            FieldLink fl = treeView1.SelectedNode.Tag as FieldLink;
            Type T = fl.GetElementType();
            bool b = (bool)GetValue(fl);
            SetValue(fl, !b);
            UpdateNode(treeView1.SelectedNode);
        }


        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.Commit();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {
                FieldLink fl = treeView1.SelectedNode.Tag as FieldLink;

                if (fl.F.FieldType.IsArray && fl.ArrayIndex < 0) return;

                Type T = fl.GetElementType();
                if (T != typeof(String) && T != typeof(int) && T != typeof(double))
                    button3_Click(null, null);
            }
        }
    }
}
