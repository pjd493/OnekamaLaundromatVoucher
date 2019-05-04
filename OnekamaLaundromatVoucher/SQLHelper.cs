using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

public class SqlHelper
{
    public static String conString = OnekamaLaundromatVoucher.Properties.Settings.Default.cs;
    public Voucher RedeemableVoucher = new Voucher();

    /// <summary>
    /// Updates the SQL Table chequingtotal value with the most recent Chequing Account Balance.
    /// </summary>
    /// <param name="ReddemableVoucher"></param>
    public static void UpdateVoucherList(Voucher RedeemableVoucher)
    {
        using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PJ\\source\\repos\\OnekamaLaundromatVoucher\\OnekamaLaundromatVoucher\\somethingdifferent.mdf;Integrated Security=True"))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO voucher(amount, name, serial, total, time) VALUES (@Amount, @Name, @Serial, @Total, @Date)", connection);


            cmd.Parameters.AddWithValue("@Serial", RedeemableVoucher.serial);
            cmd.Parameters.AddWithValue("@Amount", RedeemableVoucher.amount);
            cmd.Parameters.AddWithValue("@Name", RedeemableVoucher.name);
            cmd.Parameters.AddWithValue("@Total", RedeemableVoucher.total);
            cmd.Parameters.AddWithValue("@Date", RedeemableVoucher.date);

            cmd.ExecuteNonQuery();
           

        }

    }

    /// <summary>
    /// Gets lowest Total value to display remaining funds.
    /// </summary>
    /// <param</param>
    public static decimal CheckTotalFunds()
    {
        using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PJ\\source\\repos\\OnekamaLaundromatVoucher\\OnekamaLaundromatVoucher\\somethingdifferent.mdf;Integrated Security=True"))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT MIN(total) FROM voucher", connection);

            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            decimal total = Convert.ToDecimal(reader.GetSqlValue(0).ToString());
             
            
            return total;

        }

    }



}
