namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;

    public class JQGridState
    {
        public JQGridState()
        {
            this.QueryString = new NameValueCollection();
        }

        public NameValueCollection QueryString { get; set; }
    }
}

