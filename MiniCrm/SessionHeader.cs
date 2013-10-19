using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:partner.soap.sforce.com")]
    public class SessionHeader
    {
        private string sessionIdField;


        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string sessionId
        {
            get
            {
                return this.sessionIdField;
            }
            set
            {
                this.sessionIdField = value;

            }
        }
    }
}
