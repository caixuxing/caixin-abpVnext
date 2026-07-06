namespace CaiXin.NiuMa.Infrastructure.Dto
{
    /// <summary>
    /// 地址
    /// </summary>
    /// <param name="Province"></param>
    /// <param name="City"></param>
    /// <param name="Detail"></param>
    /// <param name="ZipCode"></param>
    public record Address(string Province, string City, string Detail, string ZipCode)
    {

        public string Province { get; init; } =
            !string.IsNullOrWhiteSpace(Province)
                ? Province
                : throw new ArgumentException("省份不能为空", nameof(Province));

        public string City { get; init; } =
            !string.IsNullOrWhiteSpace(City) ? City : throw new ArgumentException("城市不能为空", nameof(City));

        public string Detail { get; init; } =
            !string.IsNullOrWhiteSpace(Detail)
                ? Detail
                : throw new ArgumentException("详细地址不能为空", nameof(Detail));

        public string ZipCode { get; init; } =
            !string.IsNullOrWhiteSpace(ZipCode) && System.Text.RegularExpressions.Regex.IsMatch(ZipCode, @"^\d{6}$")
                ? ZipCode
                : throw new ArgumentException("邮政编码必须为6位数字", nameof(ZipCode));

        public string FullAddress => $"{Province}{City}{Detail}（邮编：{ZipCode}）";
        public bool IsMunicipality => Province is "北京" or "上海" or "天津" or "重庆";
    }

    /// <summary>
    /// 货币
    /// </summary>
    /// <param name="Amount"></param>
    /// <param name="Currency"></param>
    public record struct Money(decimal Amount, string Currency)
    {
        public decimal Amount { get; init; } = Amount >= 0 ? Amount : throw new ArgumentException("金额不能为负数", nameof(Amount));

        public string Currency { get; init; } = !string.IsNullOrWhiteSpace(Currency) && Currency.Length == 3 ? Currency.ToUpperInvariant() : throw new ArgumentException("币种必须为3位货币代码", nameof(Currency));

        public Money ToCny(decimal rate) => this with { Amount = Amount * rate, Currency = "CNY" };
        public override string ToString() => $"{Currency} {Amount:F2}";
    }
}