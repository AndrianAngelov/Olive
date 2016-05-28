/// <reference path="External/jquery-2.1.0.js" />
/// <reference path="External/jquery-2.1.0-vsdoc.js" />

$(document).ready(function () {

    $(".childrenCategoriesContainer").on('mouseover', function (event) {
        $(this).parent().parent().css({
            'background': '#E87162'
        });
    });

    $(".childrenCategoriesContainer").on('mouseleave', function (event) {
        $(this).parent().parent().css({
            'background': 'rgba(232,113,98,0)'
        });
    });
});
