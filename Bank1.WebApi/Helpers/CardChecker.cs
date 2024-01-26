namespace Bank1.WebApi.Helpers
{
    public static class CardChecker
    {
        public static bool IsCardExpired(string expiratoryDate)
        {
            try
            {
                var expiratoryDateSplit = expiratoryDate.Split('/');
                if (expiratoryDateSplit.Length != 2) return true;

                var month = int.Parse(expiratoryDateSplit[0]);
                var year = int.Parse(expiratoryDateSplit[1]);

                if (year < DateTime.Today.Year)
                {
                    return true;
                }
                else
                {
                    if (month < DateTime.Today.Month)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }
    }
}
