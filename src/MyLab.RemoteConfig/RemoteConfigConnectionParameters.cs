namespace MyLab.RemoteConfig
{
    /// <summary>
    /// Contains configuration server connection parameters
    /// </summary>
    public class RemoteConfigConnectionParameters
    {
        /// <summary>
        /// Server URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Server Host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// User id / Login
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}