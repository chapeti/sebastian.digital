;$(function () {
    $.fn.matchHeights = function() {
        var height = 0,
            thisHeight;

        this
            .height('auto')
            .each(function () {
                thisHeight = $(this).height()
                if (thisHeight > height) {
                    height = thisHeight;
                }
            })
            .height(height);

    };
});
