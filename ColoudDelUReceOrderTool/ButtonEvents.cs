using System;
using fcydata;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;

namespace ColoudDelUReceOrderTool
{
    public class ButtonEvents : AbstractBillPlugIn
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            //订单退回操作
            base.BarItemClick(e);

            var resultMessage = string.Empty;

            //删除U订货单据(除退款单外使用)
            if (e.BarItemKey == "tbDelUOrderR")
            {
                //定义获取表头信息对像
                var docScddIds1 = View.Model.DataObject;
                //获取表头中单据编号信息(注:这里的BillNo为单据编号中"绑定实体属性"项中获得)
                var dhstr = docScddIds1["BillNo"].ToString();

                fcy.Service.CnnStr = "http://172.16.4.252/websys/service.asmx";
                fcy.Service.userdmstr = "feng";
                fcy.Service.passwordstr = "";

                //将获取的单据名称进行截取,取前两位
                var orderno = dhstr.Substring(0, 2);

                //根据获取的标记分别进行单据删除
                switch (orderno)
                {
                    //其他应收单
                    case "QT":
                        resultMessage = DelReturnOrder(dhstr);
                        break;
                    //收款退款单
                    case "SK":
                        resultMessage = DelReturnOrder(dhstr);
                        break;
                    //应收单
                    case "AR":
                        resultMessage = DelReturnOrder(dhstr);
                        //更新应收单F_YTC_COMBO=0
                        UpdateK3(dhstr);
                        break;
                }
                //输出结果
                View.ShowMessage(resultMessage);
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="orderno"></param>
        private void UpdateK3(string orderno)
        {
            var generate=new Generate();
            generate.UpdateK3Record(orderno);
        }

        /// <summary>
        /// 执行删除销售订单
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        private string DelSalesOrder(string orderno)
        {
            var ab = new Service();
            var result = string.Empty;

            var udhstr = ab.str("select udh from axsdd where dh='" + orderno + "'");
            if (udhstr.Trim().Length > 0)
            {
                ab.sqlcmd("delete uddzy where cOrderNo='" + udhstr + "'");
                ab.sqlcmd("delete uOrder where cOrderNo='" + udhstr + "'");
                FcyUdhPosts.Ddht(udhstr);
               
                result =$"{orderno}已在U订货平台内删除";
            }
            else
            {
                result=$"没有在U订货平台查找到{orderno}单据记录,故没有执行删除操作";
            }
            return result;
        }

        /// <summary>
        /// 删除收款退款单
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        private string DelReturnOrder(string orderno)
        {
            var result = string.Empty;

            try
            {
                result = FcyUdhPosts.Tkddel(orderno);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除U订货上的"发货通知单"
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        private string DelReceive(string orderno)
        {
            var result = string.Empty;

            try
            {
                result= FcyUdhPosts.Xsfhdelup(orderno);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

    }
}
