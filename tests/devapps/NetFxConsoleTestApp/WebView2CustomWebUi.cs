using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Identity.Client.Extensibility;

namespace NetFxConsoleApp
{
    class WebView2CustomWebUi : ICustomWebUi
    {
        private Panel _webBrowserPanel;


        public Task<Uri> AcquireAuthorizationCodeAsync(Uri authorizationUri, Uri redirectUri, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
