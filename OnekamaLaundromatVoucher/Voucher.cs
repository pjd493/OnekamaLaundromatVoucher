using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Voucher
{
    public int amount;
    public string name;
    public string serial;
    public int total;
    public long date; 

    public Voucher()
    {

    }
    public Voucher(string cardNumber)
    {

    }

    public Voucher(int amount, string name, string serial, int total, long date)
    {
        this.amount = amount;
        this.name = name;
        this.serial = serial;
        this.total = total;
        this.date = date;
    }

}
