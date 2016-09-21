$(function () {
    $('#menu').metisMenu();
});

(function ($) {
    $(document).ready(function () {

        var $this = $('#menu'),
          resizeTimer,
          self = this;

        var initCollapse = function (el) {
            if ($(window).width() >= 768) {
                this.find('li').has('ul').children('a').off('click');
            }
        };

        $(window).resize(function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(self.initCollapse($this), 250);
        });

    });
})(jQuery);

$(document).on('click', function (event) {
    var container = $("#menu");
    if (!container.is(event.target)
        && container.has(event.target).length === 0) {
        $("li").removeClass("active");
        $("ul").removeClass("in");
        $("li").attr("aria-expanded", "false");
    }
});
