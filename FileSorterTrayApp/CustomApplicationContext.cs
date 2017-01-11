using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSorterTrayApp
{
    class CustomApplicationContext : ApplicationContext
    {


        private FileManager fileManager;
        private Form1 mainForm;



        private System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        private NotifyIcon notifyIcon;				            // the icon that sits in the system tray

        public CustomApplicationContext()
        {
            InitializeContext();
            fileManager = new FileManager();
            fileManager.start();
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            notifyIcon.ContextMenuStrip.Items.Clear();
            notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("Show &Test", showDetailsItem_Click));
            notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&Exit", exitItem_Click));
        }


        #region child forms


        private void showDetailsItem_Click(object sender, EventArgs e) { ShowMainForm(); }

        private void ShowMainForm()
        {
            if (mainForm == null)
            {
                mainForm = new Form1();
                mainForm.Closed += mainForm_Closed; // avoid reshowing a disposed form
                mainForm.Show();
            }
            else { mainForm.Activate(); }
        }

        private void mainForm_Closed(object sender, EventArgs e)
        {
            mainForm = null;
        }

        #endregion

        private void InitializeContext()
        {

            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new Icon(SystemIcons.Exclamation, 40, 40),
                Text = "Default Tool Tip",
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            notifyIcon.MouseUp += notifyIcon_MouseUp;
        }

        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        private ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            if (eventHandler != null) { item.Click += eventHandler; }
            return item;
        }

        #region disposing methods
        /// <summary>
        /// When the application context is disposed, dispose things like the notify icon.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) { components.Dispose(); }
        }

        /// <summary>
        /// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitItem_Click(object sender, EventArgs e)
        {
            ExitThread();
        }

        /// <summary>
        /// If we are presently showing a form, clean it up.
        /// </summary>
        protected override void ExitThreadCore()
        {
            // before we exit, let forms clean themselves up.
            if (mainForm != null) { mainForm.Close(); }

            notifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }
        #endregion  
    }
}
