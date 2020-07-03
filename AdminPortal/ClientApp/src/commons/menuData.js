
export const listData = [
    {
        key: 'Quản lý đề xuất',
        label: 'Quản lý đề xuất',
        icon: 'fa fa-file',     
        nodes: [
            {
                key: 'đề xuất',
                label: 'đề xuất',
                url: '/de-xuat',
                icon: 'fa fa-file',
                role:'proposal',
            },
        ]
    },
    {
        key: 'Quản lý Thu mua',
        label: 'Quản lý Thu mua',
        icon: 'fa fa-balance-scale',
        nodes: [
            {
                key: 'Báo giá',
                label: 'Báo giá',
                url: '/bao-gia-kem-de-xuat',
                icon: 'fa fa-file',
                role:'quote',
            },
            {
                key: 'Biên bản Họp giá',
                label: 'Biên bản kiểm giá',
                url: '/bien-ban-hop-gia',
                icon: 'fa fa-file',
                role:'audit',
            },
            {
                key: 'Lập kế hoạch chọn thầu',
                label: 'Kế hoạch chọn thầu',
                url: '/lap-ke-hoach-chon-thau',
                icon: 'fa fa-file',
                role:'bidPlan',
            },
            {
                key: 'Biên bản thương thảo HĐ',
                label: 'Thương thảo hợp đồng',
                url: '/bien-ban-thuong-thao-hd',
                role: 'negotiation',
                icon: 'fa fa-file',
            },
             
            {
                key: 'Hồ sơ quyết định chọn thầu',
                label: 'Quyết định',
                url: '/ho-so-quyet-dinh-chon-thau',
                role: 'decision',
                icon: 'fa fa-file',
            },
            {
                key: 'Lập hồ sơ hợp đồng',
                label: 'Hợp đồng',
                url: '/lap-ho-hop-dong',
                icon: 'fa fa-file',
                role:'contract',
            },
        ]
    },
  
    {
        key: 'Giao nhận',
        label: 'Giao nhận',
        icon: 'fa fa-file',
 
        nodes: [
           
            {
                key: 'Biên bản giao nhận',
                label: 'Biên bản giao nhận',
                url: '/bien-ban-giao-nhan',
                icon: 'fa fa-file',
                role:'DdeliveryReceipt',
            },
            {
                key: 'Biên bản nghiệm thu',
                label: 'Biên bản nghiệm thu',
                url: '/bien-ban-nghiem-thu',
                icon: 'fa fa-file',
                role:'acceptance',
            },
        ]
    },
    {
        key: 'Thống Kê',
        label: 'Thống Kê',
        icon: 'fa fa-tachometer',
        nodes: [
          
            {
                key: 'report',
                label: 'Report',
                url: '/report',
                icon: 'fa fa-file',
                role: 'admin',
            }


        ]
    },

    {
        key: 'Tài sản',
        label: 'Tài sản',
        icon: 'fa fa-tachometer',
        nodes: [         
            {
                key: 'tài sản',
                label: 'Tài sản',
                url: '/reportAnalyzer',
                icon: 'fa fa-file',
                role: 'admin',
            }
        ]
    },


    
    {
        key: 'Cài đặt',
        label: 'Cài đặt',
        icon: 'fa fa-cogs',
        role: 'admin',
        nodes: [
            {
                key: 'Quản lý nhân viên',
                label: 'Quản lý nhân viên',
                url: '/quan-ly-nhan-vien',
                icon: 'fa fa-user',
                role: 'admin',
            },
            {
                key: 'Nguồn vốn',
                label: 'Nguồn vốn',
                url: '/nguon-von',
                icon: 'fa fa-area-chart',
                role: 'admin',
            },
            {
                key: 'Quản lý người dùng',
                label: 'Quản lý người dùng',
                url: '/quan-ly-nguoi-dung',
                icon: 'fa fa-user',
                role: 'admin',
            },
            {
                key: 'Quản lý chi nhánh',
                label: 'Quản lý chi nhánh',
                url: '/quan-ly-chi-nhanh',
                icon: 'fa fa-th',
                role: 'admin',
            }
        ]
    },
]