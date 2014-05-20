(function ($, global,doc) {
    $.amphiprion = $.amphiprion || {};


    $.amphiprion.globals = {
        setTZCookie: function (options) {
            if (!options.tzOffsetSeconds)
                return;
            
            var path = "/";
            if (options.path)
                path = options.path;
            var data = "tzOffset=" + options.tzOffsetSeconds + ";path=" + options.path;
            doc.cookie = data;
        }
    };
})(jQuery, window,document);