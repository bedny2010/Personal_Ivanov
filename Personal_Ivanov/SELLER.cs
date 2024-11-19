//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Personal_Ivanov
{
    using System;
    using System.Collections.Generic;
    
    public partial class SELLER
    {
        public int Seller_ID { get; set; }
        public string Surename { get; set; }
        public string Name { get; set; }
        public string Patronumic { get; set; }
        public int Trade_point { get; set; }
        public int Position { get; set; }
        public System.DateTime Year_birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Photo { get; set; }
        public string Pasport_data { get; set; }
        public string Phone_number { get; set; }
        public System.DateTime Work_date { get; set; }
    
        public virtual POSITION POSITION1 { get; set; }
        public virtual TRADE_POINT TRADE_POINT1 { get; set; }

        public string SellerPosition { get { return POSITION1.Position_name; } }

        public decimal SellerSalary { get { return POSITION1.Salary; } }
        public string Trade_point_name1 { get { return TRADE_POINT1.Traid_point_name; } }

        public string DateWorkFormat
        {
            get
            {
                string formatDate = Work_date.ToString("d MM yyyy");
                return formatDate;
            }
        }

    }
}
