﻿using DataBaseManger.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DataBaseManger.SqlLite
{
    public class OrderTableSqlite
    {
        public OrderTableSqlite()
        {

        }
        public static void createTable()
        {
            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();
            string query = "CREATE TABLE IF NOT EXISTS OrderTable (" +
                "orderID INTEGER PRIMARY KEY , " +
                "orderDate TEXT , " +
                "paymentDone INTEGER , " +
                "customerID INTEGER, " +
                "paymentID INTEGER, " +
                "orderType INTEGER, " +
                "InvoiceNo INTEGER)";
            SQLiteCommand command = new SQLiteCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public static void addData(OrderTableModel OrderTable)
        {
            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();
            string query = "INSERT INTO OrderTable (" +
                "orderID  , " +
                "orderDate  , " +
                "paymentDone  , " +
                "customerID , " +
                "paymentID , " +
                "orderType , " +
                "InvoiceNo) " +
                "VALUES (" +
                OrderTable.orderID.ToString() + ",\'" +
                OrderTable.orderDate.ToString() + "\',\'" +
                OrderTable.paymentDone + "\',\'" +
                OrderTable.customerID + "\',\'" +
                OrderTable.paymentID + "\',\'" +
                OrderTable.orderType + "\',\'" +
                OrderTable.InvoiceNo + "\')";
            SQLiteCommand command = new SQLiteCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static List<OrderTableModel> readAll(int data)
        {
            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();
            string query = "select * from OrderTable where orderType = " + data.ToString();
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            List<OrderTableModel> OrderTableModels = new List<OrderTableModel>();

            while (reader.Read())
            {


                var ordertable = new OrderTableModel(
                    reader.GetInt32(0),
                    (string)reader["orderDate"],
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5),
                    reader.GetInt32(6)

                    );

                OrderTableModels.Add(ordertable);
            }
            conn.Close();
            return OrderTableModels;
        }
        public static List<OrderTableModel> getDatabyCustomerID(int data)
        {
            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();
            string query = "select * from OrderTable where customerID = " + data.ToString() + " ORDER BY orderDate DESC;";
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            List<OrderTableModel> OrderTableModels = new List<OrderTableModel>();

            while (reader.Read())
            {


                var ordertable = new OrderTableModel(
                    reader.GetInt32(0),
                    (string)reader["orderDate"],
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5),
                    reader.GetInt32(6)

                    );

                OrderTableModels.Add(ordertable);
            }
            conn.Close();
            return OrderTableModels;
        }
        public static OrderTableModel getSingleDatabyId(int data)
        {
            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();
            string query = "select * from OrderTable where orderID = " + data.ToString();
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            OrderTableModel ordertable = null;

            while (reader.Read())
            {


                ordertable = new OrderTableModel(
                    reader.GetInt32(0),
                    (string)reader["orderDate"],
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5),
                    reader.GetInt32(6)

                    );


            }
            conn.Close();
            return ordertable;
        }

        public static string getOrderTypebyId(int data)
        {
            string ans = "";
            switch (data)
            {
                case 1:
                    ans = "Sale";
                    break;
                case 2:
                    ans = "Purchase";
                    break;
                case 3:
                    ans = "PaymentIn";
                    break;
                case 4:
                    ans = "PaymentOut";
                    break;
                case 5:
                    ans = "Credit Note";
                    break;
                case 6:
                    ans = "Debit Note";
                    break;
            }
            return ans;
        }
        public static int getInvoiceNo(int data)
        {
            int ans = -1;
            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();
            string query = "SELECT max(InvoiceNo) as result from OrderTable where orderType = " + data.ToString();
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    ans = reader.GetInt32(0);
            }
            conn.Close();
            return ans;
        }

        public static List<SaleInvoiceModel> getInvoices(int data)
        {
            List<SaleInvoiceModel> saleInvoiceModels = new List<SaleInvoiceModel>();


            var OrderData = readAll(data);
            List<paymentModel> paymentModels = new List<paymentModel>();
            List<customerModel> customerModels = new List<customerModel>();

            if (OrderData != null)
            {
                foreach (var order in OrderData)
                {
                    var tempcustomer = CustomerSqllite.getSingleDataByID(order.customerID);
                    var temppaymnet = paymentSqlite.getSingleDataByID(order.paymentID);
                    customerModels.Add(tempcustomer);
                    paymentModels.Add(temppaymnet);

                    string ordredate = Convert.ToDateTime(order.orderDate).ToString("dd-MM-yyyy");

                    var tempsale = new SaleInvoiceModel(ordredate,
                        order.InvoiceNo, tempcustomer.customerName, temppaymnet.paymentType, temppaymnet.TotalBalance, temppaymnet.remainingBalance);
                    saleInvoiceModels.Add(tempsale);
                }
            }


            return saleInvoiceModels;
        }

        public static List<PaymentInModel> getInvoicesByPaymentInModel(int data)
        {
            List<PaymentInModel> PaymentInModels = new List<PaymentInModel>();
            string OrderTypetemp = "";
            if (data == 3)
            {
                OrderTypetemp = "Payment-In";
            }
            else if (data == 4)
            {
                OrderTypetemp = "Payment-Out";
            }
            else if (data == 5)
            {
                OrderTypetemp = "Credit Note";
            }
            else if (data == 6)
            {
                OrderTypetemp = "Debit Note";
            }
            var OrderData = readAll(data);
            List<paymentModel> paymentModels = new List<paymentModel>();
            List<customerModel> customerModels = new List<customerModel>();

            if (OrderData != null)
            {
                int i = 0;
                foreach (var order in OrderData)
                {
                    ++i;
                    var tempcustomer = CustomerSqllite.getSingleDataByID(order.customerID);
                    var temppaymnet = paymentSqlite.getSingleDataByID(order.paymentID);
                    customerModels.Add(tempcustomer);
                    paymentModels.Add(temppaymnet);

                    var tempsale = new PaymentInModel(
                        i,
                        order.orderDate,
                        order.InvoiceNo,
                        tempcustomer.customerName,
                        OrderTypetemp,
                        temppaymnet.TotalBalance,
                        temppaymnet.paymentAmount,
                        temppaymnet.remainingBalance);
                    PaymentInModels.Add(tempsale);
                }
            }


            return PaymentInModels;
        }

        public static OrderTableModel getDatabyPaymentID(int data)
        {
            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();
            string query = "select * from OrderTable where paymentID = " + data.ToString();
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            OrderTableModel ordertable = null;

            while (reader.Read())
            {


                ordertable = new OrderTableModel(
                    reader.GetInt32(0),
                    (string)reader["orderDate"],
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5),
                    reader.GetInt32(6)

                    );


            }
            conn.Close();
            return ordertable;
        }

        public static string getAllSalebyMonth(string firstMonth, string secondMonth)
        {

            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();

            string query = "SELECT sum(orderSellingPrice) from orderProductRelationModel WHERE orderID in (SELECT orderID from OrderTable WHERE orderDate>'" + firstMonth + "' AND orderDate<'" + secondMonth + "');";
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            string ans = null;

            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    ans = "0";
                }
                else
                {
                    ans = reader.GetFloat(0).ToString();
                }

            }
            conn.Close();
            return ans;
        }
        public static string getSumOfTotalReceiveAmount()
        {

            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();

            string query = "SELECT sum(TotalAmount) as ansAmount FROM CUSTOMER WHERE TotalAmount>0";
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            string ans = null;

            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    ans = "0";
                }
                else
                {
                    ans = reader.GetFloat(0).ToString();
                }

            }
            conn.Close();
            return ans;
        }
        public static List<customerModel> getAllReciversNames()
        {
            SQLiteConnection conn = DbConnection.createDbConnection();
            conn.Open();
            string query = "SELECT * FROM CUSTOMER WHERE TotalAmount>0 ORDER BY TotalAmount DESC ";
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            List<customerModel> customerModels = new List<customerModel>();

            while (reader.Read())
            {

                var customer = new customerModel(
                    reader.GetInt32(0),
                    (string)reader["customerName"],
                    reader.GetInt32(2),
                    (string)reader["customerMobile"],
                    (string)reader["customerAddress"],
                    reader.GetFloat(5),
                    reader.GetFloat(6)
                    );

                customerModels.Add(customer);
            }
            conn.Close();
            return customerModels;
        }
       
    }
}
