var ChuSo = [" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín "];
var Tien = ["", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ"];

//1. Hàm đọc số có ba chữ số;
function DocSo3ChuSo(baso ) {
  var tram;
  var chuc;
  var donvi;
  var KetQua = "";
  tram = parseInt((baso / 100).toString());
  chuc = parseInt(((baso % 100) / 10).toString());
  donvi = baso % 10;
  if (tram ===0 && chuc ===0 && donvi ===0) return "";
  if (tram !==0) {
    KetQua += ChuSo[tram] + " trăm ";
    if ((chuc ===0) && (donvi !==0)) KetQua += " linh ";
  }
  if ((chuc !==0) && (chuc !==1)) {
    KetQua += ChuSo[chuc] + " mươi";
    if ((chuc ===0) && (donvi !==0)) KetQua = KetQua + " linh ";
  }
  if (chuc ===1) KetQua += " mười ";
  switch (donvi) {
    case 1:
      if ((chuc !==0) && (chuc !==1)) {
        KetQua += " mốt ";
      }
      else {
        KetQua += ChuSo[donvi];
      }
      break;
    case 5:
      if (chuc ===0) {
        KetQua += ChuSo[donvi];
      }
      else {
        KetQua += " lăm ";
      }
      break;
    default:
      if (donvi !==0) {
        KetQua += ChuSo[donvi];
      }
      break;
  }
  return KetQua;
}

var mangso = ['không','một','hai','ba','bốn','năm','sáu','bảy','tám','chín'];
function dochangchuc(so,daydu)
{
 var chuoi = "";
 var chuc = Math.floor(so/10);
 var donvi = so%10;
 if (chuc>1) {
  chuoi = " " + mangso[chuc] + " mươi";
  if (donvi==1) {
   chuoi += " mốt";
  }
 } else if (chuc==1) {
  chuoi = " mười";
  if (donvi==1) {
   chuoi += " một";
  }
 } else if (daydu && donvi>0) {
  chuoi = " lẻ";
 }
 if (donvi==5 && chuc>1) {
  chuoi += " lăm";
 } else if (donvi>1||(donvi==1&&chuc==0)) {
  chuoi += " " + mangso[ donvi ];
 }
 return chuoi;
}
function docblock(so,daydu)
{
 var chuoi = "";
 var tram = Math.floor(so/100);
 so = so%100;
 if (daydu || tram>0) {
  chuoi = " " + mangso[tram] + " trăm";
  chuoi += dochangchuc(so,true);
 } else {
  chuoi = dochangchuc(so,false);
 }
 return chuoi;
}
function dochangtrieu(so,daydu)
{
 var chuoi = "";
 var trieu = Math.floor(so/1000000);
 so = so%1000000;
 if (trieu>0) {
  chuoi = docblock(trieu,daydu) + " triệu";
  daydu = true;
 }
 var nghin = Math.floor(so/1000);
 so = so%1000;
 if (nghin>0) {
  chuoi += docblock(nghin,daydu) + " nghìn";
  daydu = true;
 }
 if (so>0) {
  chuoi += docblock(so,daydu);
 }
 return chuoi;
}
function docso(so)
{
        if (so==0) return mangso[0];
 var chuoi = "", hauto = "";
 do {
  var ty = so%1000000000;
  so = Math.floor(so/1000000000);
  if (so>0) {
   chuoi = dochangtrieu(ty,true) + hauto + chuoi;
  } else {
   chuoi = dochangtrieu(ty,false) + hauto + chuoi;
  }
  hauto = " tỷ";
 } while (so>0);
 return chuoi;
}

export  const DocTienBangChu = (SoTien ) =>{
  return docso(SoTien);
  // var lan = 0;
  // var i = 0;
  // var so = 0;
  // var KetQua = "";
  // var tmp = "";
  // var ViTri = new Array();
  // if (SoTien < 0) return "Số tiền âm !";
  // if (SoTien ===0) return "không ";
  // if (SoTien > 0) {
  //   so = SoTien;
  // }
  // else {
  //   so = -SoTien;
  // }
  // if (SoTien > 8999999999999999) {
  //   //SoTien = 0;
  //   return "Số quá lớn!";
  // }
  // ViTri[5] = Math.floor(so / 1000000000000000);
  // if (isNaN(ViTri[5]))
  //   ViTri[5] = "0";
  // so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
  // ViTri[4] = Math.floor(so / 1000000000000);
  // if (isNaN(ViTri[4]))
  //   ViTri[4] = "0";
  // so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
  // ViTri[3] = Math.floor(so / 1000000000);
  // if (isNaN(ViTri[3]))
  //   ViTri[3] = "0";
  // so = so - parseFloat(ViTri[3].toString()) * 1000000000;
  // ViTri[2] = parseInt((so / 1000000).toString());
  // if (isNaN(ViTri[2]))
  //   ViTri[2] = "0";
  // ViTri[1] = parseInt(((so % 1000000) / 1000).toString());
  // if (isNaN(ViTri[1]))
  //   ViTri[1] = "0";
  // ViTri[0] = parseInt((so % 1000).toString());
  // if (isNaN(ViTri[0]))
  //   ViTri[0] = "0";
  // if (ViTri[5] > 0) {
  //   lan = 5;
  // }
  // else if (ViTri[4] > 0) {
  //   lan = 4;
  // }
  // else if (ViTri[3] > 0) {
  //   lan = 3;
  // }
  // else if (ViTri[2] > 0) {
  //   lan = 2;
  // }
  // else if (ViTri[1] > 0) {
  //   lan = 1;
  // }
  // else {
  //   lan = 0;
  // }
  // for (i = lan; i >= 0; i--) {
  //   tmp = DocSo3ChuSo(ViTri[i]);
  //   KetQua += tmp;
  //   if (ViTri[i] > 0) KetQua += Tien[i];
  //   if ((i > 0) && (tmp.length > 0)) KetQua += '';//&& (!string.IsNullOrEmpty(tmp))
  // }
  // if (KetQua.substring(KetQua.length - 1) ==='') {
  //   KetQua = KetQua.substring(0, KetQua.length - 1);
  // }
  // KetQua = KetQua.substring(1, 2).toLowerCase() + KetQua.substring(2);
  // KetQua = KetQua.replace(/  /g, ' ');
  // return KetQua;//.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
}

export  const stringToSlug  = function (str) {
  // remove accents
   
   str= str.replace(/Ð/g,"D");  

  return str.toUpperCase();
}

export const  formatNumber = function(n) {

  return n.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".")
}