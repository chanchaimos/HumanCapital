using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClassExecute
/// </summary>
namespace ClassExecute
{
    public class ST_Result
    {
        public string sStatus { get; set; }
        public string sMessage { get; set; }
        public string sMessage1 { get; set; }
        public string sMessage2 { get; set; }
        public string sMessage3 { get; set; }
    }

    public class CData_File
    {
        public int? nID { get; set; }
        public int? nProjectID { get; set; }
        public string sDescription { get; set; }
        public string sFilename { get; set; }
        public string sPath { get; set; }
        public string sSysFileName { get; set; }
        public int? nUserID { get; set; }
        public string sUpdate { get; set; }
    }

    public class c_djsi
    {
        public int nItem { get; set; }
        public string sName { get; set; }
        public Nullable<int> nItemHead { get; set; }
        public Nullable<int> nSibling { get; set; }
        public Nullable<bool> IsTotal { get; set; }
        public Nullable<int> nUnit { get; set; }
        public string sUnit { get; set; }
        public Nullable<bool> IsHead { get; set; }
        public Nullable<bool> IsAutoCal { get; set; }

        public Nullable<decimal> nMale_1 { get; set; }
        public Nullable<decimal> nMale_2 { get; set; }
        public Nullable<decimal> nMale_3 { get; set; }
        public Nullable<decimal> nFemale_1 { get; set; }
        public Nullable<decimal> nFemale_2 { get; set; }
        public Nullable<decimal> nFemale_3 { get; set; }
        public Nullable<decimal> nMale_Total { get; set; }
        public Nullable<decimal> nFemale_Total { get; set; }

        public bool IsChecked { get; set; }
        public bool IsDecimal { get; set; }
    }
}