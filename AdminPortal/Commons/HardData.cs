using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
namespace AdminPortal.Commons
{
    public static class HardData
    {
        public static string[] location = {
            "118 Hồng Bàng, Q.5, Tp. Hồ Chí Minh",
            "201 Phạm Viết Chánh, P. Nguyễn Cư Trinh, Q.1, Tp. Hồ Chí Minh"
        };

        public static string[] bidMethod = {
            "Chào hàng cạnh tranh rút gọn",
            "Chỉ định thầu rút gọn",
            "Chỉ định thầu thông thường"
        };

        public static string[] ROLE_HARD = {
            "proposal",
            "explanation",
            "survey",
            "quote",
            "audit",
            "bidPlan",
            "negotiation",
            "decision",
            "contract",
            "DdeliveryReceipt",
            "acceptance"
        };

        public static string[] NegotiationBankIDArr = {
            "3714.0.1019776.00000 tại Kho Bạc Quận 5",
            "3713.0.1019776.00000 tại Kho Bạc Quận 5",
            "113000087053 tại Ngân hàng TMCP Công Thương Việt Nam(Vietinbank) – CN 5 TPHCM"        
        };
        
    }
}
