using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SgmlCore;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Data;

namespace Moses.Import
{
   public class CieloDocumentParser
   {
      public CieloDocument Import(FileStream stream)
      {
         using (var reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1") ) )
         {
            return Import(reader.ReadToEnd().Replace(',','.') ); //replace é para o santander
         }
      }

      public CieloDocument Import(string cielo)
      {
         return ParseCieloDocument(cielo);
      }

      private CieloDocument ParseCieloDocument(string cieloString)
      {
         //Lê o arquivo csv
          DataTable dt = Moses.Utils.CsvToDataTable(cieloString,false,';');
          CieloDocument cielo = new CieloDocument();
          ImportTransations(cielo, dt);




          return cielo;
      }


      /// <summary>
      /// Importa as transações de um relatório da cielo
      /// </summary>
      /// <param name="csv">conteudo csv do relatorio</param>
      /// <returns>List of transactions found in OFX document</returns>
      private void ImportTransations(CieloDocument cieloDocument, DataTable csv)
      {
         string periodo = csv.Rows[0][0].ToString().Replace('\"',' ').Trim();
         string[] periodos = periodo.Split(':')[1].Split('a');
          
         cieloDocument.StatementStart = Convert.ToDateTime(periodos[0].Trim());
         cieloDocument.StatementEnd = Convert.ToDateTime(periodos[1].Trim());


         //tira o cabecalho
         var transactionNodes = ParseHeader(csv).Rows; //nao funcionou

         cieloDocument.Transactions = new List<CieloTransaction>();
         foreach (DataRow dr in transactionNodes)
         {
             try
             {
                 cieloDocument.Transactions.Add(new CieloTransaction(dr));
             }
             catch 
             {
                 continue;
             }
         }
          
      }


      /// <summary>
      /// Verifica se o arquivo é o relatório da cielo. Remove o cabeçalho.
      /// </summary>
      /// <param name="csv">arquivo da cielo em datatable</param>
      /// <returns>tabela sem o cabecalho</returns>
      private DataTable ParseHeader(DataTable csv)
      {
         //Pega o cabe'calho
         var linha1 = csv.Rows[0];
          var linha2 = csv.Rows[1];
          var linha3 = csv.Rows[2];

          //var cabecalho = csv.
          //cabecalho.Rows.Clear();
          //cabecalho.Rows.Add(linha1);
          //cabecalho.Rows.Add(linha2);
          //cabecalho.Rows.Add(linha3);

         //Verifica os erros
         //CheckHeader(cabecalho);

         //Remove 
         //Remove as linhas de cabeçalho
         csv.Rows.RemoveAt(0);
         csv.Rows.RemoveAt(0);
         csv.Rows.RemoveAt(0);

         return csv;
      }


      private void CheckHeader(DataTable header)
      {
         
      }


   }
}