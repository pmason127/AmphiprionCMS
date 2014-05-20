PageManagerApplication.module("PageManager.PageList", function (List, App, Backbone, Marionette, $, _) {

    var _pages = null;
    var _view = null;

    var isHierarchyLoaded = function() {
        if (_pages == null || _.isUndefined(App.main.currentView || _view == null))
            return false;

        return true;
    };

    List.Controller = {

            loadPageHierarchy:function(parentId,forceRefresh) {

                if (isHierarchyLoaded() && !forceRefresh)
                    return;
                
                var pagePromise  = App.request("page:hierarchy");
                
                $.when(pagePromise).done(function (pages) {
                    _pages = pages;
                    _view = new App.PageManager.Views.TreeRoot({
                        collection: _pages
                    });
                    

                    App.sidebar.show(_view);
                });
            }
        
    };
});