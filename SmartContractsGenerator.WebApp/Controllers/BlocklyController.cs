using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using SmartContractsGenerator.Mappers;

namespace SmartContractsGenerator.WebApp.Controllers
{
    public class BlocklyController : Controller
    {
        [HttpPost]
        public string GetCode()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(Request.Form["code"]);
            var mapper = new ContractMapper();
            var c = mapper.MapXmlDocumentToContract(xmlDocument);
            return c.GenerateCode();
        }
    }
}