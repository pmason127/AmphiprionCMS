(function ($, global) {
    $.amphiprion = $.amphiprion || {};

    var attachHandlers = function (options) {
        $("a.page-item",options.wrapper).click(function(e) {
            options.editor.html('Editing:' + $(this).data("path"));
        });
        $("a.add-page", options.wrapper).click(function (e) {
            var id = $(this).data("id");
            var path = $(this).data("path");
            options.editor.html('');
            $.ajax({
                url: options.baseAddUrl,
                data: { id: id },
                success: function(data) {
                    options.editor.html(data);
                    setSelectedState(path, options);
                },
                dataType: 'html'
            });
        });
        $(".edit-row", options.wrapper).hover(function (e) {
            $(this).addClass("edit-row-hover");
            $("a.add-page", $(this)).show();
        },
        function(e) {
            $("a.add-page", $(this)).hide();
            $(this).removeClass("edit-row-hover");
        });
        $("a.tree-toggle", options.wrapper).click(function (e) {
            var $this = $(this);
            $(this).parent().next("ul.page-subtree").slideToggle(function () {
                toggleIndicator($this, $(this).is(':visible'));
            });
        });
        $("#" + options.resetLinkId, options.wrapper).click(function () {
            resetTree(options);
        });

    },
    toggleIndicator = function(link,isOpen) {
        if (isOpen) {
            link.removeClass("icon-arrow-right6");
            link.addClass("icon-arrow-down6");
        } else {
            link.removeClass("icon-arrow-down6");
            link.addClass("icon-arrow-right6");
        }
    },
    resetTree = function (options) {
        $("ul.page-subtree", options.wrapper).slideToggle(function () {
            $.each($("a.tree-toggle",options.wrapper), function(index, value) {
                toggleIndicator($(value), false);
            });
        });
       
    },
    setSelectedState = function(path, options) {
        $(".edit-row-selected",options.wrapper).removeClass("edit-row-selected");
        //$.each(elems, function(i, v) {
        //    $(v)
        //});
        $('ul', options.wrapper).find("[data-path='" + path + "']").parent().addClass("edit-row-selected");
    };
   

    $.amphiprion.tree = {
        register: function (options) {
            attachHandlers(options);

        }
    };
})(jQuery, window);