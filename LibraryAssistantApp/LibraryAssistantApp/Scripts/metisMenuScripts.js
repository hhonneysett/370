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

