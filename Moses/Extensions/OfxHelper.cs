using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

namespace Moses.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>
    /// Rodrigo Diniz / Joe Mcbride
    /// </author>
    public static class OfxHelper
    {
        /// <summary>
        /// Parses the ofx file data and turns it into valid xml.
        /// </summary>
        /// <param name="ofxData">The ofx data.</param>
        /// <returns>An xml representation of the ofx data.</returns>
        public static XElement OfxParseToXElement(this string ofxData)
        {
            // add additional returns to allow for parsing
            // ofx files that are all on one line
            string data = ofxData.Replace("<", "\n\r<");

            string[] lines = data.Split(
                    new string[] { "\n", "\r" },
                    StringSplitOptions.RemoveEmptyEntries);

            // use linq to get the organization and account tags
            var orgAndAccountTags = from line in lines
                                    where line.Contains("<ORG>")
                                    || line.Contains("<BANKID>")
                                    || line.Contains("<BRANCHID>")
                                    || line.Contains("<ACCTID>")
                                    || line.Contains("<ACCTTYPE>")
                                    select line;

            // use linq to get the transaction tags
            var transactionTags = from line in lines
                                  where line.Contains("<STMTTRN>")
                                  || line.Contains("<TRNTYPE>")
                                  || line.Contains("<DTPOSTED>")
                                  || line.Contains("<TRNAMT>")
                                  || line.Contains("<FITID>")
                                  || line.Contains("<CHECKNUM>")
                                  || line.Contains("<NAME>")
                                  || line.Contains("<MEMO>")
                                  select line;

            XElement root = new XElement("OFX");

            // parse organization and account data
            foreach (var line in orgAndAccountTags)
            {
                var tagName = GetTagName(line);
                var elementChild = new XElement(tagName)
                {
                    Value = GetTagValue(line)
                };

                root.Add(elementChild);
            }

            // parse transactions
            XElement transactions = new XElement("BANKTRANLIST");

            root.Add(transactions);

            XElement child = null;

            foreach (var line in transactionTags)
            {
                if (line.IndexOf("<STMTTRN>") != -1)
                {
                    child = new XElement("STMTTRN");
                    transactions.Add(child);
                    continue;
                }

                var tagName = GetTagName(line);
                var elementChild = new XElement(tagName)
                {
                    Value = GetTagValue(line)
                };

                child.Add(elementChild);
            }

            return root;
        }

        /// <summary>
        /// Get the Tag name to create an Xelement
        /// </summary>
        /// <param name="line">One line from the file</param>
        /// <returns></returns>
        private static string GetTagName(string line)
        {
            int pos_init = line.IndexOf("<") + 1;
            int pos_end = line.IndexOf(">");
            pos_end = pos_end - pos_init;
            return line.Substring(pos_init, pos_end);
        }

        /// <summary>
        /// Get the value of the tag to put on the Xelement
        /// </summary>
        /// <param name="line">The line</param>
        /// <returns></returns>
        private static string GetTagValue(string line)
        {
            int pos_init = line.IndexOf(">") + 1;
            string retValue = line.Substring(pos_init).Trim();

            // the date contains a time zone offset
            if (retValue.IndexOf("[") != -1)
            {
                // Date - get only 8 date digits
                retValue = retValue.Substring(0, 8);
            }

            // if the value is exactly 18 digits and the 14th digit is a dot
            // this should be a date - trim it
            if (retValue.Length == 18 && retValue.IndexOf(".") == 14)
            {
                // Date - get only 8 date digits
                retValue = retValue.Substring(0, 8);
            }

            return retValue;

        }

        public static OfxAccountDTO OfxTranslateToDTO(this XElement element)
        {
            var accountID = element.Element("ACCTID");
            var accountType = element.Element("ACCTTYPE");
            var bankID = element.Element("BANKID");
            var organization = element.Element("ORG");
            OfxAccountDTO account = new OfxAccountDTO
            {
                Organization = organization != null ? organization.Value : null,
                AccountID = accountID != null ? accountID.Value : null,
                AccountType = accountType != null ? accountType.Value : null,
                BankID = bankID != null ? bankID.Value : null
            };

            account.Transactions =
                (from c in element.Descendants("STMTTRN")
                 let name = c.Element("NAME")
                 let memo = c.Element("MEMO")
                 select new OfxTransactionDTO
                 {
                     TransactionID = c.Element("FITID").Value,
                     TransactionType = c.Element("TRNTYPE").Value,
                     Date = DateTime.ParseExact(
                                        c.Element("DTPOSTED").Value,
                                        "yyyyMMdd",
                                        null),
                     Amount = decimal.Parse(
                                     c.Element("TRNAMT").Value.Replace("-", ""),
                                     NumberFormatInfo.InvariantInfo),
                     Name = name != null ? name.Value : null,
                     Memo = memo != null ? memo.Value : null
                 });

            return account;
        }

        public static OfxAccountDTO OfxParse(this string ofxFileData)
        {
            XElement root = ofxFileData.OfxParseToXElement();
            OfxAccountDTO account = root.OfxTranslateToDTO();

            return account;
        }


    }

    /// <summary>
    /// After the ImportOfx class parses the data we’re now ready to convert it to simple DTOs.  Here are the DTOs I have created and the transform I used.  Notice that some of the tags are allowed to be null.
    /// </summary>
    public class OfxAccountDTO
    {
        public string Organization { get; set; }
        public string BankID { get; set; }
        public string AccountID { get; set; }
        public string AccountType { get; set; }
        public IEnumerable<OfxTransactionDTO> Transactions { get; set; }
    }
    public class OfxTransactionDTO
    {
        public string TransactionID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }

    }
}
