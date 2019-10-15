namespace ColoudDelUReceOrderTool
{
    public class SqlList
    {
        private string _result;

        public string GetUpdate(string orderno)
        {
            _result = $@"
                            UPDATE dbo.T_AR_RECEIVABLE SET F_YTC_COMBO= 0
                            where FBILLNO='{orderno}'
                        ";
            return _result;
        }

    }
}
