using Microsoft.AspNetCore.Mvc;
using SmartContractsGenerator.Mappers;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Xml;

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
            Contract c;
            try
            { 
                c = mapper.MapXmlDocumentToContract(xmlDocument);

                if (c == null)
                {
                    return "Contract was not found! Make sure you are using the Contract block as a root of your contract";
                }
            } 
            catch (Exception e)
            {
                return "Error ocurred during deserialization your workspace\n" + e.Message;
            }
            try
            {
                return c.GenerateCode(new Indentation());
            }
            catch (Exception e)
            {
                return "Error ocurred during generating code of your contract\n" + e.Message;

            }
        }
    }
}