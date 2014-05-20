(function ($, global) {
    var events = {},
        currentListView = null,
        _views = null,
        Page = Backbone.Model.extend({            
            urlRoot: '/amphiprion/administration/page/get'
        }),
        PageCollection = Backbone.Collection.extend({
            model: Page,
            url: '/amphiprion/administration/page/list'
        }),
        PageRouter = Backbone.Router.extend({
            routes: {
                "": "list",
                ":id": "page"
            },

            list: function() {
                _listPages(null);
            },
            page: function(id) {
                _listPages(id);

                var page = new Page({ id: id },{silent:true});
                page.fetch({
                    success:function() {
                       // _views.ShowPath(page);
                    }
                });
               

            }
        }),
        _bindEvents = function(options) {
            events = {};
            _.extend(events, Backbone.Events);
        },
        _listPages = function (parentId) {
            var col = new PageCollection();
            col.on("reset", function (collection, response) {
                var current = collection.get(parentId);
                if (current)
                    _views.ShowPath(current);
            });
           
            
            _views.ShowList(col);
            if (parentId == null) {
                col.fetch({reset:true});
            } else {
                col.fetch({ data: { parentId: parentId },reset:true });
            }
           
           
        };
       

    var currentRouter;

    var ViewManager = function (options,events) {

        var _options = options;
        var _events = events;
        var $this = this;
        var PageListView = Backbone.View.extend({
            tagName: 'ul',

            initialize: function() {
                this.collection.bind("reset", this.render, this);
            },

            render: function(eventName) {
                this.$el.empty(); // clear the element to make sure you don't double your contact view
                var self = this;

                this.collection.each(function(page) { // iterate through the collection
                    var pageView = $this.CreateListItem(page);
                    self.$el.append(pageView.el);
                });
               
                return this;
            },
            close: function () {
                this.remove();
                this.unbind();
                this.collection.unbind("reset", this.render);
            }
        }),
            PageItemView = Backbone.View.extend({
                tagName: "li",
                initialize: function() {
                    _.bindAll(this, 'render');
                    this.render(); // Render in the end of initialize
                },
                events: {
                    
                },
                template: _.template($('#page-item-view').html()),

                render: function(eventName) {
                    this.$el.html(this.template(this.model.toJSON()));
                    return this;
                },
                close: function () {
                    this.remove();
                    this.unbind();
                }
            }),
            PageOptionsView = Backbone.View.extend({
                
            }),
            PagePathView = Backbone.View.extend({
                model:Page,
                initialize: function () {
                    _.bindAll(this, 'render');
                    if (this.model) {
                        this.model.on('change', this.render, this);
                    }
                },
                events: {

                },
                template: _.template($('#page-path-view').html()),

                render: function (eventName) {
                    this.$el.html(this.template(this.model.toJSON()));
                    return this;
                },
                close: function () {
                    this.remove();
                    this.unbind();
                }
            }),
            PageEditView = Backbone.View.extend({
                
            });

        var listView = null, optionsView = null, editView = null, pathView = null;

        this.ShowList = function(collection) {
            if (listView != null) {
                listView.close();
            }
            listView = new PageListView({ collection: collection });
            _options.pageListArea.html(listView.render().el);
        };
        this.CreateListItem = function (model) {
            var itemView = new PageItemView({ model: model });
            return itemView;
        };
        this.ShowPath = function(model) {
            if (pathView != null) {
                pathView.close();
            }
            pathView = new PagePathView({ model: model });
            _options.currentPathArea.html(pathView.render().el);
        };
    };

    $.amphiprion.pagemanager = {
        init: function (options) {
            _bindEvents(options);
            _views = new ViewManager(options,events);
            currentRouter = new PageRouter();
            Backbone.history.start();
            
          
        }
    };

})(jQuery, window);