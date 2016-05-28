/// <reference path="../../../../Content/JS/External/jquery-2.1.0.js" />
/// <reference path="../../../../Content/JS/External/jquery-2.1.0-vsdoc.js" />
///// <reference path="../../../../Content/External/jquery-ui-1.11.2/jquery-ui.js" />

//var adminDialog = $('#adminDialog');

$(document).ready(function () {
    $('#adminDialog').dialog({
        autoOpen: false,
        width: 400,
        resizable: false,
        modal: true 
    });
});

var AdminAjax = (function () {
    var adminDialog = $('#adminDialog');
    //var ajaxEditButons = $(".ajaxEditBtn");
    //var ajaxDeleteButons = $(".ajaxDeleteBtn");

    function loadToDialog(element, actionURL, dialogTitle) {
        var id = element.siblings("#id").val();
        adminDialog.empty();
        $('#adminDialog').dialog({
            title: dialogTitle
        });
        if (dialogTitle == 'Промени категорията') {
            $('#adminDialog').dialog({
                width: 650
            });
        }
        else {
            $('#adminDialog').dialog({
                width: 400
            });
        }

        adminDialog.load(actionURL + id);
        adminDialog.dialog('open');

    };

    function _loadFormInEditDialog(actionURL, dialogTitle) {
        $(".ajaxEditBtn").on('click', function (event) {
            event.preventDefault();
            loadToDialog($(this), actionURL, dialogTitle);
            return false;
        })
    };

    function _loadFormInDeleteDialog(actionURL, dialogTitle) {
        $('.ajaxDeleteBtn').on('click', function (event) {
            event.preventDefault();
            loadToDialog($(this), actionURL, dialogTitle);
            return false;
        })
    };

    function _showSuccessMessageDialog(messageText) {
        var successMsbDialog = $('#adminDialogMessages');
        successMsbDialog.empty();
        successMsbDialog.dialog({
            autoOpen: false,
            width: 400,
            resizable: false,
            modal: true,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            },
            open: function () {
                var successMsg = messageText;
                $(this).html(successMsg);
                $(this).parent().find('button:nth-child(1)').blur();
            }
        });
        successMsbDialog.dialog('open');
    };

    return {
        LoadSouceInEditDialog: function (dialogTitle) {
            _loadFormInEditDialog("/Administration/Sources/Edit/", dialogTitle);
        }
        , LoadSouceInDeleteDialog: function (dialogTitle) {
            _loadFormInDeleteDialog("/Administration/Sources/Delete/", dialogTitle);
        }
        , LoadIngredientInEditDialog: function (dialogTitle) {
            _loadFormInEditDialog("/Administration/Ingredients/Edit/", dialogTitle);
        }
        , LoadIngredientInDeleteDialog: function (dialogTitle) {
            _loadFormInDeleteDialog("/Administration/Ingredients/Delete/", dialogTitle);
        }
        , LoadUnitInEditDialog: function (dialogTitle) {
            _loadFormInEditDialog("/Administration/Units/Edit/", dialogTitle);
        }
        , LoadUnitInDeleteDialog: function (dialogTitle) {
            _loadFormInDeleteDialog("/Administration/Units/Delete/", dialogTitle);
        }
        , LoadCategoryInEditDialog: function (dialogTitle) {
            _loadFormInEditDialog("/Administration/Categories/Edit/", dialogTitle);
        }
        , LoadCategoryInDeleteDialog: function (dialogTitle) {
            _loadFormInDeleteDialog("/Administration/Categories/Delete/", dialogTitle);
        }
        , ShowMessageDialog: function (messageText) {
            _showSuccessMessageDialog(messageText);
        }
    };
})();

//var adminDialog = $('#adminDialog');
//var ajaxDeleteButons = $(".ajaxDeleteBtn");
//var ajaxEditButons = $(".ajaxEditBtn");

//function loadFormInDialog(editOrDeleteMode, actionURL) {

//    $(editOrDeleteMode).click(function () {
//        console.log(editOrDeleteMode);
//        var id = $(this).siblings("#id").val();
//        adminDialog.load(actionURL + id)
//        adminDialog.dialog('open');
//    });
//};    

//var a=function(){
//    loadFormInDialog(ajaxEditButons, "/Administration/Sources/Edit/");
//} ;

//var b=function() {
//    loadFormInDialog(ajaxDeleteButons, "/Administration/Sources/Delete/");
//} ;

//#region Old Version Sources

//        $('.ajaxEditBtn').click(function () {
//            var id = $(this).parent().attr('id');
//            //alert(id);
//            $('#my-dialog').load("/Administration/Sources/Edit/" + id)
//            $('#my-dialog').dialog('open');
//        });
//#endregion

//#region Test

//    $(document).ready(function () {
//        $(".SiteName").on("click", function (event) {
//            event.preventDefault();
//            var siteName = $(this).data("sitename");
//            //alert(siteName);
//            $("#ListSite")
//           .load("/EtlLog/ListSiteJobs?SiteDescription=" + encodeURIComponent(siteName))
//           .dialog({ autoOpen: true })
//        });
//    })



//        $('#cssmenu > ul > li > a').click(function () {
//            $('#cssmenu li').removeClass('active');
//            $(this).closest('li').addClass('active');
//            var checkElement = $(this).next();
//            if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
//                $(this).closest('li').removeClass('active');
//                checkElement.slideUp('normal');
//            }
//            if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
//                $('#cssmenu ul ul:visible').slideUp('normal');
//                checkElement.slideDown('normal');
//            }
//            if ($(this).closest('li').find('ul').children().length == 0) {
//                return true;
//            } else {
//                return false;
//            }
//        });

//#endregion