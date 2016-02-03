using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using System.Globalization;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Common;
using System.Web;
using System.Web.Configuration;
using NHibernate;

namespace CustodianLife.Data
{
    public class hashHelper
    {
        private static ISession GetSession()
        {
            return SessionProvider.SessionFactory.OpenSession();
        }

        public static MyErrorListing errmsgs = new MyErrorListing();

        public static string CreateSalt(int size)
        {
            // Generate a cryptographic random number using the cryptographic
            // service provider
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        public static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd =
                  FormsAuthentication.HashPasswordForStoringInConfigFile(
                                                       saltAndPwd, "SHA1");
            return hashedPwd;
        }


        //public static bool VerifyPassword(Object _user, string suppliedPassword, string objType)
        //{
        //    bool passwordMatch = false;
        //    // Get the salt and pwd from the database based on the user name.
        //    string dbPasswordHash = String.Empty;
        //    string salt = String.Empty;
        //    //objMembersCred memCred;
        //    //objUserCredentials userCred;
        ////    switch (objType)
        //    {
        //        case "member":
        //            memCred = (objMembersCred)_user;
        //            dbPasswordHash = memCred.PassWordHash;
        //            salt = memCred.Salt;
        //            break;
        //        case "admin":
        //            userCred = (objUserCredentials)_user;
        //            dbPasswordHash = userCred.PassWordHash;
        //            salt = userCred.Salt;
        //            break;

        //    }
        //    try
        //    {
        //        // Now take the salt and the password entered by the user
        //        // and concatenate them together.
        //        string passwordAndSalt = String.Concat(suppliedPassword, salt);
        //        // Now hash them
        //        string hashedPasswordAndSalt =
        //                   FormsAuthentication.HashPasswordForStoringInConfigFile(
        //                                                   passwordAndSalt, "SHA1");
        //        // Now verify them.
        //        passwordMatch = hashedPasswordAndSalt.Equals(dbPasswordHash);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("ERROR verifying password: " + ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return passwordMatch;
        //}

        public static DateTime GoodDate(string yr, string mth, string dy)
        {
            string strDte = yr + "/" + mth + "/" + dy;
            return checkDate(strDte);
        }
        public static DateTime GoodDate(string mydate)
        {
            return moveDateToCulture(mydate);
        }

        private static DateTime checkDate(string dte)
        {
            string pattern = "dd/MM/yyyy";
            DateTime myDate = DateTime.Today;
            try
            {
                // myDate = Convert.ToDateTime(dte);
                //IFormatProvider provider = new System.Globalization.CultureInfo("en-GB", true);
                // myDate = DateTime.ParseExact(dte, pattern, provider);
                myDate = Convert.ToDateTime(dte);
                //string strDate = moveDateToCulture(dte);


            }
            catch (Exception c)
            {
                throw new Exception("Invalid Date!");
            }
            return myDate;

        }

        public static string removeDateSeperators(DateTime dte)
        {
            string dy = dte.Day.ToString();
            string myday = dte.Day.ToString();
            string mt = dte.Month.ToString();
            string mth = dte.Month.ToString();
            if (mt.Length == 1)
                mth = "0" + mt;
            if (dy.Length == 1)
                myday = "0" + dy;

            string ky = dte.Year.ToString() + mth + myday;
            return ky;
        }

        public static DateTime moveDateToCulture(String dte)
        {
            //change date from dd/mm/yyyy to mm/dd/yyyy to achieve consonance with the server.
            //It seems that settings from the c# dll calls for date format is changed when it gets to VB.net client. 
            //This is still subject to research before final conclusion though.
            string[] dateparts = dte.Split('/');
            Int16 dy = Convert.ToInt16(dateparts[0]);
            Int16 mt = Convert.ToInt16(dateparts[1]);
            Int16 ky = Convert.ToInt16(dateparts[2]);
            System.DateTime dateInMay = new System.DateTime(ky, mt, dy, 0, 0, 0);
            //String myDate = mt + "/" + dy + "/" + ky;
            return dateInMay;
        }

        public static String DateToServerSetting(String dte)
        {
            //change date from dd/mm/yyyy to mm/dd/yyyy to achieve consonance with the server.
            string[] dateparts = dte.Split('/');
            Int16 dy = Convert.ToInt16(dateparts[0]);
            Int16 mt = Convert.ToInt16(dateparts[1]);
            Int16 ky = Convert.ToInt16(dateparts[2]);
            String myDate = mt + "/" + dy + "/" + ky;
            return myDate;
        }

        public static String DateFromServerSetting(String dte)
        {
            //change date from mm/dd/yyyy to dd/mm/yyyy to achieve consonance with the client.
            string[] dateparts = dte.Split('/');
            Int16 dy = Convert.ToInt16(dateparts[0]);
            Int16 mt = Convert.ToInt16(dateparts[1]);
            Int16 ky = Convert.ToInt16(dateparts[2]);
            String myDate = dy + "/" + mt + "/" + ky;
            return myDate;
        }

