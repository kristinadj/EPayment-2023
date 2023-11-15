namespace Base.Services.AppSettings
{
    public class ConsulAppSettings
    {
        public string Host { get; set; }    
        public bool Enabled { get; set; }
        public string Service { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string Type { get; set; }

        public ConsulAppSettings() 
        { 
            Host = string.Empty;
            Service = string.Empty;
            Address = string.Empty;
            Type = string.Empty;
        }
    }
}
