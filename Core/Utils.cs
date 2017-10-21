using System;
using System.Data;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace Moses
{
    /// <summary>
    /// Summary description for Utils.
    /// </summary>
    public static class Utils
	{

        /// <summary>
        /// Determina se a sequência é um número inteiro
        /// </summary>
        /// <param name="InNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(this string sequence)
        {
            if (string.IsNullOrEmpty(sequence)) return false;

            using (StringReader reader = new StringReader(sequence))
            {
                int i = 0;

                while ((i = reader.Read()) != -1)
                {
                    if (!char.IsNumber((char)i))
                        return false;
                }

                return true;
            }

            
        }

        public static bool IsNullOrEmpty(this string sequence)
        {
            return string.IsNullOrEmpty(sequence);
        }

        /// <summary>
        /// Determina se a sequência é um número
        /// </summary>
        /// <param name="InNumber"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string sequence)
        {
            if (string.IsNullOrEmpty(sequence)) return false;

            decimal d;
            return decimal.TryParse(sequence,out d);

            //StringReader reader = new StringReader(sequence);

            //int i = 0;
            //bool pointFlag = false;
            //int pointCounter = 0;
            //bool output = false;

            //while ((i = reader.Read()) != -1)
            //{
            //    char c = (char)i;

            //    if (char.IsPunctuation(c)){
            //        //se for a primeira logo, tah fora
            //        output = false;
            //        break;
            //    }

            //    if (!char.IsNumber(c))
            //        if (!char.IsPunctuation(c)) output = false;    
            //        return false;

            //    if ( !char.Equals(',',c) ) 

            //}

            //return output;


        }


        /// <summary>
        /// Pega apenas os algarismos dentro de uma sequência string
        /// </summary>
        /// <returns></returns>
        public static string GetNumbers(this string sequence)
        {
            if (string.IsNullOrEmpty(sequence)) return sequence;

            StringBuilder builder = new StringBuilder();
            using (StringReader reader = new StringReader(sequence))
            {

                int intChar;
                while ((intChar = reader.Read()) != -1)
                {
                    char c = (char)intChar;
                    if (char.IsNumber(c))
                        builder.Append(c);
                }

            }

            return builder.ToString();
        }

        public static DataTable CsvToDataTable(string csvString, bool isRowOneHeader)
        {

            DataTable csvDataTable = new DataTable();

            try
            {
                //no try/catch - add these in yourselfs or let exception happen
                String[] csvData = csvString.Split('\n');

                //if no data in file ‘manually’ throw an exception
                if (csvData.Length == 0)
                {
                    csvDataTable.Dispose();
                    throw new Exception("CSV File Appears to be Empty");
                }

                String[] headings = csvData[0].Trim('\r').Split(',');
                int index = 0; //will be zero or one depending on isRowOneHeader

                if (isRowOneHeader) //if first record lists headers
                {
                    index = 1; //so we won’t take headings as data

                    //for each heading
                    for (int i = 0; i < headings.Length; i++)
                    {
                        //replace spaces with underscores for column names
                        headings[i] = headings[i].Replace(" ", "_");

                        //add a column for each heading
                        csvDataTable.Columns.Add(headings[i], typeof(string));
                    }
                }
                else //if no headers just go for col1, col2 etc.
                {
                    for (int i = 0; i < headings.Length; i++)
                    {
                        //create arbitary column names
                        csvDataTable.Columns.Add("col" + (i + 1).ToString(), typeof(string));
                    }
                }

                //populate the DataTable
                for (int i = index; i < csvData.Length; i++)
                {
                    //create new rows
                    DataRow row = csvDataTable.NewRow();
                    string[] cells = csvData[i].Trim('\r').Split(',');

                    for (int j = 0; j < cells.Length; j++)
                    {
                        //fill them
                        row[j] = cells[j];
                    }

                    //add rows to over DataTable
                    csvDataTable.Rows.Add(row);
                }
            }
            catch (Exception e)
            {
                csvDataTable.Dispose();
                throw e;
            }

            //return the CSV DataTable
            return csvDataTable;

            
        }

        public static DataTable CsvToDataTable(string csvString, bool isRowOneHeader, char separator)
        {
            DataTable csvDataTable = new DataTable();

            try
            {
                //no try/catch - add these in yourselfs or let exception happen
                String[] csvData = csvString.Split('\n');

                //if no data in file ‘manually’ throw an exception
                if (csvData.Length == 0)
                {
                    throw new Exception("CSV File Appears to be Empty");
                }

                String[] headings = csvData[0].Trim('\r').Split(separator);
                int index = 0; //will be zero or one depending on isRowOneHeader

                if (isRowOneHeader) //if first record lists headers
                {
                    index = 1; //so we won’t take headings as data

                    //for each heading
                    for (int i = 0; i < headings.Length; i++)
                    {
                        //replace spaces with underscores for column names
                        headings[i] = headings[i].Replace(" ", "_");

                        //add a column for each heading
                        csvDataTable.Columns.Add(headings[i], typeof(string));
                    }
                }
                else //if no headers just go for col1, col2 etc.
                {
                    for (int i = 0; i < headings.Length; i++)
                    {
                        //create arbitary column names
                        csvDataTable.Columns.Add("col" + (i + 1).ToString(), typeof(string));
                    }
                }

                //populate the DataTable
                for (int i = index; i < csvData.Length; i++)
                {
                    //create new rows
                    DataRow row = csvDataTable.NewRow();
                    string[] cells = csvData[i].Trim('\r').Split(separator);

                    for (int j = 0; j < cells.Length; j++)
                    {
                        //fill them
                        row[j] = cells[j];
                    }

                    //add rows to over DataTable
                    csvDataTable.Rows.Add(row);
                }
            }
            catch (Exception e)
            {
                csvDataTable.Dispose();
                throw e;
            }

            //return the CSV DataTable
            return csvDataTable;


        }

//        public static string ConvertBarCodeEan13(this string inputValue)
//        {
////			string codBarra = null;
////			if (cod.Length < 13)
////			{
////				
////			}
////			else
////			{
////				switch (cod.Substring(0,1))
////				{
////					case "0":codBarra = "!|"; break;
////					case "1":codBarra = "\"|"; break;
////					case "2":codBarra = "#|"; break;
////					case "3":codBarra = "$|"; break;
////					case "4":codBarra = "%|"; break;
////					case "5":codBarra = "%|"; break;
////					case "6":codBarra = " |"; break;
////					case "7":codBarra = "(|"; break;
////					case "8":codBarra = ")|"; break;
////					case "9":codBarra = "*|"; break;
////				}
////				codBarra += cod.Substring(1,1);
////				string codLefthand = cod.Substring(2,5);
////				for (int i=0;codLefthand.Length>i;i++)
////				{
////					switch (codLefthand.Substring(i,1))
////					{
////						case "0":{codBarra += "@";break;}
////						case "1":{codBarra += "A";break;}
////						case "2":{codBarra += "B";break;}
////						case "3":{codBarra += "C";break;}
////						case "4":{codBarra += "D";break;}
////						case "5":{codBarra += "E";break;}
////						case "6":{codBarra += "F";break;}
////						case "7":{codBarra += "G";break;}
////						case "8":{codBarra += "H";break;}
////						case "9":{codBarra += "I";break;}
////					}
////				}
////				codBarra += "|";
////				string codRightHand = cod.Substring(7,6);
////				for (int i=0;codRightHand.Length>i;i++)
////				{
////					switch (codRightHand.Substring(i,1))
////					{
////						case "0":{codBarra += "P";break;}
////						case "1":{codBarra += "Q";break;}
////						case "2":{codBarra += "R";break;}
////						case "3":{codBarra += "S";break;}
////						case "4":{codBarra += "T";break;}
////						case "5":{codBarra += "U";break;}
////						case "6":{codBarra += "V";break;}
////						case "7":{codBarra += "W";break;}
////						case "8":{codBarra += "X";break;}
////						case "9":{codBarra += "Y";break;}
////					}
////				}
////				codBarra += "|";
////			}
////			return codBarra;
			
//            inputValue = LeftZero(inputValue,13);

//            string OutData = "";		//this is the return value of the function
//            string GoodData = "";		//This is the InputValue stripped of invalid characters
//            int LenOfData = 0;			//Length of a string
//            int idx = 0;				//for loop counter
//            int WeightedTotal = 0;		//Weighted Total for check digit
//            int WeightFactor = 0;		//The multiplier to determine the check digit.
//            int iCurrentChar = 0;		//Integer value of current number in InputValue
//            int CheckDigit = 0;			//Ascii value of check digit
//            string EAN5AddOn = "";		//Add on characters for longer EAN symbols
//            string EAN2AddOn = "";		//Add on characters for longer EAN symbols
//            string Encoding = "";		//Encoding format 
//            int LeadingDigit = 0;		//Ascii value of leading digit
//            char[] cInputValue;			//Character array of a string
//            string CurrentEncoding = "";//encoding in looop 
//            string temp = "";			//Temp string 
//            string EANAddOnToPrint = "";//Extended EAN information
//            char EANaddchar;			//Character to add to extended EAN information
//            int iEANaddchar = 0;		//integer value of above character
//            char[] tempArray;			//array to hold character of strings

//            /* Check to make sure data is numeric and remove dashes, etc. */
//            LenOfData=inputValue.Length;
//            for(idx = 0;idx < LenOfData;idx++)
//            {
//                /* Add all numbers to GoodData string */
//                if((IsNumber(inputValue.Substring(idx,1))) == true)
//                    GoodData = GoodData + inputValue.Substring(idx,1);
//            }

//            /* DataToEncode = OnlyCorrectData */
//            /* Remove check digits if they added one */
//            //The value passed in will only be 12, 13, 15, or 18 digits.  The 
//            LenOfData=GoodData.Length;
//            if(LenOfData == 13)  //we have the 12 digit EAN plus a check digit
//                GoodData = GoodData.Substring(0, 12);
//            else if(LenOfData == 15) //we have the 12 digit EAN plus a check digit plus a 2 digit add on
//                GoodData = GoodData.Substring(0, 12) + GoodData.Substring(13,2);
//            else if(LenOfData == 18) //we have the 12 digit EAN plus a check digit plus a 5 digit add on
//                GoodData = GoodData.Substring(0, 12) + GoodData.Substring(13,5);

//            //Now based on the new length of Good Data we can determine what the Add on value is.
//            /* End sub if incorrect number */
//            if(GoodData.Length == 17)
//                //Check digit has already been eliminated so this is just the EAN code and the 5 digit add on
//                EAN5AddOn = GoodData.Substring(12,5);
//            else if(GoodData.Length == 14)
//                //Check digit has already been eliminated so this is just the EAN code and the 2 digit add on
//                EAN2AddOn = GoodData.Substring(12,2);
			
//            //Now set GoodData to only the EAN code (the first 12 characters
//            GoodData = GoodData.Substring(0,12);

//            /* ' */
//            WeightFactor = 3;
//            WeightedTotal = 0;

//            /* <<<< Calculate Check Digit >>>> */
//            /* Get the value of each number starting at the end */
//            LenOfData=GoodData.Length;
//            cInputValue = GoodData.ToCharArray();

//            for(idx = (LenOfData - 1);idx >= 0;idx--)
//            {
//                /* Get the ascii value of each number starting at the end */
//                iCurrentChar = ((int) cInputValue[idx]) - 48;
//                /* multiply by the weighting factor which is 3,1,3,1... */
//                /* and add the sum together */
//                WeightedTotal = WeightedTotal + (iCurrentChar * WeightFactor);
//                /* change factor for next calculation */
//                WeightFactor = 4 - WeightFactor;
//            }

//            /* Find the CheckDigitValue by finding the number + weightedTotal that = a multiple of 10 */
//            /* divide by 10, get the remainder and subtract from 10 */
//            CheckDigit = WeightedTotal % 10;
//            if(CheckDigit != 0)
//                CheckDigit = (10 - CheckDigit);
//            else
//                CheckDigit = 0;

//            /* Now we must encode the leading digit into the left half of the EAN-13 symbol */
//            /* by using variable parity between character sets A and B */
//            cInputValue = GoodData.ToCharArray();

//            //LeadingDigit = ((int) cInputValue[0] ) - 48 ;
//            LeadingDigit = Convert.ToInt32(GoodData.Substring(0,1));

//            switch(LeadingDigit)
//            {
//                case 0:
//                    Encoding = "AAAAAACCCCCC";
//                    break;
//                case 1:
//                    Encoding = "AABABBCCCCCC";
//                    break;
//                case 2:
//                    Encoding = "AABBABCCCCCC";
//                    break;
//                case 3:
//                    Encoding = "AABBBACCCCCC";
//                    break;
//                case 4:
//                    Encoding = "ABAABBCCCCCC";
//                    break;
//                case 5:
//                    Encoding = "ABBAABCCCCCC";
//                    break;
//                case 6:
//                    Encoding = "ABBBAACCCCCC";
//                    break;
//                case 7:
//                    Encoding = "ABABABCCCCCC";
//                    break;
//                case 8:
//                    Encoding = "ABABBACCCCCC";
//                    break;
//                case 9:
//                    Encoding = "ABBABACCCCCC";
//                    break;
//            }  //end switch addresseing encoding type

//            /* add the check digit to the end of the barcode & remove the leading digit */
//            GoodData = GoodData.Substring(1,11) + (CheckDigit.ToString());

//            /* Now that we have the total number including the check digit, determine character to print */
//            /* for proper barcoding: */
//            LenOfData = GoodData.Length;
//            cInputValue = GoodData.ToCharArray();

//            for(idx = 0;idx < LenOfData;idx++)
//            {
//                /* Get the ASCII value of each number excluding the first number because */
//                /* it is encoded with variable parity */
//                iCurrentChar = (int) cInputValue[idx];
//                CurrentEncoding = Encoding.Substring(idx,1);

//                /* Print different barcodes according to the location of the CurrentChar and CurrentEncoding */
//                switch(CurrentEncoding)
//                {
//                    case "A":
//                        OutData = OutData + cInputValue[idx];
//                        break;
//                    case "B":
//                        iCurrentChar = iCurrentChar + 17;
//                        OutData = OutData + ((char) iCurrentChar);
//                        break;

//                    case "C":
//                        iCurrentChar = iCurrentChar + 27;
//                        OutData = OutData + ((char) iCurrentChar);
//                        break;
//                    default:
//                        break;
//                } //end switch to checking encoding type
//                /* add in the 1st character along with guard patterns */
//                switch(idx)
//                {
//                    case 0:
//                        /* For the LeadingDigit print the human readable character, */
//                        /* the normal guard pattern and then the rest of the barcode */
//                        //This is the character we will put before OutData
//                        if(LeadingDigit>4)
//                        {
//                            temp = LeadingDigit.ToString();
//                            tempArray = temp.ToCharArray();
//                            iCurrentChar = ((int) tempArray[0]);
//                            iCurrentChar = iCurrentChar + 64;
//                            OutData = ((char) iCurrentChar) + "(" + OutData;
//                        }
//                        else if(LeadingDigit<5)
//                        {
//                            temp = LeadingDigit.ToString();
//                            tempArray = temp.ToCharArray();
//                            iCurrentChar = ((int) tempArray[0]);
//                            iCurrentChar = iCurrentChar + 37;
//                            OutData = ((char) iCurrentChar) + "(" + OutData;
//                        }
						
//                        break;
//                    case 5:
//                        /* Print the center guard pattern after the 6th character */
//                        OutData = OutData + "*";
//                        break;
//                    case 11:
//                        /* For the last character (12) print the the normal guard pattern */
//                        /* after the barcode */
//                        OutData = OutData + "(";
//                        break;
//                    default:
//                        break;
//                }  //end switch for determining main output for EAN code
				
//            }  //end for loop that encodes the data


//            /* Process 5 digit add on if it exists */
//            if(EAN5AddOn.Length == 5)
//            {
//                /* Get check digit for add on */
//                WeightFactor=3;
//                WeightedTotal=0;
//                for(idx = (EAN5AddOn.Length - 1); idx >= 0; idx--)
//                {
//                    /* Get the value of each number starting at the end */
//                    iCurrentChar = Convert.ToInt32(EAN5AddOn.Substring(idx,1));

//                    /* multiply by the weighting factor which is 3,9,3,9. */
//                    /* and add the sum together */
//                    if(WeightFactor==3)
//                    {
//                        WeightedTotal = WeightedTotal + (iCurrentChar * 3);
//                    }
//                    if(WeightFactor==1)
//                    {
//                        WeightedTotal = WeightedTotal + (iCurrentChar * 9);
//                    }
//                    /* change factor for next calculation */
//                    WeightFactor = 4 - WeightFactor;
//                }  //end check digit for loop for EAN 5 character add on

//                /* Find the CheckDigit by extracting the right-most number from weightedTotal */
//                temp = WeightedTotal.ToString();
//                CheckDigit = Convert.ToInt32(temp.Substring(temp.Length - 1,1));

//                /* Now we must encode the add-on CheckDigit into the number sets */
//                /* by using variable parity between character sets A and B */
//                switch(CheckDigit)
//                {
//                    case 0:
//                        Encoding = "BBAAA";
//                        break;
//                    case 1:
//                        Encoding = "BABAA";
//                        break;
//                    case 2:
//                        Encoding = "BAABA";
//                        break;
//                    case 3:
//                        Encoding = "BAAAB";
//                        break;
//                    case 4:
//                        Encoding = "ABBAA";
//                        break;
//                    case 5:
//                        Encoding = "AABBA";
//                        break;
//                    case 6:
//                        Encoding = "AAABB";
//                        break;
//                    case 7:
//                        Encoding = "ABABA";
//                        break;
//                    case 8:
//                        Encoding = "ABAAB";
//                        break;
//                    case 9:
//                        Encoding = "AABAB";
//                        break;
//                } //end switch for EAN 5 encoding type


//                /* Now that we have the total number including the check digit, determine character to print */
//                /* for proper barcoding: */
//                LenOfData = EAN5AddOn.Length;
//                EANAddOnToPrint = "";
//                for(idx = 0;idx < LenOfData;idx++)
//                {
//                    /* Get the value of each number */
//                    /* it is encoded with variable parity */
//                    iCurrentChar = Convert.ToInt32(EAN5AddOn.Substring(idx,1));
//                    CurrentEncoding = Encoding.Substring(idx,1);

//                    /* Print different barcodes according to the location of the CurrentChar and CurrentEncoding */
//                    if(CurrentEncoding == "A")
//                    {
//                        switch (iCurrentChar)
//                        {
//                            case 0:
//                                iEANaddchar = 34;
//                                break;
//                            case 1:
//                                iEANaddchar = 35;
//                                break;
//                            case 2:
//                                iEANaddchar = 36;
//                                break;
//                            case 3:
//                                iEANaddchar = 37;
//                                break;
//                            case 4:
//                                iEANaddchar = 38;
//                                break;
//                            case 5:
//                                iEANaddchar = 44;
//                                break;
//                            case 6:
//                                iEANaddchar = 46;
//                                break;
//                            case 7:
//                                iEANaddchar = 47;
//                                break;
//                            case 8:
//                                iEANaddchar = 58;
//                                break;
//                            case 9:
//                                iEANaddchar = 59;
//                                break;
							
//                        } //end character switch for EAN 5 character for encoding A
//                        EANaddchar = (char) iEANaddchar;
//                        EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                    } //End if Encoding A for EAN 5 character add on
//                    else if(CurrentEncoding == "B")
//                    {
//                        switch (iCurrentChar)
//                        {
//                            case 0:
//                                iEANaddchar = 122;
//                                break;
//                            case 1:
//                                iEANaddchar = 61;
//                                break;
//                            case 2:
//                                iEANaddchar = 63;
//                                break;
//                            case 3:
//                                iEANaddchar = 64;
//                                break;
//                            case 4:
//                                iEANaddchar = 91;
//                                break;
//                            case 5:
//                                iEANaddchar = 92;
//                                break;
//                            case 6:
//                                iEANaddchar = 93;
//                                break;
//                            case 7:
//                                iEANaddchar = 95;
//                                break;
//                            case 8:
//                                iEANaddchar = 123;
//                                break;
//                            case 9:
//                                iEANaddchar = 125;
//                                break;
							
//                        }//End switch for EAN5 add on w/ B encoding
//                        EANaddchar = (char) iEANaddchar;
//                        EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                    } //end else for Encoding B for EAN add on 5 characters
//                    /* add in the space & add-on guard pattern */
//                    switch (idx)
//                    {
//                        case 0:
//                            iEANaddchar = 43;
//                            EANaddchar = (char) iEANaddchar;
//                            EANAddOnToPrint = EANaddchar + EANAddOnToPrint;
//                            iEANaddchar = 32;
//                            EANaddchar = (char) iEANaddchar;
//                            EANAddOnToPrint = EANaddchar + EANAddOnToPrint;
//                            iEANaddchar = 33;
//                            EANaddchar = (char) iEANaddchar;
//                            EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                            break;
//                        case 1:
//                            iEANaddchar = 33;
//                            EANaddchar = (char) iEANaddchar;
//                            EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                            break;
//                        case 2:
//                            iEANaddchar = 33;
//                            EANaddchar = (char) iEANaddchar;
//                            EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                            break;
//                        case 3:
//                            iEANaddchar = 33;
//                            EANaddchar = (char) iEANaddchar;
//                            EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                            break;
//                        case 4:
//                            //do nothing
//                            break;
//                    }  //end add character switch
//                } //end for loop to determine encoding characters
//            } //end EAN length = 5

//            /* Process 2 digit add on if it exists */
//            if(EAN2AddOn.Length == 2)
//            {
//                //Get the actual value of the EAN2 add on
//                iEANaddchar = Convert.ToInt32(EAN2AddOn);
				
//                /* Get encoding for add on */
//                for(idx = 0;idx <= 99;idx = idx + 4)
//                {
//                    if(iEANaddchar == idx)
//                        Encoding = "AA";
//                    else if(iEANaddchar == (idx + 1))
//                        Encoding = "AB";
//                    else if(iEANaddchar == (idx + 2))
//                        Encoding = "BA";
//                    else if(iEANaddchar == (idx + 3))
//                        Encoding = "BB";	
//                } //end for loop determining encoding type

//                /* Now that we have the total number including the encoding */
//                /* determine what to print */
//                LenOfData = EAN2AddOn.Length;

//                for(idx = 0;idx < LenOfData;idx++)
//                {
//                    /* Get the value of each number */
//                    /* it is encoded with variable parity */
//                    iCurrentChar = Convert.ToInt32(EAN2AddOn.Substring(idx,1));
//                    CurrentEncoding = Encoding.Substring(idx,1);

//                    /* Print different barcodes according to the location of the CurrentChar and CurrentEncoding */
//                    if(CurrentEncoding == "A")
//                    {
//                        switch (iCurrentChar)
//                        {
//                            case 0:
//                                iEANaddchar = 34;
//                                break;
//                            case 1:
//                                iEANaddchar = 35;
//                                break;
//                            case 2:
//                                iEANaddchar = 36;
//                                break;
//                            case 3:
//                                iEANaddchar = 37;
//                                break;
//                            case 4:
//                                iEANaddchar = 38;
//                                break;
//                            case 5:
//                                iEANaddchar = 44;
//                                break;
//                            case 6:
//                                iEANaddchar = 46;
//                                break;
//                            case 7:
//                                iEANaddchar = 47;
//                                break;
//                            case 8:
//                                iEANaddchar = 58;
//                                break;
//                            case 9:
//                                iEANaddchar = 59;
//                                break;
//                        }  //end switch encoding A
//                        EANaddchar = (char) iEANaddchar;
//                        EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                    }//End if Encoding A
//                    else if(CurrentEncoding == "B")
//                    {
//                        switch (iCurrentChar)
//                        {
//                            case 0:
//                                iEANaddchar = 122;
//                                break;
//                            case 1:
//                                iEANaddchar = 61;
//                                break;
//                            case 2:
//                                iEANaddchar = 63;
//                                break;
//                            case 3:
//                                iEANaddchar = 64;
//                                break;
//                            case 4:
//                                iEANaddchar = 91;
//                                break;
//                            case 5:
//                                iEANaddchar = 92;
//                                break;
//                            case 6:
//                                iEANaddchar = 93;
//                                break;
//                            case 7:
//                                iEANaddchar = 95;
//                                break;
//                            case 8:
//                                iEANaddchar = 123;
//                                break;
//                            case 9:
//                                iEANaddchar = 125;
//                                break;
							
//                        } //end switch encoding B
//                        EANaddchar = (char) iEANaddchar;
//                        EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                    }  //end else if for encoding B
//                    /* add in the space & add-on guard pattern */
//                    if(idx == 0)
//                    {
//                        iEANaddchar = 43;
//                        EANaddchar = (char) iEANaddchar;
//                        EANAddOnToPrint = EANaddchar + EANAddOnToPrint;
						
//                        iEANaddchar = 33;
//                        EANaddchar = (char) iEANaddchar;
//                        EANAddOnToPrint = EANAddOnToPrint + EANaddchar;
//                    }  //end if idx ==1
					
//                }//End for loop through characters for EAN Add of length 2
//            } //End if EANAdd On Length is 2


//            /* Get OutData String */
//            OutData = OutData + EANAddOnToPrint;
//            /* Return OutData */
//            return OutData;
//        }//End EAN13


        public static string GetDialogFilter(DialogFilterOptions o)
        {
            string filtro = "";
            switch (o)
            {
                case DialogFilterOptions.Images:
                    {
                        filtro = "Arquivos Bitmap (*.bmp)|*.bmp|Arquivos JPEG (*.jpg)|*.jpg";
                        break;
                    }
                case DialogFilterOptions.Txt:
                    {
                        filtro = "Arquivos Texto (*.txt)|*.txt";
                        break;
                    }
                case DialogFilterOptions.Xml:
                    {
                        filtro = "Arquivos XML (*.xml)|*.xml";
                        break;
                    }
            }
            return filtro;
        }

        public static string GetSexo(string indSexo)
        {
            switch (indSexo)
            {
                case "M":
                    return "Masculino";
                    
                case "F":
                    return "Feminino";

                default:
                    return "";
            }
        }

        public static DateTime? Latest(DateTime? dateTime1, DateTime? dateTime2)
        {
            if (!dateTime1.HasValue && !dateTime2.HasValue)
            {
                return null;
            }
            else if ( !dateTime2.HasValue)
            {
                return dateTime1;
            }
            else if (!dateTime1.HasValue)
            {
                return dateTime2;
            }

            if (dateTime1 > dateTime2)
            {
                return dateTime1;
            }
            else
            {
                return dateTime2;
            }
        }
    }
}
