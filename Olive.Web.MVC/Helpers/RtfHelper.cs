namespace Olive.Web.MVC.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class RtfHelper
    {
        public static string PlainText(string rtfString)
        {
            string str = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 MS Shell Dlg 2;}{\f1\fnil MS Shell Dlg 2;}} {\colortbl ;\red0\green0\blue0;} {\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\tx720\cf1\f0\fs20 can u send me info for the call pls\f1\par }";
            string plainText = "";
            using (RichTextBox rtBox = new RichTextBox())
            {
                // Convert the RTF to plain text.
                rtBox.Rtf = rtfString;
                plainText = rtBox.Text;

                // Now just remove the new line constants
                plainText = plainText.Replace("\r\n", ",");
            }
            return plainText;
        }


    }
}