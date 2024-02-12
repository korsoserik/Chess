using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class user
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public decimal money { get; set; }
        //public DateTime freereward { get; set; }

        public user(string sor)
        {
            string[] data = sor.Split(';');
            id = int.Parse(data[0]);
            name = data[1];
            password = data[2];
            money = decimal.Parse(data[3]);
            //freereward  = DateTime.Parse(data[3]);
        }
    }
}
