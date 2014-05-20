PageManagerApplication.module("PageManager.Page", function (Page, App, Backbone, Marionette, $, _) {

    Page.Controller = {

            showPageOptions:function(id) {

     
                var pagePromise  = App.request("page:item",id);
                
                $.when(pagePromise).done(function (page) {
                   // var model = App.PageManager.Models.Page(page);
                    var vw = new App.PageManager.Views.OptionsView({
                        model: page
                    });
                    var id = page.get("Id");
                    vw.on("page:add", function(args) {
                        App.trigger("page:add", args.model.get("Id"));
                    });
                    vw.on("page:edit", function (args) {
                        App.trigger("page:edit", args.model.get("Id"));
                    });

                    App.main.show(vw);
                });
            },
            addPage:function(parentId) {
                var page = new App.PageManager.Models.Page();
                page.set("ParentId",parentId);

                var vw = new App.PageManager.Views.AddForm({ model: page });
            
                vw.on("form:submit", function (data) {
                    page.save(data);
                });
                App.main.show(vw);
            },
            editPage: function (id) {

                var pagePromise = App.request("page:item", id);

                $.when(pagePromise).done(function (page) {
                   // var editPage = new App.PageManager.Models.Page(page);
                    var vw = new App.PageManager.Views.EditForm({ model: page });

                    vw.on("form:submit", function (data) {
                       
                    });

                    App.main.show(vw);
                    
                });
            }
        
    };
});