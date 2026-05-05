function showLoading() {
    $("#tblLoading").show();
}
function ShowgvScanbar() {

    $("#tbl_scanBar").fadeIn();
    $("#tbl_scanBar1").css("transform", "scale(1)");
}
function closeShowScanbar() {
    $("#tbl_scanBar").fadeOut(200);
    ShowgvLrbarcodelog();

}

function ShowgvLrbarcodelog() {

    $("#tbl_BarcodeLog").fadeIn();
    $("#tbl_BarcodeLog1").css("transform", "scale(1)");
}
function closeShowBarcodeLog() {
    $("#tbl_BarcodeLog").fadeOut(200);
    ShowgvInHC();

}

function ShowgvInBaoHC() {

    $("#tbl_gvInBaoHC").fadeIn();
    $("#tbl_gvInBaoHC1").css("transform", "scale(1)");
}
function closeShowInBaoHC() {
    $("#tbl_gvInBaoHC").fadeOut(200);
    ShowgvOutHC();

}

function ShowgvInHC() {
    $("#tbl_gvInHC").fadeIn();
    $("#tbl_gvInHC1").css("transform", "scale(1)");
}
function closeShowHCIn() {
    $("#tbl_gvInHC").fadeOut(200);
}

function ShowgvOutHC() {
    $("#tbl_gvOutHC").fadeIn();
    $("#tbl_gvOutHC1").css("transform", "scale(1)");
}
function closeShowOutHC() {
    $("#tbl_gvOutHC").fadeOut(200);
}

function ShowgvEbe() {
    $("#tbl_gvEbe").fadeIn();
    $("#tbl_gvEbe1").css("transform", "scale(1)");
}
function closeShowgvEbe() {
    $("#tbl_gvEbe").fadeOut(200);
}

function ShowgvOut() {
    $("#tbl_gvOut").fadeIn();
    $("#tbl_gvOut1").css("transform", "scale(1)");
}
function closeShowgvOut() {
    $("#tbl_gvOut").fadeOut(200);
}

function ShowgvWeight() {
    $("#tbl_gvWeight").fadeIn();
    $("#tbl_gvWeight1").css("transform", "scale(1)");
}
function closeShowgvWeight() {
    $("#tbl_gvWeight").fadeOut(200);
}

function ShowgvInTem() {
    $("#tbl_gvInTem").fadeIn();
    $("#tbl_gvInTem1").css("transform", "scale(1)");
}
function closeShowgvInTem() {
    $("#tbl_gvInTem").fadeOut(200);
}

function ShowgvNguyenLieu() {
    $("#tbl_gvNguyenLieu").fadeIn();
    $("#tbl_gvNguyenLieu1").css("transform", "scale(1)");
}
function closeShowgvNguyenLieu() {
    $("#tbl_gvNguyenLieu").fadeOut(200);
}

function ShowgvDoNguoc() {
    $("#tbl_gvDoNguoc").fadeIn();
    $("#tbl_gvDoNguoc1").css("transform", "scale(1)");
}
function closeShowgvDoNguoc() {
    $("#tbl_gvDoNguoc").fadeOut(200);
    ShowgvNguyenLieu();
}


function ShowgvDoNguocRL() {

  
    $("#tbl_gvDoNguocRL").fadeIn();
    $("#tbl_gvDoNguocRL1").css("transform", "scale(1)");
}
function closeShowgvDoNguocRL() {
    $("#tbl_gvDoNguocRL").fadeOut(200);
    ShowgvNguyenLieu();
}

function ShowgvHC() {
    $("#tbl_gvHC").fadeIn();
    $("#tbl_gvHC1").css("transform", "scale(1)");
}
function closeShowgvHC() {
    $("#tbl_gvHC").fadeOut(200);
    ShowgvNguyenLieu();
}

function ShowgvBonHC() {
    $("#tbl_gvBonHC").fadeIn();
    $("#tbl_gvBonHC1").css("transform", "scale(1)");
}
function closeShowgvBonHC() {
    $("#tbl_gvBonHC").fadeOut(200);
    ShowgvHC();
}

function ShowgvSearch() {
    $("#tbl_gvSearch").fadeIn();
    $("#tbl_gvSearch1").css("transform", "scale(1)");
}
function closeShowgvSearch() {
    $("#tbl_gvSearch").fadeOut(200);
}

function ShowgvThemMoi() {
    $("#tbl_gvThemMoi").fadeIn();
    $("#tbl_gvThemMoi1").css("transform", "scale(1)");
}
function closeShowgvThemMoi() {
    $("#tbl_gvThemMoi").fadeOut(200);
}

function ShowgvSua() {
    $("#tbl_gvSua").fadeIn();
    $("#tbl_gvSua1").css("transform", "scale(1)");
}
function closeShowgvSua() {
    $("#tbl_gvSua").fadeOut(200);
}

function closeMessages() {
    $("#tblMessages").fadeOut(200);
}
function showMessage() {
    $("#tblMessages").fadeIn();
    $("#tblMessages1").css("transform", "scale(1)");
}

function closeMessagess() {
    $("#tblMessagess").fadeOut(200);
    ShowgvEbe();
    
}
function showMessages() {
    $("#tblMessagess").fadeIn();
    $("#tblMessagess1").css("transform", "scale(1)");
}


function ShowgvBarcoderep() {
    $("#tbl_gvbarcoderep").fadeIn();
    $("#tbl_gvbarcoderep1").css("transform", "scale(1)");
}
function closeShowgvBarcoderep() {
    $("#tbl_gvbarcoderep").fadeOut(200);
    ShowgvEbe();
}

function clickButton(e, buttonid) {
    var evt = e ? e : window.event;
    var bt = document.getElementById(buttonid);
    if (bt) {
        if (evt.keyCode == 13) {
            bt.click();
            return false;
        }
    }
}

function LiBlock() {
    
    $("#BB2").removeClass("Li_Hidden");
    $("#IT").removeClass("Li_Hidden");
    $("#BB").removeClass("Li_Hidden");
    $("#HCMoi").removeClass("Li_Hidden");
    $("#Xemplc").removeClass("Li_Hidden");
    $("#Xemplcnew").removeClass("Li_Hidden");
    $("#KeoRL").removeClass("Li_Hidden");
    $("#BB1").removeClass("Li_Hidden");
   
    $("#BB3").removeClass("Li_Hidden");
}

function LiBlockxemlieuPLC() {
    $("#Xemplcnew").removeClass("Li_Hidden");
    $("#KeoRL").removeClass("Li_Hidden");
    $("#BB1").removeClass("Li_Hidden");
 
    $("#BB3").removeClass("Li_Hidden");
}
function LiBlockduyen() {
    $("#KeoRL").removeClass("Li_Hidden");
    $("#BB1").removeClass("Li_Hidden");
    $("#BB").removeClass("Li_Hidden");
    $("#BB3").removeClass("Li_Hidden");


}
function LiBlockphong() {
    $("#KeoRL").removeClass("Li_Hidden");
}
function LiBlockbb() {
    $("#BB1").removeClass("Li_Hidden");
}

function LiBlockTn() {
    $("#BB3").removeClass("Li_Hidden");
}

$(document).ready(function () {
    $(".back").click(function (event) {
        event.preventDefault();
        history.back(1);
    });
});