        public static Double RealNumberNoSpaces(string str)
        {
            if (str == "")
                return 0;
            return Math.Round(Convert.ToDouble(str), 2);

        }
        public static DateTime RealDateNoSpaces(string str)
        {
            if (str == "")
                return DateTime.Now;
            return Convert.ToDateTime(str);

        }

        public static void postFromExcel(String _uploadpath, String _filename, String _username,
          String _minRange, String _maxRange, string _connstring, ref List<String> _err_msg, string _data_source_sw, int BatchDate, string _entry_Date)
        {
            string strMyYear = "";
            string strMyMth = "";
            string strMyDay = "";
            string strMyDte = "";
            string mydteX = "";
            DateTime mydte = DateTime.Now;
            string sFT = "";
            float nRow = 0;
            int nCol = 1;
            int nROW_MIN = int.Parse(_minRange);
            int nROW_MAX = int.Parse(_maxRange);
            string xx = "";
            long my_intCNT = 0;
            string my_SNo = "";
            var errmsg = new List<String>();

            int Sno = 0;
            string Pay_Ref_No = "";
            string Pay_Date_Time = "";
            DateTime Pay_Date_Time1 = DateTime.Now;
            string PolNo_PhoneNo = "";
            string Receipt_No = "";
            string Cust_Name = "";
            string PolNo_PhoneNo1 = "";
            string Payment = "";
            double Amount = 0;
            string Pay_Mtd = "";
            string DepSlip_CardNo = "";
            string Cheq_Val_Date = "";
            DateTime Cheq_Val_Date1 = DateTime.Now;
            string Bank = "";
            string Trans_Ref = "";
            string Coll_Acct = "";
            string Narration = "";
            string strGen_Msg = String.Empty;
            string entry_date = String.Empty;
            string paydatetest;
            string cheq_val_datetest;
            string[] myarrData;
            string currentDay;
            string currentMonth;
            string currentYear;
            string Bank_Code = "";

            CultureInfo cu = new CultureInfo("en-GB");
            DateTime dte = DateTime.Today;
            string mystr_sn_param = "";
            mystr_sn_param = "GL_MEMBER_SN";
            int mycnt = 0;
            int tenor;
            SqlCommand myole_cmd = null;
            string mystr_sql = "";


            if (_entry_Date == String.Empty)
                entry_date = "getdate()";
            else
                entry_date = "'" + _entry_Date + "'";

            using (var session = GetSession())
            {
                using (var conn = session.Connection as SqlConnection)
                {
                    //Below block of code were commented bcos they are not needed
                    /*
                    //_connstring = conn.ConnectionString;
                    //_connstring = "Provider=SQLOLEDB;" + _connstring;
                    //OleDbConnection myole_con = null;
                    ////OleDbCommand myole_cmd = null;
                    //SqlCommand myole_cmd = null;
                    //mystr_con = _connstring;
                    //myole_con = new OleDbConnection(mystr_con);
                    //// remove the 'provider' attribute from the connection string becsuse it is for connection to oledb providers but not for sqlclient
                    //string[] connParts = { };
                    //connParts = _connstring.Split(';');
                    //string mySQLClientConn = connParts[1] + ';' + connParts[2] + ';' + connParts[3] + ';' + connParts[4] + ';';
                     */
                    String ftm = "Y";
                    String ftime = "Y";
                    sFT = "Y";
                    //obtain the parameters
                    string wksheetName = "[RECEIPTS$]"; // _filename.ToString(); // _criteria.Substring(3, _criteria.Length - 3);

                    string uploadedfilepath = _uploadpath + _filename;
                    string ext_property = "Excel 8.0;HDR=YES;IMEX=1;";
                    string connectionStringExcel = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploadedfilepath.ToString() + ";Extended Properties='" + ext_property + "'";
                    string connectionStringMySql = String.Empty;

                    //Not needed
                    //connectionStringMySql = mySQLClientConn;

                    DbProviderFactory factoryExcel = DbProviderFactories.GetFactory("System.Data.OleDb");
                    DbProviderFactory factorySQL = DbProviderFactories.GetFactory("System.Data.SqlClient");
                    SqlConnection connectionSQL = new SqlConnection();
                    //Assign the connection converted form Nhibernate to Sqlconnection into connectionSQL
                    connectionSQL = conn;

                    using (DbConnection connectionExcel = factoryExcel.CreateConnection())
                    {
                        connectionExcel.ConnectionString = connectionStringExcel;
                        using (DbCommand commandExcel = connectionExcel.CreateCommand())
                        {
                            SqlCommand commandSQL;
                            commandExcel.CommandText = "SELECT "
                                                       + wksheetName + ".*"
                                                       + " FROM " + wksheetName;

                        MyLoop_Start:

                            try
                            {
                                connectionExcel.Open();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("ERROR!: " + ex.Message);
                            }


                            using (DbDataReader dr = commandExcel.ExecuteReader())
                            {

                                try
                                {
                                    int r;
                                    int cnt = 0;

                                    if (connectionSQL.State == ConnectionState.Closed)
                                    {
                                        connectionSQL.Open();
                                    }
                                    while (dr.Read())
                                    {

                                        nRow += 1; //row
                                        if ((nRow < nROW_MIN))
                                        {
                                            //goto MyLoop_Start;

                                            break;

                                        }
                                        if ((nRow > nROW_MAX))
                                        {
                                            //goto MyLoop_999;
                                            break;
                                        }
                                        if (ftm == "Y")
                                        {
                                            ftm = "N";

                                            nRow -= 1;
                                        }


                                        //test and validate fields before inserting into the database
                                        //validation code
                                        Sno = Convert.ToInt32(dr["Sno"]);
                                        Pay_Ref_No = dr["Pay_Ref_No"].ToString();
                                        // Validate DOB

                                        Pay_Date_Time = String.Format("{0:dd/MM/yyyy}", dr["Pay_Date_Time"].ToString());
                                        PolNo_PhoneNo = dr["PolNo_PhoneNo"].ToString();
                                        Receipt_No = dr["Receipt_No"].ToString();
                                        Cust_Name = dr["Cust_Name"].ToString();
                                        PolNo_PhoneNo1 = dr["PolNo_PhoneNo1"].ToString();
                                        Payment = dr["Payment"].ToString();
                                        Amount = double.Parse(dr["Amount"].ToString());
                                        Pay_Mtd = dr["Pay_Mtd"].ToString();
                                        DepSlip_CardNo = dr["DepSlip_CardNo"].ToString();
                                        Cheq_Val_Date = dr["Cheq_Val_Date"].ToString();
                                        Bank = dr["Bank"].ToString();
                                        Trans_Ref = dr["Trans_Ref"].ToString();
                                        Coll_Acct = dr["Coll_Acct"].ToString();
                                        Narration = dr["Narration"].ToString();
                                        Bank_Code = dr["Bank_Code"].ToString();


                                        //test Payment Date
                                        myarrData = Pay_Date_Time.Split('/');
                                        if ((myarrData.Length != 3))
                                        {
                                            strGen_Msg = (" * Row: "
                                                         + (nRow.ToString() + (" - Incomplete Payment Date - " + Pay_Date_Time.ToString())));
                                            if (ftime == "Y")
                                            {
                                                ftime = "N";
                                                _err_msg = ErrRoutine(strGen_Msg);
                                            }
                                            else
                                                _err_msg.Add(ErrRoutine(strGen_Msg).ToString());
                                            continue;

                                            //                                     goto MyLoop_888;
                                        }

                                        strMyDay = myarrData[0];
                                        strMyMth = myarrData[1];
                                        strMyYear = myarrData[2].Substring(0, 4);
                                        strMyDay = double.Parse(strMyDay).ToString("00");
                                        strMyMth = double.Parse(strMyMth).ToString("00");
                                        strMyYear = double.Parse(strMyYear).ToString("0000");
                                        strMyDte = (strMyDay.Trim() + ("/"
                                                    + (strMyMth.Trim() + ("/" + strMyYear.Trim()))));


                                        if ((!gnTest_TransDate(strMyDte)))
                                        {
                                            strGen_Msg = (" * Row: "
                                                        + (nRow.ToString() + (" - Invalid Payment Date - " + strMyDte.ToString())));

                                            if (ftime == "Y")
                                            {
                                                ftime = "N";
                                                _err_msg = ErrRoutine(strGen_Msg);
                                            }
                                            else
                                                _err_msg.Add(ErrRoutine(strGen_Msg).ToString());
                                            continue;
                                            //goto MyLoop_888;
                                        }

                                        try
                                        {

                                            paydatetest = removeDateSeperators(strMyDte);
                                            if (paydatetest.Substring(0, 5) != "ERROR")
                                                Pay_Date_Time = paydatetest;
                                            else
                                                throw new Exception();

                                        }
                                        catch (Exception e)
                                        {
                                            //throw new Exception("Invalid Date Of Birth");
                                            strGen_Msg = (" * Row: "
                                           + (nRow.ToString() + (" - Invalid Payment Date - " + strMyDte.ToString())));
                                            if (ftime == "Y")
                                            {
                                                ftime = "N";
                                                _err_msg = ErrRoutine(strGen_Msg);
                                            }
                                            else
                                                _err_msg.Add(ErrRoutine(strGen_Msg).ToString());
                                            continue;

                                            //goto MyLoop_888;

                                        }
                                        //End test Payment Date


                                        //test Cheque Value  Date
                                        if (Cheq_Val_Date == "")
                                        {
                                            Cheq_Val_Date = "01/01/2014";
                                        }
                                        myarrData = Cheq_Val_Date.Split('/');
                                        if ((myarrData.Length != 3))
                                        {
                                            strGen_Msg = (" * Row: "
                                                         + (nRow.ToString() + (" - Incomplete Cheque Value  Date - " + Cheq_Val_Date.ToString())));
                                            if (ftime == "Y")
                                            {
                                                ftime = "N";
                                                _err_msg = ErrRoutine(strGen_Msg);
                                            }
                                            else
                                                _err_msg.Add(ErrRoutine(strGen_Msg).ToString());
                                            continue;

                                            //                                     goto MyLoop_888;
                                        }

                                        strMyDay = myarrData[0];
                                        strMyMth = myarrData[1];
                                        strMyYear = myarrData[2].Substring(0, 4);
                                        strMyDay = double.Parse(strMyDay).ToString("00");
                                        strMyMth = double.Parse(strMyMth).ToString("00");
                                        strMyYear = double.Parse(strMyYear).ToString("0000");
                                        strMyDte = (strMyDay.Trim() + ("/"
                                                    + (strMyMth.Trim() + ("/" + strMyYear.Trim()))));


                                        if ((!gnTest_TransDate(strMyDte)))
                                        {
                                            strGen_Msg = (" * Row: "
                                                        + (nRow.ToString() + (" - Invalid Cheque Value Date - " + strMyDte.ToString())));

                                            if (ftime == "Y")
                                            {
                                                ftime = "N";
                                                _err_msg = ErrRoutine(strGen_Msg);
                                            }
                                            else
                                                _err_msg.Add(ErrRoutine(strGen_Msg).ToString());
                                            continue;
                                            //goto MyLoop_888;
                                        }

                                        try
                                        {

                                            cheq_val_datetest = removeDateSeperators(strMyDte);
                                            if (cheq_val_datetest.Substring(0, 5) != "ERROR")
                                                Cheq_Val_Date = cheq_val_datetest;
                                            else
                                                throw new Exception();

                                        }
                                        catch (Exception e)
                                        {
                                            //throw new Exception("Invalid Date Of Birth");
                                            strGen_Msg = (" * Row: "
                                           + (nRow.ToString() + (" - Invalid Cheque Value Date - " + strMyDte.ToString())));
                                            if (ftime == "Y")
                                            {
                                                ftime = "N";
                                                _err_msg = ErrRoutine(strGen_Msg);
                                            }
                                            else
                                                _err_msg.Add(ErrRoutine(strGen_Msg).ToString());
                                            continue;

                                            //goto MyLoop_888;

                                        }

                                        //End  Cheque Value Date test

                                        if (sFT.Trim() == "Y")
                                        {
                                            sFT = "N";
                                            mystr_sql = "";
                                            mystr_sql = "DELETE FROM TBFN_RECEIPTS_DOWNLOAD";
                                            mystr_sql=mystr_sql + " WHERE TBFN_RECPT_DNLD_BATCH_DATE='"+ BatchDate +"'";
                                            myole_cmd = new SqlCommand(mystr_sql, connectionSQL);
                                            myole_cmd.CommandType = CommandType.Text;
                                            if (connectionSQL.State == ConnectionState.Closed)
                                                connectionSQL.Open();
                                            myole_cmd.ExecuteNonQuery();
                                            myole_cmd.Dispose();
                                            myole_cmd = null;

                                            mystr_sql = "";
                                            mystr_sql = "DELETE FROM TBFN_RECEIPTS_DOWNLOAD_MATCH";
                                            mystr_sql = mystr_sql + " WHERE TBFN_ACCT_BATCH_NO='" + BatchDate + "'";
                                            myole_cmd = new SqlCommand(mystr_sql, connectionSQL);
                                            myole_cmd.CommandType = CommandType.Text;
                                            if (connectionSQL.State == ConnectionState.Closed)
                                                connectionSQL.Open();
                                            myole_cmd.ExecuteNonQuery();
                                            myole_cmd.Dispose();
                                            myole_cmd = null;
                                        }

                                        string strquery = "INSERT INTO TBFN_RECEIPTS_DOWNLOAD("
                                                           + "TBFN_RECPT_DNLD_REF_NO,"
                                                           + "TBFN_RECPT_DNLD_BATCH_DATE,"
                                                           + "TBFN_RECPT_DNLD_PAY_DT_TM,"
                                                           + "TBFN_RECPT_DNLD_POL_NO,"
                                                           + "TBFN_RECPT_DNLD_RECPT_NO,"
                                                           + "TBFN_RECPT_DNLD_CUST_NAME,"
                                                           + "TBFN_RECPT_DNLD_POL_NO1,"
                                                           + "TBFN_RECPT_DNLD_PAYMENT,"
                                                           + "TBFN_RECPT_DNLD_AMOUNT,"
                                                           + "TBFN_RECPT_DNLD_PAY_MODE,"
                                                           + "TBFN_RECPT_DNLD_DEP_SLP_CARD_NO,"
                                                           + "TBFN_RECPT_DNLD_CHQ_VAL_DT,"
                                                           + "TBFN_RECPT_DNLD_BANK,"
                                                           + "TBFN_RECPT_DNLD_TRANS_REF,"
                                                           + "TBFN_RECPT_DNLD_COLL_ACCT,"
                                                           + "TBFN_RECPT_DNLD_NARRATION,"
                                                           + "TBFN_RECPT_DNLD_FLAG,"
                                                           + "TBFN_RECPT_DNLD_OPERID,"
                                                           + "TBFN_RECPT_DNLD_ENTRY_DATE,"
                                                           + "TBFN_RECPT_DNLD_MATCH,"
                                                           + "TBFN_RECPT_DNLD_BANK_CODE)"
                                                           + "values ("
                                                           + "'" + Pay_Ref_No + "'"
                                                           + ",'" + BatchDate + "'"
                                                           + ",'" + Pay_Date_Time.ToString() + "'"
                                                           + ",'" + PolNo_PhoneNo + "'"
                                                           + ",'" + Receipt_No + "'"
                                                           + ",'" + RemoveCharacter(Cust_Name) + "'"
                                                           + ",'" + PolNo_PhoneNo1 + "'"
                                                           + ",'" + Payment + "'"
                                                           + "," + Amount + ""
                                                           + ",'" + Pay_Mtd + "'"
                                                           + ",'" + RemoveCharacter(DepSlip_CardNo) + "'"
                                                           + ",'" + Cheq_Val_Date.ToString() + "'"
                                                           + ",'" + Bank + "'"
                                                           + ",'" + Trans_Ref + "'"
                                                           + ",'" + Coll_Acct + "'"
                                                           + ",'" + RemoveCharacter(Narration) + "'"
                                                           + ",'A'"
                                                           + ",'" + _username + "'"
                                                           + ", " + entry_date
                                                           + ",'A'"
                                                           +",'"+ Bank_Code+"'"
                                            // + ",'2015-01-01'"
                                                           + " )";
                                        commandSQL = new SqlCommand(strquery, connectionSQL);

                                        try
                                        {
                                            r = commandSQL.ExecuteNonQuery();

                                            if ((r >= 1))
                                            {
                                                my_intCNT = my_intCNT + 1;
                                            }
                                            else
                                            {
                                                strGen_Msg = (" * Error!. Row: "
                                                            + (nRow.ToString() + " record not save... "));
                                                if (ftime == "Y")
                                                {
                                                    ftime = "N";
                                                    _err_msg = ErrRoutine(strGen_Msg);
                                                }
                                                else
                                                    _err_msg.Add(ErrRoutine(strGen_Msg).ToString());
                                                continue;

                                                //                                            goto MyLoop_888;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            strGen_Msg = (" * Error while saving Row: "
                                                        + (nRow.ToString() + " record... "));
                                            if (ftime == "Y")
                                            {
                                                ftime = "N";
                                                _err_msg = ErrRoutine(strGen_Msg);
                                            }
                                            else
                                                _err_msg.Add(ErrRoutine(strGen_Msg).ToString());
                                            continue;

                                            //                                    goto MyLoop_888;

                                        }

                                    } //while read end
                                }

                                catch (Exception h)
                                {
                                    strGen_Msg = (" * General System Error : "
                                                + (h.ToString()));
                                    if (ftime == "Y")
                                    {
                                        ftime = "N";
                                        _err_msg = ErrRoutine(strGen_Msg);
                                        _err_msg.Add(ErrRoutine(strGen_Msg).ToString());

                                    }
                                    else
                                        _err_msg.Add(ErrRoutine(strGen_Msg).ToString());

                                    goto MyLoop_End_2;
                                    //throw new Exception("ERROR!: " + "may not be setup. Pls Check this number against a valid name and setup it up as a member");
                                }

                            }//end data reader


                MyLoop_888:

                            if ((strGen_Msg != ""))
                            {
                                errmsg.Add(strGen_Msg.ToString());
                                //_err_msg = errmsg;

                            }
                            strGen_Msg = "";
                            connectionSQL.Close();
                            connectionExcel.Close();

                            //goto MyLoop_Start;
                        MyLoop_999:
                        MyLoop_End:
                            my_intCNT = 1;
                        MyLoop_End_1:
                            _err_msg = ErrRoutine("Successful!");
                        MyLoop_End_2:
                            connectionSQL.Close();
                            connectionExcel.Close();

                        }
                    }
                }

            }
        }
        public static bool gnTest_TransDate(string MyFunc_Date)
        {
            bool pvbln;
            pvbln = false;

            if (((MyFunc_Date.Length == 10) && (((MyFunc_Date.Substring(2, 1) == "-") || (MyFunc_Date.Substring(2, 1) == "/"))
                && ((MyFunc_Date.Substring(5, 1) == "-")
            || (MyFunc_Date.Substring(5, 1) == "/")))))
            {

            }
            else
            {
                return pvbln;
            }
            string strDteMsg = "Invalid Date";
            string strDteErr = "0";
            //DateTime DteTst;
            string strDte_Start;
            string strDte_End;
            string strDteYY;
            string strDteMM;
            string strDteDD;
            strDteMsg = "";
            strDteErr = "0";
            strDteMsg = "";
            strDteErr = "0";

            strDteDD = MyFunc_Date.Substring(0, 2);
            strDteMM = MyFunc_Date.Substring(3, 2);
            strDteYY = MyFunc_Date.Substring((MyFunc_Date.Length - 4));
            strDteDD = strDteDD.Trim();
            strDteMM = strDteMM.Trim();
            strDteYY = strDteYY.Trim();


            if (((Convert.ToInt16(strDteDD) < 01) || (Convert.ToInt16(strDteDD.Trim()) > 31)))
            {
                strDteMsg = ("  -> Day < 01 or Day > 31 ..." + "\r\n");
                strDteErr = "1";
            }
            if (((Convert.ToInt16(strDteMM.Trim()) < 01)
                        || (Convert.ToInt16(strDteMM.Trim()) > 12)))
            {
                strDteMsg = (strDteMsg + ("  -> Month < 01 or Month > 12 ..." + "\r\n"));
                strDteErr = "1";
            }
            if ((strDteYY.Trim().Length < 4))
            {
                strDteMsg = (strDteMsg + ("  -> Year = 0 digit or Year < 4 digits..." + "\r\n"));
                strDteErr = "1";
            }
            strDte_Start = "";
            strDte_End = "";
            strDte_Start = MyFunc_Date;
            strDte_End = MyFunc_Date;

            switch (strDteMM.Trim())
            {
                case "01":
                case "03":
                case "05":
                case "07":
                case "08":
                case "10":
                case "12":
                    if ((double.Parse(strDteDD) > 31))
                    {
                        strDteMsg = (strDteMsg + ("  -> Invalid day in month. Month <"
                                    + (strDteMM + (">" + (" ends in <" + (" 31 " + (">" + (". Full Date: "
                                    + (strDte_End + "\r\n")))))))));
                        strDteErr = "1";
                    }
                    break;
                case "02":
                    if (double.Parse(strDteYY) % 4 == 0)
                    {
                        if ((double.Parse(strDteDD) > 29))
                        {
                            strDteMsg = (strDteMsg + ("  -> Invalid day in month. Month <"
                                        + (strDteMM + (">" + (" ends in <" + (" 29 " + (">" + (". Full Date: "
                                        + (strDte_End + "\r\n")))))))));
                            strDteErr = "1";
                        }
                    }
                    else if ((double.Parse(strDteDD) > 28))
                    {
                        strDteMsg = (strDteMsg + ("  -> Invalid day in month. Month <"
                                    + (strDteMM + (">" + (" ends in <" + (" 28 " + (">" + (". Full Date: "
                                    + (strDte_End + "\r\n")))))))));
                        strDteErr = "1";
                    }
                    break;
                case "04":
                case "06":
                case "09":
                case "11":
                    if ((double.Parse(strDteDD) > 30))
                    {
                        strDteMsg = (strDteMsg + ("  -> Invalid day in month. Month <"
                                    + (strDteMM + (">" + (" ends in <" + (" 30 " + (">" + (". Full Date: "
                                    + (strDte_End + "\r\n")))))))));
                        strDteErr = "1";
                    }
                    break;
            }

            if ((strDteErr != "0"))
            {
                //gnTest_TransDate = false;
                pvbln = false;
            }
            //gnTest_TransDate = true;
            pvbln = true;
            return pvbln;
        }

