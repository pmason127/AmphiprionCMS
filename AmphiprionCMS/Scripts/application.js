(function ($, global,doc) {
    $.amphiprion = $.amphiprion || {};

    var backdrop = $('<div class="backdrop" id="backdrop"></div>'),
    processing = $('<div  class="backdrop-spinner"></div>');
    $.amphiprion.globals = {
        setTZCookie: function (options) {
            if (!options.tzOffsetSeconds)
                return;
            
            var path = "/";
            if (options.path)
                path = options.path;
            var data = "tzOffset=" + options.tzOffsetSeconds + ";path=" + options.path;
            doc.cookie = data;
        },
        showWait: function () {
            $("input[type='submit']").prop("disabled", true);
            $("body").prepend(processing);
            $("body").prepend(backdrop);
        },
        hideWait: function () {
            processing.remove();
            backdrop.remove();
            $("input[type='submit']").prop("disabled", false);
        }
    };
})(jQuery, window,document);