using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratePDFDemo
{
    public class ComparisonQuotaValue
    {
        /// <summary>
        /// 指标ID
        /// </summary>
        public string QuotaId { get; set; }

        /// <summary>
        /// 指标名称
        /// </summary>
        public string QuotaName { get; set; }

        /// <summary>
        /// 阶段
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// 周期名称
        /// </summary>
        public string CycleName { get; set; }

        /// <summary>
        /// 显示属性
        /// </summary>
        public List<QuotaAttribute> Attributes { get; set; }

        /// <summary>
        /// 企业对标值
        /// </summary>
        public List<EnterpriseComparisonQuotaValue> EnterpriseComparisonQuotaValue { get; set; }
    }

    /// <summary>
    /// 指标属性
    /// </summary>
    public class QuotaAttribute
    {
        /// <summary>
        /// 属性编码
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 默认对标属性
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 默认图表类型
        /// </summary>
        public short CharType { get; set; }
    }

    /// <summary>
    /// 企业对标值DTO
    /// </summary>
    public class EnterpriseComparisonQuotaValue
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        ///组织名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// 对标属性值
        /// </summary>
        public Dictionary<string, object> Values { get; set; }
    }
}