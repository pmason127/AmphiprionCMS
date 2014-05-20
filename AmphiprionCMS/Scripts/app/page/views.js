PageManagerApplication.module('PageManager.Views', function (Views, App, Backbone, Marionette, $) {

    _.extend(Backbone.Validation.callbacks, {
        valid: function (view, attr, selector) {
            var $el = view.$('['+selector+'=' + attr + ']'),
                $group = $el.closest('.form-group');

            $group.removeClass('has-error');
            $group.find('.help-block').html('').addClass('hidden');
        },
        invalid: function (view, attr, error, selector) {
            var $el = view.$('['+selector+'=' + attr + ']'),
                $group = $el.closest('.form-group');

            $group.addClass('has-error');
            $group.find('.help-block').html(error).removeClass('hidden');
        }

    });
    
    Views.PageItemView = Marionette.ItemView.extend({
        tagName: 'li',
        template: '#page-item-view',
    });
    Views.SampleView = Marionette.ItemView.extend({
        tagName: 'span',
        template: '#sample-view',
    });
    Views.OptionsView = Marionette.ItemView.extend({
        tagName: 'div',
        template: '#page-options-view',
        triggers: {
            "click a.add-page": "page:add",
            "click a.edit-page": "page:edit",
            "click a.manage-widgets": "page:widgets",
        }
    });
    
    Views.PageListView = Backbone.Marionette.CompositeView.extend({
        template: '#page-list-view',
        itemView: Views.PageItemView,
        itemViewContainer: '#page-list',
    });

    Views.TreeView = Backbone.Marionette.CompositeView.extend({
        template: "#node-template",
        className:"page-tree-node",
        tagName: "li",
        itemViewContainer: "ul",
        events: {
            "click .show-page": "itemClick"
        },
        initialize: function () {
            var $this = this;
            // grab the child collection from the parent model
            // so that we can render the collection as children
            // of this parent node

            this.collection = this.model.nodes;

            App.on("current:page:statechange", function (id) {
                $this.checkState(id);
            });

        },
        itemClick: function (e) {
            e.stopPropagation();
            e.preventDefault();
            var target = $(e.currentTarget);
            var id = target.data("id");
            App.trigger("page:show", id);

            // this.checkState();

        },
        onRender: function () {
            if (_.isUndefined(this.collection) || this.collection.length <= 0) {
                this.$("ul:first").remove();
            }

            var id = App.request("page:current");
            if (id == 'undefined')
                id = null;
            this.checkState(id);

        },
        checkState: function (id) {
            var curId = id; //App.request("page:current");
            if (curId) {
                this.toggleSelected(curId == this.model.get("Id"));
            } else {
                this.toggleSelected(false);
            }
        },
        toggleSelected: function (seton) {

            if (seton) {
                if (!this.$('a:first').hasClass("selected"))
                    this.$('a:first').addClass("selected");
            } else {
                if (this.$('a:first').hasClass("selected"))
                    this.$('a:first').removeClass("selected");
            }
        }

    });

    // The tree's root: a simple collection view that renders 
    // a recursive tree structure for each item in the collection
    Views.TreeRoot = Backbone.Marionette.CollectionView.extend({
        tagName: "ul",
        className:"page-tree",
        itemView: Views.TreeView,
        itemViewEventPrefix: "tree:node"
    });
    
    
    Views.Form = Marionette.ItemView.extend({
        template: "#page-form",
      initialize:function() {
          Backbone.Validation.bind(this);
      },
        onShow:function() {
            CKEDITOR.replace('HtmlDescription', {
                filebrowserImageBrowseUrl: '/amphiprion/file/browser?t=images',
                filebrowserUploadUrl: '/amphiprion/file/upload',
                filebrowserBrowseUrl: '/amphiprion/file/browser?t=all',
                filebrowserWindowWidth: 500,
                filebrowserWindowHeight: 400
            });
            var pub = this.$el.find("#PublishDateUtc").datetimepicker();
            if (this.model.get("PublishDateUtc")) {
                var dt = this.model.get("PublishDateUtc");
                 pub.data("DateTimePicker").setDate(dt);
             }
        },
     
        events: {
            "click button.btn-submit": "submitClicked"
        },

        submitClicked: function (e) {
            e.preventDefault();
   
           // if (this.model.isValid(true)) {
                var $form = this.$("form");
                var data = $form.serialize();
                this.trigger("form:submit", data);
           // }
            
        }
    });

    Views.AddForm = Views.Form.extend({        
        
    });
    Views.EditForm = Views.Form.extend({

    });
});