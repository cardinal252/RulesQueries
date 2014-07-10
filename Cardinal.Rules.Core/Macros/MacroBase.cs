using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;
using Cardinal.Core.Configuration;
using Cardinal.Core.IoC;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace Cardinal.Rules.Lucinq.Macros
{
    /// <summary>
    ///     The lucene macro base.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class MacroBase
    {
        /// <summary>
        ///     The get selection item options.
        /// </summary>
        /// <param name="element">
        ///     The element.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="parameters">
        ///     The parameters.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="title">
        ///     The title.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="rootItemId">
        ///     The root Item Id.
        /// </param>
        protected virtual void GetSelectionItemOptions(XElement element, string name, UrlString parameters, string value,
            string title, string text, ID rootItemId)
        {
            Assert.ArgumentNotNull(element, "element");
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(parameters, "parameters");
            Assert.ArgumentNotNull(value, "value");

            Item itemFromValue = null;
            if (!string.IsNullOrEmpty(value))
            {
                itemFromValue = Client.ContentDatabase.GetItem(value);
            }

            var selectItemOptions = new SelectItemOptions
            {
                Title = title,
                Text = text,
                Root =
                    Client.ContentDatabase.GetItem(rootItemId),
                Icon = "applications/32x32/media_stop.png",
                ShowRoot = false
            };
            selectItemOptions.SelectedItem = itemFromValue
                                             ?? (selectItemOptions.Root != null
                                                 ? selectItemOptions.Root.Children.FirstOrDefault()
                                                 : null);

            string path = XElement.Parse(element.ToString()).FirstAttribute.Value;
            if (!string.IsNullOrEmpty(path))
            {
                Item filterItem = Client.ContentDatabase.GetItem(path);
                if (filterItem != null)
                {
                    selectItemOptions.FilterItem = filterItem;
                }
            }

            SheerResponse.ShowModalDialog(selectItemOptions.ToUrlString().ToString(), "1200px", "700px", string.Empty,
                true);
        }
    }
}