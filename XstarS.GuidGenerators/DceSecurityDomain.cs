namespace XstarS.GuidGenerators
{
    /// <summary>
    /// 表示 DCE Security UUID (<see cref="GuidVersion.Version2"/>) 使用的域。
    /// </summary>
    public enum DceSecurityDomain
    {
        /// <summary>
        /// 表示 DCE Security Person 域。
        /// </summary>
        Person = 1,
        /// <summary>
        /// 表示 DCE Security Group 域。
        /// </summary>
        Group = 2,
        /// <summary>
        /// 表示 DCE Security Org 域。
        /// </summary>
        Org = 3,
    }
}
