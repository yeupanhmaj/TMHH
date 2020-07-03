import moment from 'moment';
import * as React from 'react';
import { AuditLocationArr, BidMethodArr, DeliveryRoleArr, getLabelString } from '../../commons/propertiesType';
import * as Utils from '../../libs/util';




const PADDING_TOP = 7;
const SMALL_FONT = "10pt";
const NORMAL_FONT = "13pt";
const HEADER_FONT = "14pt";
const SMALL_TILTLE_FONT = "20pt";
const TILTLE_FONT = "28pt";

function formatNumber(n) {
    return Utils.formatNumber(n);
}
function getListProposalItems(listItems) {
    return (
        <div className="childDetailWarpper">
            <div className="addItemWrapper listItemWrapper" >
                <div className="listItemHeader" style={{ display: 'flex' }} >
                    <div style={{ flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên Sản Phẩm</div>
                    <div style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                    <div style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Đơn vị</div>
                </div>
                {listItems.map((item) => {
                    return (
                        <div className="listItemRow" style={{ display: 'flex' }} >
                            <div className="noWrap" style={{ flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                            <div className="noWrap" style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.amount}</div>
                            <div className="noWrap" style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                        </div>
                    )

                })
                }
            </div>
        </div>
    )
}

export const prepareProposal = (item, currentDate) => {
    let itemContent = item.itemContent;
    let listItems = item.listItems;
    for (let prop in itemContent) {
        if (itemContent[prop] === "null" || itemContent[prop] === null) {
            itemContent[prop] = ''
        }
    }
    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', fontSize: NORMAL_FONT }}>
                    <div style={{ flex: 1 }}>
                        <img src={"./images/logo.png"} width={140} />
                    </div>

                    <div style={{
                        flex: 1,
                        display: 'flex',
                        flexDirection: 'row',
                        height: 60,
                        justifyContent: 'center',
                        alignItems: 'center', padding: '15px 0px 0px 0px',
                        fontSize: TILTLE_FONT, fontWeight: "bold",
                        marginTop: 15
                    }}>
                        Phiếu Đề Xuất
                    </div>

                    <div style={{ flex: 1, fontSize: SMALL_FONT }}>
                        <div style={{ marginLeft: 120 }}>Mã số: QT-SCCS/BM01</div>
                        <div style={{ marginLeft: 120 }}> Lần ban hành : 03</div>
                        <div style={{ marginLeft: 120 }}>Ngày hiệu lực : 18/3/2019</div>
                    </div>
                </div>
                <div style={{ marginTop: 20, textAlign: 'center' }}>
                    Kính gởi: GIÁM ĐỐC BV. TRUYỀN MÁU HUYẾT HỌC
            </div>

                <div style={{ marginTop: 20 }}>
                    Khoa/ phòng: {item.itemContent.departmentName}
                </div>

                <div style={{ marginTop: 20 }}>
                    Kính đề nghị Giám đốc duyệt cho thực hiện:
            </div>

                <div style={{ marginTop: 20 }}>
                    
                    <div style={{ display: 'flex' }}>
                        1.  Mua
                <div className={`checkbox-print ${itemContent.proposalType == 1 ? "crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
                Sửa chữa
                <div className={`checkbox-print  ${itemContent.proposalType == 2 ? "crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
            Thanh lý
                <div className={`checkbox-print  ${itemContent.proposalType == 3 ? "crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
                Thu hồi
                <div className={`checkbox-print  ${itemContent.proposalType == 4 ? "crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
                    </div>


                    <div style={{ padding: 40 }}>
                        {getListProposalItems(listItems)}
                    </div>
                </div>

                <div style={{ marginTop: 20, minHeight: 150 }}>
                    2. Lý Do:
                <div style={{ marginTop: 15, marginLeft: 20, content: ".................." }}>
                        {itemContent.followComment === null ? '' : itemContent.followComment}
                    </div>
                </div>

                <div style={{ marginTop: 20, minHeight: 150 }}>
                    3. Ý kiến khoa/ phòng đã khảo sát:
                <div style={{ marginTop: 15, marginLeft: 20, content: ".................." }}>
                        {itemContent.opinion === null ? '' : itemContent.opinion}
                    </div>
                </div>

                <div style={{ marginTop: 40, display: 'flex', flexDirection: 'row', width: '100%', fontSize: NORMAL_FONT }}>
                    <div style={{
                        width: 150,
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        flexDirection: 'column',
                    }}>
                        <div>{'Ngày: ...../...../.....'}</div>
                        <div><b>Giám Đốc</b></div>
                    </div>

                    <div style={{
                        display: 'flex',
                        flex: 1,
                        justifyContent: 'center',
                        alignItems: 'center',
                        flexDirection: 'column'
                    }}>
                        <div>{'Ngày: ...../...../.....'}</div>
                        <div><b>Khoa/ phòng {itemContent.curDepartmentName}</b></div>
                    </div>

                    <div style={{
                        flex: 1,
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        flexDirection: 'column'
                    }}>
                        <div>Ngày: {moment(itemContent.dateIn, 'DD-MM-YYYY').format(`DD/MM/YYYY`)}</div>
                        <div><b>BPT Khoa/ phòng {' ' + itemContent.departmentName}</b></div>
                    </div>
                </div>

            </div>
        </div>
    )
}
export const prepareExplanation = (item, currentDate) => {
    let itemContent = item.itemContent;
    let listItems = item.listItems;
    let itemStr = '';
    for (let record of listItems) {
        itemStr += ' ' + record.itemName + ',';

    }
    let numberOfmachine = 'Không'
    if (item.itemContent.available && item.itemContent.available !== '') {
        numberOfmachine = item.itemContent.available
    }
    for (let prop in itemContent) {
        if (itemContent[prop] === "null" || itemContent[prop] === null) {
            itemContent[prop] = '  '
        }
    }
    itemStr = itemStr.substring(0, itemStr.length - 1);
    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', fontSize: NORMAL_FONT }}>
                    <div style={{ flex: 1 }}>
                        <img src={"./images/logo.png"} width={120} />
                    </div>

                    <div style={{
                        flex: 3,
                        display: 'flex',
                        flexDirection: 'row',
                        height: '60',
                        justifyContent: 'center',
                        alignItems: 'center',
                        padding: '15px 0px 0px 0px',
                        fontSize: SMALL_TILTLE_FONT,
                        fontWeight: 'bold'
                    }}>
                        BẢN GIẢI TRÌNH MUA SẮM
                </div>

                    <div style={{ flex: 1, fontSize: SMALL_FONT }}>
                        <div >Mã số: QT-MUHA/BM05</div>
                        <div >Lần ban hành : 01</div>
                        <div >Ngày hiệu lực : 1/8/2015</div>
                    </div>
                </div>


                <div style={{ marginTop: 40 }}>
                    1.	Tên hàng hóa: {itemStr}
                </div>

                <div style={{ marginTop: PADDING_TOP }}>
                    2.	Nội dung giải trình
            </div>


                <div style={{ marginTop: PADDING_TOP, marginLeft: 20 }}>
                    2.1. Sự cần thiết đầu tư
            </div>


                <div style={{ marginTop: PADDING_TOP, marginLeft: 40, display: 'flex' }}>
                    <div style={{ flex: 2 }}>
                        . Sự cần thiết:
                    </div>

                    <div style={{ flex: 1, display: 'flex' }}>
                        Có
                    <div className={`checkbox-print ${itemContent.necess == true ? " crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
                    </div>
                    <div style={{ flex: 1, display: 'flex' }}>
                        Không
                    <div className={`checkbox-print ${itemContent.necess == false ? " crossed" : ''} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
                    </div>
                </div>




                <div style={{ marginTop: PADDING_TOP, marginLeft: 40, display: 'flex' }}>
                    <div style={{ flex: 2 }}>
                        . Phù hợp với quy hoạch phát triển :
                </div>

                    <div style={{ flex: 1, display: 'flex' }}>
                        Có
                    <div className={`checkbox-print ${itemContent.suitable == true ? " crossed" : ""} `} style={{ marginLeft: 5, marginright: 15, marginTop: 5 }}>
                        </div>
                    </div>
                    <div style={{ flex: 1, display: 'flex' }}>
                        Không
                    <div className={`checkbox-print ${itemContent.suitable == false ? " crossed" : ''} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
                    </div >
                </div >


                <div style={{ marginTop: PADDING_TOP, marginLeft: 40, display: 'flex' }}>
                    <div style={{ flex: 2 }}>
                        . Nhu cầu số NB: {itemContent.nbNum} / ngày
                </div>
                    <div style={{ flex: 2 }}>
                        Số XN: {itemContent.xnNum} / ngày
                </div>
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 40, display: 'flex' }}>
                    <div style={{ flex: 2 }}>
                        . Số máy đã có: {numberOfmachine}
                    </div>
                    <div style={{ flex: 1, display: 'flex' }}>
                        Có
                    <div className={`checkbox-print ${itemContent.isAvailable == true ? " crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
                    </div>
                    <div style={{ flex: 1, display: 'flex' }}>
                        Không
                    <div className={`checkbox-print ${itemContent.isAvailable == false ? " crossed" : ''} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                        </div>
                    </div >
                </div >


                <div style={{ marginTop: PADDING_TOP, marginLeft: 40 }}>
                    . Giải thích:
            </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 50, minHeight: 80 }}>
                    - {itemContent.comment === null ? '' : itemContent.comment}
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 20 }}>
                    2.2. Tính năng cơ bản của hàng hóa
            </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 40 }}>
                    . Tính năng cơ bản:
            </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 50, minHeight: 80 }}>
                    - {itemContent.tncb === null ? '' : itemContent.tncb}
                </div>


                <div style={{ marginTop: PADDING_TOP, marginLeft: 40 }}>
                    . Dự báo lỗi thời công nghệ:
            </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 50, minHeight: 80 }}>
                    - {itemContent.dbltcn === null ? '' : itemContent.dbltcn}
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 20 }}>
                    2.3. Tính năng cơ bản của hàng hóa
            </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 40 }}>
                    . Người vận hành trang thiết bị: {itemContent.nvhttb}
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 40 }}>
                    . Đào tạo nhân lực: {itemContent.dtnl === null ? '' : itemContent.dtnl}
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 40 }}>
                    . Người quản lý: {itemContent.nql === null ? '' : itemContent.nql}
                </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 20 }}>
                    2.4. Hiệu quả kinh tế và xã hội
            </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 40, minHeight: 80 }}>
                    {itemContent.hqktxh === null ? '' : itemContent.hqktxh}
                </div>

                <div style={{ width: '100%', display: 'flex', justifyContent: 'flex-end', marginTop: 20 }}>
                    <div style={{ marginRight: 50 }}><b>TRƯỞNG KHOA/ PHÒNG</b></div>
                </div>
            </div >
        </div >
    )
}

function getListSurveyItems(listItems) {
    return (
        <div>
            <div className="listItemHeader" style={{ display: 'flex' }} >
                <div style={{ flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>STT</div>
                <div style={{ flex: 4, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên linh kiện, tài sản</div>
                <div style={{ flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>ĐVT</div>
                <div style={{ flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                <div style={{ flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Ghi chú</div>

            </div>
            {listItems.map((item,index) => {
                return (
                    < div className="listItemRow" style={{ display: 'flex' }} >
                        <div style={{ flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px', height: 40 }}>{index + 1}</div>
                        <div style={{ flex: 4, borderRight: '1px solid #ddd', padding: '5px 10px', height: 40 }}>{item.itemName}</div>
                        <div style={{ flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px', height: 40 }}>{item.itemUnit}</div>
                        <div style={{ flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px', height: 40 }}>{item.itemAmount}</div>
                        <div style={{ flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px', height: 40 }}>{item.note}</div>
                    </div>
                )
            })}
        </div >
    )
}
export const prepareSurvey = (item, currentDate) => {
    let itemContent = item.itemContent;


    let itemStr = '';
    for (let prop in itemContent) {
        if (itemContent[prop] === "null" || itemContent[prop] === null) {
            itemContent[prop] = '  '
        }
    }

   
    itemStr = item.productsName
    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', fontSize: NORMAL_FONT }}>
                    <div style={{ flex: 1 }}>
                        <img src={"./images/logo.png"} width={120} />
                    </div>

                    <div style={{
                        flex: 3,
                        display: 'flex',
                        flexDirection: 'row',
                        height: 60,
                        justifyContent: 'center',
                        alignItems: 'center', padding: '10px 0px 0px 0px',
                        fontSize: SMALL_TILTLE_FONT
                    }}>
                        PHIẾU KHẢO SÁT SỬA CHỮA
                    </div>

                    <div style={{ flex: 1, marginTop: '-20px', fontSize: SMALL_FONT }}>
                        <div >Mã số: QT-SCCS/BM02</div>
                        <div >Lần ban hành: 03</div>
                        <div >Ngày hiệu lực: 18/03/2019 </div>
                    </div>
                </div>

                <div style={{ marginTop: 20, display: 'flex', justifyContent: 'flex-end' }}>
                    Tp.HCM, ngày {moment(itemContent.dateIn, 'DD-MM-YYYY').format('DD')  + ' '} 
                    tháng {moment(itemContent.dateIn, 'DD-MM-YYYY').format('MM')+ ' '} 
                    năm {moment(itemContent.dateIn, 'DD-MM-YYYY').format('YYYY')}
                </div>

                <div style={{ marginTop: 10 }}>
                    Căn cứ vào đề xuất sửa chữa số {itemContent.proposalCode} của khoa phòng {itemContent.departmentName}, ngày {' ' + moment(new Date(itemContent.proposalDate)).format('DD') + ' '} tháng {' ' + moment(new Date(itemContent.proposalDate)).format('MM') + ' '} năm {' ' + moment(new Date(itemContent.proposalDate)).format('YYYY')}, Chúng tôi tiến hành khảo sát và đưa ra báo cáo như sau:
                </div>

                <div style={{ marginTop: 10 }}>
                    Tên tài sản: {itemStr}
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 20 }}>
                    1.	Tình trạng khi khảo sát
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 40, minHeight: 80 }}>
                    {itemContent.comment}
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 20 }}>
                    <div style={{ display: 'flex' }}>
                        <div>
                            2. Hướng xủ lý :
                </div>
                        <div style={{ marginLeft: 60, display: 'flex' }}>
                            <div>
                                Thuê ngoài sửa chửa
                    </div>
                            <div className={`checkbox-print ${itemContent.solution == 1 ? " crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 8 }}>
                            </div>
                        </div>
                        <div style={{ display: 'flex', marginLeft: 60 }}>
                            <div>
                                Tự sửa chữa
                    </div>
                            <div className={`checkbox-print  ${itemContent.solution == 2 ? " crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 8 }}>
                            </div>
                        </div>
                    </div>


                </div>

                <div style={{ display: 'flex', marginLeft: 107, marginBottom: 10 }}>
                    <div style={{ display: 'flex' }}>
                        <div>
                            Đã có hợp đồng bảo hành, bảo trì
                </div>
                        <div className={`checkbox-print  ${itemContent.solution == 3 ? " crossed" : ""}`} style={{ marginLeft: 5, marginRight: 15, marginTop: 8 }}>
                        </div>
                    </div>

                    <div style={{ display: 'flex', marginLeft: 88 }}>
                        <div>
                            Mua mới
                        </div>
                        <div className={`checkbox-print  ${itemContent.solution == 4 ? " crossed" : ""}`} style={{ marginLeft: 5, marginRight: 15, marginTop: 8 }}>
                        </div>
                    </div >
                </div >

                <div style={{ marginTop: PADDING_TOP, marginLeft: 40, minHeight: 40 }}>
                    {itemContent.solutionText}
                </div>

                <div style={{ marginTop: PADDING_TOP, marginLeft: 20 }}>
                    3.	Các vật tư, linh kiện đề nghị thay thế (nếu sửa chữa)/Tên tài sản, mã hiệu, hãng sản xuất ....(nếu có)
                </div>

                <div style={{ marginTop: 15, marginBottom: 20 }}>
                    {getListSurveyItems(itemContent.surveyItems)}
                </div>

                <div style={{ display: 'flex', marginLeft: 60 }}>
                    < div > Hàng mẫu     </div >

                    <div className={`checkbox-print ${itemContent.isSample == true ? " crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}>
                    </div >
                </div >


                <div style={{ marginTop: PADDING_TOP, marginLeft: 20, display: 'flex' }}>
                    <div>
                        4.	Ý kiến của Khoa phòng:
            </div>

                    <div style={{ marginLeft: '20', display: 'flex' }}>
                        Đồng ý <div className={`checkbox-print ${itemContent.valid == true ? " crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}></div>
                    </div>

                    <div style={{ marginLeft: 20, display: 'flex' }}>
                        Không đồng ý
             <div className={`checkbox-print ${itemContent.valid == false ? " crossed" : ""} `} style={{ marginLeft: 5, marginRight: 15, marginTop: 5 }}></div>
                    </div >
                </div >

                <div style={{ marginTop: PADDING_TOP, marginLeft: 20 }}>
                    5.	Ý kiến khác
        </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 40, minHeight: 80 }} >
                    {itemContent.validText === null ? '' : itemContent.validText}
                </div >
                <div style={{ width: '100%', display: 'flex', marginTop: 40 }}>
                    <div style={{
                        flex: 1, justifyContent: 'center',
                        flexDirection: 'column',
                        alignItems: 'center'
                    }}>
                        <div style={{ textAlign: 'center' }}>Phụ trách khoa/phòng</div>

                        <div style={{ marginTop: 80, textAlign: 'center' }}>
                            Trưởng phòng HCQT
                        </div>
                        <div style={{ marginTop: 50, textAlign: 'center' }}>
                            <b>ThS. KS Huỳnh Văn Minh </b>
                        </div>
                    </div>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', marginLeft: 230 }}>
                        Người Khảo sát
                    </div>
                </div>
            </div >
        </div >
    )
}

function getAuditMemberList(listEmployees) {
    if (listEmployees === undefined) return '';
    return (
        <div style={{ display: 'flex', flexDirection: 'column', marginTop: PADDING_TOP, marginLeft: 80 }}>

            {listEmployees.map((item) => {
                return (
                    <div style={{ display: 'flex' }}>
                        <div style={{ width: 18 }}>
                            -
                            </div>
                        <div style={{ flex:1 , flexDirection:'flex-start' }}>
                            {item.title + ' ' + item.name + " – " + item.roleName}
                        </div>
                    </div>
                )
            })
            }
        </div>
    )
}

function getListQuoteItems(listItems) {
    return (
        <div className="childDetailWarpper" style={{ marginTop: 10 }}>
            <div className="addItemWrapper listItemWrapper">
                <div className="listItemHeader" style={{ display: 'flex' }}>
                    <div className="noWrap" style={{ width: 60, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>STT</div>
                    <div className="noWrap" style={{ minWidth: 130, flex: 3, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Mặt hàng & dịch vụ</div>
                    <div className="noWrap" style={{ minWidth: 70, flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>ĐVT</div>
                    <div className="noWrap" style={{ minWidth: 70, flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>SL</div>
                    <div className="noWrap" style={{ minWidth: 100, flex: 2, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Đơn giá (VNĐ)</div>
                    <div className="noWrap" style={{ width: 144, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Thành tiền (VNĐ)</div>
                </div>
                {listItems.map((item, index) => {
                    return (
                        <div className="listItemRow" style={{ display: 'flex' }}>
                            <div className="noWrap" style={{ width: 60, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{index + 1}</div>
                            <div className="noWrap" style={{ minWidth: 130, flex: 3, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                            <div className="noWrap" style={{ minWidth: 70, flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                            <div style={{ minWidth: 70, flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.amount}</div>
                            <div className="noWrap" style={{ minWidth: 100, flex: 2, textAlign: 'center', padding: '5px 10px', borderRight: '1px solid  #ddd' }}>
                                {formatNumber(item.itemPrice)}
                            </div>
                            <div className="noWrap" style={{ width: 144, textAlign: 'right', padding: '5px 10px', borderRight: '1px solid  #ddd' }}>
                                {formatNumber(+(item.itemPrice.toString().replace(/\./g, '')) * +(item.amount))}
                            </div>
                        </div>
                    )
                })}
            </div>
        </div>
    )
}

function getpriceItemsAudit(isVAT, oldPrice, vatNumber) {
    let price = oldPrice;
    if (isVAT) {
        price = Math.round((price * vatNumber) / 100) + price
    }
    return price;
}

function getListQuoteItemsAudit(listItems, isVAT, vatNumber) {
    return (
        <div className="childDetailWarpper" style={{ marginTop: 20, width: '100%' }}>
            <div className="addItemWrapper listItemWrapper">
                <div className="listItemHeader" style={{ display: 'flex' }}>
                    <div className="noWrap"   style={{ width: 70, flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>STT</div>
                    <div className="noWrap"  style={{ width: 300, flex: 3, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Mặt hàng & dịch vụ</div>
                    <div className="noWrap"  style={{ flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: ' 5px 10px' }}>ĐVT</div>
                    <div className="noWrap"  style={{ flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>SL</div>
                    <div className="noWrap"  style={{ flex: 2, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Đơn giá có VAT{' ' + vatNumber + '% '}(VNĐ)</div>
                    <div className="noWrap"  style={{ flex: 2, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Thành tiền có VAT{' ' + vatNumber + '% '}(VNĐ)</div>
                </div>
                {listItems.map((item, index) => {
                    return (
                        <div className="listItemRow" style={{ display: 'flex', width: '100%' }}>
                            <div className="noWrap"  style={{ width: 70, flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{index + 1}</div>
                            <div className="noWrap"  style={{ width: 300, flex: 3, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                            <div className="noWrap" style={{ flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                            <div className="noWrap" style={{ flex: 1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.amount}</div>
                            <div className="noWrap" style={{ flex: 2, textAlign: 'center', padding: '5px 10px', borderRight: '1px solid  #ddd' }}>
                                {formatNumber(getpriceItemsAudit(isVAT, item.itemPrice, vatNumber))}
                            </div>
                            <div className="noWrap" style={{ flex: 2, textAlign: 'right', padding: '5px 10px', borderRight: '1px solid  #ddd' }}>
                                {formatNumber(+(getpriceItemsAudit(isVAT, item.itemPrice, vatNumber).toString().replace(/\./g, '')) * +(item.amount))}
                            </div>
                        </div>
                    )
                })
                }
            </div>
        </div>
    )
}



function getpriceItemsAuditItemPrice(isVAT, oldPrice, vatNumber) {
    let Price = oldPrice;

    let VATmoney = 0;
    if (isVAT) {
        VATmoney = Math.round((Price * vatNumber) / 100)
        Price = VATmoney + +Price
    }
    return { VATmoney, Price };
}

function getListQuoteItemsAuditItemPrice(listItems, isVAT, vatNumber) {

    return (
        <div className="childDetailWarpper" style={{ marginTop: 20, width: '100%' }}>
            <div className="addItemWrapper listItemWrapper">
                <div className="listItemHeader" style={{
                    display: 'grid',
                    gridTemplateColumns: '64px 220px 70px 70px 110px 100px 130px 130px', width: '100%'
                }}>
                    <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>STT</div>
                    <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Mặt hàng & dịch vụ</div>
                    <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>ĐVT</div>
                    <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>SL</div>
                    <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Đơn giá</div>
                    <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Thuế VAT {vatNumber}</div>
                    <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Đơn giá (Đã bao gồm thuế VAT {vatNumber}%)</div>
                    <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Thành tiền (VNĐ)</div>
                </div>
                {listItems.map((item, index) => {
                    return (
                        <div className="listItemRow" style={{
                            display: 'grid',
                            gridTemplateColumns: '64px 220px 70px 70px 110px 100px 130px 130px', width: '100%'
                        }}>
                            <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{index + 1}</div>
                            <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                            <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                            <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{item.amount}</div>

                            <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>
                                {formatNumber(item.itemPrice)}
                            </div>
                            <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>
                                {formatNumber(getpriceItemsAuditItemPrice(isVAT, item.itemPrice, vatNumber).VATmoney)}
                            </div>

                            <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>
                                {formatNumber(getpriceItemsAuditItemPrice(isVAT, item.itemPrice, vatNumber).Price)}
                            </div>
                            <div className="noWrap" style={{ textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>
                                {formatNumber(+(getpriceItemsAuditItemPrice(isVAT, item.itemPrice, vatNumber).Price.toString().replace(/\./g, '')) * +(item.amount))}
                            </div>
                        </div>
                    )
                })

                }
            </div>
        </div>
    )
}


export const prepareAudit = (item, currentDate) => {
    let itemContent = item.itemContent;
    let listItems = [];
    let listEmployees = item.listEmployees
    let IsVAT = itemContent.quotes[0].isVAT;
    let VATNumber = itemContent.quotes[0].vatNumber;
    let chkCode = ('' + itemContent.auditCode);
    chkCode.includes()

    for (let quote of item.item.quotes) {
        for (let data of quote.items) {
            listItems.push(data);
        }
    }

    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', fontSize: NORMAL_FONT }}>
                    <div style={{ display: 'flex', flex: 2, flexDirection: 'column', alignItems: 'center', marginRight: 70 }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            BV. TRUYỀN MÁU HUYẾT HỌC
                            </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                            <b> BAN KIỂM GIÁ  </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}>  </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 10 }} >
                            {('' + itemContent.auditCode).includes('/BB-BKG',0) ? 'Số: ' + itemContent.auditCode : 'Số: ......./BB-BKG' } 
                        </div>
                    </div>
                    <div style={{ display: 'flex', flex: 3, flexDirection: 'column', alignItems: 'center' }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            <b> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </b>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: '5px' }}>
                            <b> Độc lập – Tự do – Hạnh phúc </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}> </div>
                    </div>
                </div>
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: PADDING_TOP  , fontSize:TILTLE_FONT}}>
                    <b>BIÊN BẢN HỌP BAN KIỂM GIÁ </b>
                </div>
                <div style={{ display: 'flex', marginTop: 40, marginLeft: 20 }}>
                    <b>I. HÀNH CHÁNH:</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    1. Thời gian: ...........ngày......................  </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    2. Địa điểm: Phòng họp, cơ sở 1: 118 Hồng Bàng, Q.5
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    3. Chủ tọa: {itemContent.presideTitle + ' ' + itemContent.presideName + ' – ' + itemContent.presideRoleName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    4. Thư ký: {itemContent.secretaryTitle + ' ' + itemContent.secretaryName + '  ' + itemContent.secretaryRoleName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    5. Thành phần:
                </div>
                {getAuditMemberList(listEmployees)}
                <div style={{ display: 'flex', marginTop: 20, marginLeft: 20 }}>
                    <b> II. NỘI DUNG: </b>
                </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 60 }}>
                    -	Đại diện khoa/phòng Hành Chánh Quản Trị trình bày yêu cầu/ cấu hình/ tính năng của các mặt hàng liên quan đến máy móc thiết bị, vật tư và chi phí sửa chữa, bảo trì của Phòng Hành Chánh Quản Trị thực hiện và các bảng báo giá hiện thu thập được.
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    -	Sau khi Ban Kiểm giá thảo luận, chúng tôi thống nhất đồng ý các bảng báo giá thấp nhất như sau:
               </div>
                <div style={{ marginTop: 15, marginBottom: 20, marginLeft: 20 }}>
                    {getListQuoteItemsAudit(listItems, IsVAT, VATNumber)}
                </div>
                <div style={{ marginTop: 50, marginLeft: 60 }}>
                    Lưu ý: ĐVT: đơn vị tính (cái, hộp, dịch vụ,...); SL: số lượng (cần mua sắm); đơn giá: của từng món hàng/dịch vụ; thành tiền: tổng đơn hàng = SL x đơn giá
               </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 60, marginBottom: 40 }}>
                    Thư ký đọc lại biên bản họp, tất cả thành viên trong Ban Kiểm giá đều đồng ý. Cuộc họp kết thúc lúc  .............. cùng ngày.
               </div>

               <div style={{ marginTop: PADDING_TOP, display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                        <div style={{ textAlign: 'center', height: 20 }}></div>
                        <div style={{ marginTop: 20 }}><b> CHỦ TỌA </b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}> {itemContent.presideName}</div>
                    </div>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                     <div style={{ textAlign: 'center', height: 20 }}>   Tp.HCM, ngày ..... tháng .... năm ..... </div>
                        <div style={{ textAlign: 'center', marginTop: 20 }}><b> THƯ KÝ </b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}> {itemContent.secretaryName}</div>
                    </div>

                </div>


                <div style={{ marginTop: 20, marginLeft: 60 }}>
                    <div style={{ marginTop: PADDING_TOP }}>
                        <b>THÀNH VIÊN :</b>
                    </div>
                    {getlistEmployy(listEmployees)}

                </div>

            </div>
        </div>
    )
}


export const prepareAuditWithItemPrice = (item, currentDate) => {
    let itemContent = item.itemContent;
    let listItems = [];
    let listEmployees = item.listEmployees
    let IsVAT = itemContent.quotes[0].isVAT;
    let VATNumber = itemContent.quotes[0].vatNumber;

    for (let quote of item.item.quotes) {
        for (let data of quote.items) {
            listItems.push(data);
        }
    }


    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', padding: 15 }}>
                <div style={{ display: 'flex', flexDirection: 'row', padding: 15 }}>
                    <div style={{ display: 'flex', flex: 2, flexDirection: 'column', alignItems: 'center', marginRight: 70 }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            BV. TRUYỀN MÁU HUYẾT HỌC
                            </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                            <b> BAN KIỂM GIÁ  </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}>  </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 10 }}>
                        {('' + itemContent.auditCode).includes('/BB-BKG',0) ? 'Số: ' + itemContent.auditCode : 'Số: ......./BB-BKG' } 
                        </div>
                    </div>
                    <div style={{ display: 'flex', flex: 3, flexDirection: 'column', alignItems: 'center' }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            <b> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </b>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                            <b> Độc lập – Tự do – Hạnh phúc </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}> </div>
                    </div>
                </div>
                <div style={{ display: 'flex', alignItems: 'center',   justifycontent: 'center', width:'100%', marginTop: PADDING_TOP  , fontSize:TILTLE_FONT}}>
                    <b>BIÊN BẢN HỌP BAN KIỂM GIÁ</b>
                </div>
                <div style={{ display: 'flex', marginTop: 40, marginLeft: 20 }}>
                    <b>I. HÀNH CHÁNH:</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    1. Thời gian: ...........ngày......................  </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    2. Địa điểm: Phòng họp, cơ sở 1: 118 Hồng Bàng, Q.5
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    3. Chủ tọa: {itemContent.presideTitle + ' ' + itemContent.presideName + ' – ' + itemContent.presideRoleName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    4. Thư ký: {itemContent.secretaryTitle + ' ' + itemContent.secretaryName + ' – ' + itemContent.secretaryRoleName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    5. Thành phần:
                </div>
                {getAuditMemberList(listEmployees)}
                <div style={{ display: 'flex', marginTop: 20, marginLeft: 20 }}>
                    <b> II. NỘI DUNG: </b>
                </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 60 }}>
                    -	Đại diện khoa/phòng Hành Chánh Quản Trị trình bày yêu cầu/ cấu hình/ tính năng của các mặt hàng liên quan đến máy móc thiết bị, vật tư và chi phí sửa chữa, bảo trì của Phòng Hành Chánh Quản Trị thực hiện và các bảng báo giá hiện thu thập được.
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    -	Sau khi Ban Kiểm giá thảo luận, chúng tôi thống nhất đồng ý các bảng báo giá thấp nhất như sau:
               </div>
                <div style={{ marginTop: 15, marginBottom: 20, marginLeft: 20 }}>
                    {getListQuoteItemsAuditItemPrice(listItems, IsVAT, VATNumber)}
                </div>
                <div style={{ marginTop: 50, marginLeft: 60 }}>
                    Lưu ý: ĐVT: đơn vị tính (cái, hộp, dịch vụ,...); SL: số lượng (cần mua sắm); đơn giá: của từng món hàng/dịch vụ; thành tiền: tổng đơn hàng = SL x đơn giá
               </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 60, marginBottom: 40 }}>
                    Thư ký đọc lại biên bản họp, tất cả thành viên trong Ban Kiểm giá đều đồng ý. Cuộc họp kết thúc lúc .............. cùng ngày.
               </div>

                <div style={{ marginTop: PADDING_TOP, display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                        <div style={{ textAlign: 'center', height: 20 }}></div>
                        <div style={{ marginTop: 20 }}><b> CHỦ TỌA </b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}> {itemContent.presideName}</div>
                    </div>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                     <div style={{ textAlign: 'center', height: 20 }}>   Tp.HCM, ngày ..... tháng .... năm ..... </div>
                        <div style={{ textAlign: 'center', marginTop: 20 }}><b> THƯ KÝ </b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}> {itemContent.secretaryName}</div>
                    </div>

                </div>

                <div style={{ marginTop: 20, marginLeft: 60 }}>
                    <div style={{ marginTop: { PADDING_TOP } }}>
                        <b>THÀNH VIÊN :</b>
                    </div>
                    {getlistEmployy(listEmployees)}
                </div>
            </div>
        </div>
    )
}



function getlistEmployy(list) {
    return (
        <React.Fragment>
            {list.map((item) => {
                let tempLen = 50 - item.name.length;
                let dot = ' :';
                for (var i = 0; i < tempLen; i++) {
                    dot += '.';
                }
                return (
                    <div style={{ marginTop: 30 }}>
                        
                        {item.name + dot}
                    </div>
                )
            })
            }
        </React.Fragment>
    )
}

function getBiplanGeneral(itemContent, totalCost , capitalName) {
    return (
        <div className="childDetailWarpper" style={{ margin : '10px 0px'   }}>
            <div className="addItemWrapper listItemWrapper pageBreakAfter:'always' , pageBreakInside:'avoid !important'  ">
                <div className="listItemHeader" style={{ display: 'flex'  ,pageBreakAfter:'auto' , pageBreakInside:'avoid !important' }}>
                    <div className="noWrap" style={{ width: 54, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>STT</div>
                    <div className="noWrap" style={{ width: 130, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Tên gói thầu</div>
                    <div className="noWrap" style={{ width: 120, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Giá gói thầu</div>
                    <div className="noWrap" style={{ width: 140, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Nguồn vốn</div>
                    <div className="noWrap" style={{ width: 107, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Hình thức phương thức lựa chọn nhà thầu</div>
                    <div className="noWrap" style={{ width: 107, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Thời gian tổ chức lựa chọn nhà thầu</div>
                    <div className="noWrap" style={{ width: 107, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Loại hợp đồng</div>
                    <div className="noWrap" style={{ flex:1, textAlign: 'center', padding: '5px 10px' }}>Thời gian thực hiện hợp đồng</div>
                </div>
            </div>
            <div className="listItemRow  " style={{ display:'flex'  , pageBreakAfter:'always' , pageBreakInside:'avoid !important'  }}>
                <div className="noWrap" style={{ width: 54, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>1</div>
                <div className="noWrap" style={{ width: 130, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{itemContent.bidName}</div>
                <div className="noWrap" style={{ width: 120, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{formatNumber(totalCost)} VNĐ</div>
                <div className="noWrap" style={{ width: 140, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{capitalName}</div>
                <div className="noWrap" style={{ width: 107, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{itemContent.bidMethodName}</div>
                <div className="noWrap" style={{ width: 107, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{itemContent.bidTime}</div>
                <div className="noWrap" style={{ width: 107, textAlign: 'center', borderRight: '1px solid  #ddd', padding: '5px 10p' }}>{itemContent.bidType}</div>
                <div className="noWrap" style={{ flex:1, textAlign: 'center', padding: '5px 10px' }}>{itemContent.bidTime}</div>
            </div>
        </div>
    )
}
function getListQuotewithTotalPrice(listItems, isVAT, totalCost) {

    if (isVAT) {
        let totalNotVaT = Math.round(totalCost / 11 * 10)
        let vatCost = Math.round(totalCost / 11)
        return (
            <React.Fragment>
                {getListQuoteItems(listItems)}

                <div className="addItemWrapper listItemWrapper">
                    <div className="listItemRow" style={{ display: 'flex' }}>
                        <div className="noWrap" style={{ flex: 1, textAlign: 'right', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Tổng công (chưa bao gồm VAT)</div>
                        <div className="noWrap"  style={{ width: 144, textAlign: 'right', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{formatNumber(totalNotVaT)}</div>
                    </div>
                    <div className="listItemRow" style={{ display: 'flex' }}>
                        <div className="noWrap" style={{ flex: 1, textAlign: 'right', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Thuế VAT 10%</div>
                        <div className="noWrap" style={{ width: 144, textAlign: 'right', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{formatNumber(vatCost)}</div>
                    </div>
                    <div className="listItemRow" style={{ display: 'flex' }}>
                        <div className="noWrap" style={{ flex: 1, textAlign: 'right', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>TỔNG CỘNG (đã bao gồm VAT)</div>
                        <div className="noWrap" style={{ width: 144, textAlign: 'right', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{formatNumber(totalCost)}</div>
                    </div>
                </div>
            </React.Fragment>
        )
    } else {
        return (
            <React.Fragment>
                {getListQuoteItems(listItems)}
                <div className="addItemWrapper listItemWrapper">
                    <div className="listItemRow" style={{ display: 'flex' }}>
                        <div className="noWrap" style={{ flex: 1, textAlign: 'right', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>Tổng công</div>
                        <div className="noWrap"  style={{ width: 144, textAlign: 'right', borderRight: '1px solid  #ddd', padding: '5px 10px' }}>{formatNumber(totalCost)}</div>
                    </div>
                </div>
            </React.Fragment>

        )
    }
}


function getPriceItemswC34(price, amount, isVAT, vatNumber) {
    let itemPriceAfterVAT = price;
    let itemTotalAftetVAT = 0;
    if (isVAT) itemPriceAfterVAT = (price * vatNumber) / 100 + price;
    itemTotalAftetVAT = +itemPriceAfterVAT * +(amount)

    return { itemPriceAfterVAT, itemTotalAftetVAT }
}


function getListItemswC34(listItems, isVAT, vatNumber) {
    return (
        <div className="childDetailWarpper" style={{ marginTop: 20 }}>
            <div className="listItemHeader" style={{ display: 'flex' }}>
                <div  className="noWrap" style={{width: 50, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>STT</div>
                <div className="noWrap" style={{ width: 280, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Tên nhãn hiệu, quy cách, phẩm chất nguyên liệu, vật liệu, công cụ, dụng cụ</div>
                <div className="noWrap" style={{ width: 55, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Mã số</div>
                <div className="noWrap" style={{ width: 75, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Đơn vị tính</div>
                <div className="noWrap" style={{  width: 75, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Số lượng</div>
                <div className="noWrap" style={{ width: 130, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Đơn giá</div>
                <div className="noWrap" style={{  width: 130, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Thành tiền</div>
                <div className="noWrap" style={{  width:100, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Ghi chú</div>
            </div>
            <div className="listItemRow" style={{ display: 'flex' }}>
                <div className="noWrap" style={{ width: 50, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>A</div>
                <div className="noWrap"  style={{ width: 280, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>B</div>
                <div className="noWrap" style={{ width: 55, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>C</div>
                <div className="noWrap" style={{  width: 75, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>1</div>
                <div className="noWrap" style={{ width: 75, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>2</div>
                <div className="noWrap" style={{  width: 130, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>3</div>
                <div className="noWrap" style={{  width: 130, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>4</div>
                <div className="noWrap" style={{  width:100, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>D</div>
            </div>
            {listItems.map((item, index) => {
                return (
                    <div className="listItemRow" style={{ display: 'flex' }}>
                        <div  className="noWrap" style={{  width: 50, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>{index + 1}</div>
                        <div className="noWrap" style={{  width: 280, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>{item.itemName}</div>
                        <div className="noWrap" style={{  width: 55, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}></div>
                        <div className="noWrap" style={{  width: 75, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>{}</div>
                        <div className="noWrap" style={{  width: 75, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>{item.amount}</div>
                        <div className="noWrap" style={{  width: 130, textAlign: 'right', borderRight: '1px solid  #ddd', padding: 5 }}>
                            {formatNumber(getPriceItemswC34(item.itemPrice, item.amount, isVAT, vatNumber).itemPriceAfterVAT)}
                        </div>
                        <div className="noWrap" style={{  width: 130, textAlign: 'right', borderRight: '1px solid  #ddd', padding: 5 }}>
                            {formatNumber(getPriceItemswC34(item.itemPrice, item.amount, isVAT, vatNumber).itemTotalAftetVAT)}
                        </div>
                        <div className="noWrap" style={{ width:100, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>
                        </div>
                    </div>

                )
            })}

        </div>
    )
}

function getListQuotewC34(listItems, isVAT, totalCost, vatNumber) {
    return (
        <React.Fragment>
            {getListItemswC34(listItems, isVAT, vatNumber)}
            <div className="childDetailWarpper" >
            <div className="listItemRow" style={{ display: 'flex' }}>
                <div className="noWrap" style={{ width: 50, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}></div>
                <div className="noWrap" style={{  width: 280, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Tổng cộng</div>
                <div className="noWrap"  style={{width: 55, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}></div>
                <div className="noWrap" style={{  width: 75, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}></div>
                <div className="noWrap" style={{ width: 75, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}></div>
                <div className="noWrap" style={{  width: 130, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}></div>
                <div className="noWrap" style={{  width: 130, textAlign: 'right', borderRight: '1px solid  #ddd', padding: 5 }}>{formatNumber(totalCost)}</div>
                <div className="noWrap" style={{ width:100, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}></div>
            </div>
            </div>
        </React.Fragment>
    )
}

export const prepareBidPlan = (item, currentDate) => {

    let itemContent = item.itemContent;
    let listItems = item.listItems;
    let totalCost = 0;

    for (let record of listItems) {

        totalCost += +record.totalPrice;
    }
    if (itemContent.isVAT) {
        totalCost = totalCost * (+itemContent.vatNumber + 100) / 100;
    }


    for (let location of AuditLocationArr) {
        if (itemContent.bidLocation == location.value.toString()) {
            itemContent.bidLocation = location.label;
        }
    }
    for (let bidMethod of BidMethodArr) {
        if (itemContent.bidMethod == bidMethod.value.toString()) {
            itemContent.bidMethodName = bidMethod.label;
        }
    }

    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 20, paddingRight: 20,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', fontSize: NORMAL_FONT }}>
                    <div style={{ display: 'flex', flex: 1, flexDirection: 'column', alignItems: 'center', marginRight: 70 }}>

                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            SỞ Y TẾ TP.HCM
                            </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                            <b>    BV. TRUYỀN MÁU HUYẾT HỌC </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000' }}></div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 10 }}>

                        </div>
                    </div>
                    <div style={{ display: 'flex', flex: 1, flexDirection: 'column', alignItems: 'center' }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            <b> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </b>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                            <b> Độc lập – Tự do – Hạnh phúc </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 10 }}>
                            Tp.HCM, ngày {' ' + moment(new Date(itemContent.dateIn)).format('DD') + ' '} tháng {' ' + moment(new Date(itemContent.dateIn)).format('MM') + ' '} năm {' ' + moment(new Date(itemContent.dateIn)).format('YYYY') + ' '}
                        </div>
                    </div>
                </div >

                <div style={{ fontSize: SMALL_TILTLE_FONT, display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', marginTop: PADDING_TOP * 2}}>
                    <div><b>KẾ HOẠCH LỰA CHỌN NHÀ THẦU </b></div>
                  
                </div>
                <div style={{  display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', marginTop: PADDING_TOP }}>
                <div>{itemContent.bidName}</div>
                  
                </div>
               
                <div style={{ display: 'flex', marginTop: 15, marginLeft: 40 }}>
                    -    Căn cứ Luật Đấu Thầu số 43/2013/QH13 của kỳ họp Quốc Hội thứ 13 nước Cộng Hoà Xã Hội Chủ Nghĩa Việt Nam; có hiệu lực thi hành ngày 01/07/2014.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -    Căn cứ Nghị định số 63/2014/NĐ-CP ngày 26 tháng 06 năm 2014 của Chính Phủ về việc Hướng dẫn thi hành luật đấu thầu và lựa chọn nhà thầu.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -    Căn cứ Thông tư 58/2016/TT-BTC ngày 29/03/2016 của Bộ Tài chính quy định chi tiết việc sử dụng vốn nhà nước để mua sắm nhằm duy trì hoạt động thường xuyên của cơ quan nhà nước, đơn vị thuộc lực lượng vũ trang nhân dân, đơn vị sự nghiệp công lập, tổ chức chính trị, tổ chức chính trị - xã hội, tổ chức chính trị xã hội - nghề nghiệp, tổ chức xã hội, tổ chức xã hội - nghề nghiệp.
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -    Căn cứ thông tư số 10/2015/TT-BKHĐT ngày 26/10/2015 của Bộ kế hoạch và Đầu tư quy định chi tiết về kế hoạch lựa chọn nhà thầu;
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -   Căn cứ dự trù mua sắm năm {(new Date()).getFullYear()} được Giám đốc duyệt;
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -    Căn cứ 03 bảng báo giá;
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -    Căn cứ biên bản xét chọn đơn vị cung cấp hàng hóa số {itemContent.auditCode + '/BB-BKG' } ngày {moment(new Date(itemContent.auditTime)).format('DD/MM/YYYY')} {/*+   '/BB-BKG' */}
                </div>


                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    <b>I.	Mô tả tóm tắt dự án:</b>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Bên mời thầu		: {itemContent.bid}
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Tên gói thầu		: {itemContent.bidName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Tổng vốn đầu tư		: {formatNumber(totalCost)} VNĐ (Bằng chữ : {Utils.DocTienBangChu(totalCost)} đồng)
                 </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Nguồn vốn			: {itemContent.capitalName}
                 </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Thời gian tổ chức lựa chọn nhà thầu	: {itemContent.bidTime}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Địa điểm thực hiện	:  {itemContent.bidLocation}
                </div>


                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    <b>II.	Phần công việc thuộc kế hoạch lựa chọn nhà thầu</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    <b>1.	Bảng tổng hợp phần công việc thuộc kế hoạch lựa chọn nhà thầu</b>
                </div>

                <div style={{ marginTop: 270  }}>
                    {getBiplanGeneral(itemContent, totalCost , itemContent.capitalName)}
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    <b>2.	Giải trình nội dung kế hoạch lựa chọn nhà thầu:</b>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    -	Tên gói thầu		: {itemContent.bidName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 60 }}>
                    -	Số lượng gói thầu	: 01 gói
                 </div>

                <div style={{ marginTop: 15, marginBottom: PADDING_TOP }}>
                    {getListQuotewithTotalPrice(listItems, itemContent.isVAT, totalCost)}
                </div>

                <div style={{ display: 'flex', marginTop: listItems.length > 5 ? 60 : PADDING_TOP, marginLeft: 40 }}>
                    a)	Giá gói thầu		: {formatNumber(totalCost)} VNĐ (Bằng chữ : {Utils.DocTienBangChu(totalCost)} đồng)
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    b)	Nguồn vốn			:  {itemContent.capitalName}
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    c)	Hình thức và phương thức lựa chọn nhà thầu: {itemContent.bidMethodName}
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    d)	Thời gian bắt đầu tổ chức lựa chọn nhà thầu: {itemContent.bidTime}
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    e)	Loại hợp đồng		: {itemContent.bidType}
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    f)	Thời gian thực hiện hợp đồng:  {itemContent.bidExpirated + ' ' + itemContent.bidExpiratedUnit + ' kể từ ngày hợp đồng có hiệu lực.'}
                </div>

                <div style={{ display: 'flex', marginTop: 15, marginLeft: 20 }}>
                    <b>III.	Phân công thực hiện kế hoạch: </b>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    Phòng HCQT, Phòng TCKT và các Khoa/phòng Bệnh viện có liên quan chịu trách nhiệm tổ chức đấu thầu theo nội dung phê duyệt tại mục II của kế hoạch này và theo đúng quy định của Luật đấu thầu và các quy định hiện hành.
                </div>

                <div style={{ width: '100%', display: 'flex', justifyContent: 'flex-end', marginTop: 40, height: 90 }}>
                    <div style={{ marginRight: 100 }}><b>GIÁM ĐỐC</b></div>
                </div>

                <div style={{ width: '100%', display: 'flex', justifyContent: 'flex-end', marginTop: 40 }}>
                    <div style={{ marginRight: 50 }}><b>BS.CKII. Phù Chí Dũng</b></div>
                </div>
            </div >
        </div >
    )
}


export const prepareNegotiation = (item, currentDate) => {

    let itemContent = item.itemContent;
    let listItems = item.listItems;
   
    let totalCost = 0;

    for (let record of listItems) {
        totalCost += +record.totalPrice
    }
    if (itemContent.isVAT) {
        totalCost = totalCost * (+itemContent.vatNumber + 100) / 100;
    }

    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', width: '100%' }}>

                <div style={{ display: 'flex', flexDirection: 'row', padding: 15 }}>
                    <div style={{ display: 'flex', flex: 1, flexDirection: 'column', alignItems: 'center' }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            <b> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </b>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                            <b> Độc lập – Tự do – Hạnh phúc </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 10, fontSize: TILTLE_FONT }}>
                            BIÊN BẢN THƯƠNG THẢO HỢP ĐỒNG
                        </div>

                        <div style={{
                            display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: '10p'
                        }}>
                            Thương thảo hợp đồng số: {itemContent.negotiationCode}
                        </div>
                    </div>

                </div>
                <div style={{ display: 'flex', marginTop: 15, marginLeft: 40 }}>
                    - Thời gian: {' ' + moment(new Date(itemContent.dateIn)).format("HH")} giờ {' ' + moment(new Date(itemContent.dateIn)).format("mm")} phút , ngày {' ' + moment(new Date(itemContent.dateIn)).format("DD")} tháng {' ' + moment(new Date(itemContent.dateIn)).format("MM")} năm {' ' + moment(new Date(itemContent.dateIn)).format("YYYY")} 
                </div>
                <div style={{ display: 'flex', marginTop: 15, marginLeft: 40 }}>
                    - Địa điểm  : {itemContent.aLocation}
                </div>

                <div style={{ display: 'flex', marginTop: 15, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>A. Thành phần</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Bên A: {itemContent.aSide} </b>
                </div>
                <div style={{ display: 'flex', marginRop: PADDING_TOP, marginLeft: 40 }}>
                    - Địa chỉ: {itemContent.aLocation}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Điện thoại:  {itemContent.aPhone !== null ? itemContent.aPhone : ''} ,      {itemContent.aFax !== null ? 'Fax : ' + itemContent.aFax : ''}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Người Đại diện: <b>{' ' + itemContent.aRepresent}</b>     <p style={{ marginLeft: 30 }} />       Chức vụ: {itemContent.aPosition}
                </div >
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Bên B: {itemContent.bSide} </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Địa chỉ: {itemContent.bLocation}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Điện thoại:  {itemContent.bPhone !== null ? itemContent.bPhone : ''} , {itemContent.bFax !== null ? 'Fax : ' + itemContent.bFax : ''}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Người Đại diện	:  <b>{' ' + itemContent.bRepresent}</b>     <p style={{ marginLeft: 30 }} />       Chức vụ: {itemContent.bPosition}
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>B. Nội dung thương thảo :</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>I/ Căn cứ ký hợp đồng : </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    Căn cứ vào biên bản họp về việc xác minh và kiểm giá của Ban kiểm giá số {itemContent.auditCode} ký ngày {moment(new Date(itemContent.auditTime)).format('DD/MM/YYYY')}
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>II/ Đại diện bên A và bên B </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Bên B: {itemContent.aSide} </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Địa chỉ	: {itemContent.aLocation}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Điện thoại	:  {itemContent.aPhone !== null ? itemContent.aPhone : ''} , {itemContent.aFax !== null ? 'Fax : ' + itemContent.aFax : ''}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Mã số thuế	:   {itemContent.aTaxCode}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Số Tài khoản	: {itemContent.aBankIDLabel}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Người Đại diện	:  <b>{' ' + itemContent.aRepresent}</b>     <p style={{ marginLeft: 30 }} />       Chức vụ: {itemContent.aPosition}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Bên B: {itemContent.bSide} </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Địa chỉ	: {itemContent.bLocation}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Điện thoại	:  {itemContent.bPhone !== null ? itemContent.bPhone : ''} , {itemContent.bFax !== null ? 'Fax : ' + itemContent.bFax : ''}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Mã số thuế	:   {itemContent.bTaxCode}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Số Tài khoản	: {itemContent.bBankID}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Người Đại diện	:  <b>{' ' + itemContent.bRepresent}</b>    <p style={{ marginLeft: 30 }} />       Chức vụ: {itemContent.bPosition}
                </div>

                <div style={{ display: 'flex', marginTop: 15, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>III/ Nội dung hợp đồng :</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 1. Đối tượng Hợp đồng</b>
                </div>

                <div style={{ marginTop: 15, marginBottom: 20 }}>
                    {getListQuotewithTotalPrice(listItems, itemContent.isVAT, totalCost)}
                </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Tổng cộng: {formatNumber(totalCost)}</b>
                </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    (Bằng chữ : {Utils.DocTienBangChu(totalCost)} đồng)
               </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 2. Thành phần Hợp đồng</b>
                </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    Thành phần Hợp đồng và thứ tự ưu tiên pháp lý như sau:
                </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    1.	Biên bản thương thảo hợp đồng
                </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    2.	Quyết định chọn nhà cung cấp
                </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    Các tài liệu kèm theo khác (nếu có).
                </div>
                <div style={{ display: 'block', pageBreakBefore: 'always !important',  marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 3 : Trách nhiệm của bên Bán :</b>
                    <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Bên bán cam kết cung cấp cho bên mua đầy đủ các loại hàng hóa và dịch vụ như nêu tại điều 1 của văn bản này, đồng thời cam kết thực hiện đầy đủ các nghĩa vụ và trách nhiệm được nêu trong điều kiện hợp đồng.
                    <div style={{ display: 'block', marginTop: PADDING_TOP }}>
                    -	Hàng hóa và dịch vụ được cung cấp theo hợp đồng phải có xuất xứ rõ ràng, hợp pháp được bên mua chấp nhận.
                     </div>
                    </div>
                </div>
               
               
                <div style={{ display: 'block', marginTop:  listItems.length > 3 ?  40 : PADDING_TOP + 2, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 4 : Trách ngiệm của bên mua :</b>
                </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Bên mua cam kết thanh toán cho bên bán giá trị hợp đồng như tại điều 5 của văn bản này theo phương thức được qui định tại điều kiện cụ thể của hợp đồng cũng như thực hiện đầy đủ nghĩa vụ và trách nhiệm khác được qui định trong điều kiện của hợp đồng.
                 </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 5 : Giá trị hợp đồng và phương thức thanh toán:</b>
                </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Giá trị hợp đồng: {formatNumber(totalCost)} VNĐ </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    (Bằng chữ : {Utils.DocTienBangChu(totalCost)} đồng)
                 </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Phương thức thanh toán:</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    a) Hình thức thanh toán chuyển khoản.
                 </div>
                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    b) Thời hạn thanh toán: Bên mua thanh toán cho Bên bán trong vòng {itemContent.term} ngày kể từ ngày Bên mua nhận được hàng và đầy đủ chứng từ hợp lệ
                 </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    c) Đồng tiền thanh toán theo hợp đồng: Việt Nam đồng (VNĐ).
                 </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 6 : Hình thức hợp đồng: {itemContent.bidType}</b>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP * 2, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 7 : Thời gian thực hiện hợp đồng:{ " " + itemContent.bidExpirated +  " " + itemContent.bidExpiratedUnit + " " }kể từ ngày ký hợp đồng.</b>
                </div>

                <div style={{ display: 'block', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT , marginBottom : 20 }}>
                    <b>Điều 8 : Hiệu lực hợp đồng  </b>
                    <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Hợp đồng có hiệu lực kể từ ngày ký hợp đồng.
                   </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -   Hợp đồng hết hiệu lực sau khi hai bên tiến hành thanh lý hợp đồng theo luật định. Sau 10 ngày kể từ khi hợp đồng hết hiệu lực nếu 2 bên không tiến hành thủ tục thanh lý hợp đồng thì xem như hợp đồng đã thanh lý.
                 </div>
                 
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Biên bản được lập thành 6 bộ, bên A giữ 4 bộ, bên B giữ 2 bộ, các biên bản này có giá trị pháp lý như nhau.
                </div>
                </div>

               
                <div style={{ marginTop: PADDING_TOP , display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                    <div style={{
                        flex: 1, display: 'flex', justifyContent: 'flex-start ', alignItems: 'center', flexDirection: 'column'
                    }}>
                        <div style={{ textAlign: 'center', height: 20 }}>  <b>ĐẠI DIỆN HỢP PHÁP CỦA BÊN BÁN  </b></div>
                        <div style={{ marginTop: 40 , height: 20 }}><b>{itemContent.bPosition}</b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}> {itemContent.bRepresent}</div>
                    </div>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'flex-start ', alignItems: 'center', flexDirection: 'column' }}>
                        <div style={{ textAlign: 'center', height: 20 }}> <b> ĐẠI DIỆN HỢP PHÁP CỦA BÊN MUA </b> </div>
                        <div style={{ textAlign: 'center', marginTop: 40 , height: 20}}><b>GIÁM ĐỐC</b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}>BS.CKII. PHÙ CHÍ DŨNG</div>
                    </div>

                </div>
            </div >
        </div >
    )
}


export const prepareDecision = (item, currentDate) => {

    let itemContent = item.itemContent;
    let listItems = item.listItems;

    let totalCost = 0;
    for (let record of listItems) {

        totalCost += +record.totalPrice
    }
    if (itemContent.isVAT) {
        totalCost = totalCost * (+itemContent.vatNumber + 100) / 100;
    }
    for (let bidMethod of BidMethodArr) {
        if (itemContent.bidMethod == bidMethod.value) {
            itemContent.bidMethodName = bidMethod.label;
        }
    }


    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>


                <div style={{ display: 'flex', flexDirection: 'row', padding: 15 }}>
                    <div style={{ display: 'flex', flex: 3, flexDirection: 'column', alignItems: 'center', marginRight: 20 }}>

                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            SỞ Y TẾ TP.HCM
                </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                            <b>    BV. TRUYỀN MÁU HUYẾT HỌC </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}>  </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 10 }}>
                            Số: {itemContent.decisionCode + ' / QĐ-TMHH'}
                        </div>
                    </div>
                    <div style={{ dislay: 'flex', flex: 4, flexDirection: 'column', alignItems: 'center' }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            <b> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </b>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                            <b> Độc lập – Tự do – Hạnh phúc </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 10 }}>
                            Tp.HCM, ngày {' ' + moment(new Date(itemContent.dateInOri)).format('DD') + ' '} tháng {' ' + moment(new Date(itemContent.dateInOri)).format('MM') + ' '} năm  {' ' + moment(new Date(itemContent.dateInOri)).format('YYYY') + ' '}
                        </div>
                    </div>
                </div>

                <div style={{ display: 'flex', width: '100%', marginTop: 15, justifyContent: 'center', alignItems: 'center', fontSize: HEADER_FONT }}>
                    <b>QUYẾT ĐỊNH</b>
                </div>
                <div style={{ display: 'flex', width: '100%', marginTop: PADDING_TOP, justifyContent: 'center', alignItems: 'center' }}>
                    <b>Về việc chọn đơn vị cung cấp hàng hóa</b>
                </div>
                <div style={{ width: '20%', paddingTop: 15, margin: 'auto', borderBottom: '1px solid  #000' }}>
                </div>
                <div style={{ display: 'flex', width: '100%', marginTop: 15, justifyContent: 'center', alignItems: 'center', fontSize: HEADER_FONT }}>
                    <b>GIÁM ĐỐC BỆNH VIỆN TRUYỀN MÁU HUYẾT HỌC</b>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ Luật Đấu thầu số 43/2013/QH13 ngày 26 tháng 11 năm 2013 của Quốc Hội nước Cộng Hòa Xã Hội Chủ Nghĩa Việt Nam.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ Nghị định số 63/2014/NĐ-CP ngày 26 tháng 06 năm 2014 của Chính Phủ về việc Hướng dẫn thi hành luật đấu thầu và lựa chọn nhà thầu.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ Thông tư 58/2016/TT-BTC ngày 29/3/2016 của Bộ Tài chính Quy định chi tiết việc sử dụng vốn nhà nước để mua sắm nhằm duy trì hoạt động thường xuyên của cơ quan nhà nước, đơn vị thuộc lực lượng vũ trang nhân dân, đơn vị sự nghiệp công lập, tổ chức chính trị, tổ chức chính trị - xã hội, tổ chức chính trị xã hội - nghề nghiệp, tổ chức xã hội, tổ chức xã hội - nghề nghiệp.
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ quyết định số 01/SYT ngày 01/01/1988 của Sở y tế về chức năng, nhiệm vụ và quyền hạn của Giám đốc.
                  </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ vào đề xuất của {' ' + itemContent.departmentNames.replace(',', ', ')}.
                  </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ biên bản xét chọn đơn vị cung cấp hàng hóa số: {' ' + itemContent.auditCode + ' '} ngày {' ' + moment(new Date(itemContent.auditTime)).format("DD/MM/YYYY")}
                </div>
                {itemContent.bidPlanCode != undefined &&
                    <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                        - Căn cứ kế hoạch lựa chọn nhà thầu ngày {' ' + itemContent.bidPlanCode + ' '} ngày {' ' + moment(new Date(itemContent.bidPlanTime)).format("DD/MM/YYYY")}
                    </div>
                }
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ biên bản thương thảo hợp đồng số: {' ' + itemContent.negotiationCode} ngày {' ' + moment(new Date(itemContent.negotiationTime)).format("DD/MM/YYYY")}
                </div>


                <div style={{ display: 'flex', width: '100%', marginTop: 15, justifyContent: 'center', alignItems: 'center', fontSize: HEADER_FONT }}>
                    <b>QUYẾT ĐỊNH</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    Điều 1: Nay quyết định chọn đơn vị cung cấp như sau:
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Tên nhà cung cấp: {itemContent.customerName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Địa chỉ: {itemContent.address}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -   Hàng hóa cung cấp
                </div>

                <div style={{ marginTop: 15, marginBottom: 40 }}>
                    {getListQuotewithTotalPrice(listItems, itemContent.isVAT, totalCost)}
                </div>
                <div style={{ display: 'flex', marginTop: 50, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>-	Giá trị: {formatNumber(totalCost) + ' '}VNĐ</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    Bằng chữ : {Utils.DocTienBangChu(totalCost)} đồng.
               </div>
               <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -   Hình thức:  {itemContent.bidMethodName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Nguồn vốn: {itemContent.capitalName}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -    Hình thức hợp đồng: {itemContent.bidType}
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Thời gian thực hiện: {itemContent.bidExpirated + ' ' + itemContent.bidExpiratedUnit + '.'}
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    Điều 2: Phòng Hành chánh quản trị, Phòng Tài chính kế toán và các bộ phận có liên quan chịu trách nhiệm phối hợp thực hiện quyết định này
               </div>


                <div style={{ marginTop: PADDING_TOP, display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                        <div style={{ textAlign: 'center' }}> Nơi nhận:</div>
                        <div style={{ marginTop: 5 }}>- Như điều 2;</div>
                        <div style={{ textAlign: 'center', marginTop: 5 }}>- Lưu.</div>
                    </div>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                        <div style={{ textAlign: 'center', marginTop: 20 }}><b>GIÁM ĐỐC</b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}>BS.CKII. PHÙ CHÍ DŨNG</div>
                    </div>

                </div>
            </div>
        </div>
    )
}


export const prepareContract = (item, currentDate) => {

    let itemContent = item.itemContent;
    let listItems = item.listItems;

    let totalCost = 0;
    for (let record of listItems) {

        totalCost += +record.totalPrice
    }
    if (itemContent.isVAT) {
        totalCost = totalCost * (+itemContent.vatNumber + 100) / 100;
    }


    return (
        <div style={{ display: 'block', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
            <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>

                <div style={{ display: 'flex', flex: 1, flexDirection: 'column', alignItems: 'center' }}>
                    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                        <b> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </b>
                    </div>
                    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: 5 }}>
                        <b> Độc lập – Tự do – Hạnh phúc </b>
                    </div>
                    <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}>
                    </div>
                    <div style={{ display: 'flex', alignItems: 'center', justifyVontent: 'center', marginTop: 15, fontSize: TILTLE_FONT }}>
                        HỢP ĐỒNG MUA BÁN
                        </div>

                    <div style={{
                        display: 'flex', alignItems: 'center', justifyContent: 'center' , marginTop: 10
                    }}>
                        Số: {itemContent.contractCode}
                    </div>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ Bộ Luật Dân sự số: 91/2015/QH13 của Quốc hội Nước Cộng Hòa Xã Hội Chủ Nghĩa Việt Nam, ban hành ngày 24/11/2015.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ vào Bộ Luật Thương mại số: 36/2005/QH11 của Quốc hội Nước Cộng Hòa Xã Hội Chủ Nghĩa Việt Nam, ban hành ngày 14/06/2005.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ Luật Đấu thầu số 43/2013/QH13 của Quốc Hội nước Cộng Hòa Xã Hội Chủ Nghĩa Việt Nam, khóa XIII, kỳ họp thứ 6 thông qua ngày 26 tháng 11 năm 2013.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ Nghị định số 63/2014/NĐ-CP ngày 26 tháng 06 năm 2014 của Chính Phủ về việc Hướng dẫn thi hành luật đấu thầu và lựa chọn nhà thầu.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ Thông tư 58/2016/TT-BTC ngày 29/3/2016 của Bộ Tài chính Quy định chi tiết việc sử dụng vốn nhà nước để mua sắm nhằm duy trì hoạt động thường xuyên của cơ quan nhà nước, đơn vị thuộc lực lượng vũ trang nhân dân, đơn vị sự nghiệp công lập, tổ chức chính trị, tổ chức chính trị - xã hội, tổ chức chính trị xã hội - nghề nghiệp, tổ chức xã hội, tổ chức xã hội - nghề nghiệp.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ biên bản thương thảo số {itemContent.negotiationCode} ký ngày {' ' + moment(new Date(itemContent.negotiationTime)).format("DD/MM/YYYY")} giữa công ty {itemContent.bSide} và {itemContent.aSide}.
                </div>
                {itemContent.decisionCode != null &&
                    <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                        -	Căn cứ Quyết định số: {itemContent.decisionCode} ngày {' ' + moment(new Date(itemContent.decisionTime)).format("DD/MM/YYYY")} của Giám đốc Bệnh viện Truyền máu Huyết học về việc chọn đơn vị cung cấp.
                </div>
                }
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    -	Căn cứ theo nhu cầu của bên mua và khả năng cung cấp của bên bán.
            </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    Hôm nay, {' ' + moment(new Date(itemContent.dateIn)).format("HH")} giờ {' ' + moment(new Date(itemContent.dateIn)).format("mm")} phút , ngày {' ' + moment(new Date(itemContent.dateIn)).format("DD")} tháng {' ' + moment(new Date(itemContent.dateIn)).format("MM")} năm {' ' + moment(new Date(itemContent.dateIn)).format("YYYY")} , chúng tôi gồm có:
                </div>


                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Bên A (Bên Mua): {itemContent.aSide} </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Địa chỉ	: {itemContent.aLocation}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Điện thoại	:  {itemContent.aPhone !== null ? itemContent.aPhone : ''} , {itemContent.aFax !== null ? 'Fax : ' + itemContent.aFax : ''}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Mã số thuế	:  {itemContent.aTaxCode}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Số Tài khoản	:{itemContent.aBankIDLabel}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Người Đại diện	:  <span style={{ marginLeft: 5, fontEeight: 'bold' }}> {' ' + itemContent.aRepresent} </span><p style={{ marginLeft: 30 }} /> Chức vụ: {itemContent.aPosition}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Bên B (Bên Bán): {itemContent.bSide} </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Địa chỉ	: {itemContent.bLocation}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Điện thoại	:  {itemContent.bPhone !== null ? itemContent.bPhone : ''} ,  {itemContent.bFax !== null ? 'Fax : ' + itemContent.bFax : ''}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Mã số thuế	:  {itemContent.bTaxCode}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Số Tài khoản	: {itemContent.bBankID}
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    - Người Đại diện	: <span style={{ marginLeft: 5, fontWeight: 'bold' }}> {itemContent.bRepresent}  </span> <p style={{ marginLeft: 30 }} />
                         Chức vụ: {itemContent.bPosition}
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    Sau khi bàn bạc hai bên thống nhất ký kết hợp đồng mua bán hàng hóa theo các điều khoản sau:
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>ĐIỀU 1: Nội dung hợp đồng</b>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    Bên B nhận cung cấp cho bên A hàng hóa chi tiết cụ thể như sau:
                </div>


                <div style={{ marginTop: 15, marginBottom: 20 }}>
                    {getListQuotewithTotalPrice(listItems, itemContent.isVAT, totalCost)}
                </div>
                <div style={{ marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Tổng cộng: {formatNumber(totalCost) + ' VNĐ '} </b> (Bằng chữ : {Utils.DocTienBangChu(totalCost)} đồng).
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20 }}>
                    (Giá trên đã bao gồm hóa đơn thuế giá trị gia tăng, chi phí giao hàng và lắp đặt cho bên mua)
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>ĐIỀU 2: Trách nhiệm bên bán (Bên B)</b>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    1.	Bàn giao toàn bộ hàng hóa đúng theo điều 1 của hợp đồng tại Bệnh viện Truyền Máu Huyết Học.
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    2.	Cung cấp các hóa đơn chứng từ hợp lệ theo quy định của Nhà nước.
              </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    3.	Bên bán chịu trách nhiệm bảo hành thiết bị trong vòng 12 tháng kể từ ngày bàn giao – nghiệm thu.
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    4.	Trong thời gian bảo hành bên bán sẽ có trách nhiệm cho bên mua mượn thiết bị tương đương để sử dụng.
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    5.	Trường hợp có hỏng hóc trong thời gian bảo hành, bên mua thông báo cho bên bán (bằng văn bản hoặc 
               </div>
               <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    bản Fax) trong vòng 12 giờ bên bán phải có mặt để kiểm tra, sửa chữa hay thay thế. Nếu quá thời gian trên bên mua được quyền bảo hành mọi chi phí bên bán phải chịu.
               </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>ĐIỀU 3: Trách nhiệm bên mua (Bên A)</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    1.	Cử cán bộ để tiếp nhận bàn giao.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    2.	Bảo quản hàng hóa đúng quy trình, quy phạm và kịp thời thông báo cho bên bán những sự cố, hỏng hóc trong thời gian bảo hành.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    3.	Thanh toán tiền hàng cho bên bán theo điều 5 của hợp đồng.
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP * 2, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b> ĐIỀU 4: Thời gian thực hiện hợp đồng:</b>
                </div>
               
                 <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                 -	Thời gian giao hàng: Trong vòng  {itemContent.termLabel} kể từ ngày ký hợp đồng. Bên bán bàn giao đầy đủ số lượng, chất lượng, chủng loại hàng hóa tại địa điểm bệnh viện sử dụng.
                 </div>
                
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Thời gian thực hiện hợp đồng: {itemContent.bidExpirated + " " + itemContent.bidExpiratedUnit + " " } kể từ ngày ký hợp đồng.
                 </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP * 2, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 5 : Giá trị hợp đồng, phương thức và điều kiện thanh toán.</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP * 2, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Giá trị hợp đồng: {formatNumber(totalCost)} VNĐ </b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    (Bằng chữ : {Utils.DocTienBangChu(totalCost)} đồng)
                 </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    <b>Giá hợp đồng bao gồm</b>: Giá hàng hóa, VAT, mọi chi phí vận chuyển tại bệnh viện.
                 </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP * 2, marginLeft: 40, fontSize: HEADER_FONT }}>
                    <b>Phương thức thanh toán:</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    -	Hình thức thanh toán: thanh toán bằng chuyển khoản.
                 </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    -	Thời hạn thanh toán:  {itemContent.termLabel} kể từ ngày Phòng tài chính Kế toán bên A nhận được đầy đủ chứng từ thanh toán hợp lệ.
                 </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    -	Điều kiện thanh toán: Hàng hóa được xem là đủ điều kiện thanh toán khi đã được giao cho Bệnh viện và được cung cấp đầy đủ chứng từ hợp lệ
                 </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 50 }}>
                    -	Chứng từ thanh toán gồm:
                 </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 80 }}>
                    +  Phiếu giao hàng;
                 </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 80 }}>
                    +  Biên bản bàn giao nghiệm thu;
                 </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 80 }}>
                    +  Hóa đơn tài chính;
                 </div>


                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 6 : Hình thức hợp đồng: {itemContent.bidType}</b>
                </div>

                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>Điều 7: Hiệu lực hợp đồng</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Hợp đồng có hiệu lực kể từ ngày ký hợp đồng.
                   </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    -	Hợp đồng hết hiệu lực sau khi hai bên tiến hành thanh lý theo luật định. Hợp đồng được thanh lý sau khi hai bên hoàn tất mọi nghĩa vụ theo nội dung hợp đồng đã ký.
                  </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 20, fontSize: HEADER_FONT }}>
                    <b>ĐIỀU 8: Điều khoản chung</b>
                </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    Hai bên cam kết thực hiện các điều khoản đã ghi trong hợp đồng này. Trong quá trình thực hiện khi có sự vướng mắc, hai bên phải thông báo cho nhau bằng văn bản để cùng giải quyết. Trường hợp một bên vi phạm gây thiệt hại cho bên kia thì phải có trách nhiệm bồi thường thiệt hại đã gây ra dựa trên cơ sở của Bộ Luật Dân sự Nhà nước Việt Nam ban hành làm căn cứ và thực tế thiệt hại đã xảy ra. Trong trường hợp không giải quyết được bằng thương lượng thì vụ việc sẽ được đưa ra Tòa án kinh tế TP.HCM giải quyết. Quyết định của Tòa án là quyết định cuối cùng và hai bên sẽ phải tuân thủ quyết định đó. Mọi chi phí do bên thua kiện phải chịu, trừ khi có sự thỏa thuận nào khác.
                  </div>
                <div style={{ display: 'flex', marginTop: PADDING_TOP, marginLeft: 40 }}>
                    Hợp đồng này được lập thành 06 bản, bên mua giữ 04 bản, bên bán giữ 01 bản có giá trị như nhau, kể từ ngày hai bên ký kết hợp đồng.
                </div>


                <div style={{ marginTop: 20, display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                        <div style={{ textAlign: 'center', height: 20 }}>  <b>ĐẠI DIỆN BÊN B</b></div>
                        <div style={{ marginTop: 40 }}><b>{itemContent.bPosition}</b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}> {itemContent.bRepresent}</div>
                    </div>
                    <div style={{ flex: 1, display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                        <div style={{ textAlign: 'center', height: 40 }}> <b>ĐẠI DIỆN BÊN A</b> </div>
                        <div style={{ textAlign: 'center', marginTop: 20 }}><b>{itemContent.aPosition}</b></div>
                        <div style={{ textAlign: 'center', marginTop: 100 }}>BS.CKII. PHÙ CHÍ DŨNG</div>
                    </div>
                </div>
            </div>
        </div >
    )
}

export const prepareDeliveryReceiptC34 = (item, currentDate) => {

    let itemContent = item.itemContent;
    let listItems = item.listItems;
    let totalCost = 0;
    for (let record of listItems) {
        totalCost += +record.totalPrice;
    }
    if (itemContent.isVAT) {
        totalCost = totalCost * (+itemContent.vatNumber + 100) / 100;
    }
    for (let location of AuditLocationArr) {
        if (itemContent.deliveryReceiptPlace)
            if (itemContent.deliveryReceiptPlace.toString() == location.value.toString()) {
                itemContent.deliveryReceiptPlace = location.label;
            }
    }


    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
        <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>

         <div style={{display:'flex', flexDirection:'row', padding: 15}}>
                <div style={{display:'flex', flex:1, flexDirection:'column', alignItems: 'flex-start',  fontSize:SMALL_FONT,   marginRight: 70}}>       
                    <div style={{display:'flex', alignItems:'center', justifyContent:'center'}}>
                    <b>Đơn vị: Bệnh viện Truyền máu Huyết học</b>
                    </div>
                    <div style={{display:'flex', alignItems:'center', justifyContent:'center', marginTop: 2}}>
                    <b>Bộ phận: P.Hành chánh Quản trị</b>
                    </div>
                    <div style={{display:'flex', alignItems:'center', justifyContent:'center',marginTop: 2 }}>
                    <b>Mã quan hệ ngân sách:</b>
                    </div>                          
                </div>
                <div style={{display:'flex', flex:1, flexDirection:'column', alignItems: 'center', fontSize:SMALL_FONT}} >
                    <div style={{display:'flex', alignItems:'center', justifyContent:'center'}}>
                    Mẫu số C34 - HD
                    </div>
                    <div style={{display:'flex', alignItems:'center', textAlign:'center', justifyContent:'center' , marginTop: 2}}>
                    (Ban hành theo thông tư số 107/20017/TT-BTC 
                        ngày 10/10/2017 của Bộ Tài Chính)
                        
                    </div>                                                          
                </div>                  
            </div>

            <div style={{display:'flex' , width:'100%' , marginTop: 15, justifyContent:'center', alignItems:'center', fontSize:HEADER_FONT }}>
                <b>PHIẾU GIAO NHẬN NGUYÊN LIỆU, VẬT LIỆU, CÔNG CỤ, DỤNG CỤ  </b>
            </div>
            <div style={{display:'flex' , width:'100%', marginTop:2, justifyContent:'center' , alignItems:'center' }}>
                Ngày {moment(itemContent.deliveryReceiptDate , 'DD-MM-YYYY').format('DD')} tháng {moment(itemContent.deliveryReceiptDate , 'DD-MM-YYYY').format('MM')} năm {moment(itemContent.deliveryReceiptDate , 'DD-MM-YYYY').format('YYYY')} 
            </div>
               
            <div style={{display:'flex' , width:'100%', marginTop:2, justifyContent:'center', alignItems:'center' }}>
                        Số:{' ' + itemContent.deliveryReceiptCode}
            </div>

            <div style={{display:'flex' , marginTop:PADDING_TOP, marginLeft:20}}>
                <div style={{flex:3}} >
                  - Họ tên người giao:                                           
                </div>
                <div style={{flex:2}} >
                 Địa chỉ: {itemContent.curDepartmentName}
                </div>
            </div>
            <div style={{display:'flex' , marginTop:PADDING_TOP, marginLeft:20}}>
                <div style={{flex:3}} >
                - Họ tên người nhận:                                                                                   
                </div>
                <div style={{flex:2}} >
                Địa chỉ: {itemContent.departmentName}
                </div>
            </div>

            <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
                - Địa điểm giao nhận: {itemContent.deliveryReceiptPlace}             
            </div>

            <div style={{display:'flex' , marginTop:PADDING_TOP, marginLeft:20}}>
                - Theo  Đề xuất {' ' + itemContent.proposalCode + ' ' }    ngày {moment(new Date(itemContent.proposalTime)).format('DD')} tháng {moment(new Date(itemContent.proposalTime)).format('MM')} năm {moment(new Date(itemContent.proposalTime)).format('YYYY')}     của Phòng {itemContent.curDepartmentName} , Phòng  {itemContent.departmentName} tiến hành giao, nhận các loại nguyên liệu, vật liệu, công cụ, dụng cụ, như sau: 
            </div>

            <div style={{marginTop: 15, marginBottom:40}}>
                {getListQuotewC34(listItems, itemContent.isVAT, totalCost, itemContent.vatNumber)} 
            </div>        

            <div style={{marginTop:40, display:'flex', flexDirection:'row',width:'100%', fontSize:NORMAL_FONT}}>
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center' ,
               flexDirection: 'column'
               }}>
                   <div>Người lập</div>
                   <div>(Ký, họ tên)</div>
               </div>
   
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center', 
               flexDirection: 'column'}}>
                   <div>Người giao </div>
                   <div>(Ký, họ tên)</div>
               </div>
   
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center' ,
               flexDirection: 'column'
               }}>
               <div>Người nhận</div>
               <div>(Ký, họ tên)</div>
   
           </div>
           </div>
    </div>
    </div>
    )
}

export const prepareC50Employee = (listEmployees) => {
    return (    
        <React.Fragment>
        {listEmployees.map((employee)=>{
            return (
            <div style={{display:'flex', justifyContent:'space-between', width:'100%'}}>
                <div style={{flex:1, textAlign:'left'}}> – Ông/Bà {employee.name}</div>
                <div style={{flex:1, textAlign:'left'}}> Chức vụ: {employee.roleName}</div>
                <div style={{flex:1,  textAlign:'left'}}>{getLabelString(employee.role, DeliveryRoleArr)}</div>
            </div>
        )})}
    </React.Fragment>
    )
    
}


function getPriceItemsC50(price, isVAT , vatNumber , amount){

    let itemPriceAfterVAT = price;
    let itemTotalAftetVAT = 0;
    if (isVAT) itemPriceAfterVAT = (price * vatNumber) / 100 + price;
    itemTotalAftetVAT = +itemPriceAfterVAT * +(amount)
    return {itemPriceAfterVAT,itemTotalAftetVAT}
}


function getListItemsC50(listItems, isVAT, totalCost, vatNumber) {

   return (
        <div className="childDetailWarpper" style={{marginTop: 20 , marginLeft: - 20 , width : 1000}}>      
                    <div className="listItemHeader" style={{display:'flex'}}>
                        <div className="noWrap" style={{width:40, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>STT</div>
                        <div className="noWrap"  style={{width:180, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>Tên, ký hiệu quy cách (cấp hạng TSCĐ)</div>
                        <div className="noWrap" style={{width:50,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>Số hiệu TS CĐ </div>
                        <div className="noWrap" style={{width:50,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>Nước sản xuất</div>
                        <div className="noWrap" style={{width:55,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>Năm sản xuất</div>
                        <div className="noWrap" style={{width:55,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>Năm đưa vào sử dụng</div>
                        <div className="noWrap"style={{width:50,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>ĐVT</div>
                        <div className="noWrap" style={{width:50,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>SL</div>
                        <div className="noWrap" style={{width:80,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>Giá mua (ZSX)</div>  
                        <div className="noWrap"style={{width:80,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>Chi phí vận chuyển</div>  
                        <div className="noWrap" style={{width:80,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>Chi phí chạy thử</div>             
                        <div className="noWrap" style={{width:100,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>Nguyên giá TSCĐ</div>  
                        <div className="noWrap"style={{width:100,  textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>TL kỹ thuật kèm theo</div>
                    </div>
                    <div className="listItemRow" style={{display:'flex'}}>
                        <div className="noWrap" style={{width:40, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>A</div>
                        <div className="noWrap"style={{width:180, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>B</div>
                        <div  className="noWrap" style={{width:50, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>C</div>
                        <div className="noWrap" style={{width:50, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>D</div>
                        <div className="noWrap" style={{width:55, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>E</div>
                        <div className="noWrap"  style={{width:55, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>F</div>
                        <div className="noWrap" style={{width:50, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>G</div>
                        <div className="noWrap" style={{width:50, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>1</div>
                        <div className="noWrap" style={{width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>2</div>  
                        <div className="noWrap" style={{width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>3</div>  
                        <div className="noWrap" style={{width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>4</div>             
                        <div className="noWrap" style={{width:100, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>5</div>  
                        <div className="noWrap" style={{width:100, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>H</div>
                    </div>
                    {listItems.map((item,index)=>{
                        return (
                        <div className="listItemRow" style={{display:'flex'}}>
                            <div className="noWrap" style={{width:40, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>{index + 1}</div>
                            <div className="noWrap" style={{width:180, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>{item.itemName}</div>
                            <div className="noWrap" style={{width:50, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}></div>
                            <div className="noWrap" style={{width:50, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}></div>
                            <div className="noWrap" style={{width:55, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}></div>
                            <div className="noWrap" style={{width:55, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}></div>
                            <div className="noWrap" style={{width:50, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>{item.itemUnit}</div>
                            <div className="noWrap" style={{width:50, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>{item.amount}</div>
                            <div className="noWrap" style={{width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}></div>  
                            <div className="noWrap" style={{width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}></div>  
                            <div className="noWrap" style={{width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}></div>             
                            <div className="noWrap" style={{width:100, textAlign: 'right',  borderRight: '1px solid  #ddd', padding:5}}>{getPriceItemsC50(item.itemPrice,isVAT, vatNumber,item.amount).itemTotalAftetVAT}</div>  
                            <div className="noWrap" style={{width:100, textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}></div>
                        </div>
                    )})}
            <div className="listItemRow" style={{display:'flex' }}>
            <div className="noWrap"  style={{width:770 ,textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5 }}>Tổng cộng</div>      
            <div className="noWrap" style={{width:100 ,textAlign: 'right',  borderRight: '1px solid  #ddd', padding: 5 }}>{formatNumber( totalCost)}</div>
            <div className="noWrap" style={{width:100  }}></div>
        
             </div>
        </div>
    )
}




function getListQuoteC50(listItems, isVAT, totalCost, vatNumber) {

    return (
        <React.Fragment>
            {getListItemsC50(listItems, isVAT,totalCost, vatNumber)}
            
         
           
            </React.Fragment>
    )
}

function getSubC50() {
  
   return ( 
    <div className="childDetailWarpper" style={{ width:900, marginLeft:15, pageBreakInside:'avoid !important'  }}>      
        <div className="listItemRow" style={{pageBreakInside:'avoid !important' , pageBreakAfter:'always !important' ,display:'flex'}}>
            <div className="noWrap" style={{width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>STT</div>
            <div className="noWrap" style={{width:400,    textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5  }}>Tên, quy cách dụng cụ, phụ tùng</div>
            <div className="noWrap" style={{width:140, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Số lượng</div>
            <div className="noWrap" style={{width:140, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Đơn vị tính</div>
            <div className="noWrap" style={{width:140, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>Giá trị</div>
        </div>
        <div className="listItemRow" style={{pageBreakInside:'avoid !important' , pageBreakAfter:'always !important' ,display:'flex'}}>
            <div className="noWrap"  style={{width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>A</div>
            <div className="noWrap" style={{width:400,    textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5}}>B</div>
            <div className="noWrap" style={{width:140,    textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}>C</div>
            <div className="noWrap" style={{ width:140,    textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5}}>1</div>
            <div className="noWrap" style={{width:140, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5}}>2</div>
        </div>
      
       {[0,0,0,0,0].map(()=>{return (        
            <div className="listItemRow" style={{pageBreakInside:'avoid !important' , pageBreakAfter:'always !important' , display:'flex'}}>
                <div className="noWrap" style={{ height:40,width:80, textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5}}></div>
                <div className="noWrap" style={{height:40,width:400,    textAlign: 'center',  borderRight: '1px solid  #ddd', padding:5  }}></div>
                <div className="noWrap" style={{height:40,width:140,    textAlign: 'center',  borderRight: '1px solid  #ddd', padding: 5 }}></div>
                <div className="noWrap" style={{height:40, width:140,    textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5}}></div>
                <div className="noWrap" style={{height:40,width:140, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5}}></div>
            </div>       
            ) })
            }

    </div>
   )
}
export const prepareDeliveryReceiptC50 = (item, currentDate) => {

    let itemContent = item.itemContent;
    let listItems = item.listItems;
   
    let totalCost = 0;
    for (let record of listItems) {
       
        totalCost += +record.itemPrice * +record.amount
    }


    if (itemContent.isVAT) {
        totalCost = totalCost * (+itemContent.vatNumber + 100) / 100;
    }

    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
        <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>

            <div style={{display:'flex', flexDirection:'row', padding: 15}} >
                <div style={{display:'flex', flex:3, flexDirection:'column', alignItems: 'flex-start',  fontSize:SMALL_FONT,   marginRight: 70}}>
            
                    <div style={{display:'flex', alignItems:'center', justifyContent:'center'}}>
                    <b>Đơn vị: Bệnh viện Truyền máu Huyết học</b>
                    </div>
                    <div style={{display:'flex', alignItems:'center', justifyContent:'center', marginTop: 2}}>
                    <b>Bộ phận: P.Hành chánh Quản trị</b>
                    </div>
                    <div style={{display:'flex', alignItems:'center', justifyContent:'center',marginTop: 2}}>
                    <b>Mã quan hệ ngân sách:</b>
                    </div>                          
                </div>
                <div style={{display:'flex', flex:4, flexDirection:'column', alignItems: 'center',fontSize:SMALL_FONT}} >
                    <div style={{display:'flex', alignItems:'center', justifyContent:'center'}}>
                    Mẫu số C50 - HD
                    </div>
                    <div style={{display:'flex', alignItems:'center', textAlign:'center', justifyContent:'center', marginTop: 2}}>        
                    (Ban hành theo thông tư số 107/20017/TT-BTC ngày 10/10/2017 của Bộ Tài Chính)                                        
                    </div>                                                          
                </div>                  
            </div>

            <div style={{display:'flex' , width:'100%', marginTop: '15px', justifyContent:'center' , alignItems:'center', fontSize:HEADER_FONT}}>
                <b>BIÊN BẢN GIAO NHẬN TSCĐ VÀ CCLĐ</b>
            </div>
            <div style={{display:'flex' , width:'100%', marginTop:2, justifyContent:'center' , alignItems:'center' }}>
                Ngày {moment(itemContent.deliveryReceiptDate , 'DD-MM-YYYY').format('DD')} tháng {moment(itemContent.deliveryReceiptDate , 'DD-MM-YYYY').format('MM')} năm {moment(itemContent.deliveryReceiptDate , 'DD-MM-YYYY').format('YYYY')} 
            </div>
               
            <div style={{display:'flex' , width:'100%', marginTop:2, justifyContent:'center' , alignItems:'center' }}>
                        Số : {' ' + itemContent.deliveryReceiptCode}
            </div>

         
            <div style={{display:'flex' ,marginTop:PADDING_TOP, marginLeft:20}}>
            Căn cứ Quyết định số:{'           '}ngày {'           '} 	tháng {'           '} 	năm  {'           '}	của 	
            {'                                                                             '}Về việc bàn giao TSCĐ    
            </div>

            <div style={{display:'flex' , marginTop:PADDING_TOP, marginLeft:20}}>
             Ban giao nhận TSCĐ gồm:
            </div>

            <div style={{marginTop:PADDING_TOP, marginLeft:20, display:'flex', flexDirection:'row',width:'100%', fontSize:NORMAL_FONT}}>
               <div style={{flex:3,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'left' ,
               flexDirection: 'column'
               }}>
                   <div>–  Ông/Bà: BS.CKII Phù Chí Dũng</div>
                   <div>–  Ông/Bà: ThS.Trần Thị Thu Hồng</div>
                   <div>–  Ông/Bà: ThS.KS. Huỳnh Văn Minh</div>
                   <div>–  Ông/Bà: …………………………………………………………</div>
               </div>

               <div style={{
               flex:3,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'left',
               flexDirection: 'column'}}>
                   <div>Chức vụ: Giám đốc bệnh viện</div>
                   <div>Chức vụ: Trưởng phòng TCKT</div>
                   <div>Chức vụ: Trưởng phòng HCQT</div>
                   <div>Chức vụ: ……………………………………………</div>
               </div>
   
               <div style={{flex:2,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'left' ,
               flexDirection: 'column'}}>
                    <div>Đại diện P.HCQT</div>
                   <div>Đại điện P.TCKT</div>
                   <div>Đại diện bên giao</div>
                   <div>Đại diện bên nhận</div>
               </div>
   
           </div>
          
            {/* <div style={{marginTop:PADDING_TOP, marginLeft:20}}>
             {prepareC50Employee(item.listEmployees)}
            </div> */}

            <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
                Địa điểm giao nhận TSCĐ:{getLabelString(itemContent.deliveryReceiptPlace, AuditLocationArr)}
            </div>

            <div style={{display:'flex' , marginTop:PADDING_TOP, marginLeft:20 }}>
                Xác nhận việc giao nhận TSCĐ như sau:
            </div>

            <div style={{marginTop: 15, marginBottom:40}}>
                {getListQuoteC50(listItems, itemContent.isVAT, totalCost, itemContent.vatNumber)} 
            </div>        
            <div style={{display:'flex' , width:'100%', marginTop: 15, justifyContent:'center' , alignItems:'center', fontSize:HEADER_FONT}}>
                <b>DỤNG CỤ, PHỤ TÙNG KÈM THEO</b>
            </div>

            <div style={{marginTop: 9, marginBottom:40}}>
            {getSubC50()} 
            </div>        

            <div style={{marginTop:30, display:'flex', flexDirection:'row',width:'100%', fontSize:NORMAL_FONT}}>
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center' ,
               flexDirection: 'column'
               }}>
                   <div>Thủ trưởng</div>
                   <div>(Ký, họ tên, đóng dấu)</div>
               </div>

               <div style={{
               flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center',
               flexDirection: 'column'}}>
                   <div>Kế toán trưởng</div>
                   <div>(Ký, họ tên)</div>
               </div>
   
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center' ,
               flexDirection: 'column'}}>
                   <div>Người giao </div>
                   <div>(Ký, họ tên)</div>
               </div>
   
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center',
               flexDirection: 'column'
               }}>
               <div>Người nhận</div>
               <div>(Ký, họ tên)</div>
               </div>
           </div>
           <div style={{marginTop:100,display:'flex', flexDirection:'row',width:'100%', fontSize:NORMAL_FONT}}>
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center'
               }}>
                   <div>BS.CKII Phù Chí Dũng</div>
               </div>

               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center'
               }}>
                   <div>CN.Trần Thị Thu Hồng</div>
               </div>
   
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center',
               flexDirection: 'column'}}>            
               </div>  
              <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center',
               flexDirection: 'column'}}>
               </div>
           </div>
    </div>
    </div>
    )
}


function getListAcceptance(listItems) {
   return (
    <div className="childDetailWarpper">
        <div className="addItemWrapper listItemWrapper" >
            <div className="listItemHeader"  style={{display: 'flex'}} >
                <div style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' , textAlign:'center'}}>Các công việc đã thực hiện</div>
                    <div style={{flex: 2, borderRight: '1px solid #ddd' }}>
                        <div className="noWrap" style={{display:'flex', justifyContent:'center'}}>
                            Kết quả
                        </div>
                        <div className="noWrap"  style={{display:'flex', textAlign:'center'}}>
                            <div  style={{ flex: 1, borderTop: '1px solid #ddd' , borderRight: '1px solid #ddd', display:'flex', justifyContent:'center', alignItems:'center'}}>
                            Đạt
                            </div>
                            <div  style={{flex: 1, borderTop:'1px solid #ddd',display:'flex', justifyContent:'center', alignItems:'center'}}>
                            Không đạt
                            </div>
                        <div>
                    </div>
                </div>
                </div>
            </div>       
   {listItems.map((item)=>{
       return (
        <div className="listItemRow"  style={{display: 'flex'}} >
        <div className="noWrap" style={{flex: 3 ,borderRight: '1px solid #ddd' ,padding: '5px 10px'}}>{item.itemName}</div>
        <div className="noWrap" style={{flex: 1 , borderRight: '1px solid #ddd'  ,  display:'flex', justifyContent:'center', alignItems:'center'}}> 
         <div className="noWrap" className={`checkbox-print ${item.acceptanceResult == true ? "crossed" : ""} `} style={{marginLeft:5,marginRight:15, marginTop:5}}></div>
        </div>
        <div className="noWrap" style={{flex: 1, display:'flex', justifyContent:'center', alignItems:'center'}}>  
        <div className={`noWrap checkbox-print ${item.acceptanceResult != true ? "crossed" : ""} `} style={{marginLeft:5, marginRight:15, marginTop:5}}></div>
        </div>
        </div>      
       )
   })
   }  
        </div>      
     </div>       
   )
}

function getItemViewAcceptance(listItems) {
    return (
     <div >
    {listItems.map((item)=>{
        return (
         <div className="noWrap" style={{marginLeft: '20px', padding: '5px 10px'}}>{ '      ' + item.itemName}</div>
        )
    })
    }     
      </div>       
    )
 }


export const prepareAcceptance = (item, currentDate) => {
    let itemContent = item.itemContent;
    let listItems = item.listItems;
    let itemStr = ' ';
    for (let record of listItems) {
        itemStr +=  record.itemName + '. \n';
    }
    // itemStr = itemStr.substring(0, itemStr.length - 1) + '.';
    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 35, paddingRight: 35,  }}>
               
       
        <div style={{display:'flex', flexDirection:'column', padding: 15}}>
                <div style={{display:'flex', flexDirection:'row', padding: 15}} >
                        <div style={{display:'flex', flexDirection:'row', width:'100%'}}>   
                                <div style={{display:'flex', flex:1, flexDirection:'row', alignItems: 'center'}}>     
                                <div style={{position:'absolute',top:50,left:30}}>
                                <img src="./images/logo.png" width={120}/>
                                </div>                              
                                    <div style={{display:'flex',  marginLeft: 30, marginTop:'-35px', flex:1, flexDirection:'column', alignItems: 'center'}}>                   
                                        <div style={{display:'flex', alignItems:'center', justifyContent:'center'}}>
                                            BV. TRUYỀN MÁU HUYẾT HỌC
                                        </div>
                                        <div style={{display:'flex', alignItems:'center', justifyContent:'center', marginTop: 5}}>
                                            <b>    Phòng Hành Chánh Quản Trị </b>
                                        </div>
                                        <div style={{width:'50%',borderBottom:'1px solid  #000', marginTop: 15}}>  
                                        </div>
                                        <div style={{display:'flex', alignItems:'center', justifyContent:'center' ,marginTop: 10}}>                 
                                        </div>    
                                    </div>                     
                                </div>

                                <div style={{display:'flex', flex:1, flexDirection:'column', alignItems: 'center'}}>
                                    <div style={{display:'flex', alignItems:'center', justifyContent:'center'}}>
                                        <b> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </b>
                                    </div>
                                    <div style={{display:'flex', alignItems:'center', justifyContent:'center', marginTop: 5}}>
                                        <b> Độc lập – Tự do – Hạnh phúc </b>
                                    </div>
                                    <div style={{width:'50%', borderBottom:'1px solid  #000 ', marginTop: 15}}>                      
                                    </div>
                                    <div style={{display:'flex', alignItems:'center', justifyContent:'center', marginTop: 10}}>
                                        Tp.HCM, ngày .... tháng .... năm .... 
                                    </div>                                            
                                </div>                  
                        </div>
                </div>

                <div style={{display:'flex', alignItems:'center', justifyContent:'center' , marginTop: 10, fontSize:TILTLE_FONT}}>
                        BIÊN BẢN NGHIỆM THU 
                </div>     

                <div style={{display:'flex', marginTop:30 , marginLeft:20}}>
                 -	Tên thiết bị: {itemStr}
                </div>

                <div style={{display:'flex' , marginTop:PADDING_TOP, marginLeft:20}}>
                    <div style={{width:300}}>
                    -	Mã số máy:
                    </div>
                    <div style={{width:300}}>
                    Kiểu máy: 
                    </div>
                </div>
    
                <div style={{display:'flex', marginTop:PADDING_TOP,marginLeft:20}}>
                -	Hãng sản xuất: 
                </div>

                <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
                -	Nước sản xuất: 
                </div>

                <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
                - Khoa/ Phòng sử dụng:  {itemContent.departmentName}
                </div>
                
                <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
                -	Nội dung nghiệm thu:
                </div>


                <div style={{marginTop: 15, marginBottom:20, marginLeft:20}}>
                {getListAcceptance(listItems)}
                </div>
                
                <div style={{marginTop:PADDING_TOP, marginLeft:20}}>
                - Kết luận: 
                <div style={{display:'flex', marginLeft:20}}>
                    <div className={`checkbox-print ${itemContent.acceptanceResult == 1 ? "crossed" : ""} `} style={{marginLeft:5, marginRight:15, marginTop:5}}></div>
                    Đạt yêu cầu và đưa vào sử dụng
                </div>
                   
                <div style={{display:'flex' , marginLeft:20}}>
                 <div className={`checkbox-print ${itemContent.acceptanceResult == 2 ? "crossed" : ""} `} style={{marginLeft:5, marginRight:15, marginTop:5}}></div>
                   Không đạt yêu cầu
                </div>
              
                <div style={{display:'flex', marginLeft:20}}>
                    <div className={`checkbox-print ${itemContent.acceptanceResult == 3 ? "crossed" : ""} `} style={{marginLeft:5, marginRight:15, marginTop:5}}></div> 
                    Khác
                </div>
              
            </div>

            <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
            -	Ý kiến của Khoa/ Phòng sử dụng:  {itemContent.acceptanceNote ? itemContent.acceptanceNote : ''}
            </div>
        
            <div style={{marginTop:30,display:'flex', flexDirection:'row',width:'100%', fontSize:NORMAL_FONT}}>
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center' ,
               flexDirection: 'column'}}>
                   <div>Trưởng phòng</div>
               </div>

               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center',
               flexDirection: 'column',
               }}>
                   <div>Khoa/ Phòng sử dụng</div>
               </div>            
           </div>
           
           <div style={{marginTop:100,display:'flex', flexDirection:'row', width:'100%', fontSize:NORMAL_FONT}}>
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center'
               }}>
                   <div>ThS. KS Huỳnh Văn Minh</div>
               </div>

               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center'
               }}>
                   <div></div>
               </div>
   
               </div>
           </div>

        </div>
    )
}



export const prepareAcceptancePrepair = (item, currentDate) => {
    let itemContent = item.itemContent;
    let listItems = item.listItems;
    let itemStr = '';
    for (let record of listItems) {
        itemStr +=  record.itemName + '. \n';
    }
    // itemStr = itemStr.substring(0, itemStr.length - 1) + '.';
    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 20, paddingRight: 20,  }}>
               
       
        <div style={{display:'flex', flexDirection:'column', padding: 15}}>
                <div style={{display:'flex', flexDirection:'row', padding: 15}} >
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', fontSize: NORMAL_FONT }}>
                    <div style={{ flex: 1 }}>
                        <img src={"./images/logo.png"} width={120} />
                    </div>

                    <div style={{
                        flex: 3,
                        display: 'flex',
                        flexDirection: 'row',
                        height: 60,
                        justifyContent: 'center',
                        alignItems: 'center', padding: '15px 0px 0px 0px',
                        fontSize: NORMAL_FONT, fontWeight: "bold",
                        textAlign: 'center'
                
                    }}>
                        BIÊN BẢN NGHIỆM THU BẢO TRÌ, SỬA CHỮA THIẾT BỊ THÔNG DỤNG CƠ SỞ HẠ TẦNG
                    </div>

                    <div style={{ flex: 1, fontSize: SMALL_FONT }}>
                        <div style={{ marginLeft : 15 }}>Mã số: QT-SCCS/BM01</div>
                        <div style={{marginLeft : 15 }}>Lần ban hành: 03</div>
                        <div style={{ marginLeft : 15}}>Ngày hiệu lực: 18/3/2019</div>
                    </div>
                </div>
                </div>


                <div style={{width:'100%', display:'flex', alignItems:'right', justifyContent:'flex-end', marginTop: 10}}>
                                        Tp.HCM, ngày .... tháng .... năm .... 
                </div>    

                <div style={{width:'100%', display:'flex', alignItems:'center', marginTop: 10}}>
                    <div style={{flex :1,  marginRight: 100, display:'flex', alignItems:'center', justifyContent:'flex-end', marginTop: 10}}>
                            <div className={`checkbox-print`} style={{marginRight : 5}}></div>           Bảo trì
                    </div>   
                    <div style={{flex : 1, marginLeft: 100, display:'flex', alignItems:'center', justifyContent:'flex-start', marginTop: 10}}>
                    <div className={`checkbox-print`  } style={{marginRight : 5}}></div>         Sửa chữa 
                    </div>   
                </div>   


                <div style={{display:'flex', marginTop:30 , marginLeft:20}}>
                 -	Tên thiết bị: {itemStr}
                </div>


                <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
                - Khoa/ Phòng sử dụng:  {itemContent.departmentName}
                </div>
                
                <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
                -	Nội dung nghiệm thu:
                </div>


                <div style={{marginTop: 15, marginBottom:20, marginLeft:20}}>
                {getListAcceptance(listItems)}
                </div>
                
                <div style={{marginTop:PADDING_TOP, marginLeft:20}}>
                - Kết luận: 
                <div style={{display:'flex', marginLeft:20}}>
                    <div className={`checkbox-print ${itemContent.acceptanceResult == 1 ? "crossed" : ""} `} style={{marginLeft:5, marginRight:15, marginTop:5}}></div>
                    Đạt yêu cầu và đưa vào sử dụng
                </div>
                   
                <div style={{display:'flex' , marginLeft:20}}>
                 <div className={`checkbox-print ${itemContent.acceptanceResult == 2 ? "crossed" : ""} `} style={{marginLeft:5, marginRight:15, marginTop:5}}></div>
                   Không đạt yêu cầu
                </div>
              
                <div style={{display:'flex', marginLeft:20}}>
                    <div className={`checkbox-print ${itemContent.acceptanceResult == 3 ? "crossed" : ""} `} style={{marginLeft:5, marginRight:15, marginTop:5}}></div> 
                    Khác
                </div>
              
            </div>

            <div style={{display:'flex', marginTop:PADDING_TOP, marginLeft:20}}>
            -	Ý kiến của Khoa/ Phòng sử dụng:  {itemContent.acceptanceNote ? itemContent.acceptanceNote : ''}
            </div>
        
            <div style={{marginTop:30,display:'flex', flexDirection:'row',width:'100%', fontSize:NORMAL_FONT}}>
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems:'center',
               flexDirection: 'column'}}>
                   <div>Trưởng phòng</div>
               </div>

               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems:'center',
               flexDirection: 'column',
               }}>
                   <div>Khoa/ Phòng sử dụng</div>
               </div>     
                   
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
            marginTop:30,
            alignItems:'center',
               flexDirection: 'column',
               }}>
                   <div>Đơn vị thực hiện</div>
                   <div>(Nếu có)</div>
               </div>              
           </div>
        
           </div>

        </div>
    )
}

export const prepareDeliveryReceiptInternal = (item, currentDate) => {

    let itemContent = item.itemContent;
    let listItems = item.listItems;
    let totalCost = 0;
    for (let record of listItems) {
        totalCost += +record.totalPrice;
    }
    if (itemContent.isVAT) {
        totalCost = totalCost * (+itemContent.vatNumber + 100) / 100;
    }
    for (let location of AuditLocationArr) {
        if (itemContent.deliveryReceiptPlace)
            if (itemContent.deliveryReceiptPlace.toString() == location.value.toString()) {
                itemContent.deliveryReceiptPlace = location.label;
            }
    }


    return (
        <div style={{ display: 'flex', flexDirection: 'column', width: '250mm', fontSize: NORMAL_FONT, paddingLeft: 40, paddingRight: 40,  }}>
        <div style={{ display: 'flex', flexDirection: 'column', width: '100%',  }}>

         <div style={{display:'flex', flexDirection:'row', padding: 10}}>
             <div style={{ flex: 1,display:'flex',   flexDirection:'column', alignItems: 'center'}}>                   
                                        <div style={{display:'flex', alignItems:'center', justifyContent:'center'}}>
                                            BV. TRUYỀN MÁU HUYẾT HỌC
                                        </div>
                                        <div style={{display:'flex', alignItems:'center', justifyContent:'center', marginTop: 5}}>
                                            <b>    Phòng Hành Chánh Quản Trị </b>
                                        </div>
                                        <div style={{width:'50%',borderBottom:'1px solid  #000', marginTop: 15}}>  
                                        </div>
                                        <div style={{display:'flex', alignItems:'center', justifyContent:'center' ,marginTop: 10}}>                 
                                        </div>    
                                    </div>                    
                <div style={{ display: 'flex', flex: 1, flexDirection: 'column', alignItems: 'center' }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            <b> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </b>
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: '5px' }}>
                            <b> Độc lập – Tự do – Hạnh phúc </b>
                        </div>
                        <div style={{ width: '50%', borderBottom: '1px solid  #000', marginTop: 15 }}> 
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: '5px' }}>
                            TP. Hồ Chí Minh, ngày......tháng......năm {new Date().getFullYear}
                        </div>
                   
                </div>
            </div>

            <div style={{display:'flex' , width:'100%' , marginTop: 30, justifyContent:'center', alignItems:'center', fontSize:TILTLE_FONT }}>
                <b>BIÊN BẢN GIAO NHẬN</b>
            </div>
                    
            <div style={{ marginTop:PADDING_TOP * 4, marginLeft:20}}>
                Hôm nay, ngày........tháng........năm {new Date().getFullYear} tại Bệnh viện Truyền Máu Huyết Học – 201 Phạm Viết Chánh – P. Nguyễn Cư Trinh – Quận 1 – Tp. HCM. Tiến hành giao nhận các mặt hàng sau: 
            </div>          
         
            
            <div style={{ marginTop:PADDING_TOP * 2 , marginLeft:20 , fontSize:HEADER_FONT, fontWeight:'bold'}}>
                Bên giao: Phòng/Bang {itemContent.curDepartmentName}
            </div>
            <div style={{ marginTop:PADDING_TOP  , marginLeft:20}}>
                Đại diện bên giao:
            </div>
            <div style={{ marginTop:PADDING_TOP * 2 , marginLeft:20, fontWeight:'bold'}}>
                Bên nhận:  Phòng/Bang {itemContent.departmentName}
            </div>
            <div style={{ marginTop:PADDING_TOP  , marginLeft:20}}>
                Đại diện bên giao:
            </div>
            <div style={{ marginTop:PADDING_TOP * 2 , marginLeft:20}}>
                Phòng/Bang {itemContent.curDepartmentName} giao các mặt hàng như sau:
            </div>
          
            <div style={{marginTop: 15, marginBottom:40}}>
                {getListQuoteInternal(listItems)} 
            </div>  

            <div style={{ marginTop:PADDING_TOP * 2 , marginLeft:20}}>
                Các mặt hàng trên mới 100%, chưa qua sử dụng.   
            </div>

            <div style={{marginTop:40, display:'flex', flexDirection:'row',width:'100%', fontSize:NORMAL_FONT}}>             
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center', 
               flexDirection: 'column'}}>
                   <div>Bên nhận</div>                 
               </div>
   
               <div style={{flex:1,
               display: 'flex',
               justifyContent: 'center',
               alignItems: 'center' ,
               flexDirection: 'column'
               }}>
               <div>Bên giao</div>
              
   
           </div>
           </div>
    </div>
    </div>
    )
}

function getListQuoteInternal(listItems) {

    return (
        <React.Fragment>
         <div className="childDetailWarpper" style={{ marginTop: 20 }}>
         <div className="addItemWrapper listItemWrapper">
                <div className="listItemHeader" style={{ display: 'flex' }}>
                    <div  className="noWrap" style={{flex:1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>STT</div>
                    <div className="noWrap" style={{ flex:10, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>TÊN SẢN PHẨM</div>
                    <div className="noWrap" style={{ flex:2, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>ĐVT</div>
                    <div className="noWrap" style={{ flex:2, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>SL</div>
                </div>
                {listItems.map((item,index) => {
                    return  (
                    <div className="listItemRow" style={{ display: 'flex' }}>
                        <div className="noWrap" style={{ flex:1, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>{index+1}</div>
                        <div className="noWrap" style={{ flex:10, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>{item.itemName}</div>
                        <div className="noWrap"  style={{flex:2, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>{item.itemUnit}</div>
                        <div className="noWrap" style={{  flex:2, textAlign: 'center', borderRight: '1px solid  #ddd', padding: 5 }}>{item.amount}</div>
                                    </div>
                    )}
                    )
                 }
            </div>
            </div>
        </React.Fragment>
    )
}
