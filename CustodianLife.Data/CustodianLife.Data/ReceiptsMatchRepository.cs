using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using CustodianLife.Model;
using CustodianLife.Repositories;
using NHibernate;
namespace CustodianLife.Data
{
    public class ReceiptsMatchRepository
    {
        private static ISession GetSession()
        {
            return SessionProvider.SessionFactory.OpenSession();
        }

        public static MyErrorListing errmsgs = new MyErrorListing();

        public void Save(ReceiptsMatch saveObj)
        {
            using (var session = GetSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.FlushMode = FlushMode.Commit;
                    session.SaveOrUpdate(saveObj);
                    trans.Commit();
                    session.Flush();
                    //}
                }
            }
        }

        //public void GetPolicyInfo(int BatchNo, ref List<String> _err_msg, ref List<String> _unmatch_proposalnos)
        public void GetPolicyInfo(int BatchNo, ref List<String> _err_msg, ref int ReadRecords, ref int MatchRecords, ref int UnMatchRecords)
        {
            string ProposalNo;
            string CompanyCode;
            //string BatchNo;
            long SerialNo;
            string TransType;
            string Temp_TransNo;
            DateTime DocDate = DateTime.Now;
            DateTime EntryDate = DateTime.Now;
            string TransMode_Desc;
            string TransMode = "";
            string ReceiptType;
            string RefNo;//get from policy det but same as proposal no
            string ProductCode = "";//get from policy det
            string DocNo;
            string CurrencyType;
            string Chq_teller_No;
            DateTime Chq_Inward_Date = DateTime.Now;
            string Payer_Payee_Name;
            string Trans_Desc1;
            string Trans_Desc2;
            DateTime Eff_Date = DateTime.Now;
            string Branch_Code;
            string Bank_Code;
            decimal Amount_LC;
            double Amount_FC;
            string Insured_Code = "";//get from policy det
            string Agent_Code = "";//get from policy det
            string Commission_YN;
            decimal Poly_Contrib = 0;//get from premium info
            string Mop = "";//get from premium info
            string dr_Main;
            string dr_Sub;
            string Cr_Main;
            string Cr_Sub;
            string Ledger_Type_Cr;
            string Flag;
            string OperId;
            string FileNo = "";//get from policy det
            DateTime Proc_Date = DateTime.Now;
            string strquery;
            string docMonth, docYear, newReceiptNo, newSerialNum;
            CustodianLife.Model.ReceiptsMatch Rceipt;
            float nRow = 0;
            string strGen_Msg = String.Empty;
            string sFT = "";
            String ftm = "Y";
            String ftime = "Y";
            sFT = "Y";
            var errmsg = new List<String>();
            long my_intCNT = 0;
            SqlCommand myole_cmd = null;
            string mystr_sql = "";


            using (var session = GetSession())
            {
                using (var conn = session.Connection as SqlConnection)
                {
                    SqlConnection connectionSQL = new SqlConnection();
                    SqlDataAdapter da;
                    DataSet ds;
                    DataRow row;
                    //Assign the connection converted form Nhibernate to Sqlconnection into connectionSQL
                    connectionSQL = conn;
                    SqlCommand commandSQL, commandSQL1;
                    SqlDataReader dr, dr1;
                    //commandSQL.CommandText = "SELECT * FROM TBFN_RECEIPTS_DOWNLOAD";
                    //strquery = "SELECT * FROM TBFN_RECEIPTS_DOWNLOAD WHERE TBFN_RECPT_DNLD_MATCH='A'";
                    strquery = "SELECT * FROM TBFN_RECEIPTS_DOWNLOAD WHERE TBFN_RECPT_DNLD_MATCH='A'";
                    strquery = strquery + " AND TBFN_RECPT_DNLD_BATCH_DATE='" + BatchNo + "'";
                    da = new SqlDataAdapter(strquery, connectionSQL);
                    ds = new DataSet();
                    da.Fill(ds);
                //data.Close()

                MyLoop_Start:
                    //using (SqlDataReader dr = commandSQL.ExecuteReader())
                    //{
                    //SqlDataReader dr = commandSQL.ExecuteReader();


                    try
                    {
                        //while (dr.Read())
                        //{
                        for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                        {
                            ReadRecords = ds.Tables[0].Rows.Count;
                            nRow += 1; //row
                            row = ds.Tables[0].Rows[i];
                            ProposalNo = row["TBFN_RECPT_DNLD_POL_NO"].ToString();
                            RefNo = ProposalNo;
                            Payer_Payee_Name = row["TBFN_RECPT_DNLD_CUST_NAME"].ToString();
                            CompanyCode = "001";
                            CurrencyType = "0001";
                            TransType = "R";
                            Temp_TransNo = "";
                            Branch_Code = "";
                            Bank_Code = "";
                            DocDate = Convert.ToDateTime(row["TBFN_RECPT_DNLD_PAY_DT_TM"]);
                            ReceiptType = "D";
                            Trans_Desc1 = row["TBFN_RECPT_DNLD_BANK"].ToString();
                            Trans_Desc2 = row["TBFN_RECPT_DNLD_PAYMENT"].ToString();
                            Amount_LC = Convert.ToDecimal(row["TBFN_RECPT_DNLD_AMOUNT"]);
                            Chq_Inward_Date = Convert.ToDateTime(row["TBFN_RECPT_DNLD_CHQ_VAL_DT"]);
                            Commission_YN = "Y";
                            dr_Main = row["TBFN_RECPT_DNLD_BANK_CODE"].ToString();
                            TransMode_Desc = row["TBFN_RECPT_DNLD_PAY_MODE"].ToString();
                            if (TransMode_Desc == "Cash")
                            {
                                TransMode = "C";
                            }
                            else if (TransMode_Desc == "Own Bank Cheque" || TransMode_Desc == "Other Bank Cheque")
                            {
                                TransMode = "Q";
                            }
                            else if (TransMode_Desc == "Internal Transfer")
                            {
                                TransMode = "T";
                            }
                            else if (TransMode_Desc == "Debit Card")
                            {
                                //Check later
                                TransMode = "D";
                            }
                            Chq_teller_No = row["TBFN_RECPT_DNLD_DEP_SLP_CARD_NO"].ToString();
                          
                            DataSet dt1 = new DataSet();
                            DataRow dr2;
                            ReceiptsRepository recRep = new ReceiptsRepository();
                            dt1 = recRep.GetPolicyInfoDataSet(ProposalNo, "D");

                            if (dt1.Tables[0].Rows.Count > 0)
                            {
                                dr2 = dt1.Tables[0].Rows[0];
                                Insured_Code = dr2["TBIL_POLY_ASSRD_CD"].ToString();
                                Agent_Code = dr2["TBIL_POLY_AGCY_CODE"].ToString().ToString();
                                Poly_Contrib = Convert.ToDecimal(dr2["TBIL_POL_PRM_DTL_MOP_PRM_LC"]);
                                Mop = dr2["Payment_Mode"].ToString();
                                FileNo = dr2["File_No"].ToString();
                                ProductCode = dr2["Product_Code"].ToString();
                                if (dr2["TBIL_POLICY_EFF_DT"] != DBNull.Value)
                                    Eff_Date = Convert.ToDateTime(dr2["TBIL_POLICY_EFF_DT"]);
                                else
                                    Eff_Date = Convert.ToDateTime("01/01/2014");


                                docMonth = BatchNo.ToString().Substring(4, 2);
                                docYear = BatchNo.ToString().Substring(0, 4);

                                //get new serial number
                                //newSerialNum = GetNextSerialNumber("L01", "001", docMonth, docYear, " ", "12", "11");
                                newSerialNum = NextSerialNumber();
                                //get new receipt number
                                //newReceiptNo = GetNextSerialNumber("RCN", "002", "001", docYear, "IL-BR-", "12", "11");
                                newReceiptNo = "IL-MT-" + newSerialNum;
                                
                                
                                SerialNo = Convert.ToInt64(newSerialNum.Trim());
                                //dr_Main has value from the excel uploaded file.
                               // dr_Main = "";
                                dr_Sub = "";
                                Cr_Main = "";
                                Cr_Sub = "";
                                Ledger_Type_Cr = "";
                                Flag = "A";
                                OperId = "";

                                Rceipt = new ReceiptsMatch();
                                Rceipt.AgentCode = Agent_Code;
                                Rceipt.AmountFC = Amount_LC;
                                Rceipt.AmountLC = Amount_LC;
                                Rceipt.BankCode = Bank_Code;
                                Rceipt.BatchNo = BatchNo;
                                Rceipt.BranchCode = Branch_Code;
                                Rceipt.ChequeDate = Chq_Inward_Date;
                                Rceipt.ChequeInwardNo = "";
                                Rceipt.ChequeTellerNo = Chq_teller_No;
                                Rceipt.CommissionApplicable = Commission_YN;
                                Rceipt.CompanyCode = CompanyCode;
                                Rceipt.CurrencyType = CurrencyType;
                                Rceipt.EntryDate = EntryDate;
                                Rceipt.InsuredCode = Insured_Code;
                                Rceipt.MainAccountCredit = Cr_Main;
                                Rceipt.MainAccountDebit = dr_Main;
                                Rceipt.PayeeName = Payer_Payee_Name;
                                Rceipt.PolicyPaymentMode = Mop;
                                Rceipt.PolicyRegularContribution = Poly_Contrib;
                                Rceipt.ReceiptType = ReceiptType;
                                Rceipt.ReferenceNo = RefNo;
                                Rceipt.SerialNo = SerialNo;
                                Rceipt.SubAccountCredit = Cr_Sub;
                                Rceipt.SubAccountDebit = dr_Sub;
                                Rceipt.DocNo = newReceiptNo;

                                Rceipt.TempTransNo = Temp_TransNo;
                                Rceipt.TranDescription1 = Trans_Desc1;
                                Rceipt.TranDescription2 = Trans_Desc2;
                                Rceipt.TransDate = Eff_Date;
                                Rceipt.TransMode = TransMode;
                                Rceipt.TransType = TransType;
                                Rceipt.CurrencyType = CurrencyType;
                                Rceipt.LedgerTypeCredit = "T";
                                Rceipt.FileNo = FileNo;
                                Rceipt.ProductCode = ProductCode;
                                Rceipt.Flag = "A";
                                Rceipt.OperId = "001";
                                Rceipt.ProcDate = BatchNo.ToString();

                                //Saving into receipt file
                                try
                                {
                                    Save(Rceipt);
                                    UpdateMatchStatus(ProposalNo);
                                    MatchRecords = MatchRecords + 1;
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

                   // }//end data reader


                  MyLoop_888:

                    if ((strGen_Msg != ""))
                    {
                        errmsg.Add(strGen_Msg.ToString());
                        //_err_msg = errmsg;

                    }
                    strGen_Msg = "";
                    connectionSQL.Close();
                    UnMatchRecords = ReadRecords - MatchRecords;
                MyLoop_999:
                MyLoop_End:
                    my_intCNT = 1;
                MyLoop_End_1:
                    _err_msg = ErrRoutine("Successful!");
                //_unmatch_proposalnos = ErrRoutine("Successful!");
                MyLoop_End_2:
                    connectionSQL.Close();
                }
            }

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

        private void UpdateMatchStatus(string ProposalNo)
        {
            SqlCommand com;
            using (var session = GetSession())
            {
                using (var conn = session.Connection as SqlConnection)
                {
                    string stryquery = "UPDATE TBFN_RECEIPTS_DOWNLOAD SET TBFN_RECPT_DNLD_MATCH='C' WHERE TBFN_RECPT_DNLD_POL_NO='" + ProposalNo + "'";
                    com = new SqlCommand(stryquery, conn);
                    com.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public String GetNextSerialNumber(string sys_id
                                    , String sys_type
                                    , String sys_branch
                                    , String sys_year
                                    , String sys_prefix
                                    , String sys_out_int
                                    , String sys_out_char)
        {
            var session = GetSession();
            session.BeginTransaction();
            IDbCommand command = session.Connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CiSP_GetAcctSerialNo";

            IDbDataParameter inputparameter1 = command.CreateParameter();
            inputparameter1.ParameterName = "@PARAM_SYS_ID";
            inputparameter1.DbType = DbType.String;
            inputparameter1.Size = 5;
            inputparameter1.Direction = ParameterDirection.Input;
            inputparameter1.Value = sys_id;

            command.Parameters.Add(inputparameter1);

            IDbDataParameter inputparameter2 = command.CreateParameter();
            inputparameter2.ParameterName = "@PARAM_SYS_TYPE";
            inputparameter2.DbType = DbType.String;
            inputparameter2.Direction = ParameterDirection.Input;
            inputparameter2.Size = 5;
            inputparameter2.Value = sys_type;
            command.Parameters.Add(inputparameter2);

            IDbDataParameter inputparameter3 = command.CreateParameter();
            inputparameter3.ParameterName = "@PARAM_SYS_BRANCH";
            inputparameter3.DbType = DbType.String;
            inputparameter3.Direction = ParameterDirection.Input;
            inputparameter3.Size = 5;
            inputparameter3.Value = sys_branch;
            command.Parameters.Add(inputparameter3);

            IDbDataParameter inputparameter4 = command.CreateParameter();
            inputparameter4.ParameterName = "@PARAM_SYS_YEAR";
            inputparameter4.DbType = DbType.String;
            inputparameter4.Direction = ParameterDirection.Input;
            inputparameter4.Size = 5;
            inputparameter4.Value = sys_year;
            command.Parameters.Add(inputparameter4);

            IDbDataParameter inputparameter5 = command.CreateParameter();
            inputparameter5.ParameterName = "@PARAM_SYS_PREFIX";
            inputparameter5.DbType = DbType.String;
            inputparameter5.Size = 6;
            inputparameter5.Direction = ParameterDirection.Input;
            inputparameter5.Value = sys_prefix;
            command.Parameters.Add(inputparameter5);

            IDbDataParameter inputparameter6 = command.CreateParameter();
            inputparameter6.ParameterName = "@PARAM_OUT_INT";
            inputparameter6.DbType = DbType.String;
            inputparameter6.Direction = ParameterDirection.Input;
            inputparameter6.Size = 20;
            inputparameter6.Value = sys_out_int;
            command.Parameters.Add(inputparameter6);

            IDbDataParameter outparameter = command.CreateParameter();
            outparameter.ParameterName = "@PARAM_OUT_CHAR";
            outparameter.DbType = DbType.String;
            outparameter.Size = 20;
            outparameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(outparameter);

            session.Transaction.Enlist(command);
            command.ExecuteNonQuery();

            //retrieve value from out parameter
            IDbDataParameter outparameter4valu = command.CreateParameter();

            outparameter4valu = (IDbDataParameter)command.Parameters["@PARAM_OUT_CHAR"];
            string outval = (string)outparameter4valu.Value;

            session.Transaction.Commit();
            return outval;
        }

        public DataSet UnMatchProposalNumbers(string BatchDate)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adapter;
            using (var session = GetSession())
            {
                using (var conn = session.Connection as SqlConnection)
                {
                    string stryquery = "SELECT * FROM TBFN_RECEIPTS_DOWNLOAD WHERE TBFN_RECPT_DNLD_MATCH='A'";
                    stryquery = stryquery + " AND TBFN_RECPT_DNLD_BATCH_DATE='" + BatchDate + "'";
                    adapter = new SqlDataAdapter(stryquery, conn);
                    adapter.Fill(ds);
                    conn.Close();
                }
            }
            return ds;
        }

        private static string NextSerialNumber()
        {
            int NewSerialNo = 0;
            int LastSerialNo=0;
            using (var session = GetSession())
            {
                using (var conn = session.Connection as SqlConnection)
                {
                    SqlDataReader dr;
                    string stryquery = "SELECT * FROM TBFN_RECEIPTS_DOWNLOAD_MATCH ORDER BY TBFN_RECPT_FILE_REC_ID DESC";
                    SqlCommand comm = new SqlCommand(stryquery, conn);
                    dr = comm.ExecuteReader();
                    if (dr.Read())
                    {
                        LastSerialNo = Convert.ToInt32(dr["TBFN_RECPT_FILE_REC_ID"]);
                    }
                    conn.Close();
                }
            }
            NewSerialNo = LastSerialNo + 1;
            return NewSerialNo.ToString();
        }
    }
}
