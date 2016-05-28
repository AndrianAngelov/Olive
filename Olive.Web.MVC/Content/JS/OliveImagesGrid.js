/* File Created: октомври 19, 2014 */
 
function SearchRecipesList(searchString) {
    var pagingButton = $("a.searchRecipesList.pagingButton");
    var containerForAjax = $("#allRecipesList article");
    //var containerForAjax = $("#allRecipesList");
    var actionURL = '/Recipes/Search/'+searchString;
    //var actionURL = '/Рецепти/Всички';

    AjaxGetRecipesListCurrentPage(actionURL, containerForAjax, pagingButton);
    
};
     
function GetAllRecipesList() {
    var pagingButton = $("a.allRecipesList.pagingButton");
    var containerForAjax = $("#allRecipesList article");
    //var containerForAjax = $("#allRecipesList");
    var actionURL = '/Recipes/List';
    //var actionURL = '/Рецепти/Всички';

    AjaxGetRecipesListCurrentPage(actionURL, containerForAjax, pagingButton);
    
};

function GetRecipesByCategory(currentCategoryID,categoryName) {
    var pagingButton = $("a.recipesByCategory.pagingButton");
    var containerForAjax = $("#allRecipesList article");
    var actionURL = '/Recipes/RecipesByCategory/'+currentCategoryID+"/true";
    //var actionURL = Utf8.dencode('/Рецепти/Категория'+2);

    AjaxGetRecipesListCurrentPage(actionURL, containerForAjax, pagingButton);
    
};

function GetRecipesListRosiRecommedataionCurrentPage() {
    var pagingButton = $("#recipesByRosiRecommedation a.pagingButton");
    var containerForAjax = $("#recipesByRosiRecommedation article");
    var actionURL = '/Home/RecipesByRosiRecommendation';

    AjaxGetRecipesListCurrentPage(actionURL, containerForAjax, pagingButton);
    
};

function GetRecipesListRatingCurrentPage() {
    var pagingButton = $("#recipesByRating a.pagingButton");
    var containerForAjax = $("#recipesByRating article");
    var actionURL = '/Home/RecipesByRating';

    AjaxGetRecipesListCurrentPage(actionURL, containerForAjax, pagingButton);
};

function GetRecipesListHitsCurrentPage() {
    var pagingButton = $("#recipesByNubmerOfHits a.pagingButton");
    var containerForAjax = $("#recipesByNubmerOfHits article");
    var actionURL = '/Home/RecipesByNubmerOfHits';

    AjaxGetRecipesListCurrentPage(actionURL, containerForAjax, pagingButton);
};

function AjaxGetRecipesListCurrentPage(actionURL, containerForAjax, pagingButton) {
    pagingButton.click(function (event) {
        var pageNumber = $(this).text();

        //setStyleToCurrentPageButton($(this), pagingButton);
        //clearNonCurrentPageButtons(pagingButton, $(this));
        clearNonCurrentPageButtons(pagingButton, setStyleToCurrentPageButton($(this), pagingButton, containerForAjax));
        if (pageNumber == "<<") {
            pageNumber = 1;
        }
        if (pageNumber == ">>") {
            pageNumber = containerForAjax.parent().find('footer #numberOfPages').val();
        }
        if (pageNumber == "<") {
            pageNumber = containerForAjax.parent().find('footer #currentPage').val();
        }
        if (pageNumber == ">") {
            pageNumber = containerForAjax.parent().find('footer #currentPage').val();
        }
//        $.get(actionURL, { id: pageNumber }, function (data) {
//            containerForAjax.html(data);
//            //PagingButtonsHover();
//            RecipesItemsHover();

//        });
        if (actionURL==='/Recipes/List') {
            $.get(actionURL+'/'+pageNumber+'/true', function (data) {
            containerForAjax.html(data);
            RecipesItemsHover();
            PagingButtonsHover();
        });
        }
        else {
            $.get(actionURL+'/'+pageNumber, function (data) {
            containerForAjax.html(data);
            RecipesItemsHover();
            PagingButtonsHover();
        });
        }
        
    });
};

function setStyleToCurrentPageButton(pagingButton, allButtons, containerForAjax) {
     switch(pagingButton.text()) {
         case "<<":
             var firstPageButton = allButtons.eq(2);
             firstPageButton.addClass("currentPageButton");
             firstPageButton.parent().css({
                 background: '#E87162',
                 color: 'white'
             });
             firstPageButton.css({
                 color: 'white'
             });

             return firstPageButton;
             break;
         case ">>":
             var lastPageButton = allButtons.eq(allButtons.length - 3);
             var currentPage = containerForAjax.parent().find('footer #currentPage');
             var pageNumber = containerForAjax.parent().find('footer #numberOfPages').val();
             
             currentPage.val(pageNumber);

             
             lastPageButton.parent().css({
                 background: '#E87162',
             });
             lastPageButton.css({
                 color: 'white'
             });

             lastPageButton.addClass("currentPageButton");
             return lastPageButton;
             break;
         case "<":
             var currentPage = containerForAjax.parent().find('footer #currentPage');
             var pageNumber = containerForAjax.parent().find('footer #numberOfPages').val();
//             alert(currentPage.val());
             if (currentPage.val() > 1 && pageNumber>1) {
                 currentPage.val(currentPage.val() - 1);
//                 currentPage.val(currentPage.val());
             }
             console.log(currentPage.val());
            if (pageNumber!=1) {
                var currentPageButton = allButtons.eq(currentPage.val()-pageNumber+3);
                currentPageButton.parent().css({
                 background: '#E87162',
                 color: 'white'
                });
                currentPageButton.css({
                    color: 'white'
                });
             currentPageButton.addClass("currentPageButton");
             } 
             return currentPageButton;
             break;
        case ">":
             var currentPage = containerForAjax.parent().find('footer #currentPage');
             var pageNumber = containerForAjax.parent().find('footer #numberOfPages').val();
            
             if (currentPage.val() <= pageNumber-1 && pageNumber>1) {
                 currentPage.val(parseInt(currentPage.val()) + 1);
             }
             if (pageNumber!=1) {
               var currentPageButton = allButtons.eq(currentPage.val()-pageNumber+3);

                currentPageButton.parent().css({
                     background: '#E87162',
                     color: 'white'
                });
                currentPageButton.css({
                     color: 'white'
                });
                currentPageButton.addClass("currentPageButton");
            } 
//             currentPageButton.parent().css({
//                 background: '#E87162',
//                 color: 'white'
//             });
//             currentPageButton.css({
//                 color: 'white'
//             });
//             currentPageButton.addClass("currentPageButton");

             return currentPageButton;
             break;
        default:
            var currentPage = containerForAjax.parent().find('footer #currentPage');
            currentPage.val(pagingButton.text());
            pagingButton.parent().css({
                background: '#E87162'
            });
            pagingButton.css({
                color: 'white'
            });
            pagingButton.addClass("currentPageButton");
            return pagingButton;
    }
};

