namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Root Element Event Args class.
    /// </summary>
    public class RootElementEventArgs : EventArgs
    {
        // default constructor is prohibited
        private RootElementEventArgs() { }

        /// <summary>
        /// Constructs a RootElementEventArgs with context of a root element.
        /// </summary>
        /// <param name="root">The root element.</param>
        public RootElementEventArgs(XElement root)
        {
            this.Root = root;
        }

        /// <summary>
        /// Gets the related root <see cref="XElement"/>.
        /// </summary>
        public XElement Root { get; private set; }
    }
}
