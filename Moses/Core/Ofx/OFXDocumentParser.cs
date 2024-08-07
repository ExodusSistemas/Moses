﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SgmlCore;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Moses.Ofx
{
   public class OFXDocumentParser
   {
      public OFXDocument Import(FileStream stream)
      {
         using (var reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1") ) )
         {
            return Import(reader.ReadToEnd().Replace(',','.') ); //replace é para o santander
         }
      }

      public OFXDocument Import(string ofx)
      {
         return ParseOfxDocument(ofx);
      }

      private OFXDocument ParseOfxDocument(string ofxString)
      {
         //If OFX file in SGML format, convert to XML
         if (!IsXmlVersion(ofxString))
         {
            ofxString = SGMLToXML(ofxString);
         }

         return Parse(ofxString);
      }

      private OFXDocument Parse(string ofxString)
      {
         var ofx = new OFXDocument {AccType = GetAccountType(ofxString)};

         //Load into xml document
         var doc = new XmlDocument();
         doc.Load(new StringReader(ofxString));

         var currencyNode = doc.SelectSingleNode(GetXPath(ofx.AccType, OFXSection.CURRENCY));

         if (currencyNode != null)
         {
            ofx.Currency = currencyNode.FirstChild.Value;
         }
         else
         {
            throw new OFXParseException("Currency not found");
         }

         //Get sign on node from OFX file
         var signOnNode = doc.SelectSingleNode(Moses.Core.Ofx.Ofx.SignOn);

         //If exists, populate signon obj, else throw parse error
         if (signOnNode != null)
         {
            ofx.SignOn = new SignOn(signOnNode);
         }
         else
         {
            throw new OFXParseException("Sign On information not found");
         }

         //Get Account information for ofx doc
         var accountNode = doc.SelectSingleNode(GetXPath(ofx.AccType, OFXSection.ACCOUNTINFO));

         //If account info present, populate account object
         if (accountNode != null)
         {
            ofx.Account = new Account(accountNode, ofx.AccType);
         }
         else
         {
            throw new OFXParseException("Account information not found");
         }

         //Get list of transactions
         ImportTransations(ofx, doc);

         //Get balance info from ofx doc
         var ledgerNode = doc.SelectSingleNode(GetXPath(ofx.AccType, OFXSection.BALANCE) + "/LEDGERBAL");
         var avaliableNode = doc.SelectSingleNode(GetXPath(ofx.AccType, OFXSection.BALANCE) + "/AVAILBAL");

         //If balance info present, populate balance object
         // ***** OFX files from my bank don't have the 'avaliableNode' node, so i manage a 'null' situation
         if (ledgerNode != null) // && avaliableNode != null
         {
            ofx.Balance = new Balance(ledgerNode, avaliableNode);
         }
         else
         {
            throw new OFXParseException("Balance information not found");
         }

         return ofx;
      }


      /// <summary>
      /// Returns the correct xpath to specified section for given account type
      /// </summary>
      /// <param name="type">Account type</param>
      /// <param name="section">Section of OFX document, e.g. Transaction Section</param>
      /// <exception cref="OFXException">Thrown in account type not supported</exception>
      private string GetXPath(AccountType type, OFXSection section)
      {
         string xpath, accountInfo;

         switch (type)
         {
            case AccountType.BANK:
               xpath = Moses.Core.Ofx.Ofx.BankAccount;
               accountInfo = "/BANKACCTFROM";
               break;
            case AccountType.CC:
               xpath = Moses.Core.Ofx.Ofx.CCAccount;
               accountInfo = "/CCACCTFROM";
               break;
            default:
               throw new OFXException("Account Type not supported. Account type " + type);
         }

         switch (section)
         {
            case OFXSection.ACCOUNTINFO:
               return xpath + accountInfo;
            case OFXSection.BALANCE:
               return xpath;
            case OFXSection.TRANSACTIONS:
               return xpath + "/BANKTRANLIST";
            case OFXSection.SIGNON:
               return Moses.Core.Ofx.Ofx.SignOn;
            case OFXSection.CURRENCY:
               return xpath + "/CURDEF";
            default:
               throw new OFXException("Unknown section found when retrieving XPath. Section " + section);
         }
      }

      /// <summary>
      /// Returns list of all transactions in OFX document
      /// </summary>
      /// <param name="doc">OFX document</param>
      /// <returns>List of transactions found in OFX document</returns>
      private void ImportTransations(OFXDocument ofxDocument, XmlDocument doc)
      {
         var xpath = GetXPath(ofxDocument.AccType, OFXSection.TRANSACTIONS);

         ofxDocument.StatementStart = doc.GetValue(xpath + "/DTSTART").ToDate();
         ofxDocument.StatementEnd = doc.GetValue(xpath + "/DTEND").ToDate();

         var transactionNodes = doc.SelectNodes(xpath + "/STMTTRN");

         ofxDocument.Transactions = new List<Transaction>();

         foreach (XmlNode node in transactionNodes)
            ofxDocument.Transactions.Add(new Transaction(node, ofxDocument.Currency));
      }


      /// <summary>
      /// Checks account type of supplied file
      /// </summaryof
      /// <param name="file">OFX file want to check</param>
      /// <returns>Account type for account supplied in ofx file</returns>
      private AccountType GetAccountType(string file)
      {
         if (file.IndexOf("<CREDITCARDMSGSRSV1>") != -1)
            return AccountType.CC;

         if (file.IndexOf("<BANKMSGSRSV1>") != -1)
            return AccountType.BANK;

         throw new OFXException("Unsupported Account Type");
      }

      /// <summary>
      /// Check if OFX file is in SGML or XML format
      /// </summary>
      /// <param name="file"></param>
      /// <returns></returns>
      private bool IsXmlVersion(string file)
      {
         return (file.IndexOf("OFXHEADER:100") == -1);
      }

      /// <summary>
      /// Converts SGML to XML
      /// </summary>
      /// <param name="file">OFX File (SGML Format)</param>
      /// <returns>OFX File in XML format</returns>
      private string SGMLToXML(string file)
      {

            SgmlReader reader = new SgmlReader
            {
                CaseFolding = CaseFolding.None,
                DocType = "OFX",
                InputStream = new StringReader(ParseHeader(file)),
                WhitespaceHandling =  WhitespaceHandling.None ,
                SystemLiteral = "Embeded\\ofx160.dtd"
            };

            string codeBase = Assembly.GetExecutingAssembly().Location;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            
            reader.SetBaseUri(Path.GetDirectoryName(path));

            Func<XmlReader,XmlWriter, XmlWriter> callback = (System.Xml.XmlReader rs, XmlWriter wr) =>
            {
                rs.Read();
                while (!reader.EOF)
                {
                    wr.WriteNode(reader, true);
                }
                
                return wr;
            };
          
            MemoryStream str = new MemoryStream();
            XmlWriter writer = new XmlTextWriter(str, Encoding.GetEncoding("ISO-8859-1")); //encoding da america latina


            var stringWriter = new StringWriter();
            var xmlTextWriter = new XmlTextWriter(stringWriter);
            xmlTextWriter.Formatting = Formatting.Indented;
            
            callback(reader, xmlTextWriter);
            xmlTextWriter.Close();

            // reproduce the parsed document
            var actual = stringWriter.ToString();
            return actual;
      }

      /// <summary>
      /// Checks that the file is supported by checking the header. Removes the header.
      /// </summary>
      /// <param name="file">OFX file</param>
      /// <returns>File, without the header</returns>
      private string ParseHeader(string file)
      {
         //Select header of file and split into array
         //End of header worked out by finding first instance of '<'
         //Array split based of new line & carrige return
         var header = file.Substring(0, file.IndexOf('<'))
            .Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

         //Check that no errors in header
         CheckHeader(header);

         //Remove header
         return "<!DOCTYPE OFX SYSTEM \"ofx160.dtd\">" + file.Substring(file.IndexOf('<') - 1);
      }

      /// <summary>
      /// Checks that all the elements in the header are supported
      /// </summary>
      /// <param name="header">Header of OFX file in array</param>
      private void CheckHeader(string[] header)
      {
         if (header[0] != "OFXHEADER:100")
            throw new OFXParseException("Incorrect header format");

         if (header[1] != "DATA:OFXSGML")
            throw new OFXParseException("Data type unsupported: " + header[1] + ". OFXSGML required");

         if (header[2] != "VERSION:102" && header[2] != "VERSION:103")
            throw new OFXParseException("OFX version unsupported. " + header[2]);
        
         if (header[3] != "SECURITY:NONE")
            throw new OFXParseException("OFX security unsupported");

         if (header[4] != "ENCODING:USASCII")
            throw new OFXParseException("ASCII Format unsupported:" + header[4]);

         if (header[5] != "CHARSET:1252")
            throw new OFXParseException("Charecter set unsupported:" + header[5]);

         if (header[6] != "COMPRESSION:NONE")
            throw new OFXParseException("Compression unsupported");

         if (header[7] != "OLDFILEUID:NONE")
            throw new OFXParseException("OLDFILEUID incorrect");
      }

      #region Nested type: OFXSection

      /// <summary>
      /// Section of OFX Document
      /// </summary>
      private enum OFXSection
      {
         SIGNON,
         ACCOUNTINFO,
         TRANSACTIONS,
         BALANCE,
         CURRENCY
      }

      #endregion
   }
}