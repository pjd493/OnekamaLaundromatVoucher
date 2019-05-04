using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace OnekamaLaundromatVoucher
{
    public partial class VoucherForm : Form
    {
        public static string amount;
        public static string serial;
        decimal total;
        long randomNumberBase = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
        
        /// <summary>
        ///Starts Form. Gets Total funds remaining from SQL database.         
        /// </summary>
        /// <param>none</param>
        public VoucherForm()
        {
            InitializeComponent();
            total = SqlHelper.CheckTotalFunds();
            FundsLeftLabel.Text = "Funds Remaining: " + Convert.ToString(total);
        }


        /// <summary>
        ///Clear box upon click.          
        /// </summary>
        /// <param>none</param
        private void UserFundsBox_Click(object sender, EventArgs e)
        {
            UserFundsBox.Text = "";
        }

        /// <summary>
        ///Clear box upon click.          
        /// </summary>
        /// <param>none</param
        private void UserInfoBox_Click(object sender, EventArgs e)
        {
            UserInfoBox.Text = "";
        }
        
        /// <summary>
        ///Generates random serial, updates database with Voucher, and prints off Voucher.         
        /// </summary>
        /// <param>none</param
        public void CreateVoucherButton_Click(object sender, EventArgs e)
        {
            //Generates semi-random code.
            decimal FundsAmount;
            bool isNumeric = decimal.TryParse(UserFundsBox.Text, out FundsAmount);
            if (total > 0 & isNumeric == true)
            { 

            long random = (randomNumberBase * randomNumberBase);

            FundsLabel.Text = "$" + UserFundsBox.Text;
            RandomGeneratedCodeLabel.Text = UserInfoBox.Text + Convert.ToString(random);
            amount = UserFundsBox.Text;
            serial = Convert.ToString(random);

            //Creates Voucher object and sends to SQL database. 
            Voucher RedeemableVoucher = new Voucher();
            RedeemableVoucher.amount = Convert.ToInt32(amount);
            RedeemableVoucher.serial = serial;
            RedeemableVoucher.name = UserInfoBox.Text;
            RedeemableVoucher.total = Convert.ToInt32(total) - Convert.ToInt32(amount);
            RedeemableVoucher.date = randomNumberBase;

            SqlHelper.UpdateVoucherList(RedeemableVoucher);

            Validation.TestEmail.SendIt();
            total = SqlHelper.CheckTotalFunds();
            FundsLeftLabel.Text = "Funds Remaining: " + Convert.ToString(total);

            //Starts setting  up print document and dialog, sends print job gets rid of it after sending print job. 
            PrintDocument Document = new PrintDocument();

            Document.PrintPage += new PrintPageEventHandler(Document_PrintPage);

            PrintPreviewDialog Load = new PrintPreviewDialog();
            Load.Document = Document;
            Load.ShowDialog(this);

            Document.Dispose();

            }
            else
            {
                MessageBox.Show("You either have entered an invalid number or are out of funds. Please enter a number or contact Onekama Laundromat.");
            }

        }

        /// <summary>
        /// Handles what gets printed.        
        /// </summary>
        /// <param>none</param
        private void Document_PrintPage(object sender, PrintPageEventArgs e)
        {

            Bitmap pageMap = new Bitmap(400, 400, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            this.DrawToBitmap(pageMap, this.DisplayRectangle);
            //Set locations for elements.
            Point imagePoint = new Point(200, 200);
            Point serialPoint = new Point(200, 140);
            Point amountPoint = new Point(220, 160);
            Point schedulePoint = new Point(200, 115);
            Point instructionsPoint = new Point(200, 50);

            //Set font
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            
            //Draw elements
            e.Graphics.DrawImage(pictureBox1.Image, imagePoint);
            e.Graphics.DrawString(RandomGeneratedCodeLabel.Text, this.Font, drawBrush, serialPoint);
            e.Graphics.DrawString("$" + UserFundsBox.Text, this.Font, drawBrush, amountPoint);
            e.Graphics.DrawString(ScheduleLabel.Text, this.Font, drawBrush, schedulePoint);
            e.Graphics.DrawString(InstructionsLabel.Text + SponserLabel.Text, this.Font, drawBrush, instructionsPoint);

            //Remove Bitmap
            pageMap.Dispose();
        }
    }

        
}
