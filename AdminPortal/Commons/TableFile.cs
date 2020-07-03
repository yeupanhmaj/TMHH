using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Commons
{
    public enum TableFile
    {
        [Description("Đề xuất")]
        Proposal = 1,

        [Description("Giải trình")]
        Explanation = 2,

        [Description("Giải trình")]
        Survey = 3,

        [Description("Báo giá")]
        Quote = 4,

        [Description("Họp giá")]
        Audit = 5,

        [Description("Kế hoạch chọn thầu")]
        BidPlan = 6,

        [Description("Thương thảo hợp đồng")]
        Negotiation = 7,

        [Description("Quyết định chọn thầu")]
        Decision = 8,

        [Description("Hồ sơ hợp đồng")]
        Contract = 9,

        [Description("Biên bản giao nhận")]
        DeliveryReceipt = 10,

        [Description("Biên bản nghiệm thu")]
        Acceptance = 11,

        [Description("Comment")]
        Comment = 12,
    }
}
