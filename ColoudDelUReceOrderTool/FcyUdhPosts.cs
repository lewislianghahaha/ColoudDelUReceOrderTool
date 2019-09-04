using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace ColoudDelUReceOrderTool
{
    public class FcyUdhPosts
    {
        /// <summary>
        /// 删除U订货上的"退款单"(K3对应:收款退款单,其他应收单,应收单)
        /// </summary>
        /// <param name="cOutSysKey"></param>
        /// <returns></returns>
        public static string Tkddel(string cOutSysKey)
        {
            string ret = "";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("outsyskey", cOutSysKey);
            ret = FcyWeb.Post("/ws/Payments/delRefund", param);
            return ret;
        }

        /// <summary>
        /// 删除付款单
        /// </summary>
        /// <param name="cOutSysKey"></param>
        /// <returns></returns>
        public static string Zfddel(string cOutSysKey)
        {
            string ret = "";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("cOutSysKey", cOutSysKey);
            ret = FcyWeb.Post("/ws/Payments/delPayment", param);
            return ret;
        }

        /// <summary>
        /// 删除U订货上的"发货通知单"(k3对应:应收单)
        /// </summary>
        /// <param name="cOutSysKey"></param>
        /// <returns></returns>
        public static string Xsfhdelup(string cOutSysKey)
        {
            string ret = "";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("cOutSysKey", cOutSysKey);
            ret = FcyWeb.Post("/ws/Orders/delDelivery", param);
            return ret;
        }

        /// <summary>
        /// 订单 回退(k3:销售订单使用)
        /// </summary>
        /// <param name="dh"></param>
        /// <returns></returns>
        public static string Ddht(string dh)
        {
            string ret = "";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("orderno", dh);
            ret = FcyWeb.Post("/ws/Orders/orderConfirmBackApi", param);
            return ret;
        }

        /// <summary>
        /// 上传应收单
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string Ysdup(DataRow row)
        {
            string ret = "";
            fcydata.Service ab = new fcydata.Service();
            if (row["je"].ToString().Trim().IndexOf("-") > 0)
            {
                row["je"] = -(decimal)row["je"];
                MessageBox.Show(row["je"].ToString());
                Dictionary<string, string> param = new Dictionary<string, string>();

                DateTime date = (DateTime)row["rq"];
                string strs = "{'iAmount':<je>,'cOutSysKey':'<dh>','oAgent':{'cCode':'<khbh>','cOutSysKey':'<khbh>'},'oSettlementWay':{'cErpCode':'<jsfs>'},'iPayMentStatusCode':2,'cRefundPayDirection': 'TOUDH','dPayFinishDate':'" + date.ToString("yyyy-MM-dd HH:mm:ss") + "','dReceiptDate':'" + date.ToString("yyyy-MM-dd HH:mm:ss") + "','remark':'<memo>'}";

                for (int j = 0; j < row.Table.Columns.Count; j++)
                {
                    strs = strs.Replace("<" + row.Table.Columns[j].ColumnName + ">", row[j].ToString().Trim());
                }
                param.Add("refund", strs);
                ret = FcyWeb.Post("/ws/Payments/saveRefund", param);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(ret);
                if (fcydata.FcyXml.GetNodeVal(xmldoc.DocumentElement, "data") != "")
                {
                    MessageBox.Show(ret);
                }

           

            }
            else
            {
                Dictionary<string, string> param = new Dictionary<string, string>();

                DateTime date = (DateTime)row["rq"];
                string strs = "{'iAmount':<je>,'cOutSysKey':'<dh>','oAgent':{'cErpCode':'<khbh>'},'oSettlementWay':{'cErpCode':'<jsfs>'},'cVoucherType':'NORMAL','iPayMentStatusCode':2,'dPayFinishDate':'" + date.ToString("yyyy-MM-dd HH:mm:ss") + "','dReceiptDate':'" + date.ToString("yyyy-MM-dd HH:mm:ss") + "','remark':'<memo>'}";

                for (int j = 0; j < row.Table.Columns.Count; j++)
                {
                    strs = strs.Replace("<" + row.Table.Columns[j].ColumnName + ">", row[j].ToString().Trim());
                }
                param.Add("payment", strs);

                ret = FcyWeb.Post("/ws/Payments/savePayment", param);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(ret);
                if (fcydata.FcyXml.GetNodeVal(xmldoc.DocumentElement, "data") != "")
                {
                    MessageBox.Show(ret);
                }
            }
            return ret;
        }
    }
}
