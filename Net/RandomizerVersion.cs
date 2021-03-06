﻿using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SuperMetroidRandomizer.Net
{
    public static class RandomizerVersion
    {
        public static string Current = "1.4";

        public static string CurrentDisplay
        {
            get
            {
                var retVal = Current;

                if (retVal.Contains("p"))
                {
                    retVal = string.Format("{0})", retVal.Replace("P", " (preview "));
                }

                return retVal;
            }
        }

        private const int checkVersion = 0;
        private static readonly string checkAddress = "https://github.com/Galamoz/ProjectBaseRandomizer" + DateTime.Now.Ticks;
        private const string updateAddress = "https://github.com/Galamoz/ProjectBaseRandomizer";

        public static void CheckUpdate()
        {
            try
            {
                var response = GetResponse(checkAddress);

                if (string.IsNullOrWhiteSpace(response))
                    return;

                const string pattern = "Current Version: (?<version>\\S+)";
                var match = Regex.Match(response, pattern);

                if (match.Success)
                {
                    var currentVersion = match.Groups["version"].Value;
                    int currentVersionNum;

                    if (int.TryParse(currentVersion, out currentVersionNum))
                    {
                        if (checkVersion > currentVersionNum)
                        {
                            var result =
                                MessageBox.Show(
                                    string.Format(
                                        "You have v{0} and the current version is v{1}. Would you like to update?",
                                        Current,
                                        currentVersion), "Version Update", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                                Help.ShowHelp(null, updateAddress);
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                // check for update failed, do nothing here
            }
        }

        private static string GetResponse(string address)
        {
            if (!address.Contains("https://github.com/Galamoz/ProjectBaseRandomizer"))
                return "";

            var webBrowser = new WebBrowser { ScrollBarsEnabled = false, ScriptErrorsSuppressed = true };
            webBrowser.Navigate(address);
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }

            return webBrowser.Document.Body.InnerHtml;
        }
    }
}
