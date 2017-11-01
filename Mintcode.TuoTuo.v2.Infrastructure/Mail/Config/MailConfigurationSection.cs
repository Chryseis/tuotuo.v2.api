using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public class MailConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("server", IsRequired = true)]
        public TextContextElement Server
        {
            get
            {
                return this["server"] as TextContextElement;
            }
            set
            {
                this["server"] = value;
            }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public TextContextElement Port
        {
            get
            {
                return this["port"] as TextContextElement;
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("userName", IsRequired = true)]
        public TextContextElement UserName
        {
            get
            {
                return this["userName"] as TextContextElement;
            }
            set
            {
                this["userName"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public TextContextElement Password
        {
            get
            {
                return this["password"] as TextContextElement;
            }
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("enableSsl", IsRequired = true)]
        public TextContextElement EnableSsl
        {
            get
            {
                return this["enableSsl"] as TextContextElement;
            }
            set
            {
                this["enableSsl"] = value;
            }
        }


    }

    public class MailContentElement : ConfigurationElement
    {
        /// <summary>
        /// 标题
        /// </summary>
        [ConfigurationProperty("subject", IsRequired = true)]
        public TextContextElement Subject
        {
            get { return this["subject"] as TextContextElement; }
            set { this["subject"] = value; }
        }

        /// <summary>
        /// 正文
        /// </summary>
        [ConfigurationProperty("body", IsRequired = true)]
        public XMLContentElement Body { get { return this["body"] as XMLContentElement; } }
    }

    public class TextContextElement : ConfigurationElement
    {
        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            Text = reader.ReadElementContentAs(typeof(string), null) as string;
        }

        [ConfigurationProperty("data", IsRequired = false)]
        public string Text
        {
            get { return this["data"].ToString(); }
            set { this["data"] = value; }
        }
    }

    public class XMLContentElement : ConfigurationElement
    {
        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            Text = reader.ReadElementContentAs(typeof(string), null) as string;
        }

        protected override bool SerializeElement(System.Xml.XmlWriter writer, bool serializeCollectionKey)
        {
            if (writer != null)
                writer.WriteCData(Text);
            return true;
        }

        [ConfigurationProperty("data", IsRequired = false)]
        public string Text
        {
            get { return this["data"].ToString(); }
            set { this["data"] = value; }
        }
    }
}
