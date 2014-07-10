using Sitecore.Data;

namespace Cardinal.Rules.Lucinq
{
    public class Constants
    {
        public static ID LuceneOperators
        {
            get
            {
                return new ID("{0D897F93-9AAF-45D8-9DC9-A872C229150F}");
            }
        }

        public static class Templates
        {
            public static ID BasicQuery
            {
                get { return new ID("{7674603E-CE4B-45A4-A203-65E8AB1220F9}"); }
            }

            public static ID SystemQuery
            {
                get { return new ID(""); }
            }
        }

        /// <summary>
        ///     The lucene operator constants.
        /// </summary>
        public static class LuceneOperatorConstants
        {
            /// <summary>
            /// Gets the lucene condition contains string id.
            /// </summary>
            public static string Contains
            {
                get { return "{7B6D0CEE-C73C-4DA7-A9E5-6FF7C8F4DB9A}"; }
            }

            /// <summary>
            /// Gets the lucene condition equals string id.
            /// </summary>
            public static string IsEqualTo
            {
                get { return "{08D1F7C7-457B-4EF3-BB08-A898D5AA074C}"; }
            }

            /// <summary>
            /// Gets the lucene condition ends with string id.
            /// </summary>
            public static string EndsWith
            {
                get { return "{B46BB691-1CF8-48B3-821F-FEFC7146482A}"; }
            }

            /// <summary>
            /// Gets the lucene condition starts with string id.
            /// </summary>
            public static string StartsWith
            {
                get { return "{74E7642C-060E-4CB3-8595-6B36E69E3CBD}"; }
            }
        }
    }
}
