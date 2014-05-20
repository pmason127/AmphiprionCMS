PageManagerApplication.module('PageManager.Models', function (Models, App, Backbone) {


    var pageValidation =
    {
        Title: {
            required: true,
            msg:"Title is required"
        },
        HtmlDescription: {
            required: true,
            msg:"Body is required"
        }
    };

    Models.Page = Backbone.Model.extend({
        urlRoot: '/amphiprion/api/v1/page',
        defaults: {
            Title: null,
            Id: null,
            HtmlDescription: null,
            PublishDateUtc: null,
            IsApproved: true,
            Slug: null,
            MetaKeywords: null,
            MetaDescription: null,
            ParentId: null
        },
        initialize: function () {
            this.selected = false;
        },
        validation:pageValidation
    });
    Models.PageHierarchy = Backbone.Model.extend({
        initialize: function () {
            var nodes = this.get("Children");
            if (nodes) {
                this.nodes = new Models.PageHierarchyCollection(nodes);
                this.unset("Children");
            }
        }
    });
    Models.PageCollection = Backbone.Collection.extend({
        model: Models.Page,
        url: '/amphiprion/administration/page/list'
    });

    Models.PageHierarchyCollection = Backbone.Collection.extend({
        model: Models.PageHierarchy,
        url: '/amphiprion/api/v1/pagehierarchy'
    });



    var API = {
        getHierarchy: function () {
            var pages = new Models.PageHierarchyCollection();
            var defer = $.Deferred();
            pages.fetch({
                success: function (data) {
                    defer.resolve(data);
                },
                reset: true
            });

            return defer.promise();
        },
        getPage: function (id) {
            var page = new Models.Page({ id: id });
            var defer = $.Deferred();
            page.fetch({
                success: function (data) {
                    defer.resolve(data);
                }
            });

            return defer.promise();
        }
    };

    App.reqres.setHandler("page:hierarchy", function () {
        return API.getHierarchy();
    });
    App.reqres.setHandler("page:item", function (id) {
        return API.getPage(id);
    });
});