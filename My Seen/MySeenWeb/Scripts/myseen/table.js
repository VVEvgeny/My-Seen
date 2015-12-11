/*!
 * Заголовок таблицы всегда сверху
 * 
 * Важно .table.table-striped
 */

+function ($) {
    $(function () {
        var $table = $('.table.table-striped');
        var $thead = $table.find('thead');
        var $ths = $thead.find('tr th');
        var offsetTop = $thead.offset().top;
        //console.log("1 offsetTop=", offsetTop);

        $(window).on('scroll', function () {

            var thWidths = [];

            $ths.each(function (index, element) {
                var $th = $(element);

                thWidths.push($th.width());
            });

            var scrollTop = $(window).scrollTop();
            //console.log("offsetTop=", offsetTop);
            //console.log("scrollTop=", scrollTop);

            if (scrollTop > offsetTop) {
                $thead.css({ 'position': 'fixed', 'top': '50px' });

                var $firstTr = $table.find('tbody tr:first');

                $ths.each(function (index, element) {
                    var $th = $(element);

                    $th.width(thWidths[index]);
                    $firstTr.find('td').eq(index).width(thWidths[index]);
                });
            } else {
                $thead.css({ 'position': 'relative', 'top': 'auto' });
            }
        });
    });
}(jQuery);