        protected static List<String> ErrRoutine(String msg)
        {
            var errm = new List<String>();

            if ((msg != ""))
            {
                errm.Add(msg.ToString());
                // errmsgs.ErrorMsgs.Add(msg.ToString());
                errmsgs.ErrorMsgs = errm;

            }
            return errmsgs.ErrorMsgs;

        }

        static string gnGET_RATE(string pvstr_GET_WHAT,
              string pvstr_MODULE,
              string pvstr_RATE_CODE,
              string pvstr_PRODUCT_REF_CODE,
              string pvstr_PERIOD,
              string pvstr_AGE,
              string pvCtr_Label,
              ref string pvRef_Misc,
              string pvRef_Misc_02,
              string connString)
        {


            string mystr_conn = "";
            string mystr_Table = "";
            string mystr_SQL = "";
            string mystr_Key = "";
            int myint_C = 0;
            string myRetValue = "0";
            mystr_conn = connString;
            mystr_conn = ("Provider=SQLOLEDB;" + mystr_conn);
            OleDbConnection myole_CONN = null;
            myole_CONN = new OleDbConnection(mystr_conn);
            try
            {
                //  Open connection
                myole_CONN.Open();
            }
            catch (Exception ex)
            {
                myole_CONN = null;
                if (pvCtr_Label != null)
                {
                    pvCtr_Label = "Error. " + ex.Message.ToString();
                }
                return "ERR_CON";
            }
            mystr_SQL = "";
            mystr_SQL = "";
            OleDbCommand myole_CMD = new OleDbCommand();
            myole_CMD.Connection = myole_CONN;

            switch (pvstr_GET_WHAT.Trim())
            {
                case "GET_IL_PREMIUM_RATE":
                    mystr_SQL = "SPIL_GET_PREM_RATE";
                    myole_CMD.CommandType = CommandType.StoredProcedure;
                    myole_CMD.CommandText = mystr_SQL;
                    myole_CMD.Parameters.Add("@p01", OleDbType.VarChar, 3).Value = pvstr_MODULE.TrimEnd();
                    myole_CMD.Parameters.Add("@p02", OleDbType.VarChar, 10).Value = pvstr_RATE_CODE.TrimEnd();
                    myole_CMD.Parameters.Add("@p03", OleDbType.VarChar, 10).Value = pvstr_PRODUCT_REF_CODE.TrimEnd();
                    myole_CMD.Parameters.Add("@p04", OleDbType.VarChar, 4).Value = pvstr_PERIOD.TrimEnd();
                    myole_CMD.Parameters.Add("@p05", OleDbType.VarChar, 4).Value = pvstr_AGE.TrimEnd();
                    break;
                case "GET_GL_PREMIUM_RATE":
                    mystr_SQL = "SPGL_GET_PREM_RATE";
                    myole_CMD.CommandType = CommandType.StoredProcedure;
                    myole_CMD.CommandText = mystr_SQL;
                    myole_CMD.Parameters.Add("@p01", OleDbType.VarChar, 3).Value = pvstr_MODULE.TrimEnd();
                    myole_CMD.Parameters.Add("@p02", OleDbType.VarChar, 10).Value = pvstr_RATE_CODE.TrimEnd();
                    myole_CMD.Parameters.Add("@p03", OleDbType.VarChar, 10).Value = pvstr_PRODUCT_REF_CODE.TrimEnd();
                    myole_CMD.Parameters.Add("@p04", OleDbType.VarChar, 4).Value = pvstr_PERIOD.TrimEnd();
                    myole_CMD.Parameters.Add("@p05", OleDbType.VarChar, 4).Value = pvstr_AGE.TrimEnd();
                    break;
                case "GET_IL_EXCHANGE_RATE":
                case "GET_GL_EXCHANGE_RATE":
                    mystr_SQL = "SPIL_GET_EXCHANGE_RATE";
                    myole_CMD.CommandType = CommandType.StoredProcedure;
                    myole_CMD.CommandText = mystr_SQL;
                    myole_CMD.Parameters.Add("@p01", OleDbType.VarChar, 3).Value = pvstr_MODULE.TrimEnd();
                    myole_CMD.Parameters.Add("@p02", OleDbType.VarChar, 10).Value = pvstr_RATE_CODE.TrimEnd();
                    myole_CMD.Parameters.Add("@p03", OleDbType.VarChar, 10).Value = pvstr_PRODUCT_REF_CODE.TrimEnd();
                    break;
                case "GET_IL_MOP_FACTOR":
                case "GET_GL_MOP_FACTOR":
                    mystr_SQL = "SPIL_GET_MOP_FACTOR";
                    myole_CMD.CommandType = CommandType.StoredProcedure;
                    myole_CMD.CommandText = mystr_SQL;
                    myole_CMD.Parameters.Add("@p01", OleDbType.VarChar, 3).Value = pvstr_MODULE.TrimEnd();
                    myole_CMD.Parameters.Add("@p02", OleDbType.VarChar, 10).Value = pvstr_RATE_CODE.TrimEnd();
                    myole_CMD.Parameters.Add("@p03", OleDbType.VarChar, 10).Value = pvstr_PRODUCT_REF_CODE.TrimEnd();
                    break;
                default:
                    myole_CMD = null;
                    myole_CONN = null;
                    if (pvCtr_Label != null)
                    {
                        pvCtr_Label = "Error. Invalid parameter: " + pvstr_GET_WHAT.ToString();

                    }
                    return "ERR_PARAM";
            }

            OleDbDataReader myole_DR;
            try
            {
                myole_DR = myole_CMD.ExecuteReader();
                //  with the new data reader parse values and place into the return variable
                if (myole_DR.Read())
                {
                    // Me.UserCode.Text = Me.UserName.Text & " - " & oleDR("pwd_code").ToString & vbNullString
                    switch (pvstr_GET_WHAT.Trim())
                    {
                        case "GET_IL_PREMIUM_RATE":
                        case "GET_GL_PREMIUM_RATE":
                            myRetValue = (myole_DR["TBIL_PRM_RT_RATE"]).ToString();
                            if ((pvRef_Misc == null))
                            {

                            }
                            else
                            {
                                pvRef_Misc = myole_DR["TBIL_PRM_RT_PER"].ToString();
                            }
                            break;
                        case "GET_IL_EXCHANGE_RATE":
                        case "GET_GL_EXCHANGE_RATE":
                            myRetValue = myole_DR["TBIL_EXCH_RATE"].ToString();
                            break;
                        case "GET_IL_MOP_FACTOR":
                        case "GET_GL_MOP_FACTOR":
                            myRetValue = myole_DR["TBIL_MOP_RATE"].ToString();
                            if ((pvRef_Misc == null))
                            {

                            }
                            else
                            {
                                pvRef_Misc = myole_DR["TBIL_MOP_TYPE_DESC"].ToString();
                            }
                            if ((pvRef_Misc_02 == null))
                            {

                            }
                            else
                            {
                                pvRef_Misc_02 = myole_DR["TBIL_MOP_DIVIDE"].ToString();
                            }
                            break;
                        default:
                            myRetValue = "ERR_PARAM";
                            if (pvCtr_Label != null)
                            {
                                pvCtr_Label = "Error. Invalid parameter: " + pvstr_GET_WHAT.ToString();
                            }
                            break;
                    }
                    myole_DR.Close();
                    myole_CMD.Dispose();
                }
                else
                {
                    myRetValue = "ERR_RNF";
                    if (pvCtr_Label != null)
                    {
                        pvCtr_Label = "Record not found for parameters supplied...";
                    }
                }
            }
            catch (Exception ex)
            {
                //    'Throw ex
                myRetValue = "ERR";
                if (pvCtr_Label != null)
                {
                    pvCtr_Label = "Error. " + ex.Message.ToString();
                }
            }
            // myole_DA.Dispose()
            try
            {
                //  Close connection
                myole_CONN.Close();
            }
            catch (Exception ex)
            {
            }
            // myobj_ds = Nothing
            // myole_DA = Nothing
            myole_DR = null;
            myole_CMD = null;
            myole_CONN = null;
            return myRetValue;
        }


