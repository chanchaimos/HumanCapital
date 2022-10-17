var EasyPaging_OnFormat = function (type) {
    var nVal = this.value
    var isActive = this.active;
    var $pagItem = $('<li>', { 'class': 'page-item' });
    var $pagLink = $('<a>', { 'href': '#', 'class': 'page-link' });
    var isEmpty = false;

    switch (type) {
        case 'block': /*ตัวเลข*/ {
            $pagLink.html(nVal);
            if (!isActive) $pagItem.addClass('disabled');
            else if (nVal != this.page) $pagLink.attr('href', '#' + nVal);
            else $pagItem.addClass('active');
        } break;
        case 'right': /*ขวาสุด*/
        case 'left': /*ซ้ายสุด*/ {
            if (!isActive) isEmpty = true;
            else $pagLink.html(nVal).attr('href', '#' + nVal);
        } break;
        case 'next': /*ปุ่มถัดไป*/
        case 'prev': /*ปุ่มก่อนหน้า*/
        case 'first': /*ปุ่มกลับไปหน้าแรก*/
        case 'last': /*ปุ่มไปหน้าสุดท้าย*/ {
            switch (type) {
                case 'next': $pagLink.html('&#8202;<i class="fa fa-angle-right"></i>&#8202;'); break;
                case 'prev': $pagLink.html('&#8202;<i class="fa fa-angle-left"></i>&#8202;'); break;
                case 'first': $pagLink.html('&#8202;<i class="fa fa-angle-double-left"></i>&#8202;'); break;
                case 'last': $pagLink.html('&#8202;<i class="fa fa-angle-double-right"></i>&#8202;'); break;
            }

            if (isActive) $pagLink.addClass(type).attr('href', '#' + nVal);
            else $pagItem.addClass('disabled');
        } break;
        case 'fill': /*ช่วงที่ข้ามไป*/{
            if (isActive) {
                $pagItem.addClass('disabled');
                $pagLink.html('...');
            }
            else isEmpty = true;
        } break;
        default: { isEmpty = true; } break;
    }

    return (isEmpty ? '' : $pagItem.html($pagLink).prop('outerHTML'));
};