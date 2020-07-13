using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Identity.Client;
using Microsoft.Web.WebView2.WinForms;

namespace NetFxConsoleApp
{
    public class WebView2Form : Form
    {
        private sealed class WindowsFormsWin32Window : IWin32Window
        {
            public IntPtr Handle { get; set; }
        }

        private IWin32Window _parentWindow { get; set; }
        private Panel _webBrowserPanel;
        private WebView2 _webBrowser;
        private readonly string _redirectUri;

        public WebView2Form(object parentWindow, string redirectUri)
        {
            // todo - parentWindow
            InitParentWindow(parentWindow);
            InitializeComponent();

            _webBrowser.NavigationStarting += _webBrowser_NavigationStarting;
            _redirectUri = redirectUri;
        }

        private void _webBrowser_NavigationStarting(
            object sender, 
            Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.Uri.StartsWith(_redirectUri))
            {

            }
            
        }

        private void InitParentWindow(object parentWindow)
        {
            if (parentWindow is IWin32Window window)
            {
                this._parentWindow = window;
            }
            else if (parentWindow is IntPtr ptr)
            {
                this._parentWindow = new WindowsFormsWin32Window { Handle = ptr };
            }
            else
            {
                throw new MsalClientException(
                    MsalError.InvalidOwnerWindowType,
                    "Invalid owner window type. Expected types are IWin32Window or IntPtr (for window handle).");
            }
        }

        /// <summary>
        /// TODO: check for UI thread or invoke UI thread based on ui window.
        /// </summary>
        /// <param name="action"></param>
        private void InvokeOnUiThread(Action action)
        {
            //We only support WindowsForms(since our dialog is winforms based)
            if (_parentWindow is Control winFormsControl)
            {
                winFormsControl.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void InitializeComponent()
        {
            InvokeOnUiThread(() =>
            {
                Screen screen = (_parentWindow != null)
                    ? Screen.FromHandle(_parentWindow.Handle)
                    : Screen.PrimaryScreen;

                //TODO: positioning
                //// Window height is set to 70% of the screen height.
                //int uiHeight = (int)(Math.Max(screen.WorkingArea.Height, 160) * 70.0 / DpiHelper.ZoomPercent);
                _webBrowserPanel = new Panel();
                _webBrowserPanel.SuspendLayout();
                _webBrowser = new Microsoft.Web.WebView2.WinForms.WebView2();
                
                //int uiHeight = (int)(Math.Max(screen.WorkingArea.Height, 160) * 70.0 / DpiHelper.ZoomPercent);

                SuspendLayout();

                // webBrowser
                _webBrowser.Dock = DockStyle.Fill;
                _webBrowser.Location = new Point(0, 25);
                _webBrowser.MinimumSize = new Size(20, 20);
                _webBrowser.Name = "webBrowser";
                _webBrowser.Size = new Size(566, 565);
                _webBrowser.TabIndex = 1;
                _webBrowser.ContextMenu = null; // TODO: test this.

                // webBrowserPanel
                _webBrowserPanel.Controls.Add(_webBrowser);
                _webBrowserPanel.Dock = DockStyle.Fill;
                _webBrowserPanel.BorderStyle = BorderStyle.None;
                _webBrowserPanel.Location = new Point(0, 0);
                _webBrowserPanel.Name = "webBrowserPanel";
                _webBrowserPanel.Size = new Size(566, 565);  // TODO: height is uiHeight
                _webBrowserPanel.TabIndex = 2;

                // BrowserAuthenticationWindow
                AutoScaleDimensions = new SizeF(6, 13);
                AutoScaleMode = AutoScaleMode.Font;
                ClientSize = new Size(566, 565);             // TODO: height is uiHeight
                Controls.Add(_webBrowserPanel);
                FormBorderStyle = FormBorderStyle.FixedSingle;
                Name = "BrowserAuthenticationWindow";

                // Move the window to the center of the parent window only if owner window is set.
                StartPosition = (_parentWindow != null)
                    ? FormStartPosition.CenterParent
                    : FormStartPosition.CenterScreen;
                Text = string.Empty;
                ShowIcon = false;
                MaximizeBox = false;
                MinimizeBox = false;

                // If we don't have an owner we need to make sure that the pop up browser
                // window is in the task bar so that it can be selected with the mouse.
                ShowInTaskbar = null == _parentWindow;

                _webBrowserPanel.ResumeLayout(false);
                ResumeLayout(false);
            });
        }
    }
}
