$(function () {
    var $body = $('body');

    var $MENU_SIDE = $('#MENU_SIDE >.menu');
    var $SITE_CONTENT = $('#SITE_CONTENT');
    var $CONTENT_BLOCK = $('#CONTENT_BLOCK');
    var $MENU_TRIGGER = $('#MENU_TRIGGER');

    var class_SmallHEADER = 'header-sm';
    var class_SideNAVBAR = 'navbar-side';

    var Header_Preparing = function () {
        var nPositionY_Scroll = $(window).scrollTop();
        if (nPositionY_Scroll > 100) $SITE_CONTENT.addClass(class_SmallHEADER);
        else $SITE_CONTENT.removeClass(class_SmallHEADER);
    };

    var MenuSide = {
        nLevelCurrent: 0,
        Next: function () {
            this.nLevelCurrent += 1;
            this.TranslateX();
        },
        Previous: function (isToFirst) {
            isToFirst = Boolean(isToFirst);
            this.nLevelCurrent -= (isToFirst ? this.nLevelCurrent : 1);
            this.TranslateX();
            if (isToFirst) $MENU_SIDE.find('a.menu-view').removeClass('menu-view');
        },
        TranslateX: function () {
            var nRangeTrans = this.nLevelCurrent * -100; //Percentage (%)
            $MENU_SIDE.css('-webkit-transform', 'translateX(' + nRangeTrans + '%)');
            $MENU_SIDE.css('transform', 'translateX(' + nRangeTrans + '%)');
        }
    };
    $MENU_SIDE
        .delegate('li.has-children > a', 'click', function (e) {
            $(this).addClass('menu-view');
            MenuSide.Next();
            e.preventDefault();
        })
        .delegate('a.link-back', 'click', function (e) {
            MenuSide.Previous();
            $(this).parents('ul.menu-sub:first').prev('a').removeClass('menu-view');
            e.preventDefault();
        });
    $MENU_TRIGGER.on('click', function (e) { $body.addClass(class_SideNAVBAR); e.preventDefault(); });
    $CONTENT_BLOCK.on('click', function (e) { $body.removeClass(class_SideNAVBAR); MenuSide.Previous(true); e.preventDefault(); });

    window.addEventListener('scroll', Header_Preparing);
    window.addEventListener('resize', Header_Preparing);

    Header_Preparing();
});