function clearNonCurrentPageButtons(pagingButtons, currentpageButton) {
    if (currentpageButton!=undefined) {

        pagingButtons.each(function () {
            if ($(this).hasClass("currentPageButton")) {
                $(this).removeClass("currentPageButton");
            }
        });

        currentpageButton.addClass("currentPageButton");

        pagingButtons.each(function () {
            if (!($(this).hasClass("currentPageButton"))) {
                $(this).parent().css({
                    background: '#86b695'
                });
                $(this).css({
                    color: '#5F5E5D'
                });
            }
        });
    }
};


function RecipesItemsHover() {

    //Enter in recipeItem element
    //
    $(".recipeItemContainer").mouseover(function () {
        var aTag = $(this).children()
        aTag.children('.recipeItemInnerContainer').css({
            'color': '#ffffff',
            'visibility': 'visible',
            'background-color': 'rgba(40,40,40,0.75)'
        });
        aTag.children('.recipeItemTitleTextContainer').css({
            //'color': '#DCE1CB'
            //'color': 'rgba(134,182,149,0.8)'
            //'color':'rgba(233,233,233,0.85)'
            'visibility': 'hidden'
        });
//        aTag.children('.recipeItemImage').css({
//           '-ms-transform': 'scale(1.2,1.2)',
//		    '-webkit-transform': 'scale(1.2,1.2)',
//		    'transform': 'scale(1.2,1.2)'
//        });
    });

    //Leave recipeItem element
    //
    $(".recipeItemContainer").mouseleave(function () {
        var aTag = $(this).children()
        aTag.children('.recipeItemInnerContainer').css({
            'visibility': 'hidden'
        });
        //$(this).stop().animate({transform:'scale(2)'}, 3000);
        aTag.children('.recipeItemTitleTextContainer').css({
//            'color': '#5F5E5D'
            //'color': '#404040'
            'visibility': 'visible'
        });
//        aTag.children('.recipeItemImage').css({
//           '-ms-transform': 'scale(1,1)',
//		    '-webkit-transform': 'scale(1,1)',
//		    'transform': 'scale(1,1)'
//        });
    });

    //Hover motion efect recipeItem element
    //
    $(function () {
        $('ul.da-thumbs > li').hoverdir();
    });
};

function PagingButtonsHover() {
    $(".round-button-circle").mouseover(function () {
        $(this).css({
            background:'#E87162'
        });
    });
    $(".round-button-circle a").mouseover(function () {
            $(this).css({
                color:'#fff'
            });
            if ($(this).hasClass("currentPageButton")) {
                 $(this).parent().css({
                    background:'#E87162'
                });
            }
    });
    $(".round-button-circle").mouseleave(function () {
        if (!($(this).children().hasClass("currentPageButton"))) {
            $(this).css({
                background: '#86b695'
            });
        }
    });
    $(".round-button-circle a").mouseleave(function () {
        if (!($(this).hasClass("currentPageButton"))) {
            $(this).css({
                color: '#5F5E5D'
            });
        }
    });
};

function SetDefaultStartPages(){
    $(".round-button-circle").eq(2).css({
            background:'#E87162'
        });
    $(".round-button-circle").eq(2).children().css({
            color:'#fff'
    });
    $(".round-button-circle").eq(2).children().addClass("currentPageButton");

    //RecipesByRating
    $("#recipesByRating .round-button-circle").eq(2).css({
            background:'#E87162'
        });
    $("#recipesByRating .round-button-circle").eq(2).children().css({
            color:'#fff'
    });
    $("#recipesByRating .round-button-circle").eq(2).children().addClass("currentPageButton");

    //RecipesByNubmerOfHits
    $("#recipesByNubmerOfHits .round-button-circle").eq(2).css({
            background:'#E87162'
    });
    $("#recipesByNubmerOfHits .round-button-circle").eq(2).children().css({
            color:'#fff'
    });
    $("#recipesByNubmerOfHits .round-button-circle").eq(2).children().addClass("currentPageButton");
};


//#region UTF8

var Utf8 = {

    // public method for url encoding
    encode : function (string) {
        string = string.replace(/rn/g,"n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },

    // public method for url decoding
    decode : function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while ( i < utftext.length ) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i+1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i+1);
                c3 = utftext.charCodeAt(i+2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }

}

//#endregion

