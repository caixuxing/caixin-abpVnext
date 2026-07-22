namespace CaiXin.NiuMa.Host.Configs
{
    /// <summary>
    /// 私有云
    /// </summary>
    public class PrivateColud
    {
        /// <summary>
        /// 名字
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public required int Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public required string Sex { get; set; }
        /// <summary>
        /// 爱好
        /// </summary>
        public required List<string> Hobby { get; set; }
    }
}
