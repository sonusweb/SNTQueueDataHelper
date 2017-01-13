using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SNTQueueDataHelper
{
    public class QueueDataHelper
    {
       
        public void AuthenticateLogin(string UserID, string Password, out bool IsValidLogin)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_LoginAuthenticate", connect);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserID = new SqlParameter("@UserID", SqlDbType.VarChar);
                pUserID.Value = UserID;
                cmd.Parameters.Add(pUserID);

                SqlParameter pPassword = new SqlParameter("@Password", SqlDbType.VarChar);
                pPassword.Value = Password;
                cmd.Parameters.Add(pPassword);

                SqlParameter pIsValidLogin = new SqlParameter("@IsValidLogin", SqlDbType.Bit);
                pIsValidLogin.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pIsValidLogin);

                connect.Open();
                cmd.ExecuteNonQuery();
                IsValidLogin = (bool)(cmd.Parameters["@IsValidLogin"].Value);

            }
            finally
            {
                connect.Close();
            }
        }
      
        public DataSet getQueueStatus()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            SqlCommand cmd = new SqlCommand("spQM_GetQueueStatus",connect);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {              
                connect.Open();
                da.SelectCommand = cmd;
                da.Fill(ds);
                
            }
            finally
            {
                connect.Close();
            }
            return ds;

        }

        public void getMaxCustomerQueueIDByMobileNo(string mobileNo, out int customerQueueID)
        {
            
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            SqlCommand cmd = new SqlCommand("spQM_GetMaxCustomerQueueIDByMobileNo", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter pMobileNo = new SqlParameter("@MobileNo", SqlDbType.VarChar);
            pMobileNo.Value = mobileNo;
            cmd.Parameters.Add(pMobileNo);
            SqlParameter pCustomerQueueID = new SqlParameter("@CustomerQueueID", SqlDbType.Int);
            pCustomerQueueID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pCustomerQueueID);
            try
            {
                connect.Open();
                cmd.ExecuteNonQuery();

                customerQueueID = (int)cmd.Parameters["@CustomerQueueID"].Value;
               

            }
            finally
            {
                connect.Close();
            }
            

        }

    }
    public class QueueStatus
    {
        public QueueStatus()
        {
        }
        public int QueueStatusID { get; set; }
        public char Status { get; set; }
        public String StatusDesc { get; set; }
        public void Load()
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_GetQueueStatus", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                connect.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["QueueStatusID"] == null)
                    {
                        throw new Exception("Queue status not found");
                    }
                    QueueStatusID = Convert.ToInt32(dr["QueueStatusID"].ToString());
                    Status = Convert.ToChar(dr["Status"].ToString());
                    StatusDesc = dr["StatusDesc"].ToString();
                }

            }
            finally
            {
                connect.Close();
            }
        }
    }

    [Serializable]
    public class ClientDetail
    {
        public ClientDetail()
        { }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientDesc { get; set; }
       
        public ClientDetail(string UserID, string Password)
        {
            Load(UserID, Password);
        }
        public void Load(string UserID, string Password)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_GetClientDetailByLogin", connect);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserID = new SqlParameter("@UserID", SqlDbType.VarChar);
                pUserID.Value = UserID;
                cmd.Parameters.Add(pUserID);

                SqlParameter pPassword = new SqlParameter("@Password", SqlDbType.VarChar);
                pPassword.Value = Password;
                cmd.Parameters.Add(pPassword);

                connect.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["ClientID"] == null)
                    {
                        throw new Exception("Client not found");
                    }
                    ClientID = Convert.ToInt32(dr["ClientID"].ToString());
                    ClientName = dr["ClientName"].ToString();
                    ClientDesc= dr["ClientDesc"].ToString();
                 }
            }
            finally
            {
            }

        }
    }

    [Serializable]
    public class ContactDetail
    {
        public ContactDetail()
        { }
        public int ContactID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public bool IsUserAdmin { get; set; }
        public bool IsUserWebAdmin { get; set; }
        public ContactDetail(string UserID, string Password)
        {
            Load(UserID, Password);
        }
        public void Load(string UserID, string Password)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_GetContactDetailByLogin", connect);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserID = new SqlParameter("@UserID", SqlDbType.VarChar);
                pUserID.Value = UserID;
                cmd.Parameters.Add(pUserID);

                SqlParameter pPassword = new SqlParameter("@Password", SqlDbType.VarChar);
                pPassword.Value = Password;
                cmd.Parameters.Add(pPassword);

                connect.Open();
                
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["ContactID"] == null)
                    {
                        throw new Exception("Contact not found");
                    }
                    ContactID = Convert.ToInt32(dr["ContactID"].ToString());
                    FName = dr["FName"].ToString();
                    LName = dr["LName"].ToString();
                    IsUserAdmin = Convert.ToBoolean(dr["IsUserAdmin"].ToString());
                    IsUserWebAdmin = Convert.ToBoolean(dr["IsUserWebAdmin"].ToString());
                   
        

                }
            }
            finally
            {
            }

        }
    }
    [Serializable]
    public class CustomerDetail
    {
        public CustomerDetail()
        { }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public bool SendSMS { get; set; }
        public bool SMSMarketing { get; set; }
        public bool OnlyVoiceCall { get; set; }
        //public Customer(int CustomerID)
        //{
        //    Load(CustomerID);
        //}
        public CustomerDetail(string MobileNo)
        {
            Load(MobileNo);
        }
        

        public void Load()
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_GetCustomer", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                connect.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["CustomerID"] == null)
                    {
                        throw new Exception("Custmer not found");
                    }
                    CustomerID = Convert.ToInt32(dr["CustomerID"].ToString());
                    CustomerName = dr["CustomerName"].ToString();
                    MobileNo =dr["MobileNo"].ToString();
                    SendSMS = Convert.ToBoolean(dr["SendSMS"].ToString());
                    SMSMarketing = Convert.ToBoolean(dr["SMSMarketing"].ToString());
                    OnlyVoiceCall = Convert.ToBoolean(dr["OnlyVoiceCall"].ToString());
                }

            }
            finally
            {
                connect.Close();
            }
        }

        //public void Load(int CustomerID)
        //{
        //    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("spQM_GetCustomerByCustomerID", connect);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        connect.Open();
        //        SqlParameter pCustomerID = new SqlParameter("@CustomerID", SqlDbType.Int);
        //        pCustomerID.Value = CustomerID;
        //        cmd.Parameters.Add(pCustomerID);
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            if (dr["CustomerID"] == null)
        //            {
        //                throw new Exception("Custmer not found");
        //            }
        //            CustomerID = Convert.ToInt32(dr["CustomerID"].ToString());
        //            CustomerName = dr["CustomerName"].ToString();
        //            MobileNo = Convert.ToInt32(dr["MobileNo"].ToString());
        //            SMSMarketing = Convert.ToBoolean(dr["SMSMarketing"].ToString());
        //            OnlyVoiceCall = Convert.ToBoolean(dr["OnlyVoiceCall"].ToString());
        //        }

        //    }
        //    finally
        //    {
        //        connect.Close();
        //    }
        //}


        public void Load(string MobileNo)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_GetCustomerByMobileNo", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                connect.Open();
                SqlParameter pMobileNo = new SqlParameter("@MobileNo", SqlDbType.VarChar);
                pMobileNo.Value = MobileNo;
                cmd.Parameters.Add(pMobileNo);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["MobileNo"] == null)
                    {
                        throw new Exception("Custmer not found");
                    }
                    CustomerID = Convert.ToInt32(dr["CustomerID"].ToString());
                    CustomerName = dr["CustomerName"].ToString();
                    MobileNo = dr["MobileNo"].ToString();
                    SendSMS = Convert.ToBoolean(dr["SendSMS"].ToString());
                    SMSMarketing = Convert.ToBoolean(dr["SMSMarketing"].ToString());
                    OnlyVoiceCall = Convert.ToBoolean(dr["OnlyVoiceCall"].ToString());
                }

            }
            finally
            {
                connect.Close();
            }
        }

        public void Store()
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());

            SqlCommand cmd = new SqlCommand("spQM_SaveCustomer", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@CustomerName", SqlDbType.VarChar, 50);
            prms[0].Value = CustomerName;
            prms[1] = new SqlParameter("@MobileNo", SqlDbType.VarChar,15);
            prms[1].Value = MobileNo;
            prms[2] = new SqlParameter("@SendSMS", SqlDbType.Bit);
            prms[2].Value = SendSMS;
            prms[3] = new SqlParameter("@SMSMarketing", SqlDbType.Bit);
            prms[3].Value = SMSMarketing;
            prms[4] = new SqlParameter("OnlyVoiceCall", SqlDbType.Bit);
            prms[4].Value = OnlyVoiceCall;
            prms[5] = new SqlParameter("@CustomerID", SqlDbType.Int);
            prms[5].Direction = ParameterDirection.Output;

            cmd.Parameters.AddRange(prms);
            try
            {
                connect.Open();
                cmd.ExecuteNonQuery();
                CustomerID = (int)prms[5].Value;

            }
            finally
            {
                connect.Close();
            }

        }
    }

    [Serializable]
    public class CustomerQueue
    {
        public CustomerQueue()
        {}
        public int CustomerQueueID { get; set; }
        public int CustomerID {get;set;}
        public int ClientID { get; set; }
        public string QCustomerName { get; set; }
        public int PartySize { get; set; }
        public string Notes { get; set; }
        public int waitingTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string StatusDesc { get; set; }
        public string MobileNo { get; set; }

        public CustomerQueue(string Status)
        {
            Load(Status);
        }
        public void Load(string Status)
        {
             SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
             try
             {
                 SqlCommand cmd = new SqlCommand("spQM_GetCustomerQueueByStatus", connect);
                 cmd.CommandType = CommandType.StoredProcedure;
                 connect.Open();
                 SqlParameter pStatus = new SqlParameter("@Status", SqlDbType.Char);
                 pStatus.Value = Status;
                 cmd.Parameters.Add(pStatus);
                 SqlDataReader dr = cmd.ExecuteReader();
                 if (dr.Read())
                 {
                     if (dr["Status"] == null)
                     {
                         throw new Exception("Custmer Queue not found");
                     }
                     CustomerQueueID = Convert.ToInt32(dr["CustomerQueueID"].ToString());  
                     CustomerID = Convert.ToInt32(dr["CustomerID"].ToString());
                     ClientID =  Convert.ToInt32(dr["ClientID"].ToString());
                     QCustomerName = dr["QCustomerName"].ToString();
                     PartySize = Convert.ToInt32(dr["PartySize"].ToString());
                     Notes = dr["Notes"].ToString();
                     waitingTime = Convert.ToInt32(dr["WaitingTime"].ToString());
                     CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                     MobileNo = dr["MobileNo"].ToString();
                     Status = dr["Status"].ToString();
                 }

             }
             finally
             {
             }

        }
        public void Update(int customerQueueID,string status)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_UpdateCustomerQueue", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@CustomerQueueID", SqlDbType.Int);
                prms[0].Value = customerQueueID;
                prms[1] = new SqlParameter("@Status", SqlDbType.Char);
                prms[1].Value = status;
                cmd.Parameters.AddRange(prms);
                connect.Open();
                cmd.ExecuteNonQuery();

            }
            
            finally
            {
                connect.Close();
            }
              

        }
        public void Store()
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());

            SqlCommand cmd = new SqlCommand("spQM_SaveCustomerQueue", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@CustomerID", SqlDbType.Int);
            prms[0].Value = CustomerID;
            prms[1] = new SqlParameter("@ClientID", SqlDbType.Int);
            prms[1].Value = ClientID;
            prms[2] = new SqlParameter("@QCustomerName", SqlDbType.VarChar, 50);
            prms[2].Value = QCustomerName;
            prms[3] = new SqlParameter("@PartySize", SqlDbType.Int);
            prms[3].Value = PartySize;
            prms[4] = new SqlParameter("Notes", SqlDbType.VarChar,150);
            prms[4].Value = Notes;
            prms[5] = new SqlParameter("@WaitingTime", SqlDbType.Int);
            prms[5].Value = waitingTime;
            prms[6] = new SqlParameter("@Status", SqlDbType.Char);
            prms[6].Value = Status;
            cmd.Parameters.AddRange(prms);
            try
            {
                connect.Open();
                cmd.ExecuteNonQuery();
               
            }
            finally
            {
                connect.Close();
            }

        }
    }

    [Serializable]
    public class CustomerQueueCol:IList<CustomerQueue>
    {
        List<CustomerQueue> CustomerQueueColumns = new List<CustomerQueue>();
        public CustomerQueueCol(int ClientID)
        {
            Load(ClientID);
        }

        public CustomerQueueCol(string Status)
        {
            Load(Status);
        }

        public void Load(int ClientID)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_GetCustomerWaitingInQueue", connect);
                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pClientID = new SqlParameter("@ClientID", SqlDbType.Int);
                pClientID.Value = ClientID;
                cmd.Parameters.Add(pClientID);

                SqlDataReader dr = cmd.ExecuteReader();

                CustomerQueue customerQueueCol;
                while (dr.Read())
                {
                    customerQueueCol = new CustomerQueue();

                    customerQueueCol.CustomerQueueID = Convert.ToInt32(dr["CustomerQueueID"].ToString());
                    customerQueueCol.CustomerID = Convert.ToInt32(dr["CustomerID"].ToString());
                    customerQueueCol.ClientID = Convert.ToInt32(dr["ClientID"].ToString());
                    customerQueueCol.QCustomerName = dr["QCustomerName"].ToString();
                    customerQueueCol.PartySize = Convert.ToInt32(dr["PartySize"].ToString());
                    customerQueueCol.Notes = dr["Notes"].ToString();
                    customerQueueCol.waitingTime = Convert.ToInt32(dr["WaitingTime"].ToString());
                    customerQueueCol.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                    customerQueueCol.MobileNo = dr["MobileNo"].ToString();
                    customerQueueCol.Status = dr["Status"].ToString();
                    customerQueueCol.StatusDesc = dr["StatusDesc"].ToString();
                    CustomerQueueColumns.Add(customerQueueCol);

                }

            }
            finally
            {
                connect.Close();
            }
        }
        public void Load(string Status)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("spQM_GetCustomerQueueByStatus", connect);
                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pStatus = new SqlParameter("@Status", SqlDbType.VarChar);
                pStatus.Value = Status;
                cmd.Parameters.Add(pStatus);

                SqlDataReader dr = cmd.ExecuteReader();
                
                CustomerQueue customerQueueCol;
                while (dr.Read())
                {
                    customerQueueCol = new CustomerQueue();
                    
                     customerQueueCol.CustomerQueueID = Convert.ToInt32(dr["CustomerQueueID"].ToString());
                     customerQueueCol.CustomerID = Convert.ToInt32(dr["CustomerID"].ToString());
                     customerQueueCol.ClientID = Convert.ToInt32(dr["ClientID"].ToString());
                     customerQueueCol.QCustomerName = dr["QCustomerName"].ToString();
                     customerQueueCol.PartySize = Convert.ToInt32(dr["PartySize"].ToString());
                     customerQueueCol.Notes = dr["Notes"].ToString();
                     customerQueueCol.waitingTime = Convert.ToInt32(dr["WaitingTime"].ToString());
                     customerQueueCol.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                     customerQueueCol.MobileNo = dr["MobileNo"].ToString();
                     customerQueueCol.Status = dr["Status"].ToString();
                     CustomerQueueColumns.Add(customerQueueCol);
                     
                }
            }
            finally
            {
                connect.Close();
            }
        }

       #region IList<CustomerQueue> Members

        public int IndexOf(CustomerQueue item)
        {
            return CustomerQueueColumns.IndexOf(item);
        }

        public void Insert(int index, CustomerQueue item)
        {
            CustomerQueueColumns.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            CustomerQueueColumns.RemoveAt(index);
        }
     
        public CustomerQueue this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<CustomerQueue> Members

        public void Add(CustomerQueue item)
        {
            CustomerQueueColumns.Add(item);
        }

        public void Clear()
        {
            CustomerQueueColumns.Clear();
        }

        public bool Contains(CustomerQueue item)
        {
            return CustomerQueueColumns.Contains(item);
        }

        public void CopyTo(CustomerQueue[] array, int arrayIndex)
        {
            CustomerQueueColumns.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return CustomerQueueColumns.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(CustomerQueue item)
        {
            return CustomerQueueColumns.Remove(item);
        }
     
       
        #endregion

        #region IEnumerable<CustomerQueue> Members

        public IEnumerator<CustomerQueue> GetEnumerator()
        {
            return CustomerQueueColumns.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return CustomerQueueColumns.GetEnumerator();
        }

        #endregion

    }

    [Serializable]
    public class TransactionLog
    {
        public TransactionLog()
        {
        }
        public int TransactionLogID { get; set; }
        public int CustomerQueueID { get; set; }
        public string Status { get; set; }
        public bool SMSSent { get; set; }
        public bool SMSReceive { get; set; }
        public string MobileNo { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public void Store()
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["QueueManagement"].ToString());

            SqlCommand cmd = new SqlCommand("spQM_SaveTransactionLog", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] prms = new SqlParameter[5];
            prms[0] = new SqlParameter("@CustomerQueueID", SqlDbType.Int);
            prms[0].Value = CustomerQueueID;
            prms[1] = new SqlParameter("@Status", SqlDbType.Char);
            prms[1].Value = Status;
            prms[2] = new SqlParameter("@SMSSent", SqlDbType.Bit);
            prms[2].Value = SMSSent;
            prms[3] = new SqlParameter("@SMSReceive", SqlDbType.Bit);
            prms[3].Value = SMSReceive;
            prms[4] = new SqlParameter("@MobileNo", SqlDbType.VarChar);
            prms[4].Value = MobileNo;
            cmd.Parameters.AddRange(prms);
            try
            {
                connect.Open();
                cmd.ExecuteNonQuery();

            }
            finally
            {
                connect.Close();
            }

        }



    }
}
