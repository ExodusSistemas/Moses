using System;
using System.Data;
using System.Globalization;
using System.Xml;

namespace Moses.Import
{
   public class CieloTransaction
   {
       //Data de Credito
      public DateTime DueDate { get; set; }
       
       //Data de Apresentação
      public DateTime Date { get; set; }

      //Valor Bruto
      public decimal Amount { get; set; }

       //Valor Líquido
       public decimal NetAmount {get;set;}

       //Descrição
      public string Name { get; set; }

       //Bandeira
       public string CardBrand {get;set;}

      public CieloTransaction()
      {
      }

      public CieloTransaction(DataRow csvLine)
      {
          //por enquanto vai ser hardcoded
          try
          {
              DueDate = Convert.ToDateTime(csvLine[0].ToString().Replace('\"', ' ').Trim());

              Date = Convert.ToDateTime(csvLine[1].ToString().Replace('\"', ' ').Trim());

              Name = Convert.ToString(csvLine[3].ToString().Replace('\"', ' ').Trim());

              Amount = Convert.ToDecimal(csvLine[6].ToString().Replace('\"', ' ').Replace('.',',').Trim());

              NetAmount = Convert.ToDecimal(csvLine[9].ToString().Replace('\"', ' ').Replace('.', ',').Trim());

              CardBrand = Convert.ToString(csvLine[11].ToString().Replace('\"',' ').Trim());

          }
          catch { }

      }


   }
}