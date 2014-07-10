using System;
using System.Diagnostics.CodeAnalysis;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.Dialogs.RulesEditor;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;

namespace Cardinal.Rules.Lucinq.ContentEditor
{
    /// <summary>
    ///     The lucene rules.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LuceneRules : Sitecore.Shell.Applications.ContentEditor.Rules
    {
        /// <summary>
        ///     The handle message.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            base.HandleMessage(message);
            if (message["id"] != ID)
            {
                return;
            }

            switch (message.Name)
            {
                case "lucenerules:edit":
                    Sitecore.Context.ClientPage.Start(this, "Edit");
                    break;
            }
        }

        /// <summary>
        ///     The parse source.
        /// </summary>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <param name="source">
        ///     The source.
        /// </param>
        internal static void ParseSource(RulesEditorOptions options, string source)
        {
            Assert.ArgumentNotNull(options, "options");
            Assert.ArgumentNotNull(source, "source");
            if (source == string.Empty)
            {
                return;
            }

            if (source.ToLowerInvariant().Contains("rulespath") || source.ToLowerInvariant().Contains("hideactions"))
            {
                var urlString = new UrlString(source);
                options.RulesPath = urlString["rulespath"];
                string strA = urlString["hideactions"];
                if (string.IsNullOrEmpty(strA))
                {
                    return;
                }

                options.HideActions = string.Compare(strA, "true", StringComparison.InvariantCultureIgnoreCase) == 0;
            }
            else
            {
                options.RulesPath = source;
            }
        }

        /// <summary>
        ///     The edit.
        /// </summary>
        /// <param name="args">
        ///     The args.
        /// </param>
        protected override void Edit(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (Disabled)
            {
                return;
            }

            if (args.IsPostBack)
            {
                if (!args.HasResult)
                {
                    return;
                }

                Value = args.Result == "-" ? string.Empty : args.Result;
                SheerResponse.SetAttribute(ID, "value", Value);
                SheerResponse.SetModified(true);
                Refresh();
            }
            else
            {
                string str = Value;
                if (str == "__#!$No value$!#__")
                {
                    str = string.Empty;
                }

                var options = new RulesEditorOptions
                {
                    ContextItemID = ItemID,
                    RulesPath = DataSource,
                    IncludeCommon = true,
                    Value = str
                };
                ParseSource(options, Source);
                SheerResponse.ShowModalDialog(GetRulesUrlString(options).ToString(), "800px", "800px", string.Empty,
                    true);
                args.WaitForPostBack();
            }
        }

        /// <summary>
        ///     The load post data.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        protected override bool LoadPostData(string value)
        {
            Assert.ArgumentNotNull(value, "value");
            if (Value == value || Value.Replace("\r", string.Empty) == value)
            {
                return false;
            }

            SetModified();
            Value = value;
            return true;
        }

        /// <summary>
        ///     The get rules url string.
        /// </summary>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <returns>
        ///     The <see cref="UrlString" />.
        /// </returns>
        private UrlString GetRulesUrlString(RulesEditorOptions options)
        {
            var urlString =
                new UrlString(
                    "/sitecore/shell/~/xaml/Cardinal.Rules.Lucinq.ContentEditor.LuceneRulesEditorPage.aspx");
            var urlHandle = new UrlHandle();
            urlHandle["Allow Multiple"] = options.AllowMultiple ? "1" : "0";
            urlHandle["Rules Path"] = options.RulesPath;
            urlHandle["Include Common"] = options.IncludeCommon ? "1" : "0";
            urlHandle["Hide Actions"] = options.HideActions ? "1" : "0";
            urlHandle["Value"] = options.Value;
            urlHandle["ContextItemID"] = options.ContextItemID;
            urlHandle.Add(urlString);
            return urlString;
        }
    }
}