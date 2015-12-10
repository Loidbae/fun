﻿using fun.IO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace fun.IO.Parsers
{
    internal sealed class StringParser : Parser
    {
        private IElementPropertyDataStore data;

        public StringParser(IElementPropertyDataStore data)
        {
            this.data = data;
            parsers = null;
        }

        public override bool TryParse(XmlNode node)
        {
            return data.Element.GetType().GetProperty(node.Name).PropertyType == typeof(string);
        }

        public override void Parse(XmlNode node)
        {
            var prop = data.Element.GetType().GetProperty(node.Name);
            prop.SetValue(data.Element, node.Value);
        }
    }
}
