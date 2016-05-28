using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Windows.Forms;

namespace Olive.Web.MVC.Helpers
{
    public class HMLToRTFConverter
    {
        public string ConvertHTMLToRTF(string htmlString)
        {
            var thread = new Thread(ConvertToRtfInSTAThread);
            var threadData = new ConvertRtfThreadData { HtmlText = htmlString };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(threadData);
            thread.Join();
            return threadData.RtfText;
        }

        private void ConvertToRtfInSTAThread(object html)
        {
            var threadData = html as ConvertRtfThreadData;
            threadData.RtfText = MarkupConverterHTMLToRTF(threadData.HtmlText);
        }

        private string MarkupConverterHTMLToRTF(string htmlString)
        {
            string rtf = "";
            var htmlSource = htmlString;
            if (htmlSource != null)
            {
                using (WebBrowser web = new WebBrowser())
                {
                    web.CreateControl();
                    web.DocumentText = htmlSource;
                    while (web.DocumentText != htmlSource)
                        Application.DoEvents();
                    web.Document.ExecCommand("SelectAll", false, null);
                    web.Document.ExecCommand("Copy", false, null);
                }

                using (RichTextBox rtBox = new RichTextBox())
                {
                    rtBox.Paste();
                }

                rtf = Clipboard.GetData(DataFormats.Rtf) as string;
            }
            return rtf;
        }

        private class ConvertRtfThreadData
        {
            public string RtfText { get; set; }
            public string HtmlText { get; set; }
        }
    }
}