        public static string removeDateSeperators(String dte)
        {
            //split to constituents
            string[] dparts = dte.Split('/');
            if (dparts.Length != 3)
                return "ERROR: Invalid Date Format!";

            string dy = dparts[0];
            string myday = dparts[0];
            string mt = dparts[1];
            string mth = dparts[1];
            if (mt.Length == 1)
                mth = "0" + mt;

            if (dy.Length == 1)
                myday = "0" + dy;

            string ky = dparts[2] + mth + myday;
            return ky;
        }

        private static string RemoveCharacter(string remchar)
        {
            string[] chars = new string[] { "," , "." , "/" , "!",
                                            "@" , "#" , "$" , "%" , "^", "&" , "*" , "'", "\"" , ";", "_" , "(" ,
                                             ")" , ":" , "|", "[" , "]" };
            //Iterate the number of times based on the
            for (int i = 0; i < chars.Length; i++)
            {
                if (remchar.Contains(chars[i]))
                {
                    remchar = remchar.Replace(chars[i], "");
                }
            }
            return remchar;
        }

        public DataSet ProcessBind(string BatchDate)
        {
            DataSet ds = new DataSet();
            //ds = null;
            SqlDataAdapter adapter;
            using (var session = GetSession())
            {
                using (var conn = session.Connection as SqlConnection)
                {
                    string stryquery = "SELECT * FROM TBFN_RECEIPTS_DOWNLOAD WHERE TBFN_RECPT_DNLD_BATCH_DATE='" + BatchDate + "'";
                    adapter = new SqlDataAdapter(stryquery, conn);
                    adapter.Fill(ds);
                    conn.Close();
                }
            }
            return ds;
        }
    }
}
