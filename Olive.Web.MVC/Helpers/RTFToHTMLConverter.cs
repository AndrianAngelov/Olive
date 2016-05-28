using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Windows.Forms;

namespace Olive.Web.MVC.Helpers
{
    public class RTFToHTMLConverter
    {
        public string ConvertRTFToHTML(string rtfString)
        {
            var thread = new Thread(ConvertToHTMLInSTAThread);
            var threadData = new ConvertRtfThreadData { RtfText = rtfString };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(threadData);
            thread.Join();
            return threadData.HtmlText;
        }

        private void ConvertToHTMLInSTAThread(object rtf)
        {
            var threadData = rtf as ConvertRtfThreadData;
            threadData.HtmlText = MarkupConverterRTFToHTML(threadData.RtfText);
        }

        private string MarkupConverterRTFToHTML(string rtfString)
        {
            string html = "";
            var rtfSource = rtfString;
            if (rtfSource != null)
            {
                using (WebBrowser web = new WebBrowser())
                {
                    web.CreateControl();
                    web.DocumentText = rtfSource;
                    while (web.DocumentText != rtfSource)
                        Application.DoEvents();
                    web.Document.ExecCommand("SelectAll", false, null);
                    web.Document.ExecCommand("Copy", false, null);
                }

                using (RichTextBox rtBox = new RichTextBox())
                {
                    rtBox.Paste();
                }

                html = Clipboard.GetData(DataFormats.Html) as string;
            }
            return html;
        }

        private class ConvertRtfThreadData
        {
            public string RtfText { get; set; }
            public string HtmlText { get; set; }
        }
    }
}