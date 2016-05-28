/// <reference path="External/jquery-2.1.0.js" />
/// <reference path="External/jquery-2.1.0-vsdoc.js" />


function setCurrentCategoryStyle() {
    var currentCategoryID = $("#currentCategoryID").val();
    var currentAnchor = $("#categoriesMenuInnerContainer li#" + currentCategoryID + ' a');
    currentAnchor.css({
        //            'color': '#5F5E5D'
        //'color': '#404040'
        'color': 'red'
    });
};



$(document).ready(function () {

    function setCurrentMainCategoryStyle(element) {
        var allMainCategories = $("li.mainCategories");
        allMainCategories.each(function () {
            if ($(this) != element) {
                $(this).css({
//                    'background-color': '#E6E7DB',
                    'border': 'dotted #81987c 2px'
                });
            }
            var chidrenLiCollection = $(this).siblings("li").children("ul").children("li");
            chidrenLiCollection.hide();
        });
        element.css({
//            'background-color': '#81987c',
            'border': 'solid #5F5E5D 1px'
        });
        var chidrenLiCollection = $(this).siblings("li").children("ul").children("li");
        chidrenLiCollection.show();
    };

    var currentCategoryID = $("#currentCategoryID").val();
    var currentAnchor = $("#categoriesMenuInnerContainer li#" + currentCategoryID + ' a');
    currentAnchor.css({
        //            'color': '#5F5E5D'
        //'color': '#404040'
        'color': 'red'
    });

    var allLi = $(".childrenCategoriesContainer li");
    allLi.hide();

    $("li.mainCategories").on('click', function (event) {
        //event.preventDefault();
        setCurrentMainCategoryStyle($(this));

        var chidrenLiCollection = $(this).siblings("li").children("ul").children("li");

        var parentWith = $(this).parent().outerWidth();
        var parentHalfWidth = parentWith / 2;

        var firstElWidth = chidrenLiCollection.eq(0).width();
        var firstElHalfWidth = parentHalfWidth - firstElWidth / 2;

        var startPointPY = 100;
        var startPointPX = firstElHalfWidth;

        var oddTopVal = startPointPY;
        var oddLeftVal = 0;

        var evenTopVal = startPointPY;
        var evenLeftVal = startPointPX + firstElWidth;
        var counter = 0;

        chidrenLiCollection.each(function () {
            $(this).show();

            if (counter == 0) {
                $(this).animate({
                    "top": startPointPY,
                    "left": startPointPX
                }, 200);
            }
            if (counter != 0 && counter % 2 != 0) {
                var currentElHalfWidth = $(this).width() / 2;
                if (counter != 1) {
                    evenLeftVal += 20;
                }
                evenTopVal -= 20;

                $(this).animate({
                    "top": evenTopVal,
                    "left": evenLeftVal
                }, 200);
            }
            if (counter != 0 && counter % 2 == 0) {
                var currentElHalfWidth = $(this).width() / 2;
                oddLeftVal = startPointPX - currentElHalfWidth - 40;
                oddTopVal -= 20;

                $(this).animate({
                    "top": oddTopVal,
                    "left": oddLeftVal
                }, 200);
            }

            counter++;
            if (counter % 2 == 0) {

            }
            topVal = 100;
            leftVal = firstElHalfWidth;
        });
        //return false;
    });

});


//$(document).ready(function () {
//    for (var i = 1; i < 5; i++) {
//        var pos = -411 * i,
//            pospx = { 'background-position-x': pos };

//        $("#newsPix").delay(2000).animate(pospx, 1000);
//    }
//});

//lis.animate({
//    width: "70%",
//    opacity: 0.4,
//    marginLeft: "0.6in",
//    fontSize: "3em",
//    borderWidth: "10px",
//    visibility: "visible"
//}, 1500);

//    $(".trigger").click(function () {
//        $(".menu").toggleClass("active");
//    });