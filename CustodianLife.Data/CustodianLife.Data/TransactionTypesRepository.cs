using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using CustodianLife.Model;
using CustodianLife.Repositories;
using NHibernate;
using System.Data.SqlClient;
using System.Data;

namespace CustodianLife.Data
{
    public class TransactionTypesRepository
    {
        private static ISession GetSession()
        {
            return SessionProvider.SessionFactory.OpenSession();
        }

        public void Save(TransactionTypes saveObj)
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
        public void Delete(TransactionTypes delObj)
        {
            using (var session = GetSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.Delete(delObj);
                    trans.Commit();
                }
            }
        }
        public IList<TransactionTypes> TransactionTypesDetails()
        {
            using (var session = GetSession())
            {
                var pDet = session.CreateCriteria<TransactionTypes>()

                                     .List<TransactionTypes>();

                return pDet;
            }
        }
        public TransactionTypes GetById(Int32? id)
        {
            using (var session = GetSession())
            {
                return session.Get<TransactionTypes>(id);
            }
        }
        public TransactionTypes GetById(String _key)
        {
            //the _key is an array of string values (3). Split into individual values and fill the parameters
            Char[] seperator = new char[] { ',' };
            string[] keys = _key.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            string hqlOptions = "from TransactionTypes i where i.ttId = " + keys[0]
                              + " and i.CompanyCode = '" + keys[1] + "'"
                              + " and i.TransactionCode = '" + keys[2] + "'";

            using (var session = GetSession())
            {

                return (TransactionTypes)session.CreateQuery(hqlOptions).UniqueResult();
            }
        }

        public String GetTransInfo(String _transcode)
        {
            //queries the generic lifecodes table and extract info for the branches only -- L02, 003
            string query = "SELECT * "
                          + "FROM TBFN_TRANS_TYPE WHERE (TBFN_TRANS_TYP_CODE='" + _transcode + "')";


            return GetDataSet(query).GetXml();
        }
        private static DataSet GetDataSet(string qry)
        {
            using (var session = GetSession())
            {
                using (var conn = session.Connection as SqlConnection)
                {
                    var adapter = new SqlDataAdapter(qry, conn);
                    var dataSet = new System.Data.DataSet();

                    adapter.Fill(dataSet);

                    return dataSet;
                }
            }
        }
      
    }
}
