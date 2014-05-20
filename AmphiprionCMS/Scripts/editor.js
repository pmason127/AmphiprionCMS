(function ($, global) {
    $.amphiprion = $.amphiprion || {};


    $.amphiprion.editor = {
        register: function (options) {

            var edElement = $('#' + options.editorId);
            edElement.ckeditor(
            {
                filebrowserBrowseUrl: options.browserUrl,
                filebrowserUploadUrl: options.uploadUrl
            });
        }
    };
})(jQuery, window);