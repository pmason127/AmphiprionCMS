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
        },
        renderModelErrors:function(errors, targetId) {
                if (!errors || errors.length <= 0)
                    return;
                var target = $("#" + targetId);
                target.html('');
                var ul = $("<ul></ul>");
                target.append(ul);
                $.each(errors, function (index, value) {
                    var li = $("<li></li>");
                    ul.append(li);
                    if (value.key === '') {
                        li.append("<span>" + 'Errors:' + '</span>');
                    } else {
                        li.append("<span>" + value.key + '</span>');
                    }
                    var inUl = $("<ul></ul>");
                    li.append(inUl);
                    $.each(value.errors, function(i, val) {
                        var inLi = $("<li></li>");
                        inUl.append(inLi);
                        inLi.text(val);
                    });
                });
                target.show();
        },
        clearModelErrors:function(targetId) {
            $("#"+targetId).hide().html('');
        },
        getQueryStringParm:function(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    };
})(jQuery, window,document);