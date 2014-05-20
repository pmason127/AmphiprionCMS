PageManagerApplication.module('PageManager', function (PageManager, App, Backbone, Marionette, $, _) {
    var confirmNavigation = function() {
        var ask = confirm("Are you sure you wish to navigate away from this page?  Changes will not be saved.");
        return ask;
    };
    PageManager.Router = Marionette.AppRouter.extend({
        appRoutes: {
            '': 'showPageHierarchy',
            ':id': 'showOptions',
            ':parentid/add': 'addPage',
            ':id/edit': 'editPage',
        }
    });

    //This notifies the world that the current contextual page has changed
    var changeState = function(id) {
        App.currentPageId = id;
        App.trigger("current:page:statechange", id);
    };

    var API = {        
        showPageHierarchy:function() {
            PageManager.PageList.Controller.loadPageHierarchy(null, true);
            changeState(null);
        },
        showOptions:function(id) {
            PageManager.PageList.Controller.loadPageHierarchy(null, false);
            PageManager.Page.Controller.showPageOptions(id);
            changeState(id);
        },
        addPage:function(parentId) {
            PageManager.PageList.Controller.loadPageHierarchy(null, false);
            PageManager.Page.Controller.addPage(parentId);
            changeState(parentId);
        },
        editPage: function (id) {
            PageManager.PageList.Controller.loadPageHierarchy(null, false);
            PageManager.Page.Controller.editPage(id);
            changeState(id);
        }
    };
    //provides a medium to get the current pageId
    App.reqres.setHandler('page:current', function() {
        if (App.currentPageId)
            return App.currentPageId;
        
        return null;
    });
    App.on("hashchange", function() {
        alert('hashed');
    });
    App.on("page:show", function(id) {
        App.navigate(id);
        API.showOptions(id);
    });
    
    App.on("page:add", function (id) {
        App.navigate(id + "/add");
        API.addPage(id);
    });
    App.on("page:edit", function (id) {
        App.navigate(id + "/edit");
        API.editPage(id);
    });
    
    App.addInitializer(function () {
      var router =  new PageManager.Router({
            controller: API
      });
        API.showPageHierarchy();
    });
});